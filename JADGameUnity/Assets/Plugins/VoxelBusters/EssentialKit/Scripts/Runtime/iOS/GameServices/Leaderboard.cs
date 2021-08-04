#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.EssentialKit;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal sealed class Leaderboard : LeaderboardBase
    {
        #region Fields

        private     string                  m_title;

        private     Range                   m_lastRequestRange;

        private     bool                    m_isBusy;

        private     IScore[]                m_scores;

        #endregion

        #region Constructors

        static Leaderboard()
        {
            // register callbacks
            LeaderboardBinding.NPLeaderboardRegisterCallbacks(HandleLoadLeaderboardsNativeCallback, HandleLoadScoresNativeCallback, NativeCallbackResponder.HandleLoadImageNativeCallback);
        }

        public Leaderboard(string id, string platformId) 
            : base(id, platformId)
        {
            var     nativePtr   = LeaderboardBinding.NPLeaderboardCreate(platformId);
            
            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);
            m_title             = LeaderboardBinding.NPLeaderboardGetTitle(nativePtr);
            m_lastRequestRange  = new Range();
        }

        public Leaderboard(IntPtr nativePtr, string id, string platformId) 
            : base(id, platformId)
        {
            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr);
            m_title             = LeaderboardBinding.NPLeaderboardGetTitle(nativePtr);
            m_lastRequestRange  = new Range();
        }

        ~Leaderboard()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        private static Leaderboard[] CreateLeaderboardArray(ref NativeArray nativeArray)
        {
            return MarshalUtility.ConvertNativeArrayItems(
                arrayPtr: nativeArray.Pointer,
                length: nativeArray.Length, 
                converter: (input) =>
                {
                    string  platformId  = LeaderboardBinding.NPLeaderboardGetId(input);
                    var     settings    = GameServices.FindLeaderboardDefinitionWithPlatformId(platformId);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified platform id: {0}", platformId);
                        return null;
                    }

                    return new Leaderboard(nativePtr: input, id: settings.Id, platformId: platformId);
                }, 
                includeNullObjects: false);
        }

        public static void LoadLeaderboards(LoadLeaderboardsInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            LeaderboardBinding.NPLeaderboardLoadLeaderboards(tagPtr);
        }

        public static void ShowLeaderboardView(string leaderboardPlatformId, LeaderboardTimeScope timescope, ViewClosedInternalCallback callback)
        {
            var     gKTimeScope = GameCenterUtility.ConvertToGKLeaderboardTimeScope(timescope);
            LeaderboardBinding.NPLeaderboardShowView(leaderboardPlatformId, gKTimeScope, MarshalUtility.GetIntPtr(callback));
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            return m_title;
        }

        protected override LeaderboardPlayerScope GetPlayerScopeInternal()
        {
            var     playerScope = LeaderboardBinding.NPLeaderboardGetPlayerScope(AddrOfNativeObject());
            return GameCenterUtility.ConvertToLeaderboardPlayerScope(playerScope);
        }

        protected override void SetPlayerScopeInternal(LeaderboardPlayerScope value)
        {
            var     playerScope = GameCenterUtility.ConvertToGKLeaderboardPlayerScope(value);
            LeaderboardBinding.NPLeaderboardSetPlayerScope(AddrOfNativeObject(), playerScope);
        }

        protected override LeaderboardTimeScope GetTimeScopeInternal()
        {
            var     timeScope   = LeaderboardBinding.NPLeaderboardGetTimeScope(AddrOfNativeObject());
            return GameCenterUtility.ConvertToLeaderboardTimeScope(timeScope);
        }
        
        protected override void SetTimeScopeInternal(LeaderboardTimeScope value)
        {
            var     timeScope   = GameCenterUtility.ConvertToGKLeaderboardTimeScope(value);
            LeaderboardBinding.NPLeaderboardSetTimeScope(AddrOfNativeObject(), timeScope);
        }

        protected override IScore GetLocalPlayerScoreInternal()
        {
            var     scorePtr    = LeaderboardBinding.NPLeaderboardGetLocalPlayerScore(AddrOfNativeObject());
            if (IntPtr.Zero != scorePtr)
            {
                // create score object
                return new Score(scorePtr);
            }

            return null;
        }

        protected override void LoadTopScoresInternal(LoadScoresInternalCallback callback)
        {
            LoadScoreInternal(1, LoadScoresQuerySize, true, callback);
        }

        protected override void LoadPlayerCenteredScoresInternal(LoadScoresInternalCallback callback)
        {
            LoadScoreInternal(1, 1, false, (scores, error) =>
            {
                // check request status
                if (error == null)
                {
                    var     localPlayerScore    = GetLocalPlayerScoreInternal();
                    int     startIndex          = ((int)(localPlayerScore.Rank / LoadScoresQuerySize) * LoadScoresQuerySize) + 1;
                    LoadScoreInternal(startIndex, LoadScoresQuerySize, true, callback);
                }
                else
                {
                    callback(null, error);
                }
            });
        }

        protected override void LoadNextInternal(LoadScoresInternalCallback callback)
        {
            // check whether we have necessary data to support pagination
            if ((m_scores == null) || (m_scores.Length == 0))
            {
                LoadTopScoresInternal(callback);
                return;
            }

            // seek to next page results
            int     scoreLength             = m_scores.Length;
            var     lastScoreEntry          = m_scores[scoreLength - 1];
            long    nextPageStartIndex      = lastScoreEntry.Rank + 1;
            LoadScoreInternal(nextPageStartIndex, LoadScoresQuerySize, true, callback);
        }

        protected override void LoadPreviousInternal(LoadScoresInternalCallback callback)
        {
            // check whether we have necessary data to support pagination
            if ((m_scores == null) || (m_scores.Length == 0))
            {
                LoadTopScoresInternal(callback);
                return;
            }

            // seek to previous page results
            var     firstScoreEntry         = m_scores[0];
            long    prevPageStartIndex      = firstScoreEntry.Rank - LoadScoresQuerySize;
            if (prevPageStartIndex < 0)
            {
                prevPageStartIndex  = 1;
            }
            LoadScoreInternal(prevPageStartIndex, LoadScoresQuerySize, true, callback);
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            LeaderboardBinding.NPLeaderboardLoadImage(AddrOfNativeObject(), tagPtr);
        }

        #endregion

        #region Private methods

        private void LoadScoreInternal(long startIndex, int count, bool recordRequest, LoadScoresInternalCallback callback)
        {
            // record request properties, if specified
            if (recordRequest)
            {
                m_lastRequestRange.StartIndex   = startIndex;
                m_lastRequestRange.Count        = count;
            }

            // create intermediate response handler
            LoadScoresInternalCallback internalCallback = (scores, error) => OnLoadScoreFinished(scores, error, callback);

            // make request
            var     tagPtr      = MarshalUtility.GetIntPtr(internalCallback);
            LeaderboardBinding.NPLeaderboardLoadScores(AddrOfNativeObject(), startIndex, count, tagPtr);
        }

        private void OnLoadScoreFinished(IScore[] scores, Error error, LoadScoresInternalCallback callback)
        {
            // check response status
            if (error == null)
            {
                m_scores        = scores;
            }

            // send result to caller object
            callback(scores, error);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(GameServicesLoadArrayNativeCallback))]
        private static void HandleLoadLeaderboardsNativeCallback(ref NativeArray nativeArray, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     leaderboards    = CreateLeaderboardArray(ref nativeArray);
                var     errorObj        = Error.CreateNullableError(description: error);
                ((LoadLeaderboardsInternalCallback)tagHandle.Target)(leaderboards, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(GameServicesLoadArrayNativeCallback))]
        private static void HandleLoadScoresNativeCallback(ref NativeArray nativeArray, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     managedArray    = MarshalUtility.CreateManagedArray(nativeArray.Pointer, nativeArray.Length);
                var     scores          = (managedArray == null) ? null : Array.ConvertAll(managedArray, (nativePtr) => new Score(nativePtr));
                var     errorObj        = Error.CreateNullableError(description: error);
                ((LoadScoresInternalCallback)tagHandle.Target)(scores, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        #endregion

        #region Nested types

        private class Range
        {
            public long StartIndex { get; set; }

            public int Count { get; set; }
        }

        #endregion
    }
}
#endif
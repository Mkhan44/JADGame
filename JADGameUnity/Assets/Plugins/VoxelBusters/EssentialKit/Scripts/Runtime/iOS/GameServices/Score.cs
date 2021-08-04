#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal sealed class Score : ScoreBase
    {
        #region Constructors

        static Score()
        {
            // register callbacks
            ScoreBinding.NPScoreRegisterCallbacks(HandleReportScoreCallbackInternal);
        }

        public Score(string leaderboardId, string leaderboardPlatformId) 
            : base(leaderboardId, leaderboardPlatformId)
        {
            var     nativePtr   = ScoreBinding.NPScoreCreate(leaderboardPlatformId);
            
            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);
        }

        public Score(IntPtr nativePtr) 
            : base(leaderboardPlatformId: ScoreBinding.NPScoreGetLeaderboardId(nativePtr))
        {
            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr);
        }

        ~Score()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        protected override IPlayer GetPlayerInternal()
        {
            var     playerPtr   = ScoreBinding.NPScoreGetPlayer(AddrOfNativeObject());
            return new Player(playerPtr);
        }

        protected override long GetRankInternal()
        {
            return ScoreBinding.NPScoreGetRank(AddrOfNativeObject());
        }

        protected override long GetValueInternal()
        {
            return ScoreBinding.NPScoreGetValue(AddrOfNativeObject());
        }

        protected override void SetValueInternal(long value)
        {
            ScoreBinding.NPScoreSetValue(AddrOfNativeObject(), value);
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            string  dateStr     = ScoreBinding.NPScoreGetLastReportedDate(AddrOfNativeObject());
            return IosNativePluginsUtility.ParseDateTimeStringInUTCFormat(dateStr);
        }

        protected override void ReportScoreInternal(ReportScoreInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            ScoreBinding.NPScoreReportScore(AddrOfNativeObject(), tagPtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(GameServicesReportNativeCallback))]
        private static void HandleReportScoreCallbackInternal(string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);
            
            try
            {
                // send result
                var     errorObj    = Error.CreateNullableError(description: error);
                ((ReportScoreInternalCallback)tagHandle.Target).Invoke(errorObj);
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
    }
}
#endif
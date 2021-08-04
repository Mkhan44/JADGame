using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal sealed class NullLeaderboard : LeaderboardBase
    {
        #region Constructors

        public NullLeaderboard(string id, string platformId) : base(id, platformId)
        {
            LogNotSupported();
        }

        #endregion

        #region Static methods

        public static void LoadLeaderboards(LoadLeaderboardsInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        public static void ShowLeaderboardView(string leaderboardId, LeaderboardTimeScope timescope, ViewClosedInternalCallback callback)
        {
            LogNotSupported();

            callback(Diagnostics.kFeatureNotSupported);
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("Leaderboard");
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override LeaderboardPlayerScope GetPlayerScopeInternal()
        {
            LogNotSupported();

            return default(LeaderboardPlayerScope);
        }

        protected override void SetPlayerScopeInternal(LeaderboardPlayerScope value)
        {
            LogNotSupported();
        }

        protected override LeaderboardTimeScope GetTimeScopeInternal()
        {
            LogNotSupported();

            return default(LeaderboardTimeScope);
        }
        
        protected override void SetTimeScopeInternal(LeaderboardTimeScope value)
        {
            LogNotSupported();
        }

        protected override IScore GetLocalPlayerScoreInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override void LoadTopScoresInternal(LoadScoresInternalCallback callback)
        {
            LogNotSupported();
            
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        protected override void LoadPlayerCenteredScoresInternal(LoadScoresInternalCallback callback)
        {
            LogNotSupported();
            
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        protected override void LoadNextInternal(LoadScoresInternalCallback callback)
        {
            LogNotSupported();
            
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        protected override void LoadPreviousInternal(LoadScoresInternalCallback callback)
        {
            LogNotSupported();
            
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            LogNotSupported();
            
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal sealed class NullGameServicesInterface : NativeGameServicesInterfaceBase 
    {
        #region Constructors

        public NullGameServicesInterface()
            : base(isAvailable: false)
        { }

        #endregion

        #region INativeGameServicesInterface implementation

        public override void LoadLeaderboards(LoadLeaderboardsInternalCallback callback)
        {
            NullLeaderboard.LoadLeaderboards(callback);
        }

        public override void ShowLeaderboard(string leaderboardId, string leaderboardPlatformId, LeaderboardTimeScope timeScope, ViewClosedInternalCallback callback)
        {
            NullLeaderboard.ShowLeaderboardView(leaderboardId, timeScope, callback);
        }

        public override ILeaderboard CreateLeaderboard(string id, string platformId)
        {
            return new NullLeaderboard(id, platformId);
        }

        public override void LoadAchievementDescriptions(LoadAchievementDescriptionsInternalCallback callback)
        {
            NullAchievementDescription.LoadAchievementDescriptions(callback);
        }

        public override void LoadAchievements(LoadAchievementsInternalCallback callback)
        {
            NullAchievement.LoadAchievements(callback);
        }

        public override void ShowAchievements(ViewClosedInternalCallback callback)
        {
            NullAchievement.ShowAchievementView(callback);
        }

        public override IAchievement CreateAchievement(string id, string platformId)
        {
            return new NullAchievement(id, platformId);
        }

        public override void LoadPlayers(string[] playerIds, LoadPlayersInternalCallback callback)
        {
            NullPlayer.LoadPlayers(playerIds, callback);
        }

        public override void SetAuthChangeCallback(AuthChangeInternalCallback callback)
        {
            NullLocalPlayer.SetAuthChangeCallback(callback);
        }

        public override void Authenticate()
        {
            NullLocalPlayer.Authenticate();
        }

        public override void Signout()
        {
            NullLocalPlayer.Signout();
        }

        public override ILocalPlayer GetLocalPlayer()
        {
            return NullLocalPlayer.GetLocalPlayer();
        }

        public override IScore CreateScore(string leaderboardId, string leaderboardPlatformId)
        {
            return new NullScore(leaderboardId, leaderboardPlatformId);
        }

        public override void SetCanShowAchievementCompletionBanner(bool value)
        {
            NullAchievement.SetCanShowBannerOnCompletion(value);
        }

        public override void LoadServerCredentials(LoadServerCredentialsInternalCallback callback)
        {
            Diagnostics.LogNotSupported("LoadServerCredentials");
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
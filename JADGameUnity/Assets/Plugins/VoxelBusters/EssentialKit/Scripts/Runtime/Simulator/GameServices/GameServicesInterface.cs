using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    public sealed class GameServicesInterface : NativeGameServicesInterfaceBase 
    {
        #region Constructors

        public GameServicesInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region INativeGameServicesInterface implementation

        public override void LoadLeaderboards(LoadLeaderboardsInternalCallback callback)
        {
            Leaderboard.LoadLeaderboards(callback);
        }

        public override void ShowLeaderboard(string leaderboardId, string leaderboardPlatformId, LeaderboardTimeScope timeScope, ViewClosedInternalCallback callback)
        {
            Leaderboard.ShowLeaderboardView(leaderboardId, timeScope, callback);
        }

        public override ILeaderboard CreateLeaderboard(string id, string platformId)
        {
            return new Leaderboard(id: id, platformId: platformId);
        }

        public override void LoadAchievementDescriptions(LoadAchievementDescriptionsInternalCallback callback)
        {
            AchievementDescription.LoadAchievementDescriptions(callback);
        }

        public override void LoadAchievements(LoadAchievementsInternalCallback callback)
        {
            Achievement.LoadAchievements(callback);
        }

        public override void ShowAchievements(ViewClosedInternalCallback callback)
        {
            Achievement.ShowAchievementView(callback);
        }

        public override IAchievement CreateAchievement(string id, string platformId)
        {
            return new Achievement(id, platformId);
        }

        public override void LoadPlayers(string[] playerIds, LoadPlayersInternalCallback callback)
        {
            Player.LoadPlayers(playerIds, callback);
        }

        public override void SetAuthChangeCallback(AuthChangeInternalCallback callback)
        {
            LocalPlayer.SetAuthChangeCallback(callback);
        }

        public override void Authenticate()
        {
            LocalPlayer.Authenticate();
        }

        public override void Signout()
        {
            LocalPlayer.Signout();
        }

        public override ILocalPlayer GetLocalPlayer()
        {
            return LocalPlayer.GetLocalPlayer();
        }

        public override IScore CreateScore(string leaderboardId, string leaderboardPlatformId)
        {
            return new Score(leaderboardId, leaderboardPlatformId);
        }

        public override void SetCanShowAchievementCompletionBanner(bool value)
        { }

        public override void LoadServerCredentials(LoadServerCredentialsInternalCallback callback)
        {
            Diagnostics.LogNotSupportedInEditor("LoadServerCredentials");
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
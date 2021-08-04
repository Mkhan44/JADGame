#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    public sealed class GameCenterInterface : NativeGameServicesInterfaceBase 
    {
        #region Constructors

        static GameCenterInterface()
        {
            GameCenterBinding.NPGameServicesSetViewClosedCallback(NativeCallbackResponder.HandleViewClosedNativeCallback);
            GameCenterBinding.NPGameServicesLoadServerCredentialsCompleteCallback(NativeCallbackResponder.HandleLoadServerCredentialsNativeCallback);
        }

        public GameCenterInterface()
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
            Leaderboard.ShowLeaderboardView(leaderboardPlatformId, timeScope, callback);
        }

        public override ILeaderboard CreateLeaderboard(string id, string platformId)
        {
            return new Leaderboard(id, platformId);
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

        public override void SetCanShowAchievementCompletionBanner(bool value)
        {
            Achievement.SetCanShowBannerOnCompletion(value);
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

        public override ILocalPlayer GetLocalPlayer()
        {
            return LocalPlayer.GetLocalPlayer();
        }

        public override void Signout()
        {
            Diagnostics.LogNotSupported("Signout");
        }

        public override IScore CreateScore(string leaderboardId, string leaderboardPlatformId)
        {
            return new Score(leaderboardId, leaderboardPlatformId);
        }

        public override void LoadServerCredentials(LoadServerCredentialsInternalCallback callback)
        {
            GameCenterBinding.NPGameServicesLoadServerCredentials(MarshalUtility.GetIntPtr(callback));
        }

        #endregion
    }
}
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    public abstract class NativeGameServicesInterfaceBase : NativeFeatureInterfaceBase, INativeGameServicesInterface
    {
        #region Constructors

        protected NativeGameServicesInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeGameServicesInterface implementation

        public abstract void LoadLeaderboards(LoadLeaderboardsInternalCallback callback);

        public abstract void ShowLeaderboard(string leaderboardId, string leaderboardPlatformId, LeaderboardTimeScope timeScope, ViewClosedInternalCallback callback);
        
        public abstract ILeaderboard CreateLeaderboard(string id, string platformId);

        public abstract void LoadAchievementDescriptions(LoadAchievementDescriptionsInternalCallback callback);

        public abstract void LoadAchievements(LoadAchievementsInternalCallback callback);

        public abstract void ShowAchievements(ViewClosedInternalCallback callback);

        public abstract void SetCanShowAchievementCompletionBanner(bool value);
        
        public abstract IAchievement CreateAchievement(string id, string platformId);

        public abstract void LoadPlayers(string[] playerIds, LoadPlayersInternalCallback callback);

        public abstract void SetAuthChangeCallback(AuthChangeInternalCallback callback);

        public abstract void Authenticate();

        public abstract void Signout();

        public abstract ILocalPlayer GetLocalPlayer();
        
        public abstract IScore CreateScore(string leaderboardId, string leaderboardPlatformId);

        public abstract void LoadServerCredentials(LoadServerCredentialsInternalCallback callback);

        #endregion
    }
}
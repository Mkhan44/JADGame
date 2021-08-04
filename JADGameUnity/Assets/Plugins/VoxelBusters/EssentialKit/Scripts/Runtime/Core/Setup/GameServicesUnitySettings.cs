using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class GameServicesUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Nested types

        [SerializeField, FormerlySerializedAs("m_leaderboardMetaArray")]
        [Tooltip ("Array contains information of the leaderboards used within the game.")]
        public      LeaderboardDefinition[]     m_leaderboards;

        [SerializeField]
        [Tooltip ("Array contains information of the achievements used within the game.")]
        public      AchievementDefinition[]     m_achievements;

        [SerializeField]
        [Tooltip ("If enabled, a banner is displayed when an achievement is completed (iOS).")]
        private     bool                        m_showAchievementCompletionBanner;

        [SerializeField]
        [Tooltip("Android specific settings.")]
        private     AndroidPlatformProperties   m_androidProperties;

        #endregion

        #region Properties

        public LeaderboardDefinition[] Leaderboards
        {
            get
            {
                return m_leaderboards;
            }
        }

        public AchievementDefinition[] Achievements
        {
            get
            {
                return m_achievements;
            }
        }

        public bool ShowAchievementCompletionBanner
        {
            get
            {
                return m_showAchievementCompletionBanner;
            }
        }

        public AndroidPlatformProperties AndroidProperties
        {
            get
            {
                return m_androidProperties;
            }
        }

        #endregion

        #region Constructors

        public GameServicesUnitySettings(bool enabled = true, bool initializeOnStart = true, 
                                    LeaderboardDefinition[] leaderboards = null, AchievementDefinition[] achievements = null, 
                                    bool showAchievementCompletionBanner = true, AndroidPlatformProperties androidProperties = null)
            : base(enabled)
        {
            // set default values
            m_leaderboards                      = leaderboards ?? new LeaderboardDefinition[0];
            m_achievements                      = achievements ?? new AchievementDefinition[0];
            m_showAchievementCompletionBanner   = showAchievementCompletionBanner;
            m_androidProperties                 = androidProperties ?? new AndroidPlatformProperties();
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kGameServices;
        }

        #endregion
    }
}
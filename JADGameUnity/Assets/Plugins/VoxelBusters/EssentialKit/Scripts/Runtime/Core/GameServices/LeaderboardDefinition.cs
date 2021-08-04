using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Represents an object containing additional information related to game leaderboard.
    /// </summary>
    [Serializable]
    public partial class LeaderboardDefinition
    {
        #region Fields

        [SerializeField]
        private     string                      m_id;

        [SerializeField]
        private     string                      m_platformId;

        [SerializeField]
        private     NativePlatformConstantSet   m_platformIdOverrides;

        [SerializeField]
        private     string                      m_title;

        [SerializeField, HideInInspector]
        private     IosPlatformProperties       m_iosProperties;

        [SerializeField, HideInInspector]
        private     AndroidPlatformProperties   m_androidProperties;

        #endregion

        #region Properties

        /// <summary>
        /// The string that identifies the leaderboard within Unity environment. (read-only)
        /// </summary>
        public string Id
        {
            get
            {
                return m_id;
            }
        }

        /// <summary>
        /// The name of the leaderboard. (read-only)
        /// </summary>
        public string Title
        {
            get
            {
                return m_title;
            }
        }

        public IosPlatformProperties IosProperties
        {
            get
            {
                return m_iosProperties;
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

        public LeaderboardDefinition(string id = null, string platformId = null, NativePlatformConstantSet platformIdOverrides = null, string title = null, IosPlatformProperties iosProperties = null, AndroidPlatformProperties androidProperties = null)
        {
            // set default values
            m_id                    = id;
            m_platformId            = platformId;
            m_platformIdOverrides   = platformIdOverrides ?? new NativePlatformConstantSet();
            m_title                 = title;
            m_iosProperties         = iosProperties ?? new IosPlatformProperties(); 
            m_androidProperties     = androidProperties ?? new AndroidPlatformProperties(); 
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the leaderboard identifier for active platform.
        /// </summary>
        public string GetPlatformIdForActivePlatform()
        {
            return m_platformIdOverrides.GetConstantForActivePlatform(m_platformId);
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class CloudServicesUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Fields

        [SerializeField]
        [Tooltip("If enabled, data is synchronized automatically on launch.")]
        private     bool            m_synchronizeOnLoad;
        
        [SerializeField]
        [Tooltip("Time interval (in seconds) between consecutive sync calls.")]
        private     int             m_syncInterval          = 60;

        #endregion

        #region Properties

        public bool SynchronizeOnLoad
        {
            get
            {
                return m_synchronizeOnLoad;
            }
        }

        public int SyncInterval
        {
            get
            {
                return m_syncInterval;
            }
        }

        #endregion

        #region Constructors

        public CloudServicesUnitySettings(bool enabled = true, bool synchronizeOnLoad = false, int syncInterval = 60) 
            : base(enabled)
        { 
            // set properties
            m_synchronizeOnLoad = synchronizeOnLoad;
            m_syncInterval      = syncInterval;
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kCloudServices;
        }

        #endregion
    }
}
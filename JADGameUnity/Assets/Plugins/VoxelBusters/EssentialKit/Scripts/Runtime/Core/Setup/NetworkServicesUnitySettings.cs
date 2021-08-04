using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class NetworkServicesUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Fields

        [SerializeField]
        [Tooltip("Host address.")]
        private     Address             m_hostAddress;

        [SerializeField]
        [Tooltip("If enabled, rechability trackers are activated on launch.")]
        private     bool                m_autoStartNotifier;

        [SerializeField]
        [Tooltip("Ping test configuration.")]
        private     PingTestSettings    m_pingSettings;

        #endregion

        #region Properties

        public Address HostAddress
        {
            get
            {
                return m_hostAddress;
            }
        }

        public bool AutoStartNotifier
        {
            get
            {
                return m_autoStartNotifier;
            }
        }

        public PingTestSettings PingSettings
        {
            get
            {
                return m_pingSettings;
            }
        }

        #endregion

        #region Constructors

        public NetworkServicesUnitySettings(bool enabled = true, Address hostAddress = null, 
                                       bool autoStartNotifier = true, PingTestSettings pingSettings = null)
            : base(enabled)
        {
            // set properties
            m_hostAddress       = hostAddress ?? new Address();
            m_autoStartNotifier = autoStartNotifier;
            m_pingSettings      = pingSettings ?? new PingTestSettings();
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kNetworkServices;
        }

        #endregion
    }
}
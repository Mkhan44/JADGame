using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class ApplicationSettings
    {
        #region Fields

        [SerializeField]
        private     DebugLogger.LogLevel                    m_logLevel                      = DebugLogger.LogLevel.Critical;

        [SerializeField]
        [Tooltip("Stores the registration ids of this app.")]
        private     NativePlatformConstantSet               m_appStoreIds                   = new NativePlatformConstantSet();

        [SerializeField]
        [Tooltip("Usage permission settings.")]
        private     NativeFeatureUsagePermissionSettings    m_usagePermissionSettings       = new NativeFeatureUsagePermissionSettings();

        [SerializeField]
        [Tooltip("RateMyApp settings.")]
        private     RateMyAppSettings                       m_rateMyAppSettings             = new RateMyAppSettings();

        #endregion

        #region Properties

        public DebugLogger.LogLevel LogLevel
        {
            get
            {
                return m_logLevel;
            }
            set
            {
                m_logLevel  = value;
            }
        }

        public NativeFeatureUsagePermissionSettings UsagePermissionSettings
        {
            get
            {
                return m_usagePermissionSettings;
            }
            set
            {
                m_usagePermissionSettings = value;
            }
        }

        public RateMyAppSettings RateMyAppSettings
        {
            get
            {
                return m_rateMyAppSettings;
            }
            set
            {
                m_rateMyAppSettings = value;
            }
        }

        #endregion

        #region Constructors

        public ApplicationSettings(NativePlatformConstantSet appStoreIds = null, RateMyAppSettings rateMyAppSettings = null, NativeFeatureUsagePermissionSettings usagePermissionSettings = null, DebugLogger.LogLevel logLevel = DebugLogger.LogLevel.Critical)
        {
            // set properties
            m_appStoreIds               = appStoreIds ?? new NativePlatformConstantSet();
            m_rateMyAppSettings         = rateMyAppSettings ?? new RateMyAppSettings();
            m_usagePermissionSettings   = usagePermissionSettings ?? new NativeFeatureUsagePermissionSettings();
            m_logLevel                  = logLevel;
        }

        #endregion

        #region Public methods

        public string GetAppStoreIdForPlatform(NativePlatform platform)
        {
            return m_appStoreIds.GetConstantForPlatform(platform);
        }

        public string GetAppStoreIdForActivePlatform()
        {
            return m_appStoreIds.GetConstantForActivePlatform();
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    public class EssentialKitSettings : ScriptableObject
    {
        #region Static fields

        private     static      EssentialKitSettings    s_sharedInstance                = null;

        #endregion

        #region Fields

        [SerializeField]
        private     ApplicationSettings                 m_applicationSettings           = new ApplicationSettings();

        [SerializeField]
        private     AddressBookUnitySettings            m_addressBookSettings           = new AddressBookUnitySettings();

        [SerializeField]
        private     NativeUIUnitySettings               m_nativeUISettings              = new NativeUIUnitySettings();

        [SerializeField]
        private     SharingServicesUnitySettings        m_sharingServicesSettings       = new SharingServicesUnitySettings();

        [SerializeField]
        private     CloudServicesUnitySettings          m_cloudServicesSettings         = new CloudServicesUnitySettings();

        [SerializeField]
        private     GameServicesUnitySettings           m_gameServicesSettings          = new GameServicesUnitySettings();

        [SerializeField]
        private     BillingServicesUnitySettings        m_billingServicesSettings       = new BillingServicesUnitySettings();

        [SerializeField]
        private     NetworkServicesUnitySettings        m_networkServicesSettings       = new NetworkServicesUnitySettings();

        [SerializeField]
        private     WebViewUnitySettings                m_webViewSettings               = new WebViewUnitySettings();

        [SerializeField]
        private     NotificationServicesUnitySettings   m_notificationServicesSettings  = new NotificationServicesUnitySettings();

        [SerializeField]
        private     MediaServicesUnitySettings          m_mediaServicesSettings         = new MediaServicesUnitySettings();

        [SerializeField]
        private     DeepLinkServicesUnitySettings       m_deepLinkServicesSettings      = new DeepLinkServicesUnitySettings();

        #endregion

        #region Static properties

        public static EssentialKitSettings Instance
        {
            get { return GetSharedInstanceInternal(); }
        }

        #endregion

        #region Properties

        public ApplicationSettings ApplicationSettings
        {
            get
            {
                return m_applicationSettings;
            }
            set
            {
                m_applicationSettings   = value;
            }
        }

        public AddressBookUnitySettings AddressBookSettings
        {
            get
            {
                return m_addressBookSettings;
            }
            set
            {
                m_addressBookSettings   = value;
            }
        }

        public NativeUIUnitySettings NativeUISettings
        {
            get
            {
                return m_nativeUISettings;
            }
            set
            {
                m_nativeUISettings    = value;
            }
        }

        public SharingServicesUnitySettings SharingServicesSettings
        {
            get
            {
                return m_sharingServicesSettings;
            }
            set
            {
                m_sharingServicesSettings   = value;
            }
        }

        public CloudServicesUnitySettings CloudServicesSettings
        {
            get
            {
                return m_cloudServicesSettings;
            }
            set
            {
                m_cloudServicesSettings = value;
            }
        }

        public GameServicesUnitySettings GameServicesSettings
        {
            get
            {
                return m_gameServicesSettings;
            }
            set
            {
                m_gameServicesSettings  = value;
            }
        }

        public BillingServicesUnitySettings BillingServicesSettings
        {
            get
            {
                return m_billingServicesSettings;
            }
            set
            {
                m_billingServicesSettings   = value;
            }
        }

        public NetworkServicesUnitySettings NetworkServicesSettings
        {
            get
            {
                return m_networkServicesSettings;
            }
            set
            {
                m_networkServicesSettings   = value;
            }
        }

        public WebViewUnitySettings WebViewSettings
        {
            get
            {
                return m_webViewSettings;
            }
            set
            {
                m_webViewSettings   = value;
            }
        }

        public NotificationServicesUnitySettings NotificationServicesSettings
        {
            get
            {
                return m_notificationServicesSettings;
            }
            set
            {
                m_notificationServicesSettings  = value;
            }
        }

        public MediaServicesUnitySettings MediaServicesSettings
        {
            get
            {
                return m_mediaServicesSettings;
            }
            set
            {
                m_mediaServicesSettings     = value;
            }
        }

        public DeepLinkServicesUnitySettings DeepLinkServicesSettings
        {
            get
            {
                return m_deepLinkServicesSettings;
            }
            set
            {
                m_deepLinkServicesSettings  = value;
            }
        }

        #endregion

        #region Static methods

        private static EssentialKitSettings GetSharedInstanceInternal(bool throwError = true)
        {
            if (null == s_sharedInstance)
            {
                // check whether we are accessing in edit or play mode
                var    settings    = Resources.Load<EssentialKitSettings>(Constants.kPluginResourcesPath + Constants.kPluginSettingsFileNameWithoutExtension);
                if (throwError && (null == settings))
                {
                    throw Diagnostics.PluginNotConfiguredException();
                }

                // store reference
                s_sharedInstance = settings;
            }

            return s_sharedInstance;
        }

        #endregion

        #region Feature methods

        public string[] GetAvailableFeatureNames()
        {
            return new string[]
            {
                NativeFeatureType.kAddressBook,
                NativeFeatureType.kBillingServices,
                NativeFeatureType.kCloudServices,
                NativeFeatureType.kGameServices,
                NativeFeatureType.kMediaServices,
                NativeFeatureType.kNativeUI,
                NativeFeatureType.kNetworkServices,
                NativeFeatureType.kNotificationServices,
                NativeFeatureType.KSharingServices,
                NativeFeatureType.kWebView,
                NativeFeatureType.kDeepLinkServices
            };
        }

        public string[] GetUsedFeatureNames()
        {
            var     usedFeatures    = new List<string>();
            if (m_addressBookSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kAddressBook);
            }
            if (m_billingServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kBillingServices);
            }
            if (m_cloudServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kCloudServices);
            }
            if (m_gameServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kGameServices);
            }
            if (m_mediaServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kMediaServices);
            }
            if (m_nativeUISettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kNativeUI);
            }
            if (m_networkServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kNetworkServices);
            }
            if (m_notificationServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kNotificationServices);
            }
            if (m_sharingServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.KSharingServices);
            }
            if (m_webViewSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kWebView);
            }
            if (m_deepLinkServicesSettings.IsEnabled)
            {
                usedFeatures.Add(NativeFeatureType.kDeepLinkServices);
            }
            if ((usedFeatures.Count > 0) || (m_applicationSettings.RateMyAppSettings.IsEnabled))
            {
                usedFeatures.Add(NativeFeatureType.kExtras);
            }

            return usedFeatures.ToArray();
        }

        public bool IsFeatureUsed(string name)
        {
            return Array.Exists(GetUsedFeatureNames(), (item) => string.Equals(item, name));
        }

        #endregion
    }
}
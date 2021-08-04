using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class DeepLinkServicesUnitySettings : NativeFeatureUnitySettingsBase 
    {
        #region Fields

        [SerializeField]
        private     IosPlatformProperties           m_iosProperties;

        [SerializeField]
        private     AndroidPlatformProperties       m_androidProperties;

        #endregion

        #region Properties

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

        public DeepLinkServicesUnitySettings(bool enabled = true, IosPlatformProperties iosProperties = null, AndroidPlatformProperties androidProperties = null) 
            : base(enabled)
        { 
            // set properties
            m_iosProperties         = iosProperties ?? new IosPlatformProperties();
            m_androidProperties     = androidProperties ?? new AndroidPlatformProperties();
        }

        #endregion

        #region Static methods

        private static int FindDeepLinkIndexWithIdentifier(List<DeepLinkDefinition> list, string identifier)
        {
            return list.FindIndex((item) => string.Equals(item.Identifier, identifier));
        }

        private static void AddDeepLinkDefinition(List<DeepLinkDefinition> list, DeepLinkDefinition deepLinkSettings)
        {
            int     index   = FindDeepLinkIndexWithIdentifier(list, deepLinkSettings.Identifier);
            if (index != -1)
            {
                list[index] = deepLinkSettings;
            }
            else
            {
                list.Add(deepLinkSettings);
            }
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kDeepLinkServices;
        }

        #endregion

        #region Public methods

        public DeepLinkDefinition[] GetCustomSchemeUrlsForPlatform(NativePlatform platform)
        {
            switch (platform)
            {
                case NativePlatform.iOS:
                    return m_iosProperties.CustomSchemeUrls;

                case NativePlatform.Android:
                    return m_androidProperties.CustomSchemeUrls;

                default:
                    return new DeepLinkDefinition[0];
            }
        }

        public DeepLinkDefinition[] GetUniversalLinksForPlatform(NativePlatform platform)
        {
            switch (platform)
            {
                case NativePlatform.iOS:
                    return m_iosProperties.UniversalLinks;

                case NativePlatform.Android:
                    return m_androidProperties.UniversalLinks;

                default:
                    return new DeepLinkDefinition[0];
            }
        }

        public void AddCustomSchemeUrl(DeepLinkDefinition definition, NativePlatform platform)
        {
            switch (platform)
            {
                case NativePlatform.iOS:
                    m_iosProperties.AddCustomSchemeUrl(definition);
                    break;

                case NativePlatform.Android:
                    m_androidProperties.AddCustomSchemeUrl(definition);
                    break;
            }
        }

        public void AddUniversalLink(DeepLinkDefinition definition, NativePlatform platform)
        {
            switch (platform)
            {
                case NativePlatform.iOS:
                    m_iosProperties.AddUniversalLink(definition);
                    break;

                case NativePlatform.Android:
                    m_androidProperties.AddUniversalLink(definition);
                    break;
            }
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class WebViewUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Fields

        [SerializeField]
        [Tooltip("Android specific settings.")]
        private AndroidPlatformProperties m_androidProperties;

        #endregion

        #region Properties

        public AndroidPlatformProperties AndroidProperties
        {
            get
            {
                return m_androidProperties;
            }
        }

        #endregion

        #region Constructors

        public WebViewUnitySettings(bool enabled = true, AndroidPlatformProperties androidProperties = null) 
            : base(enabled)
        {
            m_androidProperties = androidProperties ?? new AndroidPlatformProperties();
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kWebView;
        }

        #endregion
    }
}
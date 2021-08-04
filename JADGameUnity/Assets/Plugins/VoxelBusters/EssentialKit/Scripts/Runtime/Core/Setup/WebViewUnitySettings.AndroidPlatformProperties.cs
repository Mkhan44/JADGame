using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public partial class WebViewUnitySettings
    {
        [Serializable]
        public class AndroidPlatformProperties 
        {
            #region Fields

            [SerializeField]
            [Tooltip ("Enabling this will allow your app to access camera from webview")]
            private     bool    m_usesCamera;

            [SerializeField]
            [Tooltip("Enabling this will allow you to dismiss webview when back navigation button on the device is pressed")]
            private bool m_allowBackNavigationKey = true;

            #endregion

            #region Properties

            public bool UsesCamera
            {
                get
                {
                    return m_usesCamera;
                }
            }

            public bool AllowBackNavigationKey
            {
                get
                {
                    return m_allowBackNavigationKey;
                }
            }

            #endregion

            #region Constructors

            public AndroidPlatformProperties(bool usesCamera = false)
            {
                // set properties
                m_usesCamera     = usesCamera;
            }

            #endregion
        }
    }
}
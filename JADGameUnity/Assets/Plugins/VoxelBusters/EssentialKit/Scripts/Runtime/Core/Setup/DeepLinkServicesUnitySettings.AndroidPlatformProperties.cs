using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    public partial class DeepLinkServicesUnitySettings 
    {
        [Serializable]
        public class AndroidPlatformProperties
        {
            #region Fields

            [SerializeField]
            private     List<DeepLinkDefinition>      m_customSchemeUrls;

            [SerializeField]
            [Tooltip("Universal links are termed as App Links on Android")]
            private     List<DeepLinkDefinition>      m_universalLinks;

            #endregion

            #region Properties

            public DeepLinkDefinition[] CustomSchemeUrls
            {
                get
                {
                    return m_customSchemeUrls.ToArray();
                }
            }

            public DeepLinkDefinition[] UniversalLinks
            {
                get
                {
                    return m_universalLinks.ToArray();
                }
            }

            #endregion

            #region Constructors

            public AndroidPlatformProperties(DeepLinkDefinition[] customSchemeUrls = null, DeepLinkDefinition[] universalLinks = null) 
            { 
                // set properties
                m_customSchemeUrls  = new List<DeepLinkDefinition>(customSchemeUrls ?? new DeepLinkDefinition[0]);
                m_universalLinks    = new List<DeepLinkDefinition>(universalLinks ?? new DeepLinkDefinition[0]);
            }

            #endregion

            #region Public methods

            public void AddCustomSchemeUrl(DeepLinkDefinition definition)
            {
                AddDeepLinkDefinition(m_customSchemeUrls, definition);
            }

            public void AddUniversalLink(DeepLinkDefinition definition)
            {
                AddDeepLinkDefinition(m_universalLinks, definition);
            }

            #endregion
        }
    }
}
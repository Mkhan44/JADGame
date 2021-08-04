using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class MediaServicesUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Fields

        [SerializeField]
        [Tooltip ("If enabled, permission required to access camera will be added.")]
        private     bool        m_usesCamera;

        [SerializeField]
        [Tooltip ("If enabled, permission required to access gallery will be added.")]
        private     bool        m_usesGallery;

        [SerializeField]
        [Tooltip ("If enabled, permission required to save file to gallery will be added.")]
        private     bool        m_savesFilesToGallery;

        #endregion

        #region Properties

        public bool UsesCamera
        {
            get
            {
                return m_usesCamera;
            }
            set
            {
                m_usesCamera    = value;
            }
        }

        public bool UsesGallery
        {
            get
            {
                return m_usesGallery;
            }
            set
            {
                m_usesGallery   = value;
            }
        }

        public bool SavesFilesToGallery
        {
            get
            {
                return m_savesFilesToGallery;
            }
            set
            {
                m_savesFilesToGallery   = value;
            }
        }

        #endregion

        #region Constructors

        public MediaServicesUnitySettings(bool enabled = true, bool usesCamera = true, 
                                            bool usesGallery = true, bool savesFilesToGallery = true)
            : base(enabled)
        {
            // set properties
            m_usesCamera            = usesCamera;
            m_usesGallery           = usesGallery;
            m_savesFilesToGallery   = savesFilesToGallery;
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kMediaServices;
        }

        #endregion
    }
}
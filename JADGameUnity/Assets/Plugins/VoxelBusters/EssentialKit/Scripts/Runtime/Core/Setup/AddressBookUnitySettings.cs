using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class AddressBookUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Fields

        [SerializeField]
        [Tooltip("The default image used for contact.")]
        private         Texture2D           m_defaultImage;

        #endregion

        #region Properties

        public Texture2D DefaultImage
        {
            get
            {
                return m_defaultImage;
            }
        }

        #endregion

        #region Constructors

        public AddressBookUnitySettings(bool enabled = true) : base(enabled)
        { 
            // set properties
            m_defaultImage      = null;
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kAddressBook;
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class SharingServicesUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Constructors

        public SharingServicesUnitySettings(bool enabled = true)
            : base(enabled)
        { }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.KSharingServices;
        }

        #endregion
    }
}
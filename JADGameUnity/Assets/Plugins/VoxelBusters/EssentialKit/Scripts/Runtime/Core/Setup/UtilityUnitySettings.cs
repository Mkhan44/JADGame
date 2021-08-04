using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.ExtrasCore
{
    public class UtilityUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Constructors

        public UtilityUnitySettings(bool enabled = true)
            : base(enabled)
        { }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return "Utility";
        }

        #endregion
    }
}
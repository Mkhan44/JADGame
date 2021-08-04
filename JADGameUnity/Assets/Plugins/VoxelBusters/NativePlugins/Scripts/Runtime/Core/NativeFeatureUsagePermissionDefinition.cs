using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [Serializable]
    public class NativeFeatureUsagePermissionDefinition
    {
        #region Fields

        [SerializeField]
        private     string                      m_description;

        [SerializeField]
        private     NativePlatformConstantSet   m_descriptionOverrides;

        #endregion

        #region Constructors

        public NativeFeatureUsagePermissionDefinition(string description = null, NativePlatformConstantSet descriptionOverrides = null)
        {
            // set properties
            m_description               = description;     
            m_descriptionOverrides      = descriptionOverrides ?? new NativePlatformConstantSet();
        }

        #endregion

        #region Public methods

        public string GetDescriptionForActivePlatform()
        {
            return GetDescription(PlatformMappingServices.GetActivePlatform());
        }

        public string GetDescription(NativePlatform platform)
        {
            // check whether overrides are available
            string  targetValue     = m_descriptionOverrides.GetConstantForPlatform(platform, m_description);
            if (targetValue == null)
            {
                DebugLogger.LogError("Permission is not defined!");
                return null;
            }
            else
            {
                return FormatDescription(targetValue, platform);
            }
        }

        #endregion

        #region Private methods

        private string FormatDescription(string description, NativePlatform targetPlatform)
        {
            switch (targetPlatform)
            {
                case NativePlatform.iOS:
                case NativePlatform.tvOS:
                    return description.Replace("$productName", "$(PRODUCT_NAME)");
                case NativePlatform.Android:
                    return description.Replace("$productName", "%app_name%");
                default:
                    return description;
            }
        }

        #endregion
    }
}
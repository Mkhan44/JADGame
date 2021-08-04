using System;
using System.Collections;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public class NativeFeaturePackageConfiguration
    {
        #region Properties

        public NativeFeaturePackageDefinition[] Packages
        {
            get;
            private set;
        }

        public NativeFeaturePackageDefinition SimulatorPackage
        {
            get;
            private set;
        }

        public NativeFeaturePackageDefinition FallbackPackage
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public NativeFeaturePackageConfiguration(NativeFeaturePackageDefinition[] packages, NativeFeaturePackageDefinition simulatorPackage = null, NativeFeaturePackageDefinition fallbackPackage = null)
        {
            // set properties
            Packages            = packages;
            SimulatorPackage    = simulatorPackage;
            FallbackPackage     = fallbackPackage;
        }

        #endregion

        #region Public methods

        public NativeFeaturePackageDefinition GetPackageForPlatform(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.LinuxEditor:
                    return SimulatorPackage;

                default:
                    return Array.Find(Packages, (item) => item.Platform == platform);
            }
        }

        #endregion
    }
}
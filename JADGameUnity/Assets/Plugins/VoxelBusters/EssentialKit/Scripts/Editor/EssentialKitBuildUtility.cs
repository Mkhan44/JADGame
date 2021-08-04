using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;

namespace VoxelBusters.EssentialKit.Editor
{
    public static class EssentialKitBuildUtility 
    {
        #region Stripping files

        public static void CreateStrippingFile(BuildTarget buildTarget)
        {
            // check whether plugin is configured
            if (!EssentialKitSettingsEditorUtility.SettingsExists)
            {
                EssentialKitSettingsEditorUtility.ShowSettingsNotFoundErrorDialog();
                return;
            }

            // generate stripping file
            var     settings            = EssentialKitSettingsEditorUtility.DefaultSettings;
            var     strippingWriter     = new LinkerStrippingSettingsWriter(path: Constants.kPluginFolderStructure + "link.xml");
            var     availableFeatures   = settings.GetAvailableFeatureNames();
            var     usedFeatures        = settings.GetUsedFeatureNames();
            if (IsReleaseBuild() && usedFeatures.Length > 0)
            {
                var     platform        = EditorApplicationUtility.ConvertBuildTargetToRuntimePlatform(buildTarget);
                foreach (string feature in availableFeatures)
                {
                    var     featureConfiguration    = ImplementationBlueprint.GetFeatureConfiguration(feature);
                    if (!Array.Exists(usedFeatures, (item) => string.Equals(feature, item)))
                    {
                        continue;
                    }

                    var     packageConfiguration    = featureConfiguration.GetPackageForPlatform(platform);
                    if (packageConfiguration == null)
                    {
                        DebugLogger.LogWarning("Configuration not found for feature: " +  feature);
                        var     fallbackConfiguration   = featureConfiguration.FallbackPackage;
                        strippingWriter.AddRequiredType(fallbackConfiguration.Assembly, fallbackConfiguration.NativeInterfaceType);
                    }
                    else
                    {
                        strippingWriter.AddRequiredNamespace(packageConfiguration.Assembly, packageConfiguration.Namespace);
                    }
                }

                // add support modules
                var     extrasConfiguration         = ImplementationBlueprint.Extras;
                var     extrasPackageConfiguration  = extrasConfiguration.GetPackageForPlatform(platform);
                if (extrasPackageConfiguration != null)
                {
                    strippingWriter.AddRequiredNamespace(extrasPackageConfiguration.Assembly, extrasPackageConfiguration.Namespace);
                }
                else
                {
                    var     fallbackConfiguration   = extrasConfiguration.FallbackPackage;
                    strippingWriter.AddRequiredType(fallbackConfiguration.Assembly, fallbackConfiguration.NativeInterfaceType);
                }
            }
            strippingWriter.WriteToFile();
        }

        public static bool IsReleaseBuild()
        {
            var     firstPackage    = ImplementationBlueprint.AddressBook.GetPackageForPlatform(RuntimePlatform.OSXEditor);
            return !(firstPackage == null || ReflectionUtility.FindAssemblyWithName(firstPackage.Assembly) == null);
        }

        #endregion
    }
}
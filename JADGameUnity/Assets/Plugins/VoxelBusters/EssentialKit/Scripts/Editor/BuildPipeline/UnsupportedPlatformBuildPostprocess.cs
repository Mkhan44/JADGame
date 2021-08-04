using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;

namespace VoxelBusters.EssentialKit.Editor.Build
{
    [InitializeOnLoad]
    public static class UnsupportedPlatformBuildPostprocess
    {
        #region Constructors

        static UnsupportedPlatformBuildPostprocess()
        {
            // unregister from events
            BuildProcessObserver.OnPreprocessUnityBuild     -= OnPreprocessBuild;

            // register for events
            BuildProcessObserver.OnPreprocessUnityBuild     += OnPreprocessBuild;
        }

        #endregion

        #region Static methods

        public static void OnPreprocessBuild(BuildInfo build)
        {
            // check whether plugin is configured
            if (!EssentialKitSettingsEditorUtility.SettingsExists || IsBuildTargetSupported(build.Target))
            {
                return;
            }

            DebugLogger.Log("[EssentialKit] Initiating pre-build task execution.");

            // execute tasks
            EssentialKitBuildUtility.CreateStrippingFile(build.Target);

            DebugLogger.Log("[EssentialKit] Successfully completed pre-build task execution.");
        }

        private static bool IsBuildTargetSupported(BuildTarget buildTarget)
        {
            return ((BuildTarget.iOS == buildTarget) || (BuildTarget.tvOS == buildTarget) || (BuildTarget.tvOS == buildTarget));
        }

        #endregion
    }
}
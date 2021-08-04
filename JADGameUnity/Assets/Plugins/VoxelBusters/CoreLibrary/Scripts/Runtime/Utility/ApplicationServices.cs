using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class ApplicationServices
    {
        #region Static fields

        private     static      float       s_originalTimeScale     = Time.timeScale;

        #endregion

        #region Static methods

        public static void SetApplicationPaused(bool pause)
        {
            if (pause)
            {
                // cache original value
                s_originalTimeScale = Time.timeScale;

                // set new value
                Time.timeScale      = 0f;
            }
            else
            {
                Time.timeScale      = s_originalTimeScale;
            }
        }

        public static RuntimePlatform GetActiveOrSimulationPlatform()
        {
#if UNITY_EDITOR
            return GetSimulationPlatform(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
#else
            return Application.platform;
#endif
        }

        public static bool IsPlayingOrSimulatingMobilePlatform()
        {
#if UNITY_EDITOR
            var     platform    = GetActiveOrSimulationPlatform();
            return (platform == RuntimePlatform.Android) || (platform == RuntimePlatform.IPhonePlayer);
#else
            return Application.isMobilePlatform;
#endif
        }

#if UNITY_EDITOR
        private static RuntimePlatform GetSimulationPlatform(UnityEditor.BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case UnityEditor.BuildTarget.iOS:
                    return RuntimePlatform.IPhonePlayer;

                case UnityEditor.BuildTarget.tvOS:
                    return RuntimePlatform.tvOS;

                case UnityEditor.BuildTarget.Android:
                    return RuntimePlatform.Android;

                case UnityEditor.BuildTarget.StandaloneOSX:
                    return RuntimePlatform.OSXPlayer;

                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return RuntimePlatform.WindowsPlayer;

                case UnityEditor.BuildTarget.WebGL:
                    return RuntimePlatform.WebGLPlayer;

                default:
                    return Application.platform;
            }
        }
#endif

        #endregion
    }
}
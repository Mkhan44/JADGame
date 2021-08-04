using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build
{
    public class BuildProcessObserver : IActiveBuildTargetChanged,
#if !UNITY_2018_1_OR_NEWER
    IPreprocessBuild, IPostprocessBuild
#else
    IPreprocessBuildWithReport, IPostprocessBuildWithReport
#endif
    {
        #region Delegates

        public delegate void BuildTargetChangeCallback(BuildTarget previousTarget, BuildTarget newTarget);

        public delegate void ProcessBuildCallback(BuildInfo buildInfo);

        #endregion

        #region Static events

        public static event BuildTargetChangeCallback OnBuildTargetChange;

        public static event ProcessBuildCallback OnPreprocessUnityBuild;

        public static event ProcessBuildCallback OnPostprocessUnityBuild;
        
        #endregion

        #region IActiveBuildTargetChanged implementation

        public int callbackOrder
        {
            get
            {
                return 99;
            }
        }

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            // notify listeners
            if (OnBuildTargetChange != null)
            {
                OnBuildTargetChange(previousTarget, newTarget);
            }
        }

        #endregion

        #region IPreprocessBuild implementation

#if !UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            // notify listeners
            if (OnPreprocessUnityBuild != null)
            {
                OnPreprocessUnityBuild(new BuildInfo() { Target = target, Path = path });
            }
        }
#else
        public void OnPreprocessBuild(BuildReport report)
        {
            // notify listeners
            if (OnPreprocessUnityBuild != null)
            {
                OnPreprocessUnityBuild(new BuildInfo() { Target = report.summary.platform, Path = report.summary.outputPath });
            }
        }
#endif

        #endregion

        #region IPostprocessBuild implementation

#if !UNITY_2018_1_OR_NEWER
        public void OnPostprocessBuild(BuildTarget target, string path)
        {
            // notify listeners
            if (OnPostprocessUnityBuild != null)
            {
                OnPostprocessUnityBuild(new BuildInfo() { Target = target, Path = path });
            }
        }
#else
        public void OnPostprocessBuild(BuildReport report)
        {
            // notify listeners
            if (OnPostprocessUnityBuild != null)
            {
                OnPostprocessUnityBuild(new BuildInfo() { Target = report.summary.platform, Path = report.summary.outputPath });
            }
        }
#endif

        #endregion
    }
}
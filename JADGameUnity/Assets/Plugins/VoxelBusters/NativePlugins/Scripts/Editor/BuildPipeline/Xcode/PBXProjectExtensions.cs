#if UNITY_IOS || UNITY_TVOS
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.iOS.Xcode;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public static class PBXProjectExtensions
    {
        public static void AddHeaderSearchPath(this PBXProject project, string targetGuid, string headerSearchPath)
        {
            AddBuildProperty(project, targetGuid, PBXBuildConfigurationKey.kHeaderSearchPaths, GetPlatformCompatiblePath(headerSearchPath));
        }

        public static void AddLibrarySearchPath(this PBXProject project, string targetGuid, string librarySearchPath)
        {
            AddBuildProperty(project, targetGuid, PBXBuildConfigurationKey.kLibrarySearchPaths, GetPlatformCompatiblePath(librarySearchPath), false, false);
        }

        public static void AddFrameworkSearchPath(this PBXProject project, string targetGuid, string frameworkSearchPath)
        {
            AddBuildProperty(project, targetGuid, PBXBuildConfigurationKey.kFrameworkSearchPaths, GetPlatformCompatiblePath(frameworkSearchPath), false, false);
        }

        private static void AddBuildProperty(PBXProject project, string targetGuid, string key, string value, bool recursive = true, bool quoted = true)
        {
            // add recursive symbol for folder paths
            if (recursive)
            {
                value   = value.TrimEnd('/') + "/**";
            }

            // xcode uses space as the delimiter here, so if there's a space in the filename, we must add quote
            if (quoted)
            {
                if (value.EndsWith("/**"))
                {
                    value = "\"" + value.Replace("/**", "/\"/**");
                }
                else
                {
                    value = "\"" + value + "\"";
                }
            }

            project.AddBuildProperty(targetGuid, key, value);
        }

        private static string GetPlatformCompatiblePath(string path)
        {
            path = path.Replace("\\", "/");
            return path;
        }
    }
}
#endif
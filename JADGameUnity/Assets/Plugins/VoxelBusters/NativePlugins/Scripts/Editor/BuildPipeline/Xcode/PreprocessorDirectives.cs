#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    public static class PreprocessorDirectives
    {
        #region Constants

        private     const      string      kConfigFilePath      = "Assets/Plugins/VoxelBusters/NativePlugins/Plugins/iOS/NPConfig.h";

        #endregion

        #region Static methods

        public static void WriteMacros(string[] macros)
        {
            // find marker definition
            string  content     = IOServices.ReadFile(kConfigFilePath);
            var     lines       = content.Split('\n');
            int     pragmaIndex = Array.FindIndex(lines, (item) => item.StartsWith("#pragma", StringComparison.InvariantCultureIgnoreCase));
            if (pragmaIndex == -1)
            {
                DebugLogger.LogError("Unknown error!");
                return;
            }

            // copy contents existing before marker definition
            var     newLines    = new List<string>();
            for (int iter = 0; iter <= pragmaIndex; iter++)
            {
                newLines.Add(lines[iter]);
            }
            newLines.Add("");

            // add specified macro symbols
            for (int uIter = 0; uIter < macros.Length; uIter++)
            {
                var     definition  = string.Format("#define {0} 1", macros[uIter]);
                newLines.Add(definition);
            }

            // write new contents to file
            IOServices.CreateFile(kConfigFilePath, string.Join("\n", newLines.ToArray()));
        }

        #endregion
    }
}
#endif
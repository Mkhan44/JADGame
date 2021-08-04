using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor
{
    public static class AssetDatabaseUtility
    {
        #region Resources methods

        public static void CreateFolder(string folder)
        {
            string[]    pathComponents  = folder.Split('/');

            string      currentPath     = string.Empty;
            for (int iter = 0; iter < pathComponents.Length; iter++)
            {
                string  component       = pathComponents[iter]; 
                string  newPath         = Path.Combine(currentPath, component);
                if (!AssetDatabase.IsValidFolder(newPath))
                {
                    AssetDatabase.CreateFolder(currentPath, component);
                }

                // update path
                currentPath = newPath;
            }
        }

        public static void CreateAssetAtPath(Object asset, string assetPath)
        {
            // create container folder
            string      folderFullPath  = assetPath.Substring(0, assetPath.LastIndexOf('/'));
            CreateFolder(folderFullPath);

            // create asset
            AssetDatabase.CreateAsset(asset, assetPath);
        }

        public static T LoadAssetAtPath<T>(string assetPath) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        #endregion
    }
}
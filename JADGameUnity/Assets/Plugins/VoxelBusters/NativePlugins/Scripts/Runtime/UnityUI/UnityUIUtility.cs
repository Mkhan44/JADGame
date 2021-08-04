using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.UnityUI
{
    public static class UnityUIUtility
    {
        #region Constants

        private const   string      kResourcesFullPath                          = "Assets/Plugins/VoxelBusters/NativePlugins/PackageResources/";

        public  const   string      kDefaultUnityUIRendererPrefabFullPath       = kResourcesFullPath + "UnityUIRenderer.prefab";

        public  const   string      kDefaultUnityUIAlertDialogPrefabFullPath    = kResourcesFullPath + "UnityUIAlertDialog.prefab";

        #endregion
    }
}
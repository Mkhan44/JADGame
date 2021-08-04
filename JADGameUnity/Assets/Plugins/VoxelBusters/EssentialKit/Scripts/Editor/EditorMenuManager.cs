using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;
using VoxelBusters.EssentialKit.Editor.Android;

namespace VoxelBusters.EssentialKit.Editor
{
    public static class EditorMenuManager
    {
        #region Constants

        private const string kMenuItemPath = "Window/Voxel Busters/Native Plugins/Essential Kit";

        #endregion

        #region Menu items

        [MenuItem(kMenuItemPath + "/Open Settings")]
        public static void OpenSettings()
        {
            /*EssentialKitSettings settings = EssentialKitSettingsEditorUtility.DefaultSettings;

            // save
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            // ping this object
            Selection.activeObject = settings;
            EditorGUIUtility.PingObject(settings);*/

            SettingsService.OpenProjectSettings("Project/Voxel Busters/Essential Kit");
        }

        [MenuItem(kMenuItemPath + "/Import Settings")]
        public static void ImportSettings()
        {
            var     settings        = UpgradeUtility.ImportSettings();

            // save
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            // ping this object
            Selection.activeObject  = settings;
            EditorGUIUtility.PingObject(settings);
        }

        [MenuItem(kMenuItemPath + "/Open Simulator Database")]
        public static void OpenSimulatorDatabase()
        {
            var     database        = SimulatorDatabase.Instance;

            // save
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();

            // ping this object
            Selection.activeObject  = database;
            EditorGUIUtility.PingObject(database);
        }

        [MenuItem(kMenuItemPath + "/Force Update Library Dependencies")]
        public static void ForceUpdateAndroidLibraryDependencies()
        {
#if UNITY_ANDROID
            AndroidLibraryDependenciesGenerator.CreateLibraryDependencies();
            //GooglePlayServices.PlayServicesResolver.MenuForceResolve();
#endif
        }

        [MenuItem(kMenuItemPath + "/Uninstall")]
        public static void Uninstall()
        {
            UninstallPlugin.Uninstall();
        }

        #endregion
    }
}
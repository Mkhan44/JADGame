using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary.Editor;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary.NativePlugins.UnityUI;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.Editor
{
    public static class EssentialKitSettingsEditorUtility
    {
        #region Static fields

        private     static      EssentialKitSettings        s_defaultSettings       = null;

        #endregion

        #region Static properties

        public static EssentialKitSettings DefaultSettings
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    s_defaultSettings = LoadDefaultSettings(throwError: false) ?? CreateDefaultSettings();
                }
                return s_defaultSettings;
            }
        }

        public static bool SettingsExists
        {
            get
            {
                if (s_defaultSettings == null)
                {
                    s_defaultSettings   = LoadDefaultSettings();
                }
                return (s_defaultSettings != null);
            }
        }

        #endregion

        #region Static methods

        public static void ShowSettingsNotFoundErrorDialog()
        {
            EditorUtility.DisplayDialog(
                title: "Error",
                message: "Native plugins is not configured. Please select plugin settings file from menu and configure it according to your preference.",
                ok: "Ok");
        }

        #endregion

        #region Private static methods

        private static EssentialKitSettings CreateDefaultSettings()
        {
            string  filePath    = Constants.kPluginSettingsFileFullPath;
            var     settings    = ScriptableObject.CreateInstance<EssentialKitSettings>();
            SetDefaultProperties(settings);

            // create file
            AssetDatabaseUtility.CreateAssetAtPath(settings, filePath);
            AssetDatabase.Refresh();

            return settings;
        }

        private static EssentialKitSettings LoadDefaultSettings(bool throwError = true)
        {
            string  filePath    = Constants.kPluginSettingsFileFullPath;
            var     settings    = AssetDatabaseUtility.LoadAssetAtPath<EssentialKitSettings>(filePath);
            if (settings)
            {
                SetDefaultProperties(settings);
                return settings;
            }

            if (throwError)
            {
                throw Diagnostics.PluginNotConfiguredException();
            }

            return null;
        }

        private static void SetDefaultProperties(EssentialKitSettings settings)
        {
            // set properties
            var     uiCollection        = settings.NativeUISettings.CustomUICollection;
            if (uiCollection.RendererPrefab == null)
            {
                uiCollection.RendererPrefab         = AssetDatabaseUtility.LoadAssetAtPath<UnityUIRenderer>(UnityUIUtility.kDefaultUnityUIRendererPrefabFullPath);
            }
            if (uiCollection.AlertDialogPrefab == null)
            {
                var     prefabObject    = AssetDatabaseUtility.LoadAssetAtPath<GameObject>(UnityUIUtility.kDefaultUnityUIAlertDialogPrefabFullPath);
                if (prefabObject)
                {
                    uiCollection.AlertDialogPrefab  = prefabObject.GetComponent<UnityUIAlertDialog>();
                }
            }
        }

        #endregion
    }
}
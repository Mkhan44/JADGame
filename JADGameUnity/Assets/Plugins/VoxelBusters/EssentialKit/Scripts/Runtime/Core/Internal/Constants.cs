using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    internal static class Constants
    {
        #region Constants

        // file paths
        public const string kPluginFolderStructure                      = "Assets/Plugins/VoxelBusters/EssentialKit/";

        public const string kPluginEditorSourcePath                     = kPluginFolderStructure + "Scripts/Editor/";
        public const string kPluginAndroidEditorSourcePath              = kPluginEditorSourcePath + "Android/";

        public const string kPluginAndroidProjectFolderName             = "com.voxelbusters.essentialkit.androidlib";
        public const string kPluginAndroidProjectPath                   = "Assets/Plugins/Android/" + kPluginAndroidProjectFolderName  + "/";
        public const string kPluginAndroidProjectAllLibsPath            = kPluginAndroidProjectPath + "all_libs/";
        public const string kPluginAndroidProjectLibsPath               = kPluginAndroidProjectPath + "libs/";
        public const string kPluginAndroidProjectResPath                = kPluginAndroidProjectPath + "res/";
        public const string kPluginAndroidProjectResDrawablePath        = kPluginAndroidProjectResPath + "drawable/";
        public const string kPluginAndroidProjectResValuesPath          = kPluginAndroidProjectResPath + "values/";
        public const string kPluginAndroidProjectResValuesStringsPath   = kPluginAndroidProjectResValuesPath + "essential_kit_strings.xml";

        public const string kPluginiOSSourcePath                        = kPluginFolderStructure + "Plugins/iOS/";

        public const string kPluginCodebasePath                         = kPluginFolderStructure + "Scripts/Runtime/";

        public const string kPluginEditorResourcesFullPath              = kPluginFolderStructure + "EditorResources/";

        public const string kPluginResourcesPath                        = "";
                
        public const string kPluginResourcesFullPath                    = kPluginFolderStructure + "Resources/";

        // file names
        public const string kPluginSettingsFileName                     = "EssentialKitSettings.asset";

        public const string kPluginSettingsFileNameWithoutExtension     = "EssentialKitSettings";

        public const string kPluginSettingsFilePath                     = kPluginResourcesPath + kPluginSettingsFileName;

        public const string kPluginSettingsFileFullPath                 = kPluginResourcesFullPath + kPluginSettingsFileName;
                
        // product information
        public const string kProductDisplayName                         = "Cross Platform Essential Kit";

        public const string kProductVersion                             = "Version 2.0.4p1";
         
        public const string kProductCopyrights                          = "Copyright © 2021 Voxel Busters Interactive LLP.";

        #endregion
    }
}
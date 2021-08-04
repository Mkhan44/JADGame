#if UNITY_ANDROID && UNITY_2018_4_OR_NEWER
using System.Linq;
using System.Text;


using UnityEditor.Android;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace VoxelBusters.EssentialKit.Editor.Android
{
    class GradleFileProperties
    {
        public string GradleVersion;
        public string CompileSdkVersion;
        public string TargetSdkVersion;
        public string BuildToolsVersion;

        public override string ToString()
        {
            return string.Format("Gradle Version : {0} \n" +
                "Compile Sdk Version : {1} \n" +
                "Target Sdk Version : {2} \n" +
                "BuildToolsVersion : {3}", GradleVersion, CompileSdkVersion, TargetSdkVersion, BuildToolsVersion);
        }
    }

    class GradlePostProcessor : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostGenerateGradleAndroidProject(string path)
        {
#if UNITY_2019_3_OR_NEWER
                path = path + "/..";
#endif
            //MatchUnityGradleVersion(path);
            //EnableJetifierIfRequired(path);
            GradleFileProperties gradleFileProperties = GetMainGradleFileProperties(path);
            Debug.Log("Main gradle properties : " + gradleFileProperties);

            RegenerateManifestFileIfRequired(path, gradleFileProperties);
            PatchGradleForPackingOptions(path);
        }

        private void MatchUnityGradleVersion(string path)
        {
            string rootGradleVersion = null;
            string rootCompileSdkVersion = null;
            string rootBuildToolsVersion = null;
            string rootTargetSDKVersion = null;

            string[] targetProjectPaths =
            {
                "/com.voxelbusters.essentialkit.androidlib"
            };

            // First read the main build.gradle file
            string[] lines = File.ReadAllLines(path + "/build.gradle");

            foreach (string eachLine in lines)
            {
                // Detect gradle version
                if (HasText(eachLine, "classpath", "tools.build:gradle"))
                {
                    rootGradleVersion = eachLine;
                }
                // Detect compileSdkVersion version
                else if (HasText(eachLine, "compileSdkVersion"))
                {
                    rootCompileSdkVersion = eachLine;
                }
                // Detect buildToolsVersion version
                else if (HasText(eachLine, "buildToolsVersion"))
                {
                    rootBuildToolsVersion = eachLine;
                }
                // Detect targetSdkVersion version
                else if (HasText(eachLine, "targetSdkVersion"))
                {
                    rootTargetSDKVersion = eachLine;
                }
            }


            foreach (string eachProject in targetProjectPaths)
            {
                UpdateGradleFile(path + eachProject, rootGradleVersion, rootCompileSdkVersion, rootBuildToolsVersion, rootTargetSDKVersion);
            }
        }

        private void UpdateGradleFile(string projectPath, string rootGradleVersion, string rootCompileSdkVersion, string rootBuildToolsVersion, string rootTargetSDKVersion)
        {
            string filePath = projectPath + "/build.gradle";

            if(File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                string[] updatedLines = new string[lines.Length];

                for (int i=0; i< lines.Length; i++)
                {
                    string eachLine     = lines[i];
                    string updatedText  = eachLine;

                    // Detect gradle version
                    if (HasText(eachLine, "classpath", "tools.build:gradle"))
                    {
                        updatedText = rootGradleVersion;
                    }
                    // Detect compileSdkVersion version
                    else if (HasText(eachLine, "compileSdkVersion"))
                    {
                        updatedText = rootCompileSdkVersion;
                    }
                    // Detect buildToolsVersion version
                    else if (HasText(eachLine, "buildToolsVersion"))
                    {
                        updatedText = rootBuildToolsVersion;
                    }
                    // Detect targetSdkVersion version
                    else if (HasText(eachLine, "targetSdkVersion"))
                    {
                        updatedText = rootTargetSDKVersion;
                    }

                    updatedLines[i] = updatedText; 
                }

                File.WriteAllLines(filePath, updatedLines);
            }
        }

        private bool HasText(string inputString, string startSearchString, string additionalStringToSearch = null)
        {
            string trimmedText = inputString.Trim();

            if(trimmedText.StartsWith(startSearchString, System.StringComparison.InvariantCulture))
            {
                if(additionalStringToSearch != null)
                {
                    return trimmedText.Contains(additionalStringToSearch);
                }

                return true;
            }

            return false;
        }

        private GradleFileProperties GetMainGradleFileProperties(string path)
        {
            GradleFileProperties properties = new GradleFileProperties();
            string[] lines = File.ReadAllLines(path + "/build.gradle");

            foreach (string eachLine in lines)
            {
                // Detect gradle version
                if (HasText(eachLine, "classpath", "tools.build:gradle"))
                {
                    string[] components = eachLine.Trim().Split(':');
                    if(components.Length > 2)
                    {
                        properties.GradleVersion = components[2].Replace("'", "").Replace("\"", "");
                    }
                }
                // Detect compileSdkVersion version
                else if (HasText(eachLine, "compileSdkVersion"))
                {
                    string[] components = eachLine.Trim().Split(' ');
                    if (components.Length > 1)
                    {
                        properties.CompileSdkVersion = components[components.Length-1];
                    }
                        
                }
                // Detect buildToolsVersion version
                else if (HasText(eachLine, "buildToolsVersion"))
                {
                    string[] components = eachLine.Trim().Split(' ');
                    if (components.Length > 1)
                    {
                        properties.BuildToolsVersion = components[components.Length - 1].Replace("'", "").Replace("\"", "");
                    }
                }
                // Detect targetSdkVersion version
                else if (HasText(eachLine, "targetSdkVersion"))
                {
                    string[] components = eachLine.Trim().Split(' ');
                    if (components.Length > 1)
                    {
                        properties.TargetSdkVersion = components[components.Length - 1];
                    }
                }
            }

            return properties;
        }

        private void RegenerateManifestFileIfRequired(string path, GradleFileProperties properties)
        {
            if(!string.IsNullOrEmpty(properties.TargetSdkVersion))
            {
                int versionNumber;

                if(int.TryParse(properties.TargetSdkVersion, out versionNumber))
                {
                    if(versionNumber >= 30) //Starting from Android API 30, we need queries entry to query extenral packages.
                    {
                        // check whether plugin is configured
                        if (!EssentialKitSettingsEditorUtility.SettingsExists)
                        {
                            return;
                        }

                        EssentialKitSettings settings = EssentialKitSettingsEditorUtility.DefaultSettings;

                        string manifestPath = string.Format("{0}/{1}/{2}", path, Constants.kPluginAndroidProjectFolderName, "AndroidManifest.xml");
                        AndroidManifestGenerator.GenerateManifest(settings, true, manifestPath);

                        //https://stackoverflow.com/questions/62969917/how-do-i-fix-unexpected-element-queries-found-in-manifest
                        Debug.LogWarning("You are using TargetApi >= 30. Make sure your Unity version supports any of the minimum gradle versions " +
                            "from 3.3.3, 3.4.3, 3.5.4, 3.6.4, 4.0.1. If not, try setting a lower Target Api or set a custom gradle version in unity");
                    }
                }

            }
        }

        private void EnableJetifierIfRequired(string path)
        {
            string[] files = Directory.GetFiles(UnityEngine.Application.dataPath + "/Plugins/Android" , "androidx.*.aar");

            if(files.Length > 0)
            {
                string gradlePropertiesPath = path + "/gradle.properties";

                string[] lines = File.ReadAllLines(gradlePropertiesPath);

                // Need jetifier patch process
                bool hasAndroidXProperty = lines.Any(text => text.Contains("android.useAndroidX"));
                bool hasJetifierProperty = lines.Any(text => text.Contains("android.enableJetifier"));

                StringBuilder builder = new StringBuilder();

                foreach(string each in lines)
                {
                    builder.AppendLine(each);
                }

                if (!hasAndroidXProperty)
                {
                    builder.AppendLine("android.useAndroidX=true");
                }

                if (!hasJetifierProperty)
                {
                    builder.AppendLine("android.enableJetifier=true");
                }

                File.WriteAllText(gradlePropertiesPath, builder.ToString());
            }
        }

        private void PatchGradleForPackingOptions(string path)
        {
            Debug.Log("Patching gradle for packing options at path : " + path);

            // First read the main build.gradle file
            string gradleFilePath = path + Path.DirectorySeparatorChar + "build.gradle";
            List<string> lines = File.ReadAllLines(gradleFilePath).ToList();

            int index = -1;
            for(int i=0; i<lines.Count; i++)
            {
                if (HasText(lines[i], "apply plugin: 'com.android.application'"))
                {
                    index = i;
                    break;
                }
            }

            if(index != -1)
            {
                string textToInsert = @"android {
     packagingOptions {
         exclude 'META-INF/proguard/androidx-annotations.pro'
     }
 }";
                
                lines.Insert(index + 1, textToInsert);

                File.WriteAllLines(gradleFilePath, lines);
            }
        }
    }
}
#endif
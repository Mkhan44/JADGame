#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins.Build.Xcode
{
    [InitializeOnLoad]
    public static class NativeFeatureExportProcess
    {
        #region Constants

        private static readonly string[]    ignoreFileExtensions    = new string[]
        {
            ".meta", 
            ".pdf",
            ".DS_Store",
            ".mdown",
            ".asset",
            ".cs",
        };

        private static readonly string      kPluginPath              = "VoxelBusters/NativePlugins/";

        #endregion

        #region Fields

        private     static  NativeFeatureExporterSettings[]     s_activeExporters       = null;

        private     static  string                              s_projectPath           = null;

        private     static  List<string>                        s_librarySearchPaths    = null;

        private     static  List<string>                        s_frameworkSearchPaths  = null;
        
        #endregion

        #region Constructors

        static NativeFeatureExportProcess()
        {
            // unregister from events
            BuildProcessObserver.OnPostprocessUnityBuild    -= OnPostprocessBuild;

            // register for events
            BuildProcessObserver.OnPostprocessUnityBuild    += OnPostprocessBuild;
        }

        #endregion

        #region Callback methods

        public static void OnPostprocessBuild(BuildInfo buildInfo)
        {
            if (buildInfo.Target == BuildTarget.iOS ||
                buildInfo.Target == BuildTarget.tvOS)
            {
                // set properties
                s_activeExporters       = NativeFeatureExporterSettings.FindAllExporters(includeInactive: !NativeFeatureUnitySettingsBase.CanToggleFeatureUsageState());
                s_projectPath           = buildInfo.Path;
                s_librarySearchPaths    = new List<string>();
                s_frameworkSearchPaths  = new List<string>();

                // execute tasks
                PrepareForExport();
                UpdateMacroDefinitions();
                UpdatePBXProject();

                // reset properties
                s_activeExporters       = null;
                s_projectPath           = null;
                s_librarySearchPaths    = null;
                s_frameworkSearchPaths  = null;
            }
        }

        #endregion

        #region Private static methods

        private static void PrepareForExport()
        {
            string  pluginExportPath    = Path.Combine(s_projectPath, kPluginPath);
            IOServices.DeleteDirectory(pluginExportPath);
        }

        private static string GetProjectTarget(PBXProject project)
        {
#if UNITY_2019_3_OR_NEWER
                return project.GetUnityFrameworkTargetGuid();
#else
                return project.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif
        }

        private static void UpdatePBXProject()
        {
            DebugLogger.Log("[XcodeBuildProcess] Linking native files.");

            // open project file for editing
            string  projectFilePath     = PBXProject.GetPBXProjectPath(s_projectPath);
            var     project             = new PBXProject();
            project.ReadFromFile(projectFilePath);
            string  targetGuid          = GetProjectTarget(project);

            // read exporter settings for adding native files 
            foreach (var featureExporter in s_activeExporters)
            {
                Debug.Log("Is Feature enabled : " + featureExporter.name + " " + featureExporter.IsEnabled);
                string  exporterFilePath    = Path.GetFullPath(AssetDatabase.GetAssetPath(featureExporter));
                string  exporterFolder      = Path.GetDirectoryName(exporterFilePath);
                var     iOSSettings         = featureExporter.IosProperties;

                string  exporterName        = featureExporter.name;
                string  exporterGroup       = kPluginPath + exporterName + "/";

                // add files
                foreach (var fileInfo in iOSSettings.Files)
                {
                    AddFileToProject(project, fileInfo.AbsoultePath, targetGuid, exporterGroup, fileInfo.CompileFlags);
                }

                // add folder
                foreach (var folderInfo in iOSSettings.Folders)
                {
                    AddFolderToProject(project, folderInfo.AbsoultePath, targetGuid, exporterGroup, folderInfo.CompileFlags);
                }

                // add headerpaths
                foreach (var pathInfo in iOSSettings.HeaderPaths)
                {
                    string  destinationPath = GetFilePathInProject(pathInfo.AbsoultePath, exporterFolder, exporterGroup);
                    string  formattedPath   = FormatFilePathInProject(destinationPath);
                    project.AddHeaderSearchPath(targetGuid, formattedPath);
                }

                // add frameworks
                foreach (var framework in iOSSettings.Frameworks)
                {
                    project.AddFrameworkToProject(targetGuid, framework.Name, framework.IsWeak);
                }
            }

            // add header search paths
            foreach (string path in s_librarySearchPaths)
            {
                project.AddLibrarySearchPath(targetGuid, FormatFilePathInProject(path));
            }

            // add framework search paths
            foreach (string path in s_frameworkSearchPaths)
            {
                project.AddFrameworkSearchPath(targetGuid, FormatFilePathInProject(path));
            }

            // apply changes
            File.WriteAllText(projectFilePath, project.WriteToString());

            // add entitlements
            AddEntitlements(project);
        }  

        private static void AddEntitlements(PBXProject project)
        {
            // create capability manager
            string  projectFilePath     = PBXProject.GetPBXProjectPath(s_projectPath);
#if UNITY_2019_3_OR_NEWER
            var     capabilityManager   = new ProjectCapabilityManager(projectFilePath, "ios.entitlements", null, project.GetUnityMainTargetGuid());
#else
            var     capabilityManager   = new ProjectCapabilityManager(projectFilePath, "ios.entitlements", PBXProject.GetUnityTargetName());
#endif

            // add required capability
            foreach (var featureExporter in s_activeExporters)
            {
                if (!featureExporter.IsEnabled)
                    continue;

                foreach (var capability in featureExporter.IosProperties.Capabilities)
                {
                    switch (capability.Type)
                    {
                        case PBXCapabilityType.GameCenter:
                            capabilityManager.AddGameCenter();
                            break;

                        case PBXCapabilityType.iCloud:
                            capabilityManager.AddiCloud(enableKeyValueStorage: true, enableiCloudDocument: false, enablecloudKit: false, addDefaultContainers: false, customContainers: null);
                            break;

                        case PBXCapabilityType.InAppPurchase:
                            capabilityManager.AddInAppPurchase();
                            break;

                        case PBXCapabilityType.PushNotifications:
                            capabilityManager.AddPushNotifications(Debug.isDebugBuild);
                            capabilityManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
                            break;

                        case PBXCapabilityType.AssociatedDomains:
                            var     associatedDomainsEntitlement    = capability.AssociatedDomainsEntitlement;
                            capabilityManager.AddAssociatedDomains(domains: associatedDomainsEntitlement.Domains);
                            break;

                        default:
                            throw VBException.SwitchCaseNotImplemented(capability.Type);
                    }
                }
            }

            // save changes
            capabilityManager.WriteToFile();
        }

        private static void UpdateMacroDefinitions()
        {
            var     requiredMacros  = new List<string>();
            foreach (var featureExporter in s_activeExporters)
            {
                var     macros      = featureExporter.IosProperties.Macros;
                if (macros == null || macros.Length == 0)
                {
                    continue;
                }
                foreach (var entry in macros)
                {
                    if (!requiredMacros.Contains(entry))
                    {
                        requiredMacros.Add(entry);
                    }
                }
            }

            PreprocessorDirectives.WriteMacros(requiredMacros.ToArray());
        }

        private static void AddFileToProject(PBXProject project, string sourceFilePath, string targetGuid, string parentGroup, string[] compileFlags)
        {
            // convert relative path to absolute path
            if (!Path.IsPathRooted(sourceFilePath))
            {
                sourceFilePath          = Path.GetFullPath(sourceFilePath);
            }

            // copy file to the target folder
            string  fileName            = Path.GetFileName(sourceFilePath);
            string  destinationFolder   = IOServices.CombinePath(s_projectPath, parentGroup);
            string  destinationFilePath = CopyFileToProject(sourceFilePath, destinationFolder);
            DebugLogger.Log(string.Format("[NativePluginsExportManager] Adding file {0} to project.", fileName));

            // add copied file to the project
            string  fileGuid            = project.AddFile(FormatFilePathInProject(destinationFilePath, rooted: false),  parentGroup + fileName);
            project.AddFileToBuildWithFlags(targetGuid, fileGuid, string.Join(" ", compileFlags));

            // add search path project
            string  fileExtension       = Path.GetExtension(destinationFilePath);
            if (string.Equals(fileExtension, ".a", StringComparison.InvariantCultureIgnoreCase))
            {
                CacheLibrarySearchPath(destinationFilePath);
            }
            else if (string.Equals(fileExtension, ".framework", StringComparison.InvariantCultureIgnoreCase))
            {
                CacheFrameworkSearchPath(destinationFilePath);
            }
        }

        private static void AddFolderToProject(PBXProject project, string sourceFolder, string targetGuid, string parentGroup, string[] compileFlags)
        {
            // check whether given folder is valid
            var     sourceFolderInfo    = new DirectoryInfo(sourceFolder);
            if (!sourceFolderInfo.Exists)
            {
                return;
            }

            // add files placed within this folder
            foreach (var fileInfo in FindFiles(sourceFolderInfo))
            {
                AddFileToProject(project, fileInfo.FullName, targetGuid, parentGroup, compileFlags);
            }

            // add folders placed within this folder
            foreach (var subFolderInfo in sourceFolderInfo.GetDirectories())
            {
                string  folderGroup     = parentGroup + subFolderInfo.Name + "/";
                AddFolderToProject(project, subFolderInfo.FullName, targetGuid, folderGroup, compileFlags);
            }
        }

        private static string CopyFileToProject(string filePath, string targetFolder)
        {
#if VB_DEVELOPMENT_MODE
            return filePath;
#else
            // create target folder directory, incase if it doesn't exist
            if (!IOServices.DirectoryExists(targetFolder))
            {
                IOServices.CreateDirectory(targetFolder);
            }

            // copy specified file
            string  fileName        = Path.GetFileName(filePath);
            string  destPath        = Path.Combine(targetFolder, fileName);

            DebugLogger.Log(string.Format("[NativePluginsExportManager] Copying file {0} to {1}.", filePath, destPath));
            FileUtil.CopyFileOrDirectory(filePath, destPath);

            return destPath;
#endif
        }

        private static string GetFilePathInProject(string path, string parentFolder, string parentGroup)
        {
#if VB_DEVELOPMENT_MODE
            return path;
#else
            string  relativePath        = IOServices.GetRelativePath(parentFolder, path);
            string  destinationFolder   = IOServices.CombinePath(s_projectPath, parentGroup);
            return IOServices.CombinePath(destinationFolder, relativePath);
#endif
        }

        private static string FormatFilePathInProject(string path, bool rooted = true)
        {
#if VB_DEVELOPMENT_MODE
            return path;
#else
            if (path.Contains("$(inherited)"))
            {
                return path;
            }

            string  relativePathToProject   = IOServices.GetRelativePath(s_projectPath, path);
            return rooted ? Path.Combine("$(SRCROOT)", relativePathToProject) : relativePathToProject;
#endif
        }

        private static void CacheLibrarySearchPath(string path)
        {
            string  directoryPath   = Path.GetDirectoryName(path);
            if (!s_librarySearchPaths.Contains(directoryPath))
            {
                s_librarySearchPaths.Add(directoryPath);
            }
        }

        private static void CacheFrameworkSearchPath(string path)
        {
            string  directoryPath   = Path.GetDirectoryName(path);
            if (!s_frameworkSearchPaths.Contains(directoryPath))
            {
                s_frameworkSearchPaths.Add(directoryPath);
            }
        }

        private static FileInfo[] FindFiles(DirectoryInfo folder)
        {
            return folder.GetFiles().Where((fileInfo) =>
            {
                string  fileExtension   = fileInfo.Extension;
                return !Array.Exists(ignoreFileExtensions, (ignoreExt) => string.Equals(fileExtension, ignoreExt, StringComparison.InvariantCultureIgnoreCase));
            }).ToArray();
        } 
        #endregion
    }
}
#endif
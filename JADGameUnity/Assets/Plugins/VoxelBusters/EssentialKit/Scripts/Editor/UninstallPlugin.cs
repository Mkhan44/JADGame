using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using VoxelBusters.EssentialKit;

namespace VoxelBusters.EssentialKit.Editor
{
	public class UninstallPlugin
	{
		#region Constants
	
		private const	string	kUninstallAlertTitle	= "Uninstall - Cross Platform Native Plugin 2.0";
		
        private const	string	kUninstallAlertMessage	= "Backup before doing this step to preserve changes done in this plugin. This deletes files only related to Cross Platform Native Plugins 2.0 plugin. Do you want to proceed?";

		private static string[] kPluginFolders	        =	new string[]
		{
            Constants.kPluginCodebasePath,
            Constants.kPluginEditorSourcePath,
            Constants.kPluginiOSSourcePath,
            Constants.kPluginAndroidProjectPath
        };
		
		#endregion	
	
		#region Methods
	
		public static void Uninstall()
		{
			bool _startUninstall = EditorUtility.DisplayDialog(kUninstallAlertTitle, kUninstallAlertMessage, "Uninstall", "Cancel");

			if (_startUninstall)
			{
				foreach (string _eachFolder in kPluginFolders)
				{
                    string _absolutePath = Application.dataPath + "/../" + _eachFolder;
                    FileUtil.DeleteFileOrDirectory(_absolutePath);
                    FileUtil.DeleteFileOrDirectory(_absolutePath + ".meta");
				}
				
				AssetDatabase.Refresh();
				EditorUtility.DisplayDialog("Cross Platform Native Plugins 2.0",
				                            "Uninstall successful!", 
				                            "Ok");
			}
		}
		
		#endregion
	}
}
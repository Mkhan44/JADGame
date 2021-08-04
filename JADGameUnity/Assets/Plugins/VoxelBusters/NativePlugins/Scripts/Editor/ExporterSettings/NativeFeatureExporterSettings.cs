using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
	[Serializable]
	public partial class NativeFeatureExporterSettings : ScriptableObject
	{
		#region Fields

		[SerializeField]
		private     bool			        m_isEnabled			= true;
		
        [SerializeField, FormerlySerializedAs("m_iOSSettings")]
		private	    IosPlatformProperties   m_iosProperties		= new IosPlatformProperties();

		#endregion

		#region Properties

		public bool IsEnabled
		{
			get 
			{ 
				return m_isEnabled; 
			}
			set 
			{ 
				m_isEnabled = value; 
                ChangeInternalFileState(value);
			}
		}

		public IosPlatformProperties IosProperties
        {
			get 
			{ 
				return m_iosProperties; 
			}
			set 
			{ 
				m_iosProperties = value; 
			}
		}

        #endregion

        #region Static methods

        public static NativeFeatureExporterSettings[] FindAllExporters(bool includeInactive = false)
        {
            var     directory   = new DirectoryInfo(Application.dataPath);
            var     files       = directory.GetFiles("*.asset" , SearchOption.AllDirectories);
            var     assetPaths  = Array.ConvertAll(files, (item) =>
            {
                string      filePath    = item.FullName;
                return filePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            });

            // filter assets
            var     exporters   = new List<NativeFeatureExporterSettings>();
            foreach (string path in assetPaths)
            {
                var     scriptableObject        = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                if (scriptableObject is NativeFeatureExporterSettings)
                {
                    // add to list
                    var     nativeFeatureExporter   = (NativeFeatureExporterSettings)scriptableObject;
                    if (includeInactive || nativeFeatureExporter.IsEnabled)
                    {
                        exporters.Add(nativeFeatureExporter);
                    }
                }
            }

            return exporters.ToArray();
        }

        #endregion

        #region Private methods

        private void ChangeInternalFileState(bool active)
        { }

        #endregion
    }
}
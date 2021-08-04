using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
	public partial class NativeFeatureExporterSettings
	{
		[Serializable]
		public class IosPlatformProperties
		{
			#region Fields

			[SerializeField]
			private     List<PBXFile>	            m_files				= new List<PBXFile>();

            [SerializeField]
            private     List<PBXFile>               m_folders           = new List<PBXFile>();

            [SerializeField]
            private     List<PBXFile>               m_headerPaths       = new List<PBXFile>();

			[SerializeField]
			private	    List<PBXFramework>		    m_frameworks		= new List<PBXFramework>();

			[SerializeField]
			private	    List<PBXCapability>         m_capabilities		= new List<PBXCapability>();

            [SerializeField]
            private     List<string>                m_macros            = new List<string>();

			#endregion

			#region Properties

			public PBXFile[] Files
			{
				get 
				{ 
					return m_files.ToArray(); 
				}
				set 
				{ 
                    Assertions.AssertIfArrayIsNull(value, "value");

                    // set new value
					m_files     = new List<PBXFile>(value); 
				}
			}

            public PBXFile[] Folders
            {
                get 
                { 
                    return m_folders.ToArray(); 
                }
                set 
                { 
                    Assertions.AssertIfArrayIsNull(value, "value");

                    // set new value
                    m_folders   = new List<PBXFile>(value); 
                }
            }

            public PBXFile[] HeaderPaths
            {
                get 
                { 
                    return m_headerPaths.ToArray(); 
                }
                set 
                { 
                    Assertions.AssertIfArrayIsNull(value, "value");

                    // set new value
                    m_headerPaths   = new List<PBXFile>(value); 
                }
            }

			public PBXFramework[] Frameworks
			{
				get 
				{ 
					return m_frameworks.ToArray(); 
				}
				set 
				{ 
					Assertions.AssertIfArrayIsNull(value, "value");

                    // set new value
                    m_frameworks    = new List<PBXFramework>(value); 
				}
			}

			public PBXCapability[] Capabilities
			{
				get 
				{ 
					return m_capabilities.ToArray(); 
				}
				set 
				{ 
					Assertions.AssertIfArrayIsNull(value, "value");

                    // set new value
                    m_capabilities = new List<PBXCapability>(value); 
				}
			}

            public string[] Macros
            {
                get
                {
                    return m_macros.ToArray(); 
                }
                set
                {
                    Assertions.AssertIfArrayIsNull(value, "value");

                    // set new value
                    m_macros    = new List<string>(value); 
                }
            }

			#endregion

            #region Public methods

            public void AddCapability(PBXCapability value)
            {
                m_capabilities.Add(value);
            }

            public void RemoveCapability(PBXCapability value)
            {
                m_capabilities.Remove(value);
            }

            public void ClearCapabilities()
            {
                m_capabilities.Clear();
            }

            public void AddMacro(string value)
            {
                m_macros.Add(value);
            }

            public void RemoveMacro(string value)
            {
                m_macros.Remove(value);
            }

            public void ClearMacros()
            {
                m_macros.Clear();
            }

            public void AddFramework(PBXFramework value)
            {
                m_frameworks.Add(value);
            }

            public void RemoveFramework(PBXFramework value)
            {
                m_frameworks.Remove(value);
            }

            #endregion
		}
    }
}
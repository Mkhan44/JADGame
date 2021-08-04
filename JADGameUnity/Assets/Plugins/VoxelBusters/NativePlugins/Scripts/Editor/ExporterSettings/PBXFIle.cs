using System.IO;
using UnityEngine;
using UnityEditor;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
	[System.Serializable]
	public class PBXFile
	{
		#region Fields

		[SerializeField]
		private     Object			m_reference = null;

		[SerializeField]
		private	    string			m_compileFlags;

		#endregion

		#region Properties

		public string RelativePath
		{
			get
			{
                return AssetDatabase.GetAssetPath(m_reference);
            }
		}

        public string AbsoultePath
        {
            get
            {
                return Path.GetFullPath(RelativePath);
            }
        }

		public string[] CompileFlags
		{
			get
			{
				return m_compileFlags.Split(',');
			}
			set
			{
				m_compileFlags = string.Join(",", value);
			}
		}

		#endregion
	}
}
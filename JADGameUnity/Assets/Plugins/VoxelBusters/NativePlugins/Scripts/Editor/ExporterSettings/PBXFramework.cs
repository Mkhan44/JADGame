using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
	[Serializable]
	public class PBXFramework
	{
		#region Fields

		[SerializeField, PBXFrameworkName]
		private			string			m_name		    = string.Empty;
		
        [SerializeField]
		private			bool			m_isWeak	    = false;

		#endregion

		#region Properties

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public bool IsWeak
		{
			get
			{
				return m_isWeak;
			}
			set
			{
				m_isWeak = value;
			}
		}

        #endregion

        #region Constructors

        public PBXFramework(string name, bool isWeak = false)
        {
            // set properties
            m_name      = name;
            m_isWeak    = isWeak;
        }

        #endregion

        #region Base class methods

        public override bool Equals(object obj)
        {
            if (obj is PBXFramework)
            {
                return string.Equals(m_name, ((PBXFramework)obj).m_name);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
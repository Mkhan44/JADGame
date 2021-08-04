using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.Editor.NativePlugins
{
    [Serializable]
    public class PBXAssociatedDomainsEntitlement
    {
        #region Fields
        
        [SerializeField]
        private     string[]        m_domains;

        #endregion

        #region Properties

        public string[] Domains
        {
            get
            {
                return m_domains;
            }
        }

        #endregion

        #region Constructors

        public PBXAssociatedDomainsEntitlement(string[] domains)
        {
            // set properties
            m_domains   = domains;
        }

        #endregion
    }
}
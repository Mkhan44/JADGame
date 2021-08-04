using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class DeepLinkDefinition
    {
        #region Fields
        
        [SerializeField]
        private     string                      m_identifier;

        [SerializeField]
        private     string                      m_serviceType;

        [SerializeField]
        private     string                      m_scheme;

        [SerializeField]
        private     string                      m_host;

        [SerializeField]
        private     string                      m_path;

        #endregion

        #region Properties

        public string Identifier 
        { 
            get { return m_identifier; } 
            private set { m_identifier = value; } 
        }

        public string ServiceType 
        { 
            get { return string.IsNullOrEmpty(m_serviceType) ? "applinks" : m_serviceType; } 
            private set { m_serviceType = value; } 
        }

        public string Scheme 
        { 
            get { return m_scheme; } 
            private set { m_scheme = value; } 
        }

        public string Host 
        { 
            get { return m_host; } 
            private set { m_host = value; } 
        }

        public string Path 
        { 
            get { return m_path; } 
            private set { m_path = value; } 
        }

        #endregion

        #region Constructors
        
        public DeepLinkDefinition(string identifier = null, string serviceType = null, string scheme = null, string host = null, string path = null)
        {
            // set properties
            m_identifier    = identifier ?? "identifier";
            m_serviceType   = serviceType;
            m_scheme        = scheme;
            m_host          = host;
            m_path          = path;
        }

        #endregion
    }
}
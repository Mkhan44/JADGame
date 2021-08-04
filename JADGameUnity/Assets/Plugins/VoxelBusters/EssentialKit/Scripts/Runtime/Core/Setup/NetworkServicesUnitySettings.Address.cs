using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public partial class NetworkServicesUnitySettings
    {
        [Serializable]
        public class Address
        {
            #region Fields

            [SerializeField]
            [Tooltip("IPV4 format address.")]
            private     string      m_ipv4;

            [SerializeField]
            [Tooltip("IPV6 format address.")]
            private     string      m_ipv6;
            
            #endregion

            #region Properties

            public string IpV4
            {
                get
                {
                    return string.IsNullOrEmpty(m_ipv4) ? null : m_ipv4;
                }
            } 

            public string IpV6
            {
                get
                {
                    return string.IsNullOrEmpty(m_ipv6) ? null : m_ipv6;
                }
            }

            #endregion

            #region Constructors

            public Address(string ipv4 = "8.8.8.8", string ipv6 = "0:0:0:0:0:FFFF:0808:0808")
            {
                // set properties
                m_ipv4  = ipv4;
                m_ipv6  = ipv6;
            }

            #endregion
        }
    }
}
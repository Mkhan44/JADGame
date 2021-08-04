using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public partial class BillingProductDefinition
    {
        [Serializable]
        public class AndroidPlatformProperties
        {
            #region Fields

            [SerializeField]
            private     string              m_publicKey;

            [SerializeField]
            private     string              m_developerPayload;

            #endregion

            #region Properties

            public string PublicKey
            {
                get
                {
                    return m_publicKey;
                }
            }

            public string DeveloperPayload
            {
                get
                {
                    return m_developerPayload;
                }
            }

            #endregion

            #region Properties

            public AndroidPlatformProperties(string publicKey = null, string developerPayload = null)
            {
                // set properties
                m_publicKey         = publicKey;
                m_developerPayload  = developerPayload;
            }

            #endregion
        }
    }
}
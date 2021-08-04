using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using UnityEngine.Serialization;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class BillingServicesUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Fields

        [SerializeField, FormerlySerializedAs("m_billingProductMetaArray")]
        [Tooltip("Array contains information of the products used in the app.")]
        private     BillingProductDefinition[]  m_products                      = new BillingProductDefinition[0];

        [SerializeField]
        [Tooltip("If enabled, system tracks non-consummable purchase information (locally).")]
        private     bool                        m_maintainPurchaseHistory       = true;

        [SerializeField]
        [Tooltip("If enabled, completed transactions are removed from queue automatically.")]
        private     bool                        m_autoFinishTransactions        = true;

        [SerializeField] 
        [Tooltip("If enabled, payment receipts are validated for completed transactions.")]
        private     bool                        m_verifyTransactionReceipts     = true;

        [SerializeField]
        [Tooltip("Android specific properties.")]
        private     IosPlatformProperties       m_iosProperties                 = new IosPlatformProperties();

        [SerializeField]
        [Tooltip("Android specific properties.")]
        private     AndroidPlatformProperties   m_androidProperties             = new AndroidPlatformProperties();

        #endregion

        #region Properties

        public BillingProductDefinition[] Products
        {
            get
            {
                return m_products;
            }
        }
            
        public bool MaintainPurchaseHistory
        {
            get
            {
                return m_maintainPurchaseHistory;
            }
        }

        public bool AutoFinishTransactions
        {
            get
            {
                return m_autoFinishTransactions;
            }
        }

        public bool VerifyPaymentReceipts
        {
            get
            {
                return m_verifyTransactionReceipts;
            }
        }

        public IosPlatformProperties IosProperties
        {
            get
            {
                return m_iosProperties;
            }
        }

         public AndroidPlatformProperties AndroidProperties
        {
            get
            {
                return m_androidProperties;
            }
        }

        #endregion

        #region Constructors

        public BillingServicesUnitySettings(bool enabled = true, BillingProductDefinition[] products = null, 
                                       bool maintainPurchaseHistory = true, bool autoFinishTransactions = true, 
                                       bool verifyTransactionReceipts = true, IosPlatformProperties iosProperties = null, 
                                       AndroidPlatformProperties androidProperties = null)
            : base(enabled)
        {
            // set properties
            m_products                      = products ?? new BillingProductDefinition[0];
            m_maintainPurchaseHistory       = maintainPurchaseHistory;
            m_autoFinishTransactions        = autoFinishTransactions;
            m_verifyTransactionReceipts     = verifyTransactionReceipts;
            m_iosProperties                 = iosProperties ?? new IosPlatformProperties();
            m_androidProperties             = androidProperties ?? new AndroidPlatformProperties();
        }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kBillingServices;
        }

        #endregion
    }
}
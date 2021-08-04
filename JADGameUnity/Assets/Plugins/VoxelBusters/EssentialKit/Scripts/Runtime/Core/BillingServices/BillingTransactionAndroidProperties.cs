using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public sealed class BillingTransactionAndroidProperties
    {
        #region Fields

        /// <summary>
        /// Json string data that is received from google billing servers post purchase and is equal to INAPP_PURCHASE_DATA. This is useful for server verification
        /// </summary>
        public string PurchaseData
        {
            get;
            private set;
        }

        /// <summary>
        /// Signature of the purchase made and is equal to INAPP_DATA_SIGNATURE useful for validating a purchase
        /// </summary>
        public string Signature
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public BillingTransactionAndroidProperties(string purchaseData, string signature)
        {
            // set properties
            PurchaseData = purchaseData;
            Signature = signature;
        }

        #endregion
    }
}
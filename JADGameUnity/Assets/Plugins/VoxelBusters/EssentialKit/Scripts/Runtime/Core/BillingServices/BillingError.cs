using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Billing services error domain.
    /// </summary>
    public static class BillingErrorDomain
    {
        public  const   string  kInitializeStore        = "InitializeStore";

        public  const   string  kBuyProduct             = "BuyProduct";

        public  const   string  kRestorePurchases       = "RestorePurchases";
    }

    /// <summary>
    /// Constants indicating the possible error that might occur when initializing store.
    /// </summary>
    public static class BillingInitializeStoreErrorCode
    {
        /// <summary> Error code indicating that an unknown or unexpected error occurred. </summary>
        public  const   int     kUnknown                = 0;
    }

    /// <summary>
    /// Constants indicating the possible error that might occur when purchasing product.
    /// </summary>
    public static class BillingBuyProductErrorCode
    {
        /// <summary> Error code indicating that an unknown or unexpected error occurred. </summary>
        public  const   int     kUnknown                = 0;
    }

    /// <summary>
    /// Constants indicating the possible error that might occur when restoring old purchases.
    /// </summary>
    public static class BillingRestorePurchasesErrorCode
    {
        /// <summary> Error code indicating that an unknown or unexpected error occurred. </summary>
        public  const   int     kUnknown                = 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="BillingServices.InitializeStore"/> operation is completed.
    /// </summary>
    public class BillingServicesInitializeStoreResult
    {
        #region Properties

        /// <summary>
        /// An array of products returned by the Store.
        /// </summary>
        public IBillingProduct[] Products
        {
            get;
            internal set;
        }

        /// <summary>
        /// An array of product identifiers not recongnized by the Store.
        /// </summary>
        public string[] InvalidProductIds
        {
            get;
            internal set;
        }

        #endregion
    }
}
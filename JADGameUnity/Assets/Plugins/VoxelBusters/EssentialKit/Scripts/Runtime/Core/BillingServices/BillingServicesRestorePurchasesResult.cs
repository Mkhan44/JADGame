using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="BillingServices.OnRestorePurchasesComplete"/> event is triggered.
    /// </summary>
    public class BillingServicesRestorePurchasesResult
    {
        #region Properties

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        public IBillingTransaction[] Transactions
        {
            get;
            internal set;
        }

        #endregion
    }
}

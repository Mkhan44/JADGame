using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="BillingServices.OnTransactionStateChange"/> event is triggered.
    /// </summary>
    public class BillingServicesTransactionStateChangeResult
    {
        #region Properties

        /// <summary>
        /// An array of active transactions provided by the Store.
        /// </summary>
        public IBillingTransaction[] Transactions
        {
            get;
            internal set;
        }

        #endregion
    }
}

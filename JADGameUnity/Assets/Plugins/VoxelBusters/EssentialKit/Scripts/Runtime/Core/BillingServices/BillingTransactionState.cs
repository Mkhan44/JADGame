using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The state of a billing product payment.
    /// </summary>
    public enum BillingTransactionState
    {
        /// <summary> Transaction is being added to the server queue. </summary>
        Purchasing,

        /// <summary> Transaction is in queue, user has been charged. </summary>
        Purchased,

        /// <summary> Transaction was cancelled or failed before being added to the server queue. </summary>
        Failed,

        /// <summary> This transaction restores content previously purchased by the user. </summary>
        Restored,

        /// <summary> The transaction is in the queue, but its final status is pending external action. </summary>
        Deferred,

        /// <summary> This transaction was refunded back to the user. You can restrict/remove associated item. </summary>
        Refunded,
    }
}
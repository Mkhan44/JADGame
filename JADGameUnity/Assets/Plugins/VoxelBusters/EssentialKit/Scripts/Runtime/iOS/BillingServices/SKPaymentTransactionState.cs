#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    internal enum SKPaymentTransactionState : long
    {
        SKPaymentTransactionStatePurchasing,    // Transaction is being added to the server queue.

        SKPaymentTransactionStatePurchased,     // Transaction is in queue, user has been charged.  Client should complete the transaction.

        SKPaymentTransactionStateFailed,        // Transaction was cancelled or failed before being added to the server queue.

        SKPaymentTransactionStateRestored,      // Transaction was restored from user's purchase history.  Client should complete the transaction.

        SKPaymentTransactionStateDeferred,      // The transaction is in the queue, but its final status is pending external action.
    }
}
#endif
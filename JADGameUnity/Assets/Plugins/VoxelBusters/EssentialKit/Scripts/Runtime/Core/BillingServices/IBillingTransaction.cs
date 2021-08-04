using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides an interface to access transaction information of the purchased product.
    /// </summary>
    public interface IBillingTransaction
    {
        #region Properties

        /// <summary>
        /// The string that uniquely identifies a payment transaction. (read-only)
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Returns the payment request associated with this transaction.
        /// </summary>
        IBillingPayment Payment
        {
            get;
        }

        /// <summary>
        /// The UTC date and time, when user initiated this transaction.
        /// </summary>
        DateTime DateUTC
        {
            get;
        }

        /// <summary>
        /// The local date and time, when user initiated this transaction.
        /// </summary>
        DateTime Date
        {
            get;
        }

        /// <summary>
        /// The current state of the transaction. (read-only)
        /// </summary>
        BillingTransactionState TransactionState
        {
            get;
        }

        /// <summary>
        /// The current state of the validation.
        /// </summary>
        BillingReceiptVerificationState ReceiptVerificationState
        {
            get;
            set;
        }

        string Receipt
        {
            get;
        }

        /// <summary>
        /// An object describing the error that occurred while processing the transaction. (read-only)
        /// </summary>
        Error Error
        {
            get;
        }

        /// <summary>
        /// Android specific properties useful for receipt validation on server. (read-only)
        /// </summary>
        BillingTransactionAndroidProperties AndroidProperties
        {
            get;
        }

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Simulator
{
    [Serializable]
    public sealed class BillingTransactionData 
    {
        #region Properties

        public string Tag
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }

        public string ProductId
        {
            get;
            set;
        }

        public string TransactionId
        {
            get;
            set;
        }

        public DateTime TransactionDate
        {
            get;
            set;
        }

        public BillingTransactionState TransactionState
        {
            get;
            set;
        }

        public Error Error
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public BillingTransactionData(string productId, int quantity, string tag, string transactionId, DateTime transactionDate, BillingTransactionState transactionState, Error error = null)
        {
            // set properties
            ProductId           = productId;
            Quantity            = quantity;
            Tag                 = tag;
            TransactionId       = transactionId;
            TransactionDate     = transactionDate;
            TransactionState    = transactionState;
            Error               = error;
        }

        #endregion
    }
}
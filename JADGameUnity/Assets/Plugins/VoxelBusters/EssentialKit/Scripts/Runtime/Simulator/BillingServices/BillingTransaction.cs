using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Simulator
{
    internal sealed class BillingTransaction : BillingTransactionBase
    {
        #region Fields

        private     DateTime                            m_date;

        private     BillingTransactionState             m_transactionState;

        private     BillingReceiptVerificationState     m_verificationState;

        private     string                              m_receipt;

        private     Error                               m_error;
        
        #endregion

        #region Constructors

        public BillingTransaction(string transactionId, IBillingPayment payment, DateTime transactionDate, BillingTransactionState transactionState, BillingReceiptVerificationState verificationState, string receipt, Error error = null)
            : base(transactionId, payment)
        {
            // set properties
            m_date                  = transactionDate;
            m_transactionState      = transactionState;
            m_verificationState     = verificationState;
            m_receipt               = receipt;
            m_error                 = error;
        }

        ~BillingTransaction()
        {
            Dispose(false);
        }

        #endregion

        #region Base methods

        protected override DateTime GetTransactionDateUTCInternal()
        {
            return m_date;
        }

        protected override BillingTransactionState GetTransactionStateInternal()
        {
            return m_transactionState;
        }

        protected override BillingReceiptVerificationState GetReceiptVerificationStateInternal()
        {
            return m_verificationState;
        }

        protected override void SetReceiptVerificationStateInternal(BillingReceiptVerificationState value)
        {
            m_verificationState = value;
        }

        protected override string GetReceiptInternal()
        {
            return m_receipt;
        }

        protected override Error GetErrorInternal()
        {
            return m_error;
        }

        protected override BillingTransactionAndroidProperties GetAndroidPropertiesInternal()
        {
            return null;
        }

        #endregion
    }
}
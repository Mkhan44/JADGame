#if UNITY_IOS || UNITY_TVOS
using System;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
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

        public BillingTransaction(IntPtr nativeObjectPtr, string transactionId, 
            IBillingPayment payment, BillingTransactionState transactionState, 
            BillingReceiptVerificationState verificationState, DateTime date, 
            string receipt, Error error)
            : base(transactionId: transactionId, payment: payment)
        {
            // set properties
            NativeObjectRef         = new IosNativeObjectRef(nativeObjectPtr);
            m_transactionState      = transactionState;
            m_verificationState     = verificationState;
            m_date                  = date;
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
#endif
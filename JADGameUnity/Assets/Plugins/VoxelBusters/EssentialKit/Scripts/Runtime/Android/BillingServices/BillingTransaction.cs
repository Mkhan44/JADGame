#if UNITY_ANDROID
using System;
using VoxelBusters.CoreLibrary;


namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    internal sealed class BillingTransaction : BillingTransactionBase
    {
        #region Fields

        private NativeBillingTransaction m_instance;
        private Error m_error;

        #endregion

        #region Constructors

        public BillingTransaction(NativeBillingTransaction nativeBillingTransaction)
            : this(nativeBillingTransaction, null)
        {
           
        }

        public BillingTransaction(NativeBillingTransaction nativeBillingTransaction, Error error)
            : base(transactionId: nativeBillingTransaction.GetId(), payment: CreateBillingPayment(nativeBillingTransaction))
        {
            m_instance = nativeBillingTransaction;
            m_error = error;
        }

        ~BillingTransaction()
        {
            Dispose(false);
        }

        #endregion

        #region Base methods

        protected override DateTime GetTransactionDateUTCInternal()
        {
            return m_instance.GetPurchaseDate().GetDateTime();
        }

        protected override BillingTransactionState GetTransactionStateInternal()
        {
            return Converter.from(m_instance.GetState());
        }

        protected override BillingReceiptVerificationState GetReceiptVerificationStateInternal()
        {
            return Converter.from(m_instance.GetVerificationState());
        }

        protected override void SetReceiptVerificationStateInternal(BillingReceiptVerificationState value)
        {
            m_instance.SetVerificationState(Converter.from(value));
        }

        protected override string GetReceiptInternal()
        {
            return m_instance.GetReceipt(); // This is equal to purchaseToken now in 2.0v
        }

        protected override Error GetErrorInternal()
        {
            return m_error;
        }

        #endregion

        #region Utility methods

        private static BillingPayment CreateBillingPayment(NativeBillingTransaction nativeBillingTransaction)
        {
            BillingPayment payment = new BillingPayment(nativeBillingTransaction.GetProductIdentifier(), 1, nativeBillingTransaction.GetUserTag());
            return payment;
        }

        protected override BillingTransactionAndroidProperties GetAndroidPropertiesInternal()
        {
            BillingTransactionAndroidProperties androidProperties = new BillingTransactionAndroidProperties(m_instance.GetPurchaseData(), m_instance.GetSignature());
            return androidProperties;
        }

        #endregion
    }
}
#endif
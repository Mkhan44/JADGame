using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.BillingServicesCore
{
    public abstract class NativeBillingServicesInterfaceBase : NativeFeatureInterfaceBase, INativeBillingServicesInterface
    {
        #region Constructors

        protected NativeBillingServicesInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeBillingInterface implementation

        public event RetrieveProductsInternalCallback OnRetrieveProductsComplete;

        public event PaymentStateChangeInternalCallback OnTransactionStateChange;

        public event RestorePurchasesInternalCallback OnRestorePurchasesComplete;

        public abstract void RetrieveProducts(BillingProductDefinition[] productDefinitions);

        public abstract bool CanMakePayments();

        public abstract bool StartPayment(IBillingPayment payment, out Error error);

        public abstract IBillingTransaction[] GetTransactions();

        public abstract void FinishTransactions(IBillingTransaction[] transactions);

        public abstract void RestorePurchases(string tag);

        public abstract bool TryClearingUnfinishedTransactions();

        #endregion

        #region Private methods

        protected void SendRetrieveProductsCompleteEvent(IBillingProduct[] products, string[] invalidProductIds, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnRetrieveProductsComplete(products, invalidProductIds, error));
        }

        protected void SendPaymentStateChangeEvent(IBillingTransaction[] transactions)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnTransactionStateChange(transactions));
        }

        protected void SendRestorePurchasesCompleteEvent(IBillingTransaction[] transactions, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnRestorePurchasesComplete(transactions, error));
        }

        #endregion
    }
}
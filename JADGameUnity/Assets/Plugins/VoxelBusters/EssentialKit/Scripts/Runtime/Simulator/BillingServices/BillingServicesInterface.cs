using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Simulator
{
    public sealed class BillingServicesInterface : NativeBillingServicesInterfaceBase
    {
        #region Constructors

        public BillingServicesInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Create methods

        private static BillingProduct[] ConvertToProductArray(BillingProductData[] array)
        {
            return SystemUtility.ConvertEnumeratorItems(
                enumerator: ((IEnumerable<BillingProductData>)array).GetEnumerator(), 
                converter: (input) =>
                {
                    string  productId   = input.Id;
                    var     settings    = BillingServices.FindProductDefinitionWithId(productId);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified id: {0}", productId);
                        return null;
                    }

                    return new BillingProduct(
                        id: productId, 
                        platformId: settings.GetPlatformIdForActivePlatform(),
                        localizedTitle: input.LocalizedTitle,
                        localizedDescription: "Description",
                        price: input.LocalizedPrice,
                        localizedPrice: input.LocalizedPrice,
                        priceCurrencyCode: input.PriceCurrencyCode,
                        tag: settings.Tag);
                },
                includeNullObjects: false);
        }

        private static BillingTransaction[] ConvertToTransactionArray(BillingTransactionData[] array, BillingReceiptVerificationState verificationState)
        {
            return SystemUtility.ConvertEnumeratorItems(
                enumerator: ((IEnumerable<BillingTransactionData>)array).GetEnumerator(), 
                converter: (input) =>
                {
                    string  productId   = input.ProductId;
                    var     settings    = BillingServices.FindProductDefinitionWithId(productId);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified id: {0}", productId);
                        return null;
                    }

                    var     payment     = new BillingPayment(
                        productId: productId, 
                        productPlatformId: settings.GetPlatformIdForActivePlatform(), 
                        quantity: input.Quantity, 
                        tag: input.Tag);

                    return new BillingTransaction(
                        transactionId: input.TransactionId,
                        payment: payment,
                        transactionDate: input.TransactionDate,
                        transactionState: input.TransactionState,
                        verificationState: verificationState,
                        receipt: "receipt",
                        error: input.Error);
                },
                includeNullObjects: false);
        }

        #endregion

        #region Base class methods

        public override bool CanMakePayments()
        {
            return true;
        }

        public override void RetrieveProducts(BillingProductDefinition[] productDefinitions)
        {
            BillingStoreSimulator.Instance.GetProducts(productDefinitions, (dataArray, error) =>
            {
                var     products    = ConvertToProductArray(dataArray);
                SendRetrieveProductsCompleteEvent(products, null, error);
            });
        }

        public override bool StartPayment(IBillingPayment payment, out Error error)
        {
            // set default value to reference parameters
            error   = null;

            // initate request
            return BillingStoreSimulator.Instance.BuyProduct(payment.ProductId, payment.Quantity, payment.Tag, (data) =>
            {
                var     transactions    = ConvertToTransactionArray(new BillingTransactionData[] { data }, GetReceiptVerificationState());
                SendPaymentStateChangeEvent(transactions);
            });
        }

        public override IBillingTransaction[] GetTransactions()
        {
            return null;
        }

        public override void FinishTransactions(IBillingTransaction[] transactions)
        { }

        public override void RestorePurchases(string tag)
        {
            BillingStoreSimulator.Instance.RestorePurchases(tag, (dataArray, error) =>
            {
                var     transactions    = ConvertToTransactionArray(dataArray, GetReceiptVerificationState());
                SendRestorePurchasesCompleteEvent(transactions, error);
            });
        }

        public override bool TryClearingUnfinishedTransactions()
        {
            return false;
        }

        #endregion

        #region Private methods

        private BillingReceiptVerificationState GetReceiptVerificationState()
        {
            var     unitySettings   = BillingServices.UnitySettings;
            return unitySettings.VerifyPaymentReceipts ? BillingReceiptVerificationState.Success : BillingReceiptVerificationState.NotDetermined;
        }

        #endregion
    }
}
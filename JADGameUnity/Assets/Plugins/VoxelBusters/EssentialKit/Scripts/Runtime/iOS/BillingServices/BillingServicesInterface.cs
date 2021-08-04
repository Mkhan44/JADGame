#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    public sealed class BillingServicesInterface : NativeBillingServicesInterfaceBase, INativeBillingServicesInterface
    {
        #region Static fields

        private static  BillingServicesInterface    s_staticInstance;

        #endregion

        #region Constructors

        static BillingServicesInterface()
        {
            BillingServicesBinding.NPBillingServicesRegisterCallbacks(HandleRequestForProductsCallbackInternal, HandlePaymentStateChangeCallbackInternal, HandleRestorePurchasesCallbackInternal);
        }

        public BillingServicesInterface()
            : base(isAvailable: true)
        {
            // initialise component
            var     unitySettings           = BillingServices.UnitySettings;
            string  customVerificationURL   = unitySettings.IosProperties.CustomVerificationServerURL;
            BillingServicesBinding.NPBillingServicesInit(unitySettings.VerifyPaymentReceipts, customVerificationURL);

            // cache reference
            s_staticInstance    = this;
        }

        #endregion

        #region Base class methods

        public override bool CanMakePayments()
        {
            return BillingServicesBinding.NPBillingServicesCanMakePayments();
        }

        public override void RetrieveProducts(BillingProductDefinition[] productDefinitions)
        {
            var     productIds  = Array.ConvertAll(productDefinitions, (item) => item.GetPlatformIdForActivePlatform());
            BillingServicesBinding.NPBillingServicesRequestForBillingProducts(productIds, productIds.Length);
        }

        public override bool StartPayment(IBillingPayment payment, out Error error)
        {
            // set default value for reference
            error   = null;

            // make request
            if (BillingServicesBinding.NPBillingServicesBuyProduct(payment.ProductPlatformId, payment.Quantity, payment.Tag))
            {
                return true;
            }

            error   = new Error(description: "Failed to create transaction.");
            return false;
        }

        public override IBillingTransaction[] GetTransactions()
        {
            int     length;
            IntPtr  transactionsPtr = BillingServicesBinding.NPBillingServicesGetTransactions(out length);

            try
            {
                // convert native array to unity type
                return BillingServicesUtility.CreateTransactionArray(transactionsPtr, length);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
            finally
            {
                IosNativePluginsUtility.FreeCPointerObject(transactionsPtr);
            }
        }

        public override void FinishTransactions(IBillingTransaction[] transactions)
        { 
            // create native array
            var     transactionsPtr     = Array.ConvertAll(transactions, (item) => ((BillingTransaction)item).AddrOfNativeObject());
            var     unmangedPtr         = MarshalUtility.CreateUnmanagedArray(transactionsPtr);

            try
            {
                BillingServicesBinding.NPBillingServicesFinishTransactions(unmangedPtr, transactions.Length);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release 
                MarshalUtility.ReleaseUnmanagedArray(unmangedPtr);
            }
        }

        public override void RestorePurchases(string tag)
        {
            BillingServicesBinding.NPBillingServicesRestorePurchases(tag);
        }

        public override bool TryClearingUnfinishedTransactions()
        {
            return BillingServicesBinding.NPBillingServicesTryClearingUnfinishedTransactions();
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(RequestForProductsNativeCallback))]
        private static void HandleRequestForProductsCallbackInternal(IntPtr productsPtr, int length, string error, ref NativeArray invalidProductIds)
        {
            // send event
            var     products                = BillingServicesUtility.CreateProductArray(productsPtr, length);
            var     invalidIdManagedArray   = MarshalUtility.CreateStringArray(invalidProductIds.Pointer, invalidProductIds.Length);
            var     errorObj                = Error.CreateNullableError(description: error);
            s_staticInstance.SendRetrieveProductsCompleteEvent(products, invalidIdManagedArray, errorObj);
        }

        [MonoPInvokeCallback(typeof(TransactionStateChangeNativeCallback))]
        private static void HandlePaymentStateChangeCallbackInternal(IntPtr transactionsPtr, int length)
        {
            // send event
            var     transactions    = BillingServicesUtility.CreateTransactionArray(transactionsPtr, length);
            s_staticInstance.SendPaymentStateChangeEvent(transactions);
        }

        [MonoPInvokeCallback(typeof(RestorePurchasesNativeCallback))]
        private static void HandleRestorePurchasesCallbackInternal(IntPtr transactionsPtr, int length, string error)
        {
            // send event
            var     transactions    = BillingServicesUtility.CreateTransactionArray(transactionsPtr, length);
            var     errorObj        = Error.CreateNullableError(description: error);
            s_staticInstance.SendRestorePurchasesCompleteEvent(transactions, errorObj);
        }

        #endregion
    }
}
#endif
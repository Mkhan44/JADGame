#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    internal static class BillingServicesBinding
    {
        [DllImport("__Internal")]
        public static extern bool NPBillingServicesCanMakePayments();

        [DllImport("__Internal")]
        public static extern void NPBillingServicesRegisterCallbacks(RequestForProductsNativeCallback requestForProductsCallback, TransactionStateChangeNativeCallback transactionStateChangeCallback, RestorePurchasesNativeCallback restorePurchasesCallback);
        
        [DllImport("__Internal")]
        public static extern void NPBillingServicesInit(bool usesReceiptVerification, string customReceiptVerificationServerURL);
        
        [DllImport("__Internal")]
        public static extern void NPBillingServicesRequestForBillingProducts(string[] productIds, int length);
        
        [DllImport("__Internal")]
        public static extern bool NPBillingServicesBuyProduct(string productId, int quantity, string username);

        [DllImport("__Internal")]
        public static extern IntPtr NPBillingServicesGetTransactions(out int length);

        [DllImport("__Internal")]
        public static extern NPStoreReceiptVerificationState NPBillingServicesGetReceiptVerificationState(IntPtr transactionPtr);

        [DllImport("__Internal")]
        public static extern void NPBillingServicesRestorePurchases(string username);

        [DllImport("__Internal")]
        public static extern void NPBillingServicesFinishTransactions(IntPtr transactionsPtr, int length);

        [DllImport("__Internal")]
        public static extern void NPBillingServicesGetOriginalTransaction(IntPtr transactionPtr, ref SKPaymentTransactionData data);

        [DllImport("__Internal")]
        public static extern bool NPBillingServicesTryClearingUnfinishedTransactions();
    }
}
#endif
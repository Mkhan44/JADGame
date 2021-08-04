#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    internal static class BillingServicesUtility
    {
        #region Converter methods

        public static BillingTransactionState ConvertToBillingTransactionState(SKPaymentTransactionState state)
        {
            switch (state)
            {
                case SKPaymentTransactionState.SKPaymentTransactionStatePurchasing:
                    return BillingTransactionState.Purchasing;

                case SKPaymentTransactionState.SKPaymentTransactionStatePurchased:
                    return BillingTransactionState.Purchased;

                case SKPaymentTransactionState.SKPaymentTransactionStateFailed:
                    return BillingTransactionState.Failed;

                case SKPaymentTransactionState.SKPaymentTransactionStateRestored:
                    return BillingTransactionState.Restored;

                case SKPaymentTransactionState.SKPaymentTransactionStateDeferred:
                    return BillingTransactionState.Deferred;

                default:
                    throw VBException.SwitchCaseNotImplemented(state);
            }
        }

        public static BillingReceiptVerificationState ConvertToBillingReceiptVerificationState(NPStoreReceiptVerificationState state)
        {
            switch (state)
            {
                case NPStoreReceiptVerificationState.NPStoreReceiptVerificationStateNotChecked:
                    return BillingReceiptVerificationState.NotDetermined;

                case NPStoreReceiptVerificationState.NPStoreReceiptVerificationStateSuccess:
                    return BillingReceiptVerificationState.Success;

                case NPStoreReceiptVerificationState.NPStoreReceiptVerificationStateFailed:
                    return BillingReceiptVerificationState.Failed;

                default:
                    throw VBException.SwitchCaseNotImplemented(state);
            }
        }

        public static BillingProduct[] CreateProductArray(IntPtr productsPtr, int length)
        {
            return MarshalUtility.ConvertNativeArrayItems<SKProductData, BillingProduct>(
                arrayPtr: productsPtr,
                length: length, 
                converter: (input) =>
                {
                    string  platformId  = MarshalUtility.ToString(input.IdentifierPtr);
                    var     settings    = BillingServices.FindProductDefinitionWithPlatformId(platformId);
                    if (settings == null)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified platform id: {0}", platformId);
                        return null;
                    }

                    return new BillingProduct(
                        nativeObjectPtr: input.NativeObjectPtr,
                        id: settings.Id,
                        platformId: platformId,
                        localizedTitle: MarshalUtility.ToString(input.LocalizedTitlePtr),
                        localizedDescription: MarshalUtility.ToString(input.LocalizedDescriptionPtr),
                        price: input.Price.ToString(System.Globalization.CultureInfo.InvariantCulture),
                        localizedPrice: MarshalUtility.ToString(input.LocalizedPricePtr),
                        priceCurrencyCode: MarshalUtility.ToString(input.CurrencyCodePtr),
                        tag: settings.Tag);
                }, 
                includeNullObjects: false);
        }

        public static BillingTransaction[] CreateTransactionArray(IntPtr transactionsPtr, int length)
        {
            return MarshalUtility.ConvertNativeArrayItems<SKPaymentTransactionData, BillingTransaction>(
                arrayPtr: transactionsPtr,
                length: length, 
                converter: (input) =>
                {
                    var     nativeObjectPtr             = input.NativeObjectPtr;
                    var     nativeVerificationState     = BillingServicesBinding.NPBillingServicesGetReceiptVerificationState(nativeObjectPtr);
                    string  dateStr                     = MarshalUtility.ToString(input.DatePtr);

                    return new BillingTransaction(
                        nativeObjectPtr: nativeObjectPtr,
                        transactionId: MarshalUtility.ToString(input.IdentifierPtr),
                        payment: CreatePayment(input.PaymentData),
                        transactionState: ConvertToBillingTransactionState(input.TransactionState),
                        verificationState: ConvertToBillingReceiptVerificationState(nativeVerificationState),
                        date: IosNativePluginsUtility.ParseDateTimeStringInUTCFormat(dateStr),
                        receipt: MarshalUtility.ToString(input.ReceiptDataPtr),
                        error: Error.CreateNullableError(description: MarshalUtility.ToString(input.ErrorPtr)));
                },
                includeNullObjects: false);
        }

        public static IBillingPayment CreatePayment(SKPaymentData data)
        {
            return new BillingPayment(
                productPlatformId: MarshalUtility.ToString(data.ProductIdentifierPtr),
                quantity: data.Quantity,
                tag: MarshalUtility.ToString(data.ApplicationUsernamePtr));
        }

        #endregion
    }
}
#endif
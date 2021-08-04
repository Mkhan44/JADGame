#if UNITY_ANDROID
using System;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    internal static class Converter
    {
        public static BillingTransactionState from(NativeBillingTransactionState nativeBillingTransactionState)
        {
            switch (nativeBillingTransactionState)
            {
                case NativeBillingTransactionState.Pending:
                    return BillingTransactionState.Deferred;
                case NativeBillingTransactionState.Started:
                    return BillingTransactionState.Purchasing;
                case NativeBillingTransactionState.Purchased:
                    return BillingTransactionState.Purchased;
                case NativeBillingTransactionState.Restored:
                    return BillingTransactionState.Restored;
                case NativeBillingTransactionState.Failed:
                    return BillingTransactionState.Failed;
                default:
                    throw VBException.SwitchCaseNotImplemented(nativeBillingTransactionState);
            }
        }

        public static BillingReceiptVerificationState from(NativeBillingTransactionVerificationState nativeBillingTransactionVerificationState)
        {
            switch (nativeBillingTransactionVerificationState)
            {
                case NativeBillingTransactionVerificationState.Success:
                    return BillingReceiptVerificationState.Success;
                case NativeBillingTransactionVerificationState.Failure:
                    return BillingReceiptVerificationState.Failed;
                case NativeBillingTransactionVerificationState.Unknown:
                    return BillingReceiptVerificationState.NotDetermined;
                default:
                    throw VBException.SwitchCaseNotImplemented(nativeBillingTransactionVerificationState);
            }
        }

        public static NativeBillingTransactionVerificationState from(BillingReceiptVerificationState value)
        {
            switch(value)
            {
                case BillingReceiptVerificationState.NotDetermined:
                    return NativeBillingTransactionVerificationState.Unknown;
                case BillingReceiptVerificationState.Success:
                    return NativeBillingTransactionVerificationState.Success;
                case BillingReceiptVerificationState.Failed:
                    return NativeBillingTransactionVerificationState.Failure;
                default:
                    throw VBException.SwitchCaseNotImplemented(value);
            }

        }
    }
}
#endif

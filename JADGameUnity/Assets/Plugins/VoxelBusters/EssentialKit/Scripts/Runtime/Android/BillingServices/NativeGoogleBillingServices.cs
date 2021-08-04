#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    public class NativeGoogleBillingServices : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Private properties
        private NativeActivity Activity
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        public NativeGoogleBillingServices(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
            Activity    = new NativeActivity(context);
        }

        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }
        #endregion
        #region Public methods

        public void Initialise(string publicKey, NativeBillingTransactionStateListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : Initialise]");
#endif
            Call(Native.Method.kInitialise, publicKey, listener);
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void RestorePurchases(string userTag, NativeRestorePurchasesListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : RestorePurchases]");
#endif
            Call(Native.Method.kRestorePurchases, userTag, listener);
        }
        public bool CanMakePayments()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : CanMakePayments]");
#endif
            return Call<bool>(Native.Method.kCanMakePayments);
        }
        public void SetProducts(string[] consumableProductIds, string[] nonConsumableProductIds, string[] subscriptionProductIds)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : SetProducts]");
#endif
            Call(Native.Method.kSetProducts, consumableProductIds, nonConsumableProductIds, subscriptionProductIds);
        }
        public void FetchProductDetails(NativeFetchBillingProductsListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : FetchProductDetails]");
#endif
            Call(Native.Method.kFetchProductDetails, listener);
        }
        public void BuyProduct(string productIdentifier, string userTag)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeGoogleBillingServices][Method(RunOnUiThread) : BuyProduct]");
#endif
                Call(Native.Method.kBuyProduct, productIdentifier, userTag);
            });
        }
        public NativeList<NativeBillingTransaction> GetIncompleteBillingTransactions()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : GetIncompleteBillingTransactions]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetIncompleteBillingTransactions);
            NativeList<NativeBillingTransaction> data  = new  NativeList<NativeBillingTransaction>(nativeObj);
            return data;
        }
        public void FinishBillingTransaction(string transactionId, bool isValidPurchase)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : FinishBillingTransaction]");
#endif
            Call(Native.Method.kFinishBillingTransaction, transactionId, isValidPurchase);
        }
        public bool TryClearingIncompleteTransactions()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGoogleBillingServices][Method : TryClearingIncompleteTransactions]");
#endif
            return Call<bool>(Native.Method.kTryClearingIncompleteTransactions);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.billingservices.providers.google.GoogleBillingServices";

            internal class Method
            {
                internal const string kTryClearingIncompleteTransactions = "tryClearingIncompleteTransactions";
                internal const string kFetchProductDetails = "fetchProductDetails";
                internal const string kSetProducts = "setProducts";
                internal const string kInitialise = "initialise";
                internal const string kBuyProduct = "buyProduct";
                internal const string kFinishBillingTransaction = "finishBillingTransaction";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kCanMakePayments = "canMakePayments";
                internal const string kRestorePurchases = "restorePurchases";
                internal const string kGetIncompleteBillingTransactions = "getIncompleteBillingTransactions";
            }

        }
    }
}
#endif
using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.BillingServicesCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface to request payment from a user for additional functionality or content that your application delivers.    
    /// </summary>
    /// <description>
    /// <para>
    /// This feature connects to the Store on your app’s behalf to securely process payments from users, prompting them to authorize payment. 
    /// The feature then notifies your app, which provides the purchased items to users. 
    /// For processing requests, feature contacts App Store, Google Play Store on iOS, Android platform respectively.
    /// You need to configure iOS billing product details at <a href="https://developer.apple.com/library/ios/IAPConfigGuide">iTunes Connect</a>. 
    /// Similarly for Android, you can set these details at <a href="http://developer.android.com/google/play/billing/billing_admin.html">Google Play Developer Console</a>.
    /// </para>
    /// <para>
    /// The interaction between the user, your app, and the Store during the purchase process take place in three stages.
    /// First, the your app displays purchasable products received from the Store. 
    /// Second, the user selects a product to buy and the app requests payment from the Store. 
    /// Third, the Store processes the payment and your app delivers the purchased product.
    /// </para>
    /// <para>
    /// Optionally, you can choose to verify receipts of completed transactions. The receipt is a record of purchase made from within the application and enabling receipt validation, adds one more level security to avoid unauthorised purchases.</para>
    /// <para>
    /// Users can also restore products that were previously purchased. As per iOS guidelines, if your application supports product types that are restorable, you must include an interface that allows users to restore these purchases.
    /// </para>
    /// </description>
    /// <example>
    /// The following example illustrates how to handle use billing services.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     private void OnEnable()
    ///     {
    ///         // registering for event
    ///         BillingServices.OnInitializeStoreComplete   += OnInitializeStoreComplete;
    ///         BillingServices.OnTransactionStateChange    += OnTransactionStateChange;
    ///         BillingServices.OnRestorePurchasesComplete  += OnRestorePurchasesComplete;
    /// 
    ///         // update product catalogue
    ///         BillingServices.InitializeStore();
    ///     }
    /// 
    ///     private void OnDisable()
    ///     {
    ///         // unregistering event
    ///         BillingServices.OnInitializeStoreComplete   -= OnInitializeStoreComplete;
    ///         BillingServices.OnTransactionStateChange    -= OnTransactionStateChange;
    ///         BillingServices.OnRestorePurchasesComplete  -= OnRestorePurchasesComplete;
    ///     }
    /// 
    ///     private void OnInitializeStoreComplete(BillingServicesInitializeStoreResult result, Error error)
    ///     {
    ///         // check response status
    ///         if (error == null)
    ///         {
    ///             // update interface 
    ///             IBillingProduct[]   products    = result.Products;
    ///         }
    ///         else
    ///         {
    ///             // show error
    ///         }
    ///     }
    /// 
    ///     private void OnTransactionStateChange(BillingServicesTransactionStateChangeResult result)
    ///     {
    ///         // check response status
    ///         if (error == null)
    ///         {
    ///             IBillingTransaction[]   transactions    = result.Transactions;
    ///             for (int iter = 0; iter < transactions.Length; iter++)
    ///             {
    ///                 IBillingTransaction transaction     = transactions[iter];
    ///                 switch (transaction.TransactionState)
    ///                 {
    ///                     case BillingTransactionState.Purchased:
    ///                     if (transaction.ReceiptVerificationState == BillingReceiptVerificationState.Success)
    ///                     {
    ///                         // add associated item to user profile
    ///                     }
    ///                     break;
    /// 
    ///                     case BillingTransactionState.Failed:
    ///                     // transaction failed, show associated message
    ///                     break;
    ///                 }
    ///             }
    ///         }
    ///     }
    /// 
    ///     private void OnRestorePurchasesComplete(BillingServicesRestorePurchasesResult result, Error error)
    ///     {
    ///         // check response status
    ///         if (error == null)
    ///         {
    ///             IBillingTransaction[]   transactions    = result.Transactions;
    ///             for (int iter = 0; iter < transactions.Length; iter++)
    ///             {
    ///                 IBillingTransaction transaction     = transactions[iter];
    ///                 if (transaction.ReceiptVerificationState == BillingReceiptVerificationState.Success)
    ///                 {
    ///                     // add associated item to user profile
    ///                 }
    ///             }
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class BillingServices
    {
        #region Constants

        private     const   string  kPurchaseHistoryPrefKey     = "purchase-history";

        #endregion

        #region Static fields

        private     static  INativeBillingServicesInterface     s_nativeInterface;

        #endregion

        #region Static properties

        public static BillingServicesUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.BillingServicesSettings;
            }
        }

        public static BillingProductDefinition[] ProductDefinitions
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the cached products array retrieved from store.
        /// </summary>
        /// <remarks>
        /// \note This property is invalid until a call to <see cref="InitializeStore"> is completed.
        /// </remarks>
        public static IBillingProduct[] Products
        {
            get;
            private set;
        }

        #endregion

        #region Static events

        /// <summary>
        /// Event that will be called when registered billing products are retreived from the Store.
        /// </summary>
        public static event EventCallback<BillingServicesInitializeStoreResult> OnInitializeStoreComplete;

        /// <summary>
        /// Event that will be called when payment state changes.
        /// </summary>
        public static event Callback<BillingServicesTransactionStateChangeResult> OnTransactionStateChange;

        /// <summary>
        /// Event that will be called when restored transaction details are received from the Store.
        /// </summary>
        public static event EventCallback<BillingServicesRestorePurchasesResult> OnRestorePurchasesComplete;
        
        #endregion

        #region Setup methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        internal static void Initialize()
        {
            // configure component
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeBillingServicesInterface>(ImplementationBlueprint.BillingServices, UnitySettings.IsEnabled);

            ProductDefinitions      = new BillingProductDefinition[0];
            Products                = new IBillingProduct[0];

            // setup internal events
            RegisterForEvents();
        }

        internal static BillingProductDefinition FindProductDefinitionWithId(string id)
        {
            return Array.Find(ProductDefinitions, (item) => string.Equals(item.Id, id));
        }

        internal static BillingProductDefinition FindProductDefinitionWithPlatformId(string platformId)
        {
            return Array.Find(ProductDefinitions, (item) => string.Equals(item.GetPlatformIdForActivePlatform(), platformId));
        }

        #endregion

        #region Product methods

        /// <summary>
        /// Sends a request to retrieve localized information about the billing products from the Store.    
        /// </summary>
        /// <description> 
        /// Call to this method retrieves information of the products that are configured in <c>Billing Settings</c>.
        /// Your application uses this request to present localized prices and other information to the user without having to maintain that list itself. 
        /// </description>
        /// <remarks>
        /// \note When the request completes, <see cref="OnInitializeStoreComplete"/> is fired.
        /// </remarks>
        public static void InitializeStore()
        {
            InitializeStore(productDefinitions: UnitySettings.Products);
        }

        /// <summary>
        /// Sends a request to retrieve localized information about the billing products from the Store.    
        /// </summary>
        /// <description> 
        /// Call to this method retrieves information of the products that are configured in <c>Billing Settings</c>.
        /// Your application uses this request to present localized prices and other information to the user without having to maintain that list itself. 
        /// </description>
        /// <remarks>
        /// \note When the request completes, <see cref="OnInitializeStoreComplete"/> is fired.
        /// </remarks>
        public static void InitializeStore(BillingProductDefinition[] productDefinitions)
        {
            // validate arguments
            Assertions.AssertIfArrayIsNullOrEmpty(productDefinitions, "productDefinitions");

            try
            {
                // copy active definitions
                ProductDefinitions  = productDefinitions;
                
                // make request
                s_nativeInterface.RetrieveProducts(ProductDefinitions);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Gets the billing product with localized information, which was previously fetched from the Store.
        /// </summary>
        /// <returns>The billing product fetched with localized information.</returns>
        /// <param name="id">A string used to identify a billing product.</param>
        public static IBillingProduct GetProductWithId(string id)
        {
            // validate arguments
            Assertions.AssertIfStringIsNullOrEmpty(id, "Id is null/empty");

            try
            {
                // find object with specified id
                return Array.Find(Products, (item) => string.Equals(item.Id, id));
            }
            catch (NullReferenceException exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
        }

        /// <summary>
        /// Gets the billing product with localized information, which was previously fetched from the Store.
        /// </summary>
        /// <returns>The billing product fetched with localized information.</returns>
        /// <param name="tag">A tag associated with billing product.</param>
        public static IBillingProduct GetProductWithTag(object tag)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(tag, "tag");

            try
            {
                // find object with specified tag
                return Array.Find(Products, (item) => object.Equals(item.Tag, tag));
            }
            catch (NullReferenceException exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
        }

        #endregion

        #region Payment methods

        /// <summary>
        /// Determines whether the user is authorised to make payments.
        /// </summary>
        /// <returns><c>true</c> if the user is allowed to make product purchase payment; otherwise, <c>false</c>.</returns>
        public static bool CanMakePayments()
        {
            try
            {
                // make request
                return s_nativeInterface.CanMakePayments();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return false;
            }
        }

        /// <summary>
        /// Determines whether specified billing product is already purchased.
        /// </summary>
        /// <returns><c>true</c> if specified billing product is already purchased; otherwise, <c>false</c>.</returns>
        /// <param name="product">The object identifies the billing product registered in the Store.</param>
        /// <remarks> 
        /// \note This works only for Non-Consumable (Managed) billing product. For Consumable products, this will always returns false.
        /// </remarks>
        public static bool IsProductPurchased(IBillingProduct product)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(product, "product");

            try
            {
                // make request
                return CheckWhetherPurchaseHistoryContainsProductId(product.Id);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return false;
            }
        }

        /// <summary>
        /// Initiates purchase process for the specified billing product.
        /// </summary>
        /// <returns><c>true</c>, if request was initiated, <c>false</c> otherwise failed. This can happen if product is not found</returns>
        /// <remarks>
        /// \note The payment request must have a product identifier registered with the Store.
        /// </remarks>
        /// <param name="productId">The product you want to purchase.</param>
        /// <param name="quantity">The number of units you want to purchase. Default quantity value is 1.</param>
        /// <param name="applicationUsername">Application provided username that initiated this request. (optional)</param>
        public static bool BuyProduct(string productId, int quantity = 1, string applicationUsername = null)
        {
            // check whether store is initialized
            if ((Products == null) || (Products.Length == 0))
            {
                DebugLogger.LogWarning("Not initialized!");
                return false;
            }

            // find specified product
            var     targetProduct   = Array.Find(Products, (item) => string.Equals(item.Id, productId));
            if (targetProduct == null)
            {
                DebugLogger.LogWarning("Product not found!");
                return false;
            }

            return BuyProduct(targetProduct, quantity, applicationUsername);
        }

        /// <summary>
        /// Initiates purchase process for the specified billing product.
        /// </summary>
        /// <returns><c>true</c>, if request was initiated, <c>false</c> otherwise failed. This can happen if product is not found</returns>
        /// <remarks>
        /// \note The payment request must have a product identifier registered with the Store.
        /// </remarks>
        /// <param name="product">The product you want to purchase.</param>
        /// <param name="quantity">The number of units you want to purchase. Default quantity value is 1.</param>
        /// <param name="tag">Specify user data associated with the purchase. Eg: Application provided username that initiated this request. (optional)</param>
        public static bool BuyProduct(IBillingProduct product, int quantity = 1, string tag = null)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(product, "product");
            if (quantity < 1)
            {
                quantity    = 1;
            }
                
            try
            {
                // create payment object
                var     newPayment  = new BillingPayment(product.Id, product.PlatformId, quantity, tag);

                // make request           
                Error   error;
                if (s_nativeInterface.StartPayment(newPayment, out error))
                {
                    return true;
                }

                DebugLogger.LogError("Failed to initiate product purchase. Error: " +  error);
                return false;
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return false;
            }
        }

        /// <summary>
        /// Returns the pending transactions available in transaction queue.
        /// </summary>
        /// /// <remarks>
        /// \note User needs to manually call this method after receiving completed transactions, incase if autoFinishTransactions flag is turned off in billing settings.
        /// </remarks>
        /// <returns>An array of unfinished transactions.</returns>
        public static IBillingTransaction[] GetTransactions()
        {
            try
            {
                // make request
                return s_nativeInterface.GetTransactions() ?? new IBillingTransaction[0];
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return new IBillingTransaction[0];
            }
        }

        /// <summary>
        /// Completes the pending transactions and removes it from transaction queue.
        /// </summary>
        /// <param name="transactions">An array of unfinished transactions.</param>
        public static void FinishTransactions(IBillingTransaction[] transactions)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(transactions, "transactions");

            try
            {
                // make request
                var     finishableTransactions  = Array.FindAll(transactions, (item) =>
                {
                    var     transactionState    = item.TransactionState;
                    return !(transactionState == BillingTransactionState.Purchasing ||
                             transactionState == BillingTransactionState.Deferred);
                });
                if (finishableTransactions.Length > 0)
                {
                    s_nativeInterface.FinishTransactions(finishableTransactions);
                }
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Restore methods

        /// <summary>
        /// Sends a request to restore completed purchases.
        /// </summary>
        /// <description>
        /// Your application calls this method to restore transactions that were previously purchased so that you can process them again.
        /// </description>
        /// <remarks> 
        /// \note 
        /// Internally this feature requires consumable product information. So ensure that <see cref="InitializeStore"/> is called prior to this. 
        /// </remarks>
        /// <param name="tag">Application provided username that initiated this request. (optional)</param>
        public static void RestorePurchases(string tag = null)
        {
            try
            {
                // make request
                s_nativeInterface.RestorePurchases(tag);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Private methods

        private static void RegisterForEvents()
        {
            s_nativeInterface.OnRetrieveProductsComplete    += HandleOnRetrieveProductsComplete;
            s_nativeInterface.OnTransactionStateChange      += HandleOnTransactionStateChange;
            s_nativeInterface.OnRestorePurchasesComplete    += HandleOnRestorePurchasesComplete;
        }

        private static void UnregisterFromEvents()
        {
            s_nativeInterface.OnRetrieveProductsComplete    -= HandleOnRetrieveProductsComplete;
            s_nativeInterface.OnTransactionStateChange      -= HandleOnTransactionStateChange;
            s_nativeInterface.OnRestorePurchasesComplete    -= HandleOnRestorePurchasesComplete;
        }

        private static bool IsProductOfType(string productId, BillingProductType type)
        {
            var     productDefinition   = FindProductDefinitionWithId(productId);
            if (productDefinition != null)
            {
                return (productDefinition.ProductType == type);
            }

            return false;
        }

        #endregion

        #region Purchase history methods

        /// <summary>
        /// Clears the purchase history.
        /// </summary>
        public static void ClearPurchaseHistory()
        {
            var     saveSystem          = ExternalServiceProvider.SaveServiceProvider;
            saveSystem.RemoveKey(kPurchaseHistoryPrefKey);
        }

        private static bool CheckWhetherPurchaseHistoryContainsProductId(string productId)
        {
            var     saveSystem          = ExternalServiceProvider.SaveServiceProvider;
            var     existingProductIds  = saveSystem.GetStringArray(kPurchaseHistoryPrefKey);
            if (existingProductIds != null)
            {
                return Array.Exists(existingProductIds, (item) => string.Equals(item, productId));
            }

            return false;
        }

        private static void UpdatePurchaseHistory(IBillingTransaction[] transactions, bool rewrite)
        {
            // check whether we need to maintain purchase history
            if (false == UnitySettings.MaintainPurchaseHistory)
            {
                return;
            }

            // flush old values, if specified
            var     saveSystem          = ExternalServiceProvider.SaveServiceProvider;
            if (rewrite)
            {
                saveSystem.RemoveKey(kPurchaseHistoryPrefKey);
            }

            // get updated info
            var     existingProductIds  = saveSystem.GetStringArray(kPurchaseHistoryPrefKey) ?? new string[0];
            var     newProductIdList    = new List<string>(existingProductIds);
            for (int iter = 0; iter < transactions.Length; iter++)
            {
                var     transaction     = transactions[iter];
                if ((BillingTransactionState.Purchased == transaction.TransactionState) || 
                    (BillingTransactionState.Restored == transaction.TransactionState))
                {
                    string  productId   = transaction.Payment.ProductId;
                    if (IsProductOfType(productId, BillingProductType.NonConsumable))
                    {
                        newProductIdList.Add(productId);
                    }
                }
            }

            // save information
            saveSystem.SetStringArray(kPurchaseHistoryPrefKey, newProductIdList.ToArray());
            saveSystem.Save();
        }

        #endregion

        #region Event callback methods

        private static void HandleOnRetrieveProductsComplete(IBillingProduct[] products, string[] invalidProductIds, Error error)
        {
            // update cache
            Products                = products ?? new IBillingProduct[0];

            // invoke event
            var     result          = new BillingServicesInitializeStoreResult() 
            { 
                Products            = products, 
                InvalidProductIds   = invalidProductIds ?? new string[0] 
            };
            CallbackDispatcher.InvokeOnMainThread(OnInitializeStoreComplete, result, error);

            // check whether any pending transactions are available
            if (error == null)
            {
                if (s_nativeInterface.TryClearingUnfinishedTransactions())
                {
                    DebugLogger.Log("Found unfinished transactions. We are attempting to handle it, make sure you are calling BillingServices.FinishTransactions after processing transactions.");
                }
            }
        }
       
        private static void HandleOnTransactionStateChange(IBillingTransaction[] transactions)
        {
            // prepare transaction array
            transactions        = transactions ?? new IBillingTransaction[0];

            // save new copy of purchased information
            UpdatePurchaseHistory(transactions, rewrite: false);

            // invoke event
            var     result      = new BillingServicesTransactionStateChangeResult()
            {
                Transactions    = transactions,
            };
            CallbackDispatcher.InvokeOnMainThread(OnTransactionStateChange, result);

            // mark transactions as completed
            if (UnitySettings.AutoFinishTransactions)
            {
                FinishTransactions(transactions);
            }
        }

        private static void HandleOnRestorePurchasesComplete(IBillingTransaction[] transactions, Error error)
        {
            // prepare transaction array
            transactions        = transactions ?? new IBillingTransaction[0];

            // save new copy of purchased information
            if (error == null)
            {
                UpdatePurchaseHistory(transactions, rewrite: true);
            }

            // invoke event
            var     result      = new BillingServicesRestorePurchasesResult()
            {
                Transactions    = transactions,
            };
            CallbackDispatcher.InvokeOnMainThread(OnRestorePurchasesComplete, result, error);

            // mark transactions as completed
            if (UnitySettings.AutoFinishTransactions)
            {
                FinishTransactions(transactions);
            }
        }

        #endregion
    }
}
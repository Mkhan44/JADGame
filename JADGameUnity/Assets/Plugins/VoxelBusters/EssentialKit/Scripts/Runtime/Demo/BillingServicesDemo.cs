using System;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class BillingServicesDemo : DemoActionPanelBase<BillingServicesDemoAction, BillingServicesDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     RectTransform       m_yourProductsNode              = null;

        [SerializeField]
        private     RectTransform[]     m_productsDependentObjects      = null;

        [SerializeField]
        private     Toggle              m_productPrefab                 = null;

        private     int                 m_productIndex                  = 0;

        #endregion

        #region Base class methods

        protected override void Start()
        {
            base.Start();

            // set default state
            SetIsReady(false);
            CreateProductOutlets(null);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // register for events
            BillingServices.OnInitializeStoreComplete   += OnInitializeStoreComplete;
            BillingServices.OnTransactionStateChange    += OnTransactionStateChange;
            BillingServices.OnRestorePurchasesComplete  += OnRestorePurchasesComplete;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // unregister from events
            BillingServices.OnInitializeStoreComplete   -= OnInitializeStoreComplete;
            BillingServices.OnTransactionStateChange    -= OnTransactionStateChange;
            BillingServices.OnRestorePurchasesComplete  -= OnRestorePurchasesComplete;
        }

        protected override void OnActionSelectInternal(BillingServicesDemoAction selectedAction)
        {
            var     product     = GetCurrentProduct();

            switch (selectedAction.ActionType)
            {
                case BillingServicesDemoActionType.InitializeStore:
                    BillingServices.InitializeStore();
                    break;

                case BillingServicesDemoActionType.CanMakePayments:
                    bool    canPurchase     = BillingServices.CanMakePayments();
                    Log("Can make payment: " + canPurchase);
                    break;

                case BillingServicesDemoActionType.IsProductPurchased:
                    if (product == null)
                    {
                        LogSelectProduct();
                    }
                    else
                    {
                        bool    isPurchased = BillingServices.IsProductPurchased(product);
                        Log("Is purchased: " + isPurchased);
                    }
                    break;

                case BillingServicesDemoActionType.BuyProduct:
                    if (product == null)
                    {
                        LogSelectProduct();
                    }
                    else
                    {
                        Log("Initiated purchase");
                        BillingServices.BuyProduct(product);
                    }
                    break;

                case BillingServicesDemoActionType.GetTransactions:
                    var     transactions1   = BillingServices.GetTransactions();
                    Log("Total incomplete transactions: " + transactions1.Length);
                    break;

                case BillingServicesDemoActionType.FinishTransactions:
                    var     transactions2   = BillingServices.GetTransactions();
                    if (transactions2.Length > 0)
                    {
                        Log("Initiated finish transaction process.");
                        BillingServices.FinishTransactions(transactions2);
                    }
                    break;

                case BillingServicesDemoActionType.RestorePurchases:
                    BillingServices.RestorePurchases();
                    break;

                case BillingServicesDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kBillingServices);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Plugin event methods

        private void OnInitializeStoreComplete(BillingServicesInitializeStoreResult result, Error error)
        {
            if (error == null)
            {
                // update UI
                SetIsReady(true);
                CreateProductOutlets(result.Products);

                // show console messages
                var     products    = result.Products;
                Log("Store initialized successfully.");
                Log("Total products fetched: " + products.Length);
                Log("Below are the available products:");
                for (int iter = 0; iter < products.Length; iter++)
                {
                    var     product = products[iter];
                    Log(string.Format("[{0}]: {1}", iter, product));
                }
            }
            else
            {
                Log("Store initialization failed with error. Error: " + error);
            }

            var     invalidIds  = result.InvalidProductIds;
            Log("Total invalid products: " + invalidIds.Length);
            if (invalidIds.Length > 0)
            {
                Log("Here are the invalid product ids:");
                for (int iter = 0; iter < invalidIds.Length; iter++)
                {
                    Log(string.Format("[{0}]: {1}", iter, invalidIds[iter]));
                }
            }
        }

        private void OnTransactionStateChange(BillingServicesTransactionStateChangeResult result)
        {
            var     transactions    = result.Transactions;
            for (int iter = 0; iter < transactions.Length; iter++)
            {
                var     transaction = transactions[iter];
                switch (transaction.TransactionState)
                {
                    case BillingTransactionState.Purchased:
                        //Note: transaction.ReceiptVerificationState also needs to be considered for avoiding fraud transactions
                        Log(string.Format("Buy product with id:{0} finished successfully with verification state {1}.", transaction.Payment.ProductId, transaction.ReceiptVerificationState));
                        break;

                    case BillingTransactionState.Failed:
                        Log(string.Format("Buy product with id:{0} failed with error. Error: {1}", transaction.Payment.ProductId, transaction.Error));
                        break;
                }
            }
        }

        private void OnRestorePurchasesComplete(BillingServicesRestorePurchasesResult result, Error error)
        {
            if (error == null)
            {
                var     transactions    = result.Transactions;
                Log("Request to restore purchases finished successfully.");
                Log("Total restored products: " + transactions.Length);
                for (int iter = 0; iter < transactions.Length; iter++)
                {
                    var     transaction = transactions[iter];
                    Log(string.Format("[{0}]: {1}", iter, transaction.Payment.ProductId));
                }
            }
            else
            {
                Log("Request to restore purchases failed with error. Error: " +  error);
            }
        }

        #endregion

        #region Private methods

        private int GetCurrentProductIndex()
        {
            return m_productIndex;
        }

        private void SetCurrentProductIndex(int index)
        {
            var     products    = BillingServices.Products;
            m_productIndex      = Mathf.Clamp(index, 0, products.Length);
        }

        private IBillingProduct GetCurrentProduct()
        {
            var     products    = BillingServices.Products;
            if (products.Length > 0)
            {
                int     index   = GetCurrentProductIndex();
                return  products[index];
            }

            return null;
        }

        #endregion

        #region Misc methods

        private void SetIsReady(bool active)
        {
            foreach (RectTransform rect in m_productsDependentObjects)
            {
                rect.gameObject.SetActive(active);
            }
        }

        private void CreateProductOutlets(IBillingProduct[] products)
        {
            var     items   = (products == null) ? null : Array.ConvertAll(products, (item) => item.Id);
            DemoHelper.CreateItems(m_yourProductsNode, m_productPrefab, items, SetCurrentProductIndex);
        }

        private void LogSelectProduct()
        {
            Log("Product not selected.");
        }

        #endregion
    }
}

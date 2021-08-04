using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Simulator
{
    public sealed class BillingStoreSimulator : SingletonObject<BillingStoreSimulator>
    {
        #region Fields

        private                 BillingProductData[]        m_products;

        private     readonly    BillingStoreSimulatorData   m_simulatorData     = null;
        
        #endregion

        #region Constructors

        private BillingStoreSimulator()
        {
            // set properties
            m_products          = null;
            m_simulatorData     = LoadFromDisk() ?? new BillingStoreSimulatorData();
        }

        #endregion

        #region Create methods

        private static BillingProductData CreateProduct(BillingProductDefinition productDefinition)
        {
            return new BillingProductData(productDefinition.Id, productDefinition.Title, "0.99", "USD", productDefinition.ProductType);
        }

        private static BillingTransactionData CreateTransaction(string productId, int quantity, string user, BillingTransactionState transactionState, Error error = null)
        {
            return new BillingTransactionData(productId, quantity, user, "transactionId", DateTime.Now, transactionState, error);
        }

        #endregion

        #region Public methods

        public void GetProducts(BillingProductDefinition[] productDefinitions, Action<BillingProductData[], Error> callback)
        {
            BillingProductData[]    storeProducts   = null;
            Error                   error           = null;

            // check input
            if (productDefinitions.Length == 0)
            {
                error           = new Error(description: "The operation could not be completed because product list is empty.");
            }
            else
            {
                // create product info
                storeProducts   = Array.ConvertAll(productDefinitions, (item) => CreateProduct(item));
            }

            // update cache
            m_products          = storeProducts;

            // send result
            callback(storeProducts, error);
        }

        public bool BuyProduct(string productId, int quantity, string user, Action<BillingTransactionData> callback)
        {
            // check input
            var     product     = FindProductWithId(productId);
            if (product == null)
            {
                callback(CreateTransaction(productId, quantity, user, BillingTransactionState.Failed, new Error(description: Response.kProductPurchasedError)));
                return false;
            }

            // check whether item is purchased
            if (m_simulatorData.IsProductPurchased(product.Id) && product.ProductType == BillingProductType.NonConsumable)
            {
                callback(CreateTransaction(productId, quantity, user, BillingTransactionState.Failed, new Error(description: Response.kProductPurchasedError)));
                return true;
            }

            // confirm with user
            string  message         = string.Format("Do you want to buy {0} for {1}?", product.LocalizedTitle, product.LocalizedPrice);
            var     newAlertDialog  = new AlertDialogBuilder()
                .SetTitle("Confirm your purchase")
                .SetMessage(message)
                .AddButton("Ok", () => 
                {
                    // update purchase data
                    if (product.ProductType == BillingProductType.NonConsumable)
                    {
                        m_simulatorData.AddProductToPurchaseHistory(product.Id);
                        SaveData();
                    }

                    // send result
                    callback(CreateTransaction(productId, quantity, user, BillingTransactionState.Purchased));
                })
                .AddCancelButton("Cancel", () => 
                { 
                    // send result
                    callback(CreateTransaction(productId, quantity, user, BillingTransactionState.Failed, new Error(description: Response.kUserCancelledError)));
                })
                .Build();
            newAlertDialog.Show();

            return true;
        }

        public void RestorePurchases(string applicationUsername, Action<BillingTransactionData[], Error> callback)
        {
            if (m_products == null)
            {
                callback(null, new Error(description: Response.kNotInitialisedError));
                return;
            }

            var     purchasedIds        = m_simulatorData.GetPurchasedProductIds();
            var     transactions        = new List<BillingTransactionData>();

            for (int iter = 0; iter < purchasedIds.Length; iter++)
            {
                string  productId       = purchasedIds[iter];
                var     product         = FindProductWithId(productId);
                if (product != null)
                {
                    var     transaction = CreateTransaction(productId, 1, applicationUsername, BillingTransactionState.Restored);
                    transactions.Add(transaction);
                }
            }
            callback(transactions.ToArray(), null);
        }

        #endregion

        #region Database methods

        private BillingStoreSimulatorData LoadFromDisk()
        {
            return SimulatorDatabase.Instance.GetObject<BillingStoreSimulatorData>(NativeFeatureType.kBillingServices);
        }

        private void SaveData()
        {
            SimulatorDatabase.Instance.SetObject(NativeFeatureType.kBillingServices, m_simulatorData);
        }

        public static void Reset() 
        {
            SimulatorDatabase.Instance.RemoveObject(NativeFeatureType.kBillingServices);
        }

        #endregion

        #region Private methods

        private BillingProductData FindProductWithId(string id)
        {
            return Array.Find(m_products, (item) => item.Id == id);
        }

        #endregion

        #region Nested types

        public class Response
        {
            public const string kProductNotFoundError   = "The operation could not be completed because given product id information not found.";

            public const string kProductPurchasedError  = "The operation could not be completed because requested product is purchased already.";

            public const string kUserCancelledError     = "The operation could not be completed because user cancelled purchase.";

            public const string kNotInitialisedError    = "The operation could not be completed because user cancelled purchase.";
        }

        #endregion
    }
}
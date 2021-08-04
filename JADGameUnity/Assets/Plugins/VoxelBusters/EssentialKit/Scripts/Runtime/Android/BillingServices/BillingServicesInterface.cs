#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    public sealed class BillingServicesInterface : NativeBillingServicesInterfaceBase
    {
        #region Static fields

        private static  NativeGoogleBillingServices    m_instance;

        #endregion

        #region Constructors

        public BillingServicesInterface()
            : base(isAvailable: true)
        {
            try
            {
                Debug.Log("Creating Billing Services Interface : Android");
                m_instance = new NativeGoogleBillingServices(NativeUnityPluginUtility.GetContext());
                BillingServicesUnitySettings settings = BillingServices.UnitySettings;

                m_instance.Initialise(settings.AndroidProperties.PublicKey, new NativeBillingTransactionStateListener()
                {
                    onStartedCallback = (nativeTransaction) =>
                    {
                        BillingTransaction transaction = new BillingTransaction(nativeTransaction);
                        //#warning "Transaction id's can be empty"
                        SendPaymentStateChangeEvent(new BillingTransaction[] { transaction });
                    },
                    onUpdatedCallback = (nativeTransaction) =>
                    {
                        BillingTransaction transaction = new BillingTransaction(nativeTransaction);
                        SendPaymentStateChangeEvent(new BillingTransaction[] { transaction });
                    },
                    onFailedCallback = (nativeTransaction, error) =>
                   {
                       BillingTransaction transaction = new BillingTransaction(nativeTransaction, new Error(error));
                       SendPaymentStateChangeEvent(new BillingTransaction[] { transaction });
                   }
                });
                
                SetProducts(settings.Products);
            }
            catch (Exception e)
            {
                Debug.LogError("Exception : " + e.Message + "   \n  " + e.StackTrace.ToString());
                     
            }
        }

        #endregion

        #region Base class methods

        public override bool CanMakePayments()
        {
            return m_instance.CanMakePayments();
        }

        public override void RetrieveProducts(BillingProductDefinition[] productDefinitions)
        {
            SetProducts(productDefinitions);
            m_instance.FetchProductDetails(new NativeFetchBillingProductsListener()
            {
                onSuccessCallback = (NativeList<NativeBillingProduct> nativeList, NativeArrayBuffer<string> invalidIds) =>
                {
                    //Filter billing products which are listed in the settings only
                    List<NativeBillingProduct> nativeBillingProducts = nativeList.Get();
                    List<BillingProduct> filteredBillingProducts = new List<BillingProduct>();

                    foreach (NativeBillingProduct each in nativeBillingProducts)
                    {
                        var settings = BillingServices.FindProductDefinitionWithPlatformId(each.GetProductIdentifier());
                        if (settings != null)
                        {
                            filteredBillingProducts.Add(new BillingProduct(settings.Id, each, settings.Tag));
                        }
                    }

                    BillingProduct[] products = filteredBillingProducts.ToArray();
                    SendRetrieveProductsCompleteEvent(products, invalidIds.GetArray(), null);
                },
                onFailureCallback = (string error) =>
                {
                    SendRetrieveProductsCompleteEvent(null, null, new Error(error));
                }
            });
        }

        public override bool StartPayment(IBillingPayment payment, out Error error)
        {
            // set default value for reference
            error   = null;
            //#warning "Need to confirm : Reason for returning boolean in buy product call"
            m_instance.BuyProduct(payment.ProductPlatformId, payment.Tag);
            return true;
        }

        public override IBillingTransaction[] GetTransactions()
        {
            NativeList<NativeBillingTransaction> nativeList = m_instance.GetIncompleteBillingTransactions();
            BillingTransaction[] transactions = NativeUnityPluginUtility.Map<NativeBillingTransaction, BillingTransaction>(nativeList.Get());

            return transactions;
        }

        public override void FinishTransactions(IBillingTransaction[] transactions)
        {
            foreach(IBillingTransaction each in transactions)
            {
                if (!string.IsNullOrEmpty(each.Id)) //Id will be null for invalid or cancelled transactions
                {
                    m_instance.FinishBillingTransaction(each.Id, each.ReceiptVerificationState == BillingReceiptVerificationState.Success);
                }
            }
        }

        public override void RestorePurchases(string applicationUsername)
        {
            m_instance.RestorePurchases(applicationUsername, new NativeRestorePurchasesListener()
            {
                onSuccessCallback = (nativeTransactions) =>
                {
                    BillingTransaction[] transactions = NativeUnityPluginUtility.Map<NativeBillingTransaction, BillingTransaction>(nativeTransactions.Get());
                    SendRestorePurchasesCompleteEvent(transactions, null);
                },
                onFailureCallback = (error) =>
                {
                    SendRestorePurchasesCompleteEvent(null, new Error(error));
                }
            });
        }

        public override bool TryClearingUnfinishedTransactions()
        {
            return m_instance.TryClearingIncompleteTransactions();
        }

        #endregion

        #region Helpers

        private void SetProducts(BillingProductDefinition[] productDefinitions)
        {
            BillingProductDefinition[] consumableProducts = productDefinitions.Where(item => item.ProductType == BillingProductType.Consumable).ToArray();
            var consumableProductIds = Array.ConvertAll(consumableProducts, (item) => item.GetPlatformIdForActivePlatform());

            BillingProductDefinition[] nonConsumableProducts = productDefinitions.Where(item => item.ProductType == BillingProductType.NonConsumable).ToArray();
            var nonConsumableProductIds = Array.ConvertAll(nonConsumableProducts, (item) => item.GetPlatformIdForActivePlatform());

            m_instance.SetProducts(consumableProductIds, nonConsumableProductIds, null);
        }

        #endregion
    }
}
#endif
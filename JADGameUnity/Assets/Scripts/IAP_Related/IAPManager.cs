using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using TMPro;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private string noAdsKey = "noAdsKey";
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI boltText;

    public GameObject noAdsButton;
    //Step 1 create your products

   // private string removeAds = "remove_ads_lifesadrag";
    private const string coins1000 = "coins1000rprt";
    private const string coins10000 = "coins10000rprt";
    private const string coins100000 = "coins100000rprt";
    private const string bolts100 = "bolts100rprt";
    private const string bolts500 = "bolts500rprt";
    private const string bolts1000 = "bolts1000rprt";

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable


        //builder.AddProduct(removeAds, ProductType.NonConsumable);
        builder.AddProduct(coins1000, ProductType.Consumable);
        builder.AddProduct(coins10000, ProductType.Consumable);
        builder.AddProduct(coins100000, ProductType.Consumable);
        builder.AddProduct(bolts100, ProductType.Consumable);
        builder.AddProduct(bolts500, ProductType.Consumable);
        builder.AddProduct(bolts1000, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods

    public void buyRemoveAds()
    {
        //BuyProductID(removeAds);
    }

    public void buyCoins1000()
    {
         BuyProductID(coins1000);
    }

    public void buyCoins10000()
    {
        BuyProductID(coins10000);
    }

    public void buyCoins100000()
    {
        BuyProductID(coins100000);
    }

    public void buyBolts100()
    {
        BuyProductID(bolts100);
    }
    public void buyBolts500()
    {
        BuyProductID(bolts500);
    }
    public void buyBolts1000()
    {
        BuyProductID(bolts1000);
    }


    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Copy this if statement and change 'removeAds' to whatever else we are purchasing for other purchase options.

        /*
        if (String.Equals(args.purchasedProduct.definition.id, removeAds, StringComparison.Ordinal))
        {
            //Need a reference to playerprefs or something to ensure that player now has no ads.
            Debug.Log("Remove ads successful!");
            PlayerPrefs.SetInt(noAdsKey, 1);
            if(noAdsButton != null)
            {
                noAdsButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                noAdsButton = GameObject.Find("Remove_Ads_Button");
                if (noAdsButton != null)
                {
                    noAdsButton.GetComponent<Button>().interactable = false;
                }
                else
                {
                    Debug.LogWarning("Hey, the button isn't assigned!");
                }
                
            }
            GameObject tempBannerAds = GameObject.Find("AdsManager");
            tempBannerAds.GetComponent<Banner_Ads>().hideBanner();

        }
        else
        {
            Debug.Log("Purchase Failed");
        }
        */

        //1000 Coins
        if (String.Equals(args.purchasedProduct.definition.id, coins1000, StringComparison.Ordinal))
        {
            //Need a reference to playerprefs or something to ensure that player now has no ads.
            Debug.Log("You bought 1000 coins!");

            //Give player 1000 coins...Collectmanager?
            //CHECK IF THIS GETS CALLED EVERYTIME...WE DON'T WANT TO KEEP SPAMMING THE PLAYER WITH 1000...THOUGH THIS IS A CONSUMABLE SO I DON'T THINK IT SHOULD.
            Collect_Manager.instance.totalCoins += 1000;
            Save_System.SaveCollectables(Collect_Manager.instance);
            coinText.text = ": " + Collect_Manager.instance.totalCoins.ToString();

        }
        else
        {
            Debug.Log("Purchase Failed");
        }

        //10000 Coins
        if (String.Equals(args.purchasedProduct.definition.id, coins10000, StringComparison.Ordinal))
        {
            //Need a reference to playerprefs or something to ensure that player now has no ads.
            Debug.Log("You bought 10000 coins! Got 1000 bonus!");

        
            Collect_Manager.instance.totalCoins += 11000;
            Save_System.SaveCollectables(Collect_Manager.instance);
            coinText.text = ": " + Collect_Manager.instance.totalCoins.ToString();

        }
        else
        {
            Debug.Log("Purchase Failed");
        }

        //100000 Coins
        if (String.Equals(args.purchasedProduct.definition.id, coins100000, StringComparison.Ordinal))
        {
            //Need a reference to playerprefs or something to ensure that player now has no ads.
            Debug.Log("You bought 100000 coins! Got 10000 bonus!");


            Collect_Manager.instance.totalCoins += 110000;
            Save_System.SaveCollectables(Collect_Manager.instance);
            coinText.text = ": " + Collect_Manager.instance.totalCoins.ToString();

        }
        else
        {
            Debug.Log("Purchase Failed");
        }

        //100 Bolts
        if (String.Equals(args.purchasedProduct.definition.id, bolts100, StringComparison.Ordinal))
        {
            //Need a reference to playerprefs or something to ensure that player now has no ads.
            Debug.Log("You bought 100 bolts!");

            Collect_Manager.instance.totalBolts += 100;
            Save_System.SaveCollectables(Collect_Manager.instance);
            boltText.text = ": " + Collect_Manager.instance.totalBolts.ToString();

        }
        else
        {
            Debug.Log("Purchase Failed");
        }

        //500 Bolts
        if (String.Equals(args.purchasedProduct.definition.id, bolts500, StringComparison.Ordinal))
        {
            //Need a reference to playerprefs or something to ensure that player now has no ads.
            Debug.Log("You bought 500 bolts! Got 100 bonus bolts!");

            Collect_Manager.instance.totalBolts += 600;
            Save_System.SaveCollectables(Collect_Manager.instance);
            boltText.text = ": " + Collect_Manager.instance.totalBolts.ToString();

        }
        else
        {
            Debug.Log("Purchase Failed");
        }


        //1000 Bolts
        if (String.Equals(args.purchasedProduct.definition.id, bolts1000, StringComparison.Ordinal))
        {
            //Need a reference to playerprefs or something to ensure that player now has no ads.
            Debug.Log("You bought 1000 bolts! Got 500 bonus bolts!");

            Collect_Manager.instance.totalBolts += 1500;
            Save_System.SaveCollectables(Collect_Manager.instance);
            boltText.text = ": " + Collect_Manager.instance.totalBolts.ToString();

        }
        else
        {
            Debug.Log("Purchase Failed");
        }

        return PurchaseProcessingResult.Complete;
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                //Save to the file.
                Save_System.SaveCollectables(Collect_Manager.instance);
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
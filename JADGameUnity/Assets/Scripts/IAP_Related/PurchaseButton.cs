﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public GameObject dialougeBox;
    public GameObject noAdsButton;
    public enum PurchaseType
    {
        coins1000,
        coins10000
    }
    public PurchaseType purchaseType;


    /*
    void Awake()
    {
        switch (purchaseType)
        {
            case PurchaseType.removeAds:
                {
                    if(PlayerPrefs.GetInt(noAdsKey) > 0)
                    {
                        this.GetComponent<Button>().interactable = false;
                    }
                    break;
                }
            default:
                {
                    Debug.Log("called default!");
                    break;
                }

        }
    }
     */
    public void clickPurchaseButton()
    {
        switch (purchaseType)
        {
            case PurchaseType.coins1000:
                {
                    IAPManager.instance.buyCoins1000();
                    break;
                }
            case PurchaseType.coins10000:
                {
                    IAPManager.instance.buyCoins10000();
                    break;
                }
            default:
                {
                    Debug.LogWarning("called default in IAP purchase button!");
                    break;
                }
               
        }
    }
}
//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

//This script populates the shop for the current category. I.E. Items, currency, skins, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Vertical_Layout_Formatter : MonoBehaviour
{

    public enum WhichTabAreWeOn
    {
        Items,
        Skins,
        Currency
    }



    public WhichTabAreWeOn thisTab;
    [Tooltip("Put in any new items here that will be in the shop. These should be scriptable objects.")]
    //Item should consist of an image of the item, description of the item in text form, and the price...Ex: x100 -coinimage- or x200 -gemimage-
    public List<Shop_Item> itemsForSale;

    [Tooltip("The item prefab that will be a child of the VertLayout.")]
    public GameObject itemPrefab;

    public ScrollRect scrollArea;

    List<TextMeshProUGUI> tempTextMeshList = new List<TextMeshProUGUI>();

    public TextMeshProUGUI coinsTotalText;
    public TextMeshProUGUI gemsTotalText;

    //Skin related purchase stuff.
    public Button purchaseWithCoinsButton;
    public Button purchaseWithBoltsButton;
    public GameObject skinPurchasePanel;
    public Button closeShopButton;

    private void Start()
    {
        coinsTotalText.text = ": " + Collect_Manager.instance.totalCoins.ToString();
        gemsTotalText.text = ": " + Collect_Manager.instance.totalBolts.ToString();


        for (int i = 0; i < itemsForSale.Count; i++)
        {
            GameObject tempSpawn = Instantiate(itemPrefab, this.transform);
            tempSpawn.transform.SetParent(this.transform);

            Image tempImg = tempSpawn.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI tempName = tempSpawn.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI tempDesc = tempSpawn.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            GameObject tempPurchaseButton = tempSpawn.transform.GetChild(3).gameObject;
            TextMeshProUGUI tempPlayerOwnedText = tempSpawn.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            tempTextMeshList.Add(tempPlayerOwnedText);
            Image tempCoinImg = tempSpawn.transform.GetChild(5).GetComponent<Image>();
            TextMeshProUGUI tempCoinCostText = tempSpawn.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
            Image tempGemImg = tempSpawn.transform.GetChild(7).GetComponent<Image>();
            TextMeshProUGUI tempGemCostText = tempSpawn.transform.GetChild(8).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI tempCurrencyCostText = tempSpawn.transform.GetChild(9).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI tempORText = tempSpawn.transform.GetChild(10).GetComponent<TextMeshProUGUI>();

            tempImg.sprite = itemsForSale[i].itemImage;
            tempName.text = itemsForSale[i].theName;
            tempDesc.text = itemsForSale[i].description;
            tempCoinCostText.text = itemsForSale[i].coinPrice.ToString();
            tempGemCostText.text = itemsForSale[i].boltPrice.ToString();


            //This part should probably also only be if we're on currency...not sure yet.
            Button tempButtonAddListener = tempPurchaseButton.GetComponent<Button>();
            
            //Debug.Log("The coin price of this item is: " + itemsForSale[i].coinPrice);
            //Debug.Log("The gem price of this item is: " + itemsForSale[i].gemPrice);

            if(thisTab == WhichTabAreWeOn.Items)
            {
                int tempNum = i;
                tempButtonAddListener.onClick.AddListener(() => Collect_Manager.instance.purchaseItemWithCoins(itemsForSale[tempNum].coinPrice, itemsForSale[tempNum].theItem));
                tempButtonAddListener.onClick.AddListener(() => changeText(tempNum));


                //These fields are not applicable to items.
                tempGemImg.gameObject.SetActive(false);
                tempGemCostText.gameObject.SetActive(false);
                tempCurrencyCostText.gameObject.SetActive(false);

        
                tempPlayerOwnedText.text = "You own: " + Collect_Manager.instance.numPlayerOwns(itemsForSale[i].theItem);
                tempORText.gameObject.SetActive(false);


            }
            else if(thisTab == WhichTabAreWeOn.Skins)
            {
                //Need to add listener to the button for skins.
                int tempNum = i;
                tempButtonAddListener.onClick.AddListener(() => addListenerForSkins(itemsForSale[tempNum], tempButtonAddListener));
                //tempButtonAddListener.onClick.AddListener(() => changeText(tempNum));

                foreach (int j in System.Enum.GetValues(typeof(Collect_Manager.skinTypes)))
                {
                    //Cast the enum to an integer to compare it.
                    if ((int)itemsForSale[i].thisSkinType == j)
                    {
                        for(int k = 0; k < Collect_Manager.instance.skinsUnlocked.Count; k++)
                        {
                            //Checking if we have the skin unlocked already.
                            if(Collect_Manager.instance.skinsUnlocked[k] == j)
                            {
                                Debug.Log("Current skin number is: " + j + " Which corresponds to: " + itemsForSale[i].thisSkinType);
                                Debug.Log("We are skipping putting this skin in the menu. Turn off everything corresponding to this skin since player has it already.");
                                tempSpawn.SetActive(false);
                                break;
                            }
                        }
                       
                        break;
                    }
                }

                tempCurrencyCostText.gameObject.SetActive(false);
                tempPlayerOwnedText.gameObject.SetActive(false);
            }
            else if(thisTab == WhichTabAreWeOn.Currency)
            {
                //We need a way to tell it to add the purchase_button script and know which currency to purchase.

                tempSpawn.SetActive(false);
                tempGemImg.gameObject.SetActive(false);
                tempCoinImg.gameObject.SetActive(false);
                tempCoinCostText.gameObject.SetActive(false);
                tempGemCostText.gameObject.SetActive(false);
                tempPlayerOwnedText.gameObject.SetActive(false);
                tempORText.gameObject.SetActive(false);
                tempCurrencyCostText.text = "$" + itemsForSale[i].currencyUSD + " USD"; 
            }


            
        }

        ScrollToTop();
    }

    //Update the text when the player buys an item.
    public void changeText(int indexOfItem)
    {
        tempTextMeshList[indexOfItem].text = "You own: " + Collect_Manager.instance.numPlayerOwns(itemsForSale[indexOfItem].theItem);
        coinsTotalText.text = ": " + Collect_Manager.instance.totalCoins.ToString();
        gemsTotalText.text = ": " + Collect_Manager.instance.totalBolts.ToString();
    }

    public void ScrollToTop()
    {
        scrollArea.normalizedPosition = new Vector2(0, 1);
    }

    public void addListenerForSkins(Shop_Item theItemForSale, Button buyButton)
    {
        skinPurchasePanel.SetActive(true);
        closeShopButton.interactable = false;
        purchaseWithCoinsButton.onClick.RemoveAllListeners();
        purchaseWithBoltsButton.onClick.RemoveAllListeners();
        purchaseWithCoinsButton.onClick.AddListener(() => Collect_Manager.instance.purchaseSkin(theItemForSale.thisSkinType, theItemForSale.coinPrice, theItemForSale.boltPrice, false, buyButton));
        purchaseWithBoltsButton.onClick.AddListener(() => Collect_Manager.instance.purchaseSkin(theItemForSale.thisSkinType, theItemForSale.coinPrice, theItemForSale.boltPrice, true, buyButton));

    }

    public void turnStuffOn()
    {
        closeShopButton.interactable = true;
        skinPurchasePanel.SetActive(false);
    }


}

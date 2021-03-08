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

    private void Start()
    {
        for(int i = 0; i < itemsForSale.Count; i++)
        {
            GameObject tempSpawn = Instantiate(itemPrefab, this.transform);
            tempSpawn.transform.SetParent(this.transform);

            Image tempImg = tempSpawn.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI tempName = tempSpawn.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI tempDesc = tempSpawn.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            GameObject tempPurchaseButton = tempSpawn.transform.GetChild(3).gameObject;
            TextMeshProUGUI tempPlayerOwnedText = tempSpawn.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
            tempTextMeshList.Add(tempPlayerOwnedText);

            tempImg.sprite = itemsForSale[i].itemImage;
            tempName.text = itemsForSale[i].theName;
            tempDesc.text = itemsForSale[i].description;

            //This part should probably also only be if we're on currency...not sure yet.
            Button tempButtonAddListener = tempPurchaseButton.GetComponent<Button>();
            int tempNum = i;
            tempButtonAddListener.onClick.AddListener( () => Menu_Manager.instance.purchaseItemWithCoins(itemsForSale[tempNum].coinPrice, itemsForSale[tempNum].collectManagerNumString));
            tempButtonAddListener.onClick.AddListener(() => changeText(tempNum));

            //Debug.Log("The coin price of this item is: " + itemsForSale[i].coinPrice);
            //Debug.Log("The gem price of this item is: " + itemsForSale[i].gemPrice);

            if (thisTab != WhichTabAreWeOn.Currency)
            {
                //Have a switch statement and then based on the currency amount we'll have diff buttons.
                if (itemsForSale[i].collectManagerNumString != null)
                {
                    tempPlayerOwnedText.text = "You own: " + Collect_Manager.instance.numPlayerOwns(itemsForSale[i].collectManagerNumString);
                }
                else
                {
                    Debug.LogWarning("String for item " + i + " is null!");
                }
            }

            
        }

        ScrollToTop();
    }

    //Update the text when the player buys an item.
    public void changeText(int indexOfItem)
    {
        tempTextMeshList[indexOfItem].text = "You own: " + Collect_Manager.instance.numPlayerOwns(itemsForSale[indexOfItem].collectManagerNumString);
    }

    public void ScrollToTop()
    {
        scrollArea.normalizedPosition = new Vector2(0, 1);
    }


}

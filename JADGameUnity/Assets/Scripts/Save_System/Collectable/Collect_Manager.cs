//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.


//This script will be attached to a manager that persists throughout the game. 
//Stats related to collecting items, coins, gems, skins, etc will be updated here and used to save via other scripts.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collect_Manager : MonoBehaviour
{
    [Header("Currency")]
    [Tooltip("Total amount of coin currency the player has.")]
    public int totalCoins;
    [Tooltip("Total amount of gem currency the player has.")]
    public int totalBolts;

    [Header("Items")]
    //Numbers of each item the player has.
    public int handWarmers;
    public int numDefrosters;
    public int numFireVests;
    public int numLiquidNitrogenCanisters;
    public int numNeutralTablets;

    public enum typeOfItem
    {
        HandWarmers,
        Defroster,
        FireVest,
        LiquidNitrogenCanister,
        NeutralTablet,
        //DEBUG
        none
    }

    public enum skinTypes
    {
        AverageJoe,
        AverageJoanne,
        Elincia,
        Crek,
        Kitsune,
        SheriffShepard,
        BUKU1000M,
        OldManJunkins,
        Masquerade,
        MysterySubject,
    }

    [Header("Loadout")]
    //Loadout...Save as Int and we can figure out the corresponding enum index based on that. 0 = HandWarmers, 1 = defroster, etc.
    //We will make this -1 if no item is selected.
    public int item1;
    public int item2;
    public int item3;


    [Header("Score related")]
    public int highScore;
    public int highestWave;
    public int mostWavesSurvived;
    public int totalWavesSurvived;


    [Header("Skin related")]
    //Index of the current skin that the player had been using during last session.
    public int currentSkin;

    //Array storing which skin indexes are unlocked. 0 and 2 (Male and female default) unlocked by default.
    public List<int> skinsUnlocked = new List<int>();

    [Header("Things to reference throughout the game.")]
    [Tooltip("Current skins: AverageJoe,AverageJoanne,Elincia,Crek, Crek,Kitsune,SheriffShepard,BUKU1000M,Dummy, ")]
    public List<SkinInfo> skinsToPick;
    public List<Shop_Item> itemsToPick;
    public bool isMuted;
    public bool tutCompleted;
    public bool firstTimePlaying;

    [Header("External checks")]
    public bool disclaimerDisplayIOSDone;

    [Header("SFX")]
    public AudioClip shopBuySound;
    public AudioClip shopErrorSound;
    public AudioClip equipUnequipSound;

    [Header("DEBUG")]
    [SerializeField] Level_Manager.timePeriod debugPeriod = Level_Manager.timePeriod.None;

    [Header("Non-Saving variables")]
    [SerializeField] bool firstTimeBootingUp;

   

    [Header("Singleton")]
    public static Collect_Manager instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            firstTimeBootingUp = false;
            LoadCollectableData();
            Debug.Log("Save file will be located in: " + Application.persistentDataPath);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        
      //  Universal_Dialouge_Box.instance.activatePopup("Test.");
        //Debug.Log(System.Enum.GetValues(typeof(skinTypes)).Length);

    }

    //Loading from a file...
    void LoadCollectableData()
    {
        CollectableData collect = Save_System.LoadCollectables();



        if (collect != null)
        {
            totalCoins = collect.totalCoins;
            totalBolts = collect.totalBolts;
            handWarmers = collect.handWarmers;
            numDefrosters = collect.numDefrosters;
            numFireVests = collect.numFireVests;
            numLiquidNitrogenCanisters = collect.numLiquidNitrogenCanisters;
            numNeutralTablets = collect.numNeutralTablets;

            currentSkin = collect.currentSkin;
            item1 = collect.item1;
            item2 = collect.item2;
            item3 = collect.item3;


            highScore = collect.highScore;
            highestWave = collect.highestWave;
            mostWavesSurvived = collect.mostWavesSurvived;
            totalWavesSurvived = collect.totalWavesSurvived;
            isMuted = collect.isMuted;
            tutCompleted = collect.tutCompleted;
            firstTimePlaying = collect.firstTimePlaying;
            disclaimerDisplayIOSDone = collect.disclaimerDisplayIOSDone;
            for (int i = 0; i < collect.skinsUnlocked.Count; i++)
            {
                skinsUnlocked.Add(collect.skinsUnlocked[i]);
            }

         
            //Check against the cloud.
            if(Cloud_Saving.instance != null)
            {
                if(Cloud_Saving.instance.myCloudData != null)
                {
                    if (Cloud_Saving.instance.myCloudData.totalCoins != totalCoins)
                    {
                        totalCoins = (int)Cloud_Saving.instance.myCloudData.totalCoins;
                    }
                }
           
            }

            /*
            //test
            if(Cloud_Saving.instance != null)
            {
                if(Cloud_Saving.instance.cloudTotalCoinsTest != totalCoins)
                {
                    Universal_Dialouge_Box.instance.activatePopup("The coins on the server were: " + Cloud_Saving.instance.cloudTotalCoinsTest + " so we have updated the local save file to match that value.");
                    totalCoins = Cloud_Saving.instance.cloudTotalCoinsTest;
                 
                }
                else
                {
                    Universal_Dialouge_Box.instance.activatePopup("The coins on the server were: " + Cloud_Saving.instance.cloudTotalCoinsTest + " And our local save is equal to that.");
                }
            }
            //test
            */
        }
        //IF THERE IS NO SAVE FILE, THIS WILL BE CALLED.
        else
        {
            currentSkin = 0;
            //Player will have default skin male & female unlocked no matter what.
            skinsUnlocked.Add(0);
            skinsUnlocked.Add(1);

            //Default the loadout to -1's so we don't put any items in the loadout.
            item1 = -1;
            item2 = -1;
            item3 = -1;
            tutCompleted = false;

            //DEBUG STATEMENT, DON'T GIVE THEM 50K AT START LMAO.
            Debug.LogWarning("Collect is null, we probably don't have a save file! Setting skin to default.");
        }
        

    }

    //Add to this function as we add more items into the game.
    public int numPlayerOwns(typeOfItem typePassed)
    {
        switch(typePassed)
        {
            case typeOfItem.HandWarmers:
                {
                    return handWarmers;
                    break;
                }
            case typeOfItem.Defroster:
                {
                    return numDefrosters;
                    break;
                }
            case typeOfItem.FireVest:
                {
                    return numFireVests;
                    break;
                }
            case typeOfItem.LiquidNitrogenCanister:
                {
                    return numLiquidNitrogenCanisters;
                    break;
                }
            case typeOfItem.NeutralTablet:
                {
                    return numNeutralTablets;
                    break;
                }
            default:
                {
                    return -1;
                    break;
                }

        }
    }


    public void setCurrentSkin(skinTypes theSkin)
    {
        //Debug.Log(System.Enum.GetValues(typeof(skinTypes)).Length);

        foreach(int i in System.Enum.GetValues(typeof(skinTypes)))
        {
            //Cast the enum to an integer to compare it.
            if((int)theSkin == i)
            {
                Debug.Log("Current skin number is: " + i + " Which corresponds to: " + theSkin);
                currentSkin = i;
                break;
            }
        }
    }

    public int getCurrentSkin()
    {
        return currentSkin;
    }

    //Shop related stuff.

    //Function for increasing item when it is purchased.
    public bool purchaseItemConfirm(typeOfItem typePassed)
    {
        bool have99OrNah = false;
        switch (typePassed)
        {
            case typeOfItem.HandWarmers:
                {
                    if(handWarmers >= 99)
                    {
                        Debug.Log("We already have 99 of this item. Can't get anymore!");
                        have99OrNah = true;
                        handWarmers = 99;
                        break;
                    }
                    handWarmers += 1;
                    break;
                }
            case typeOfItem.Defroster:
                {
                    if (numDefrosters >= 99)
                    {
                        Debug.Log("We already have 99 of this item. Can't get anymore!");
                        have99OrNah = true;
                        numDefrosters = 99;
                        break;
                    }
                    numDefrosters += 1;
                    break;
                }
            case typeOfItem.FireVest:
                {
                    if (numFireVests >= 99)
                    {
                        Debug.Log("We already have 99 of this item. Can't get anymore!");
                        have99OrNah = true;
                        numFireVests = 99;
                        break;
                    }
                    numFireVests += 1;
                    break;
                }
            case typeOfItem.LiquidNitrogenCanister:
                {
                    if (numLiquidNitrogenCanisters >= 99)
                    {
                        Debug.Log("We already have 99 of this item. Can't get anymore!");
                        have99OrNah = true;
                        numLiquidNitrogenCanisters = 99;
                        break;
                    }
                    numLiquidNitrogenCanisters += 1;
                    break;
                }
            case typeOfItem.NeutralTablet:
                {
                    if (numNeutralTablets >= 99)
                    {
                        Debug.Log("We already have 99 of this item. Can't get anymore!");
                        have99OrNah = true;
                        numNeutralTablets = 99;
                        break;
                    }
                    numNeutralTablets += 1;
                    break;
                }
            default:
                {
                    Debug.LogWarning("Hey! We are in the default switch for purchaseItemConfirm()!");
                    break;
                }

        }
        return have99OrNah;
    }


    public void purchaseItemWithCoins(int cost, typeOfItem typePassed)
    {
        int playerCoins = totalCoins;

        if (playerCoins >= cost)
        {
           
            if(!purchaseItemConfirm(typePassed))
            {
                Audio_Manager.Instance.playSFX(shopBuySound);
                totalCoins -= cost;
             //   Debug.Log("You just bought: " + typePassed);
                Menu_Manager.instance.setupShopTextAnimation("You just bought: " + typePassed.ToString() + " x1!");
            }
            else
            {
                Audio_Manager.Instance.playSFX(shopErrorSound);
                Menu_Manager.instance.setupShopTextAnimation("You already have: 99 " + typePassed.ToString() + "!");
            }
          
           // purchaseItemConfirm(typePassed);

            //Save the purchase!
            Save_System.SaveCollectables(this);
        }
        else
        {
            Audio_Manager.Instance.playSFX(shopErrorSound);
            int tempDifference = cost - playerCoins;
           // Debug.Log("Hey, you don't have enough coins for this! Your total coins are: " + playerCoins.ToString());
            if(cost - totalCoins == 1)
            {
                Menu_Manager.instance.setupShopTextAnimation("You need " + tempDifference.ToString() + " more coin to purchase this item.");
            }
            else
            {
                Menu_Manager.instance.setupShopTextAnimation("You need " + tempDifference.ToString() + " more coins to purchase this item.");
            }
            
        }

    }

    public void purchaseSkin(skinTypes theSkin, int coinCost, int boltCost, bool coinsOrBolts, Button skinBuyButton, TextMeshProUGUI playerCurrencyText)
    {

        foreach (int i in System.Enum.GetValues(typeof(skinTypes)))
        {
            //Cast the enum to an integer to compare it.
            if ((int)theSkin == i)
            {
                //Check method that player is using to purchase the skin:
                //If 0, = coins, 1 = bolts for coinsOrBolts.
                if (!coinsOrBolts)
                {
                    if(coinCost <= totalCoins)
                    {
                        Audio_Manager.Instance.playSFX(shopBuySound);
                        skinsUnlocked.Add(i);
                     //   Debug.Log("You just bought skin number: " + i + " Which corresponds to: " + theSkin);
                        Menu_Manager.instance.setupShopTextAnimation("You can now play as: " + theSkin.ToString() + "!");
                        totalCoins -= coinCost;
                        playerCurrencyText.text = ": " + totalCoins;
                        skinBuyButton.interactable = false;
                    }
                    else
                    {
                        Audio_Manager.Instance.playSFX(shopErrorSound);
                        int tempDifference = coinCost - totalCoins;
                     //   Debug.LogWarning("You don't have enough coins to purchase this item.");
                        if(totalCoins - coinCost == 1)
                        {
                            Menu_Manager.instance.setupShopTextAnimation("You need " + tempDifference.ToString() + " more coin to purchase this test subject.");
                        }
                        else
                        {
                            Menu_Manager.instance.setupShopTextAnimation("You need " + tempDifference.ToString() + " more coins to purchase this test subject.");
                        }
                       
                    }
                }
                else
                {
                    if(boltCost <= totalBolts)
                    {
                        Audio_Manager.Instance.playSFX(shopBuySound);
                        skinsUnlocked.Add(i);
                     //   Debug.Log("You just bought skin number: " + i + " Which corresponds to: " + theSkin);
                        Menu_Manager.instance.setupShopTextAnimation("You can now play as: " + theSkin.ToString() + "!");
                        totalBolts -= boltCost;
                        playerCurrencyText.text = ": " + totalBolts;
                        skinBuyButton.interactable = false;
                    }
                    else
                    {
                        Audio_Manager.Instance.playSFX(shopErrorSound);
                        int tempDifference = boltCost - totalBolts;
                      //  Debug.LogWarning("You don't have enough bolts to purchase this item.");
                        if (totalBolts - boltCost == 1)
                        {
                            Menu_Manager.instance.setupShopTextAnimation("You need " + tempDifference.ToString() + " more bolt to purchase this test subject.");
                        }
                        else
                        {
                            Menu_Manager.instance.setupShopTextAnimation("You need " + tempDifference.ToString() + " more bolts to purchase this test subject.");
                        }
                       
                    }
                }
                break;
            }

          
        }

        //Save the purchase!
        Save_System.SaveCollectables(this);




    }

    //Shop related stuff.



    //Customization stuff
    public void equipItem(typeOfItem typePassed)
    {
        switch (typePassed)
        {
            case typeOfItem.HandWarmers:
                {
                    handWarmers -= 1;
                    break;
                }
            case typeOfItem.Defroster:
                {
                    numDefrosters -= 1;
                    break;
                }
            case typeOfItem.FireVest:
                {
                    numFireVests -= 1;
                    break;
                }
            case typeOfItem.LiquidNitrogenCanister:
                {
                    numLiquidNitrogenCanisters -= 1;
                    break;
                }
            case typeOfItem.NeutralTablet:
                {
                    numNeutralTablets -= 1;
                    break;
                }
            default:
                {
                    Debug.LogWarning("Hey! We are in the default switch for purchaseItemConfirm()!");
                    break;
                }

        }
    }

    //Customization stuff

    //DEBUG FUNCTIONS

    public void deleteSave()
    {
        Save_System.DeleteCollectables();
    }

    public void changeTimePeriodStart(string timePeriodName)
    {
        switch(timePeriodName)
        {
            case "Prehistoric":
                {
                    debugPeriod = Level_Manager.timePeriod.Prehistoric;
                    break;
                }
            case "FeudalJapan":
                {
                    debugPeriod = Level_Manager.timePeriod.FeudalJapan;
                    break;
                }
            case "WildWest":
                {
                    debugPeriod = Level_Manager.timePeriod.WildWest;
                    break;
                }
            case "Medieval":
                {
                    debugPeriod = Level_Manager.timePeriod.Medieval;
                    break;
                }
            case "Future":
                {
                    debugPeriod = Level_Manager.timePeriod.Future;
                    break;
                }
            default:
                {
                    debugPeriod = Level_Manager.timePeriod.None;
                    Debug.LogWarning("Invalid string, setting to none.");
                    break;
                }
        }

    }

    public Level_Manager.timePeriod getTimePeriod()
    {
        return debugPeriod;
    }
    //DEBUG FUNCTIONS


    //NON SAVING VARIABLE FUNCTIONS
    public bool getBootupStatus()
    {
        return firstTimeBootingUp;
    }

    public void setBootupStatus(bool set)
    {
        firstTimeBootingUp = set;
    }
    //NON SAVING VARIABLE FUNCTIONS

    //External checks.
    public void SetFirstTimePlaying()
    {

    }

    //External checks
}

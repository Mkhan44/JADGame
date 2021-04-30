//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.


//This script will be attached to a manager that persists throughout the game. 
//Stats related to collecting items, coins, gems, skins, etc will be updated here and used to save via other scripts.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Manager : MonoBehaviour
{
    [Header("Currency")]
    [Tooltip("Total amount of coin currency the player has.")]
    public int totalCoins;
    [Tooltip("Total amount of gem currency the player has.")]
    public int totalGems;

    [Header("Items")]
    //Numbers of each item the player has.
    public int handWarmers;
    public int numDefrosters;
    public int numFireVests;
    public int numLiquidNitrogenCanisters;

    public enum typeOfItem
    {
        HandWarmer,
        Defroster,
        FireVest,
        LiquidNitrogenCanister,
        //DEBUG
        none
    }

    public enum skinTypes
    {
        guy1,
        dummy,
        sonic,
        mario,
        snake
    }

    [Header("Loadout")]
    //Loadout...Save as Int and we can figure out the corresponding enum index based on that. 0 = handwarmer, 1 = defroster, etc.
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

    //Array storing which skin indexes are unlocked. 0 and 1 (Male and female default) unlocked by default.
    public List<int> skinsUnlocked = new List<int>();

    [Header("Things to reference throughout the game.")]
    public List<SkinInfo> skinsToPick;
    public List<Shop_Item> itemsToPick;

    [Header("Singleton")]
    public static Collect_Manager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            LoadCollectableData();
            Debug.Log("Save file will be located in: " + Application.persistentDataPath);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        //Debug.Log(System.Enum.GetValues(typeof(skinTypes)).Length);

    }

    //Loading from a file...
    void LoadCollectableData()
    {
        CollectableData collect = Save_System.LoadCollectables();


        if(collect != null)
        {
            totalCoins = collect.totalCoins;
            handWarmers = collect.handWarmers;
            numDefrosters = collect.numDefrosters;
            numFireVests = collect.numFireVests;
            numLiquidNitrogenCanisters = collect.numLiquidNitrogenCanisters;

            currentSkin = collect.currentSkin;
            item1 = collect.item1;
            item2 = collect.item2;
            item3 = collect.item3;


            highScore = collect.highScore;
            highestWave = collect.highestWave;
            mostWavesSurvived = collect.mostWavesSurvived;
            totalWavesSurvived = collect.totalWavesSurvived;
            for (int i = 0; i < collect.skinsUnlocked.Count; i++)
            {
                skinsUnlocked.Add(collect.skinsUnlocked[i]);
            }

        }
        //IF THERE IS NO SAVE FILE, THIS WILL BE CALLED.
        else
        {
            currentSkin = 0;
            //Player will have default skin unlocked no matter what. That value = 0. Should also have another skin maybe like 1 alt for female unlocked from the start as well. That will = 1.
            skinsUnlocked.Add(0);
            skinsUnlocked.Add(1);
            skinsUnlocked.Add(2);

            //Default the loadout to -1's so we don't put any items in the loadout.
            item1 = -1;
            item2 = -1;
            item3 = -1;

            //DEBUG STATEMENT, DON'T GIVE THEM 50K AT START LMAO.
            totalCoins = 50000;
            Debug.LogWarning("Collect is null, we probably don't have a save file! Setting skin to default.");
        }
        

    }

    //Add to this function as we add more items into the game.
    public int numPlayerOwns(typeOfItem typePassed)
    {
        switch(typePassed)
        {
            case typeOfItem.HandWarmer:
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
            //Case the enum to an integer to compare it.
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
    public void purchaseItemConfirm(typeOfItem typePassed)
    {
        switch (typePassed)
        {
            case typeOfItem.HandWarmer:
                {
                    handWarmers += 1;
                    break;
                }
            case typeOfItem.Defroster:
                {
                    numDefrosters += 1;
                    break;
                }
            case typeOfItem.FireVest:
                {
                    numFireVests += 1;
                    break;
                }
            case typeOfItem.LiquidNitrogenCanister:
                {
                    numLiquidNitrogenCanisters += 1;
                    break;
                }
            default:
                {
                    Debug.LogWarning("Hey! We are in the default switch for purchaseItemConfirm()!");
                    break;
                }

        }
    }


    public void purchaseItemWithCoins(int cost, typeOfItem typePassed)
    {
        int playerCoins = Collect_Manager.instance.totalCoins;

        if (playerCoins >= cost)
        {
            Collect_Manager.instance.totalCoins -= cost;
            Debug.Log("You just bought: " + typePassed);

            Collect_Manager.instance.purchaseItemConfirm(typePassed);

            //Save the purchase!
            Save_System.SaveCollectables(Collect_Manager.instance);
        }
        else
        {
            Debug.Log("Hey, you don't have enough coins for this! Your total coins are: " + playerCoins.ToString());
        }

    }

    //Shop related stuff.



    //Customization stuff
    public void equipItem(typeOfItem typePassed)
    {
        switch (typePassed)
        {
            case typeOfItem.HandWarmer:
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
            default:
                {
                    Debug.LogWarning("Hey! We are in the default switch for purchaseItemConfirm()!");
                    break;
                }

        }
    }

    //Customization stuff


}

//Stores the save data for collectables in the game (Coins, gems, etc)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Makes it so that we can serialize this class...Basically we can save it.
[System.Serializable]
public class CollectableData
{
    public int totalCoins;
    public int totalGems;
    //Numbers of each item the player has.
    public int numHandWarmers;
    public int numDefrosters;
    public int numFireVests;

    //Index of the current skin that the player had been using during last session.
    public int currentSkin;

    //Array storing which skin indexes are unlocked. 0 and 1 (Male and female default) unlocked by default.
    public int[] skinsUnlocked;

    //Constructor.
    public CollectableData(Collect_Manager collectmanager)
    {
        totalCoins = Collect_Manager.instance.totalCoins;
        totalGems = Collect_Manager.instance.totalGems;
        numHandWarmers = Collect_Manager.instance.numHandWarmers;
        numDefrosters = Collect_Manager.instance.numDefrosters;
        numFireVests = Collect_Manager.instance.numFireVests;
        currentSkin = Collect_Manager.instance.currentSkin;

        //Loop through and insert every numbered skin that the player has unlocked.
        if(Collect_Manager.instance.skinsUnlocked.Length != 0)
        {
            for (int i = 0; i < Collect_Manager.instance.skinsUnlocked.Length; i++)
            {
                skinsUnlocked[i] = Collect_Manager.instance.skinsUnlocked[i];
            }
        }
       
    }

}

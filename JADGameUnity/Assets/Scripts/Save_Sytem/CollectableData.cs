//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

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
    public int handWarmers;
    public int numDefrosters;
    public int numFireVests;

    //Loadout items that will correspond to enum in the Collect_Manager.
    int item1;
    int item2;
    int item3;

    //Index of the current skin that the player had been using during last session.
    public int currentSkin;

    //Array storing which skin indexes are unlocked. 0 and 1 (Male and female default) unlocked by default.
    public List<int> skinsUnlocked = new List<int>();

    //Constructor.
    public CollectableData(Collect_Manager collectmanager)
    {
        totalCoins = Collect_Manager.instance.totalCoins;
        totalGems = Collect_Manager.instance.totalGems;
        handWarmers = Collect_Manager.instance.handWarmers;
        numDefrosters = Collect_Manager.instance.numDefrosters;
        numFireVests = Collect_Manager.instance.numFireVests;
        currentSkin = Collect_Manager.instance.currentSkin;
        item1 = Collect_Manager.instance.item1;
        item2 = Collect_Manager.instance.item2;
        item3 = Collect_Manager.instance.item3;

        //Loop through and insert every numbered skin that the player has unlocked.
      
        for (int i = 0; i < Collect_Manager.instance.skinsUnlocked.Count; i++)
        {
            skinsUnlocked.Add(Collect_Manager.instance.skinsUnlocked[i]);
        }

        skinsUnlocked.Sort();


    }

}

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
    public int totalBolts;
    //Numbers of each item the player has.
    public int handWarmers;
    public int numDefrosters;
    public int numFireVests;
    public int numLiquidNitrogenCanisters;
    public int numNeutralTablets;

    //Loadout items that will correspond to enum in the Collect_Manager.
    public int item1;
    public int item2;
    public int item3;

    //Score related variables.
    public int highScore;
    public int highestWave;
    public int mostWavesSurvived;
    public int totalWavesSurvived;

    //Index of the current skin that the player had been using during last session.
    public int currentSkin;

    //Array storing which skin indexes are unlocked. 0 and 1 (Male and female default) unlocked by default.
    public List<int> skinsUnlocked = new List<int>();

    //Determines whether game audio should be muted or not.
    public bool isMuted;

    //Determines if player has completed tutorial alreaady or not.
    public bool tutCompleted;

    //For IOS. Determines if the player has seen the disclaimer on their first bootup or not.
    public bool disclaimerDisplayIOSDone;

    //Basically to check if the player has played the tutorial or not.
    public bool firstTimePlaying;

    //Constructor.
    public CollectableData(Collect_Manager collectmanager)
    {
        //Currency
        totalCoins = Collect_Manager.instance.totalCoins;
        totalBolts = Collect_Manager.instance.totalBolts;

        //Items
        handWarmers = Collect_Manager.instance.handWarmers;
        numDefrosters = Collect_Manager.instance.numDefrosters;
        numFireVests = Collect_Manager.instance.numFireVests;
        numLiquidNitrogenCanisters = Collect_Manager.instance.numLiquidNitrogenCanisters;
        numNeutralTablets = Collect_Manager.instance.numNeutralTablets;

        currentSkin = Collect_Manager.instance.currentSkin;

        //Gameplay
        item1 = Collect_Manager.instance.item1;
        item2 = Collect_Manager.instance.item2;
        item3 = Collect_Manager.instance.item3;

        //Scores
        highScore = Collect_Manager.instance.highScore;
        highestWave = Collect_Manager.instance.highestWave;
        mostWavesSurvived = Collect_Manager.instance.mostWavesSurvived;
        totalWavesSurvived = Collect_Manager.instance.totalWavesSurvived;

        //Extra stuff.
        isMuted = Collect_Manager.instance.isMuted;
        tutCompleted = Collect_Manager.instance.tutCompleted;
        disclaimerDisplayIOSDone = Collect_Manager.instance.disclaimerDisplayIOSDone;
        firstTimePlaying = Collect_Manager.instance.firstTimePlaying;

        //Loop through and insert every numbered skin that the player has unlocked.

        for (int i = 0; i < Collect_Manager.instance.skinsUnlocked.Count; i++)
        {
            skinsUnlocked.Add(Collect_Manager.instance.skinsUnlocked[i]);
        }

        skinsUnlocked.Sort();


    }

}

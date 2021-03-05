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
    public int numHandWarmers;
    public int numDefrosters;
    public int numFireVests;

    [Header("Skin related")]
    //Index of the current skin that the player had been using during last session.
    public int currentSkin;

    //Array storing which skin indexes are unlocked. 0 and 1 (Male and female default) unlocked by default.
    public int[] skinsUnlocked;

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

    }

    void LoadCollectableData()
    {
        CollectableData collect = Save_System.LoadCollectables();

        //Fill in the rest later.

        if(collect != null)
        {
            totalCoins = collect.totalCoins;
        }
        

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
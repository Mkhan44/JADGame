//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    [Header("Singleton")]
    public static Menu_Manager instance;

    Scene gameplayScene;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //gameplayScene = SceneManager.GetSceneByName("Gameplay");
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        
    }

    public void PlayGame()
    {
        //Might want to make sure that this string works.
        SceneManager.LoadScene("Gameplay");
    }


    //Shop related stuff.

    public void purchaseItemWithCoins(int cost, string itemNameForCollectable)
    {
        int playerCoins = Collect_Manager.instance.totalCoins;

        if(playerCoins >= cost)
        {
            Collect_Manager.instance.totalCoins -= cost;
            Debug.Log("You just bought: " + itemNameForCollectable);

            Collect_Manager.instance.purchaseItemConfirm(itemNameForCollectable);

            //Save the purchase!
            Save_System.SaveCollectables(Collect_Manager.instance);
        }
        else
        {
            Debug.Log("Hey, you don't have enough coins for this! Your total coins are: " + playerCoins.ToString());
        }

    }    

    //Shop related stuff.
}

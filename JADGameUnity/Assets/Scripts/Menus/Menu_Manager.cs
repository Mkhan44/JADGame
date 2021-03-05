//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Manager : MonoBehaviour
{
    Scene gameplayScene;
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
}

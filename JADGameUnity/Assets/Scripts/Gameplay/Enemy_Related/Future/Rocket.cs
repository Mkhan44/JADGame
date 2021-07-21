//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rocket : Obstacle_Behaviour
{
    [SerializeField] Animator rocketAnimator;
    [SerializeField] float elapsedTime;
    [SerializeField] List<Sprite> numbersList = new List<Sprite>();
    [SerializeField] SpriteRenderer numberSprite;
    Coroutine switchCo;
    bool isFlipped;
    [Tooltip("A number that determines the rate of switching. Closer to 0 means faster rate.")]
    [SerializeField] float switchRate;
    [Tooltip("Number of switches allowed. The rocket can never switch directions more than this amount.")]
    [SerializeField] int maxNumSwitches;
    //This will represent the number on the rocket at any given point.
    int numSwitchesLeft;

    const string IsTeleporting = "IsTeleporting";
    const string teleportVar = "TeleportVar";
    bool inCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        setNumberSprite();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setNumberSprite()
    {
        if (numSwitchesLeft > 0 || numSwitchesLeft < numbersList.Count)
        {
            numberSprite.sprite = numbersList[numSwitchesLeft];
        }
    }
}

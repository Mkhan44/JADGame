//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ninja : Obstacle_Behaviour
{
    [SerializeField] Animator ninjaAnimator;
    const string IsTeleporting = "IsTeleporting";
    const string teleportVar = "teleportVar";

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    protected override void Movement()
    {
        base.Movement();   
    }

    IEnumerator timeBetweenTele()
    {
        yield return null;

        float timeVariance;
        timeVariance = Random.Range(0.3f, 1.0f);

        //Make it so that it takes this long before the Ninja teleports.
    }
}

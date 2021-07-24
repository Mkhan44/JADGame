//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UFO : Obstacle_Behaviour
{
    [SerializeField] GameObject droppedEnemyPrefab;
    [SerializeField] float dropRate;
    [SerializeField] int maxDrops;
    [SerializeField] Animator UFOAnimator;
    bool inCoroutine;
    int currentDropsLeft;
    protected override void Awake()
    {
        base.Awake();
        currentDropsLeft = maxDrops;
        inCoroutine = false;
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        currentDropsLeft = maxDrops;
        inCoroutine = false;
    }

    protected override void Movement()
    {
        base.Movement();

        if(!inPlayerVicinity && !inIndicatorVicinity)
        {

        }
    }

    IEnumerator dropEnemy()
    {
        int randNum = Random.Range(1, 3);

        if(currentDropsLeft == 0)
        {
            yield break;
        }

        if(randNum > 1)
        {
            //Spawn enemy.
            //UFOAnimator.play...
            currentDropsLeft -= 1;
        }
        else
        {
            Debug.Log("Not dropping an enemy.");
            yield break;
        }

        GameObject tempDroppedEnemy = Instantiate(droppedEnemyPrefab, this.transform);

        yield return null;
    }

    


}

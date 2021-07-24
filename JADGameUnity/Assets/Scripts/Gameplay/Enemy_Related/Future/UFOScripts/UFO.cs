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
    int timesSinceLastDrop;
    protected override void Awake()
    {
        base.Awake();
        currentDropsLeft = maxDrops;
        inCoroutine = false;
        timesSinceLastDrop = 0;
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        currentDropsLeft = maxDrops;
        inCoroutine = false;
        timesSinceLastDrop = 0;
    }

    protected override void Movement()
    {
        base.Movement();

        if(!inPlayerVicinity && onScreenIndicator && !inCoroutine)
        {
            StartCoroutine(dropEnemy());
        }
    }

    IEnumerator dropEnemy()
    {
        inCoroutine = true;
        int randNum = Random.Range(1, 3);

        if(currentDropsLeft == 0)
        {
     
            inCoroutine = false;
            yield break;
        }

        if(randNum > 1 || timesSinceLastDrop > 3)
        {
            //Spawn enemy.
            //UFOAnimator.play...
            currentDropsLeft -= 1;
        }
        else
        {
            Debug.Log("Not dropping an enemy.");
            timesSinceLastDrop += 1;
            yield return new WaitForSeconds(0.5f);
            inCoroutine = false;
            yield break;
        }

        GameObject tempDroppedEnemy = Instantiate(droppedEnemyPrefab, this.transform);
        tempDroppedEnemy.GetComponent<UFO_Dropped_Enemy>().initializeDroppedEnemy(this.gameObject);

        yield return new WaitForSeconds(dropRate);

        inCoroutine = false;

        yield return null;
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner")
        {

            if (thisType == typeOfObstacle.obstacle)
            {
                Wave_Spawner.Instance.updateEnemiesLeft(1);
                Level_Manager.Instance.increaseEnemiesDodged();
                if (scoreValue <= 0)
                {
                    Level_Manager.Instance.updateScore(100);
                }
                else
                {
                    Level_Manager.Instance.updateScore(scoreValue);
                }
            }
            inPlayerVicinity = false;

            //TURN ON WHEN WE ARE READY TO POOL
            thisRigid.velocity = Vector2.zero;

            foreach(Transform child in transform)
            {
                if(child.gameObject.GetComponent<UFO_Dropped_Enemy>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
            Object_Pooler.Instance.AddToPool(gameObject);
            //Destroy(gameObject);
            return;
        }
    }



}

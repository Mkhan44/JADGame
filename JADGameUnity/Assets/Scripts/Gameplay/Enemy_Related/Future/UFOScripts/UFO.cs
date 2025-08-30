//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UFO : Obstacle_Behaviour
{
    [SerializeField] List<GameObject> droppedEnemyPrefabs = new List<GameObject>();
    [SerializeField] float dropRate;
    [SerializeField] int maxDrops;
    [SerializeField] Animator UFOAnimator;
    [SerializeField] SpriteRenderer lightSpriteRend;
    bool inCoroutine;
    int currentDropsLeft;
    int timesSinceLastDrop;
    float adjustedDropRate;

    //Animation clips
    AnimationClip lightOn;
    AnimationClip lightRetract;

    //SFX
    [SerializeField] AudioClip ufoSpinSound;
    [SerializeField] AudioClip beamDownSound;
    [SerializeField] AudioClip beamUpSound;
    AudioClip activeSound;

    const string ufoLightOn = "ufolight";
    const string ufoLightRetract = "ufolightretract";
    const string ufoLightIdle = "ufolightidle";
    protected override void Awake()
    {
        base.Awake();
        initializeUFO();
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        initializeUFO();
        activeSound = ufoSpinSound;
        Audio_Manager.Instance.playSFX(activeSound, true, 0.05f);
    }

    private void initializeUFO()
    {
        currentDropsLeft = maxDrops;
        inCoroutine = false;
        timesSinceLastDrop = 0;
        lightOn = UFOAnimator.runtimeAnimatorController.animationClips[0];
        lightRetract = UFOAnimator.runtimeAnimatorController.animationClips[1];
        lightSpriteRend.color = new Color(255, 255, 255, 0);
        adjustedDropRate = (lightOn.length + lightRetract.length);

        if(adjustedDropRate < 0f)
        {
            adjustedDropRate = 0f;
        }

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

        if(this.transform.position.x <= 2 && this.transform.position.x >= -0.6f)
        {
            //Spawn enemy.
            //UFOAnimator.play...
            currentDropsLeft -= 1;
        }
        else
        {
            inCoroutine = false;
            yield break;
        }

        int randJunkSpawn = Random.Range(0, droppedEnemyPrefabs.Count);

        UFOAnimator.Play(ufoLightOn);

        lightSpriteRend.color = new Color(255, 255, 255, 255);
        Audio_Manager.Instance.stopSFX(activeSound.name);
        activeSound = beamDownSound;

        Audio_Manager.Instance.playSFX(activeSound, false, 0.2f);
        yield return new WaitForSeconds(0.3f);

        Vector2 currentUFOPos = this.transform.position;
        //GameObject tempDroppedEnemy = Instantiate(droppedEnemyPrefabs[randJunkSpawn], this.transform);
        GameObject tempDroppedEnemy = Instantiate(droppedEnemyPrefabs[randJunkSpawn], new Vector2(currentUFOPos.x, currentUFOPos.y - 0.2f), this.transform.rotation);
        tempDroppedEnemy.GetComponent<UFO_Dropped_Enemy>().initializeDroppedEnemy(this.gameObject);
        tempDroppedEnemy.transform.SetParent(this.transform);
        yield return new WaitForSeconds(lightOn.length - 0.3f);

        UFOAnimator.Play(ufoLightRetract);

        activeSound = ufoSpinSound;
        Audio_Manager.Instance.playSFX(activeSound, true, 0.05f);
        // activeSound = beamUpSound;
        //Audio_Manager.Instance.playSFX(activeSound);

        yield return new WaitForSeconds(lightRetract.length);

        lightSpriteRend.color = new Color(255, 255, 255, 0);

        UFOAnimator.Play(ufoLightIdle);

     
        yield return new WaitForSeconds(adjustedDropRate);

        if(thisObstacleDiff == obstacleDiff.easy)
        {
            yield return new WaitForSeconds(0.2f);
        }
        else if (thisObstacleDiff == obstacleDiff.medium)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {

        }

        inCoroutine = false;

        yield return null;
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner")
        {
            Audio_Manager.Instance.stopSFX(activeSound.name);
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
            thisRigid.linearVelocity = Vector2.zero;

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

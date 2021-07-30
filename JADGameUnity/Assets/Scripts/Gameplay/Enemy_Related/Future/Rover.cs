//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Rover : Obstacle_Behaviour
{
    bool inCoroutine;
    int randNum;
    Animator roverAnimator;
    AnimationClip stopAni;
    bool didStop;
    bool stopping;

    const string stopString = "";
    const string goString = "";

    protected override void Awake()
    {
        base.Awake();
        initializeRover();
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        initializeRover();
    }

    void initializeRover()
    {
        inCoroutine = false;
        didStop = false;
        stopping = false;
    }

    protected override void Movement()
    {
        
        if(!inPlayerVicinity && onScreenIndicator && !inCoroutine && !didStop)
        {
            StartCoroutine(roverStuck());
        }


        if(!didStop && !stopping)
        {
            base.Movement();
        }

        if(!inCoroutine && didStop)
        {
         
            speed = maxSpeed * 1.3f;
            if (thisRigid != null)
            {
                thisRigid.velocity = Vector2.left * speed;
            }
        }
    }

    IEnumerator roverStuck()
    {
        inCoroutine = true;

        if (didStop)
        {
            inCoroutine = false;
            yield break;
        }

        randNum = Random.Range(0, 2);
        Debug.Log(randNum);

        if(randNum == 0)
        {
            stopping = true;
            float timeToWait = 0;

            //Get the animationClip time.
            //stopAni = roverAnimator.runtimeAnimatorController.animationClips[0];
            //timeToWait = stopAni.length;

            //Do it.
            thisRigid.velocity = Vector2.zero;

            //Play animation.

            //yield return new WaitForSeconds(timeToWait);

            //Placeholder.
            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            //Don't stop.
            yield return new WaitForSeconds(0.5f);
            inCoroutine = false;
            yield break;
        }

        inCoroutine = false;
        stopping = false;
        didStop = true;

        yield return null;
    }


}

//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Frisball : Obstacle_Behaviour
{
    [SerializeField] int randNum;
    [SerializeField] int maxTransforms;
    int currentTransformsLeft;
    bool inCoroutine;
    [SerializeField] Animator frisballAnimator;
    AnimationClip ballTrans;
    AnimationClip frisTrans;

    [SerializeField] AudioClip frisballRollSound;
    [SerializeField] AudioClip frisballSpinSound;
    [SerializeField] AudioClip frisballTransformSound;

    AudioClip activeSound;


    const string ballToFrisTransString = "frisballtrans";
    const string frisToBallTransString = "frisballtransout";
    const string frisIdleString = "frisballflat";
    const string ballIdleString = "frisballround";

    //True = bottom, False = Mid.
    bool botOrMid;

    protected override void Awake()
    {
        base.Awake();
        initializeTheFrisball();
    }


    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        initializeTheFrisball();
        Audio_Manager.Instance.playSFX(activeSound, true);

    }

    //Call this whenever it spawns.
    void initializeTheFrisball()
    {
        randNum = 0;
        currentTransformsLeft = maxTransforms;
        botOrMid = false;
        inCoroutine = false;
        if (spawnPoint == Wave_Spawner.spawnPointNum.spawnPoint1)
        {
            //Start as frisbee.
            frisballAnimator.Play(frisIdleString);
            activeSound = frisballSpinSound;
            botOrMid = false;
        }
        else
        {
            //Start as ball.
            frisballAnimator.Play(ballIdleString);
            activeSound = frisballRollSound;
            botOrMid = true;
        }
    }

    protected override void Movement()
    {
        if(onScreenIndicator && !inPlayerVicinity && !inCoroutine)
        {
            StartCoroutine(transformIt());
        }
        else
        {
            base.Movement();
        }
       
    }

    IEnumerator transformIt()
    {
        inCoroutine = true;

        if(currentTransformsLeft == 0)
        {
            inCoroutine = false;
            yield break;
        }

        randNum = Random.Range(0, 2);

        if(randNum == 0 && this.transform.position.x >= 0)
        {
            //Go forward.
            thisRigid.velocity = Vector2.zero;
            currentTransformsLeft -= 1;
        }
        else
        {
            //Don't do anything.
            yield return new WaitForSeconds(0.25f);
            inCoroutine = false;
            yield break;
        }

        Vector3 tempPos = this.transform.localPosition;
        float targetYValue = tempPos.y;

        Audio_Manager.Instance.stopSFX(activeSound.name);
        activeSound = frisballTransformSound;
        Audio_Manager.Instance.playSFX(activeSound);
        //We're on the bottom, so turn it into the frisbee and go up.
        if (botOrMid)
        {
            //ballTrans = frisballAnimator.runtimeAnimatorController.animationClips[0];

            targetYValue = this.transform.localPosition.y + 0.8f;

            frisballAnimator.Play(ballToFrisTransString);
      
            //Play transform animation here.
            while (this.transform.localPosition.y < targetYValue)
            {
                tempPos.y += 0.1f;
                thisRigid.velocity = new Vector2(0f, tempPos.y);

                yield return null;
            }
            thisRigid.velocity = Vector2.zero;

            //Need to find a way to wait till animation is done but also not be stuck in while loop...
            //frisballAnimator.Play(frisIdleString);

            botOrMid = false;

            activeSound = frisballSpinSound;
            Audio_Manager.Instance.playSFX(activeSound,true);
        }
        //We're on the top, so turn it into the ball and go down.
        else
        {
            //ballTrans = frisballAnimator.runtimeAnimatorController.animationClips[0];

            targetYValue = this.transform.localPosition.y - 0.8f;

            frisballAnimator.Play(frisToBallTransString);

            //Play transform animation here.
            while (this.transform.localPosition.y > targetYValue)
            {
                tempPos.y -= 0.1f;
                thisRigid.velocity = new Vector2(0f, tempPos.y);

                yield return null;
            }

            activeSound = frisballRollSound;
            Audio_Manager.Instance.playSFX(activeSound,true);


            //Need to find a way to wait till animation is done but also not be stuck in while loop...
            //frisballAnimator.Play(ballIdleString);

            botOrMid = true;
        }

        //Wait time between each try if we do a transform.
        yield return new WaitForSeconds(0.5f);

        inCoroutine = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner")
        {
            Audio_Manager.Instance.stopSFX(activeSound.name);
        }

        base.OnCollisionEnter2D(collision);
    }


}

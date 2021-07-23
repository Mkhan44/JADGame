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
    [Tooltip("A number that determines the rate of switching. Closer to 0 means faster rate. SHOULD NEVER BE LOWER THAN 1.")]
    [SerializeField] float switchRate;
    [Tooltip("Number of switches allowed. The rocket can never switch directions more than this amount.")]
    [SerializeField] int maxNumSwitches;
    [Tooltip("Rate at which the rocket rotates. Bigger the number faster the rotation.")]
    [SerializeField] int rotationRate;

    //If this is true, we're on bottom. If false on top.
    bool botOrTop;
    bool turning;
    bool justFinishedTurning;
    //This will represent the number on the rocket at any given point.
    int numSwitchesLeft;

    const string IsTeleporting = "IsTeleporting";
    const string teleportVar = "TeleportVar";
    bool inCoroutine;


    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        if(spawnPoint == Wave_Spawner.spawnPointNum.spawnPoint2)
        {
            botOrTop = true;
        }
        else
        {
            botOrTop = false;
        }
        turning = false;
        inCoroutine = false;
        numSwitchesLeft = maxNumSwitches;
        setNumberSprite();
        justFinishedTurning = false;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    protected override void Awake()
    {
        base.Awake();
        if (spawnPoint == Wave_Spawner.spawnPointNum.spawnPoint2)
        {
            botOrTop = true;
        }
        else
        {
            botOrTop = false;
        }
        turning = false;
        inCoroutine = false;
        numSwitchesLeft = maxNumSwitches;
        setNumberSprite();
        justFinishedTurning = false;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    protected override void Movement()
    {
        if(!turning)
        {
            base.Movement();
        }
        else
        {
            return;
        }
 

        if(!inPlayerVicinity && !inCoroutine && !inIndicatorVicinity)
        {
            StartCoroutine(turnRocket());
        }
    }


    void setNumberSprite()
    {
        if (numSwitchesLeft > 0 || numSwitchesLeft < numbersList.Count)
        {
            numberSprite.sprite = numbersList[numSwitchesLeft];
        }
        else
        {
            numberSprite.sprite = numbersList[0];
        }
    }

    void topBotToggle()
    {
        if(botOrTop)
        {
            botOrTop = false;
        }
        else
        {
            botOrTop = true;
        }
    }

    IEnumerator turnRocket()
    {
        inCoroutine = true;
        if(numSwitchesLeft == 0)
        {
            yield break;
        }
        //See if we will turn or not.
        float randomNum = Random.Range(0, switchRate);

        if (randomNum > 1 && !justFinishedTurning)
        {
            Debug.Log("Rocket will turn!");
            thisRigid.velocity = Vector2.zero;
            turning = true;
            numSwitchesLeft -= 1;
            setNumberSprite();
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            Debug.Log("Not gonna turn the rocket.");
            yield return new WaitForSeconds(0.2f);
            //CODE NOT GETTING TO HERE...
            if(justFinishedTurning)
            {
                yield return new WaitForSeconds(0.7f);
            }
            justFinishedTurning = false;
            inCoroutine = false;
            yield break;
        }

        //90 degrees if turning down
        //-90 if turning up.
        Vector3 tempRotation;
        tempRotation = this.transform.eulerAngles;

        //thisRigid.velocity = Vector2.zero;
        if(!botOrTop)
        {
            while(this.transform.eulerAngles.z != 90)
            {
                tempRotation.z += rotationRate;
                if (tempRotation.z > 90)
                {
                    tempRotation.z = 90;
                }
                this.transform.eulerAngles = tempRotation;
            
                yield return null;
            }

            //Test.
            thisRigid.velocity = Vector2.down * 10;

            yield return new WaitForSeconds(0.1f);

            thisRigid.velocity = Vector2.zero;

            while (this.transform.eulerAngles.z != 0)
            {
                tempRotation.z -= rotationRate;
                if (tempRotation.z < 0)
                {
                    tempRotation.z = 0;
                }
                this.transform.eulerAngles = tempRotation;
                yield return null;
            }
        }
        else
        {

            //Right now, the z rotation is instead going to 270 since it can't be negative.
            while (this.transform.eulerAngles.z != 270)
            {
                tempRotation.z -= rotationRate;
                if (tempRotation.z > 270)
                {
                    tempRotation.z = 270;
                }
                this.transform.eulerAngles = tempRotation;
                Debug.Log("Stuck in while loop. The value of the z rotation is: " + this.transform.eulerAngles.z.ToString());
                yield return null;
            }
            Debug.Log("Should be going up now.");
            //Test.
            thisRigid.velocity = Vector2.up * 10;

            yield return new WaitForSeconds(0.1f);

            thisRigid.velocity = Vector2.zero;

            while (this.transform.eulerAngles.z != 0)
            {
                tempRotation.z += rotationRate;
                if (tempRotation.z > 0)
                {
                    tempRotation.z = 0;
                }
                this.transform.eulerAngles = tempRotation;
                yield return null;
            }
        }
        turning = false;
        inCoroutine = false;
        justFinishedTurning = true;
        topBotToggle();

        yield return null;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
  
    }
}

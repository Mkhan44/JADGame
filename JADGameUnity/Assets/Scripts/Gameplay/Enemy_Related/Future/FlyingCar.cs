//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyingCar : Obstacle_Behaviour
{
    [SerializeField] bool spedUp;
    bool inCoroutine;
    float speedUpMaxSpeed;
    float newIncreaseRate;

    [SerializeField] AudioClip carForwardSound1;
    [SerializeField] AudioClip carForwardSound2;

    protected override void Awake()
    {
        base.Awake();
        initializeFlyingCar();
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        initializeFlyingCar();
    }

    void initializeFlyingCar()
    {
        spedUp = false;
        inCoroutine = false;
    }

    protected override void Movement()
    {
        if(!spedUp && !onScreenIndicator)
        {
            base.Movement();
        }
        else if(!spedUp && !inCoroutine)
        {
            StartCoroutine(speedUp());
        }
        else if(spedUp)
        {
            Debug.Log("Car is going forward at it's fast speed now!");
       
            if (speed < speedUpMaxSpeed)
            {
                speed += newIncreaseRate * Time.deltaTime;
            }
            else
            {
                speed = speedUpMaxSpeed;
            }

            if (thisRigid != null)
            {
                thisRigid.linearVelocity = Vector2.left * speed;
            }

            
        }
     
    }

    IEnumerator speedUp()
    {
        inCoroutine = true;

        float rngReverseTime = Random.Range(0.3f, 0.4f);
        float timeToReverse;
        if (thisObstacleDiff == obstacleDiff.easy)
        {
            timeToReverse = 1.0f;
            newIncreaseRate = increaseRate * 2.5f;
            speedUpMaxSpeed = maxSpeed * 2.5f;
        }
        else if(thisObstacleDiff == obstacleDiff.medium)
        {
            timeToReverse = 0.8f;
            newIncreaseRate = increaseRate * 3.5f;
            speedUpMaxSpeed = maxSpeed * 3.0f;
        }
        else
        {
            timeToReverse = 0.6f;
            newIncreaseRate = increaseRate * 4.5f;
            speedUpMaxSpeed = maxSpeed * 3.8f;
        }
        //Asign new max speed + increase rate based on difficulty here.

        //This value should probably be a rate that fluctuates via rng...So like it won't speed up until x amount of time passes. For now the value = a test.
        yield return new WaitForSeconds(rngReverseTime);

        thisRigid.linearVelocity = Vector2.right * 0.5f;

        yield return new WaitForSeconds(timeToReverse);

        //Slingshot forward by making speed way higher.
        speed = 0;

      //  speedUpMaxSpeed = maxSpeed * 3.5f;
      //  newIncreaseRate = increaseRate * 4.5f;

        inCoroutine = false;
        spedUp = true;

        int randSoundPlay = Random.Range(0, 2);

        if(randSoundPlay == 0)
        {
            Audio_Manager.Instance.playSFX(carForwardSound1, false, 0.3f);
        }
        else
        {
            Audio_Manager.Instance.playSFX(carForwardSound2, false, 0.3f);
        }
       

        yield return null;
    }
}

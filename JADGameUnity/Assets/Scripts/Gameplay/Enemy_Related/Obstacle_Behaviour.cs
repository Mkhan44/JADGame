﻿//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour , IPooled_Object
{
    protected Vector2 startPos;
    protected Vector2 endPos;
    //Speed
    [Tooltip("Speed of the obstacle moving from right to left. KEEP THIS AS 0.")]
    public float speed = 0f;

    float ogSpeed;
    [Tooltip("How fast will this object go? This determines the speed increase rate.")]
    public float increaseRate;
    [Tooltip("Maximum speed this object should approach. If 0f , it will be set to increase rate by default.")]
    public float maxSpeed;
    //We will have spawn points set up in the Wave_Spawner script.
    [Tooltip("Spawnpoint1 = Top, Spawnpoint2 = Bottom, Spawnpoint3 = hanging from ceiling")]
    public Wave_Spawner.spawnPointNum spawnPoint;
    public enum ElementType
    {
        neutral,
        fire,
        ice
    }

    //What difficulty of wave can this enemy spawn in on?
    public enum obstacleDiff
    {
        easy,
        medium,
        hardPause,
        bonus,
        timeSwap
    }


    //Is this an enemy or something we can collect?
    public enum typeOfObstacle
    {
        obstacle,
        coin,
        chest,
        timePortal
    }

    [SerializeField]
    ElementType objectElement;

    public obstacleDiff thisObstacleDiff;

    [SerializeField]
    protected typeOfObstacle thisType;

    [Tooltip("The score value of this enemy. Should be in values of 100's.")]
    [SerializeField] protected int scoreValue;

    public Level_Manager.timePeriod theEra;

    protected Rigidbody2D thisRigid;
    private void Awake()
    {
        thisRigid = this.GetComponent<Rigidbody2D>();
        ogSpeed = speed;
        if(maxSpeed == 0)
        {
            maxSpeed = increaseRate;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
       
        endPos = new Vector2((startPos.x - 20), (startPos.y));
      //  Debug.Log("startPos is: " + startPos + " And endPos is: " + endPos);
    }

    //'start' method whenever this is reused via Pooling.
    public void OnObjectSpawn()
    {
        speed = ogSpeed;
        startPos = this.transform.position;
        endPos = new Vector2((startPos.x - 20), (startPos.y));
       // Debug.Log("startPos is: " + startPos + " And endPos is: " + endPos);
    }
  

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //Movement for regular obstacles/treasures and timeswap...Will be changed on other obstacles.
    protected virtual void Movement()
    {
        //If it's anything that's not a chest.
        if (thisType != typeOfObstacle.chest && thisType != typeOfObstacle.timePortal)
        {
            if (increaseRate != 0f)
            {
                if (speed < maxSpeed)
                {
                    speed += increaseRate * Time.deltaTime;
                }
                else
                {
                    speed = maxSpeed;
                }
            }
            else
            {
                if (speed < maxSpeed)
                {
                    speed += 0.15f * Time.deltaTime;
                }
                else
                {
                    speed = maxSpeed;
                }

            }

            if (thisRigid != null)
            {
                thisRigid.velocity = Vector2.left * speed;
            }

        }
        //If it's a chest or time portal.
        else
        {
            endPos = new Vector2((startPos.x - 5), (startPos.y));

            if (this.transform.position != new Vector3(endPos.x, endPos.y, this.transform.position.z))
            {
                speed += 1.0f * Time.deltaTime;
            }

            this.transform.position = Vector2.Lerp(startPos, endPos, speed);

        }

    }

    //Get the element from another script if needed.
    public ElementType getElement()
    {
        return objectElement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Despawner")
        {
            if(thisType == typeOfObstacle.obstacle)
            {
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
         

            //TURN ON WHEN WE ARE READY TO POOL
            thisRigid.velocity = Vector2.zero;
            Object_Pooler.Instance.AddToPool(gameObject);
            //Destroy(gameObject);
            return;
        }
    }
}
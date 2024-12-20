//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver_Bullet : MonoBehaviour
{
    [SerializeField] BoxCollider2D thisCollider;
    [SerializeField] Transform thisTransform;
    [SerializeField] GameObject cowboyParent;
    [SerializeField] Cowboy cowboyScript;
    [SerializeField] Rigidbody2D thisRigid;
    [SerializeField] Animator thisAnimator;
    [SerializeField] bool inPlayerVicinity;

    //1 = up , 2 = down.
    [Tooltip("If shot up, then this is 1, if shot down then it should be 2.")]
    [SerializeField] int shootDirection;
    [SerializeField] float shotSpeedHorizontal;
    float minShotSpeedHorizontalModifier;
    float maxShotSpeedHorizontalModifier;
    [SerializeField] float shotSpeedVertical;
    float minShotSpeedVerticalModifier;
    float maxShotSpeedVerticalModifier;
    [SerializeField] Vector3 ogRot;
    [SerializeField] BoxCollider2D playerCollider;

    [SerializeField] AudioClip ricochetSound;

    bool bouncedYet;

    const string Bounce = "Bounce";

    private void Awake()
    {
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        inPlayerVicinity = false;
        ogRot = this.transform.rotation.eulerAngles;
        thisRigid = this.GetComponent<Rigidbody2D>();
        thisAnimator = this.GetComponent<Animator>();
        bouncedYet = false;
    }

    private void Update()
    {
        
       // thisRigid.velocity = Vector2.left * shotSpeed;
       // thisRigid.velocity = Vector2.up * shotSpeed;
        if(shootDirection == 1)
        {
            thisRigid.velocity = new Vector2(-shotSpeedHorizontal, shotSpeedVertical);
        }
        else
        {
            thisRigid.velocity = new Vector2(-shotSpeedHorizontal -1.5f, -shotSpeedVertical);
        }
    }

    //Initialization.
    public void initializeBullet(int setDir, GameObject theCowboy)
    {
        shootDirection = setDir;
        Vector3 rot = new Vector3(ogRot.x, ogRot.y, ogRot.z);
        if (shootDirection == 1)
        {
            rot = new Vector3(ogRot.x, ogRot.y, ogRot.z - 50);
            this.transform.rotation = Quaternion.Euler(rot);
        }
        else
        {
            rot = new Vector3(ogRot.x, ogRot.y, ogRot.z + 50);
            this.transform.rotation = Quaternion.Euler(rot);
        }
        //shotSpeedHorizontal = theHorSpeed;
        //shotSpeedVertical = theVertSpeed;
        cowboyParent = theCowboy;
        cowboyScript = theCowboy.GetComponent<Cowboy>();

        if (cowboyScript.thisObstacleDiff == Obstacle_Behaviour.obstacleDiff.easy)
        {
            minShotSpeedHorizontalModifier = 3.6f;
            minShotSpeedVerticalModifier = 6.0f;
        }
        else if (cowboyScript.thisObstacleDiff == Obstacle_Behaviour.obstacleDiff.medium)
        {
            minShotSpeedHorizontalModifier = 4.1f;
            minShotSpeedVerticalModifier = 6.9f;
        }
        else
        {
            minShotSpeedHorizontalModifier = 4.5f;
            minShotSpeedVerticalModifier = 7.5f;
        }
        setShotSpeed(minShotSpeedHorizontalModifier, minShotSpeedVerticalModifier);
    }

    public void setShootDirection(int setDir)
    {
        shootDirection = setDir;
    }

    //This will be dependent on the difficulty.
    public void setShotSpeed(float theSpeedHor, float theSpeedVert)
    {
        shotSpeedHorizontal = theSpeedHor;
        shotSpeedVertical = theSpeedVert;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player_Vicinity_Blocker")
        {
            inPlayerVicinity = true;
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Despawner")
        {
            Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            if (cowboyScript.getScoreVal() <= 0)
            {
                Level_Manager.Instance.updateScore(100);
            }
            else
            {
                Level_Manager.Instance.updateScore(cowboyScript.getScoreVal());
            }
            
            //TURN ON WHEN WE ARE READY TO POOL
            thisRigid.velocity = Vector2.zero;
            Object_Pooler.Instance.AddToPool(cowboyParent.gameObject);
            Destroy(gameObject);
            return;
        }

        if(collision.gameObject.tag == "Ceiling")
        {

            if(inPlayerVicinity)
            {
                //Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), playerCollider);
                thisAnimator.SetBool(Bounce, true);
                StartCoroutine(waitBounce());
               //Wave_Spawner.Instance.updateEnemiesLeft(1);
                //Level_Manager.Instance.increaseEnemiesDodged();
                /*
                if (cowboyScript.getScoreVal() <= 0)
                {
                    Level_Manager.Instance.updateScore(100);
                }
                else
                {
                    Level_Manager.Instance.updateScore(cowboyScript.getScoreVal());
                }
              */
                return;
            }
          // thisAnimator.SetBool(Bounce, true);
//            StartCoroutine(waitBounce());

            Vector3 rot = new Vector3(ogRot.x, ogRot.y, ogRot.z);

            shootDirection = 2;
            rot = new Vector3(ogRot.x, ogRot.y, ogRot.z + 50);
            this.transform.rotation = Quaternion.Euler(rot);


            Audio_Manager.Instance.playSFX(ricochetSound, false, 0.3f);
            // thisAnimator.SetBool(Bounce, true);

        }

        if (collision.gameObject.tag == "Ground")
        {
            if (inPlayerVicinity)
            {
                //Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), playerCollider);
                thisAnimator.SetBool(Bounce, true);
                StartCoroutine(waitBounce());
               // Wave_Spawner.Instance.updateEnemiesLeft(1);
              //  Level_Manager.Instance.increaseEnemiesDodged();
              /*
                if (cowboyScript.getScoreVal() <= 0)
                {
                    Level_Manager.Instance.updateScore(100);
                }
                else
                {
                    Level_Manager.Instance.updateScore(cowboyScript.getScoreVal());
                }
              */
                return;
            }

            // thisAnimator.SetBool(Bounce, true);
            // StartCoroutine(waitBounce());

            Vector3 rot = new Vector3(ogRot.x, ogRot.y, ogRot.z);

            shootDirection = 1;
            rot = new Vector3(ogRot.x, ogRot.y, ogRot.z - 50);
            this.transform.rotation = Quaternion.Euler(rot);


            Audio_Manager.Instance.playSFX(ricochetSound,false,0.3f);
            //  thisAnimator.SetBool(Bounce, true);

        }

        if (collision.gameObject.tag == "Player")
        {
            //Wave_Spawner.Instance.updateEnemiesLeft(1);
            //Level_Manager.Instance.increaseEnemiesDodged();
            Level_Manager.Instance.Damage();
            Object_Pooler.Instance.AddToPool(cowboyParent.gameObject);
            Destroy(gameObject);
        }

    }

    IEnumerator waitBounce()
    {
        //thisRigid.velocity = Vector2.zero;
        thisRigid.velocity = Vector2.left * 4.0f;
      //  Debug.Log("Velocity of the bullet is: " + thisRigid.velocity);

        bool animDone = false;


        while(!animDone)
        {
            if (thisAnimator.GetCurrentAnimatorStateInfo(0).IsName("ricochet"))
            {
                thisAnimator.SetBool(Bounce, false);
                animDone = true;
            }
            yield return null;
        }

        
       // Object_Pooler.Instance.AddToPool(cowboyParent.gameObject);
       // Destroy(gameObject);


        yield return null;
    }

}

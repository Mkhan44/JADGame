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
    [SerializeField] float elapsedTime;
    Coroutine teleTimeCo;
    bool isFlipped;
    [Tooltip("A number that determines the rate of teleporting. Closer to 0 means faster rate.")]
    [SerializeField] float teleportRate;
    [Tooltip("Number of teleports allowed. Mostly used for debug. But the ninja can never teleport more than this amount.")]
    [SerializeField] int maxNumTeleports;
    int numTeleportsLeft;

    const string IsTeleporting = "IsTeleporting";
    const string teleportVar = "TeleportVar";
    bool inCoroutine;

    protected override void Awake()
    {
        base.Awake();
        if(maxNumTeleports <= 0)
        {
            maxNumTeleports = 2;
        }
    }

    public override void OnObjectSpawn()
    {
        numTeleportsLeft = maxNumTeleports;
        inCoroutine = false;
        isFlipped = false;
        Vector3 rot = this.transform.rotation.eulerAngles;
        rot = new Vector3(0f, rot.y, rot.z);
        this.transform.rotation = Quaternion.Euler(rot);
        thisRigid.gravityScale = 30;
        elapsedTime = 0f;
        teleTimeCo = StartCoroutine(timeBetweenTele());
        base.OnObjectSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(teleTimeCo == null)
        {
            teleTimeCo = StartCoroutine(timeBetweenTele());
        }
        Movement();
    }

    protected override void Movement()
    {
        base.Movement();   
    }

    IEnumerator timeBetweenTele()
    {
        float tempTeleRate = teleportRate;
        //If the Ninja has already teleported once, we can make it so the next teleport happens potentially faster. A bit of fun extra RNG...I mean challenge xD.
        if(numTeleportsLeft < maxNumTeleports)
        {
            float randomChance;

            if(teleportRate > 1.0f)
            {
                randomChance = Random.Range(1.0f, teleportRate);
                tempTeleRate = randomChance;
              //  Debug.Log("Randomized the tele rate, it's now: " + tempTeleRate);
            }
        }
        float timeVariance = 0f;
        timeVariance = Random.Range(0.3f, 0.7f);
        while(elapsedTime < timeVariance)
        {
            elapsedTime += Time.deltaTime/tempTeleRate;
          //  Debug.Log("Elapsed time is: " + elapsedTime.ToString() + " and timeVariance is: " + timeVariance.ToString());
            yield return null;
        }

        if (inPlayerVicinity || numTeleportsLeft <= 0)
        {
            yield break;
        }
        //Debug.Log("TELEPORTING!");
        numTeleportsLeft-=1;
      //  Debug.Log("Num teleports = " +numTeleportsLeft.ToString());
        elapsedTime = 0f;
        ninjaAnimator.SetBool(IsTeleporting, true);
        ninjaAnimator.SetInteger(teleportVar, 1);
        speed = 0f;
        float tempIncreaseRate = 0f;
        float ogIncreaseRate = increaseRate;
        increaseRate = tempIncreaseRate;

        bool animDone = false;

        //Move ninja to top if it's at bottom, bottom if it's at top.
        while(!animDone)
        {
            if(ninjaAnimator.GetCurrentAnimatorStateInfo(0).IsName("ninjadisappear"))
            {
                animDone = true;
                flipSprite();
            }
            yield return null;
        }

        animDone = false;

        ninjaAnimator.SetBool(IsTeleporting, false);

        while (!animDone)
        {
            if(ninjaAnimator.GetCurrentAnimatorStateInfo(0).IsName("ninjareappear"))
            {
                animDone = true;
                increaseRate = ogIncreaseRate;
            }
            yield return null;
        }
        speed = maxSpeed-1f;

        ninjaAnimator.SetInteger(teleportVar, 0);

        StopCoroutine(teleTimeCo);
        teleTimeCo = null;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Despawner" || collision.gameObject.tag == "Player")
        {
            if(teleTimeCo != null)
            {
                StopCoroutine(teleTimeCo);
                teleTimeCo = null;
            }
          
        }
        base.OnCollisionEnter2D(collision);

        if(collision.gameObject.tag == "Ceiling")
        {
            thisRigid.gravityScale = -5;
        }
        if(collision.gameObject.tag == "Ground")
        {
            thisRigid.gravityScale = 5;
        }
    }

    void flipSprite()
    {
        //Flip updside down.
        if (!isFlipped)
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x + 180, rot.y, rot.z);
            this.transform.rotation = Quaternion.Euler(rot);
            isFlipped = true;
            thisRigid.gravityScale = -30;
        }
        //Flip right side up.
        else
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x - 180, rot.y, rot.z);
            this.transform.rotation = Quaternion.Euler(rot);
            isFlipped = false;
            thisRigid.gravityScale = 30;
        }
        
       
    }
}

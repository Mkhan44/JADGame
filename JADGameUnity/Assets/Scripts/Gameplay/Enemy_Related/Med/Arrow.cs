//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Arrow : Obstacle_Behaviour
{
    int randNum;
    bool hasShot;
    bool hasStopped;
    [SerializeField] Animator arrowAnimator;

    //SFX
    [SerializeField] AudioClip arrowPullbackSound;
    [SerializeField] AudioClip arrowReleaseSound;

    protected override void Awake()
    {
        base.Awake();
      
    }
    protected override void Movement()
    {
        if(!onScreenIndicator)
        {
            base.Movement();
        }
        else
        {
            if(!hasStopped)
            {
                StartCoroutine(fireArrow());
                thisRigid.velocity = Vector2.zero;
                hasStopped = true;
            }
        }
     
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        randNum = 0;
        hasShot = false;
        hasStopped = false;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        thisRigid.gravityScale = 0f;
    }

    //Make it rotate to face up and then shoot so that it arcs.
    IEnumerator fireArrow()
    {
        randNum = Random.Range(1, 3);
        Vector3 thisRotation = this.transform.eulerAngles;
        Vector3 thisPosition = this.transform.position;

        float xIncreaseRate;
        float yIncreaseRateDuck;
        float yIncreaseRateJump;
        float jumpRate;
        float waitRate;

        if(thisObstacleDiff == obstacleDiff.easy)
        {
            xIncreaseRate = -2f;
            yIncreaseRateDuck = 1.5f;
            yIncreaseRateJump = 0.3f;
            waitRate = 0.1f;
        }
        else if(thisObstacleDiff == obstacleDiff.medium)
        {
            xIncreaseRate = -3f;
            yIncreaseRateDuck = 0.8f;
            yIncreaseRateJump = 0.5f;
            jumpRate = 0.06f;
            waitRate = 0.05f;
        }
        else
        {
            xIncreaseRate = -4f;
            yIncreaseRateDuck = 0.7f;
            yIncreaseRateJump = 0.7f;
            jumpRate = 0.09f;
            waitRate = 0.035f;
        }

        Audio_Manager.Instance.playSFX(arrowPullbackSound, false, 0.25f);

        //Arrow to duck under.
        if (randNum == 1)
        {
            while (thisRotation.z > -10)
            {
                thisPosition.x += 0.01f;
                thisRotation.z -= 1;
                this.transform.eulerAngles = thisRotation;
                this.transform.position = thisPosition;
                yield return new WaitForSeconds(waitRate);
            }
            
        }
        else
        {
          //  Debug.LogWarning("Yo, we're gonna jump over the arrow!");
            float pullBackDistance = 0.2f;
            Vector3 pullBackFullDistance = thisPosition;
            pullBackFullDistance.x += pullBackDistance;

            while (thisPosition.x < pullBackFullDistance.x)
            {
                thisPosition.x += 0.02f;
                this.transform.position = thisPosition;
                if(thisPosition.x > pullBackFullDistance.x)
                {
                    thisPosition.x = pullBackFullDistance.x;
                }
                yield return new WaitForSeconds(waitRate);
            }
        }

        Audio_Manager.Instance.stopSFX(arrowPullbackSound.name);
        Audio_Manager.Instance.playSFX(arrowReleaseSound, false, 0.25f);
        //Arrow to duck under.
        if (randNum == 1)
        {
            thisRigid.velocity = new Vector2(xIncreaseRate, yIncreaseRateDuck);
        }
        //Arrow to Jump over.
        else
        {
            thisRigid.velocity = new Vector2(xIncreaseRate, yIncreaseRateJump);
        }

        thisRotation.z -= 1;
        this.transform.eulerAngles = thisRotation;

        //Arrow to duck under.
        if(randNum == 1)
        {
            while(this.transform.position.x > 1.2f && this.transform.position.y < 0.6f)
            {
                yield return null;
            }
        }
        else
        {
           
            while (this.transform.position.x > 1.2f)
            {
               // Debug.LogWarning(this.transform.position.x + " Is the position x value of the arrow.");
                yield return null;
            }
        }

        while (thisRotation.z < 10)
        {
            thisRotation.z += 5;
            if (thisRotation.z > 10)
            {
                thisRotation.z = 10;
            }
            this.transform.eulerAngles = thisRotation;
            yield return new WaitForSeconds(0.01f);
        }

        /*

        //Arrow to duck under.
        if (randNum == 1)
        {
            thisRigid.velocity = new Vector2(-speed, -1.8f);
           // thisRigid.velocity = Vector2.MoveTowards(this.transform.position, Level_Manager.Instance.player.transform.position, 0.5f);
        }
        //Arrow to Jump over.
        else
        {
            thisRigid.velocity = new Vector2(-speed, -5f);
        }
        */

        thisRigid.velocity = Vector2.left * speed;

        while (thisRotation.z < 20)
        {
            thisRotation.z += 5;
            if (thisRotation.z > 20)
            {
                thisRotation.z = 20;
            }
            this.transform.eulerAngles = thisRotation;
            yield return new WaitForSeconds(0.01f);
        }

        //Arrow to duck under.
        if (randNum == 1)
        {
            thisRigid.gravityScale = 0.3f;
        }
        else
        {

            thisRigid.gravityScale = 1.5f;
        }

       
        yield return null;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if(collision.gameObject.tag == "Ground")
        {
            thisRigid.velocity = Vector2.left * speed;
            thisRigid.gravityScale = 0f;
        }
    }


}

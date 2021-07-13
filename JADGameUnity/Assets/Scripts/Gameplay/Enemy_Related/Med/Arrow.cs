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
    }

    //Make it rotate to face up and then shoot so that it arcs.
    IEnumerator fireArrow()
    {
        randNum = Random.Range(1, 3);
        Vector3 thisRotation = this.transform.eulerAngles;
        Vector3 thisPosition = this.transform.position;
        while(thisRotation.z > -10)
        {
            thisPosition.x += 0.01f;
            thisRotation.z -= 1;
            this.transform.eulerAngles = thisRotation;
            this.transform.position = thisPosition;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);
        //Arrow to duck under.
        if (randNum == 1)
        {
            thisRigid.velocity = new Vector2(-2f, 1.5f);
        }
        //Arrow to Jump over.
        else
        {
            thisRigid.velocity = new Vector2(-2f, 0.7f);
        }
        //thisRigid.velocity = new Vector2(-2f, 1.5f);
        thisRotation.z -= 1;
        this.transform.eulerAngles = thisRotation;
       // arrowAnimator.Play("arrow");

        yield return new WaitForSeconds(0.5f);

        while (thisRotation.z < 10)
        {
            thisRotation.z += 2;
            if (thisRotation.z > 10)
            {
                thisRotation.z = 10;
            }
            this.transform.eulerAngles = thisRotation;
            yield return new WaitForSeconds(0.01f);
        }


        //Arrow to duck under.
        if (randNum == 1)
        {
            thisRigid.velocity = new Vector2(-speed, -2.5f);
        }
        //Arrow to Jump over.
        else
        {
            thisRigid.velocity = new Vector2(-speed, -4f);
        }

        while (thisRotation.z < 20)
        {
            thisRotation.z += 2;
            if (thisRotation.z > 20)
            {
                thisRotation.z = 20;
            }
            this.transform.eulerAngles = thisRotation;
            yield return new WaitForSeconds(0.01f);
        }

        // thisRigid.velocity = new Vector2(-speed, -2f);
        // thisRigid.gravityScale = 1f;


        yield return null;
    }


}

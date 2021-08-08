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
    [SerializeField] Animator roverAnimator;
    AnimationClip stopAni;
    bool didStop;
    bool stopping;

    int randEle;
    const string fireGo = "roverfire";
    const string iceGo = "roverice";
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
        randEle = Random.Range(0, 2);

        if(randEle == 0)
        {
            this.objectElement = ElementType.fire;
            roverAnimator.Play(fireGo);
        }
        else
        {
            this.objectElement = ElementType.ice;
            roverAnimator.Play(iceGo);
        }
        changeOutline();


        //Switch to appropriate animation based on element here.
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

    void changeOutline()
    {

        //Change element to opposite + redraw outline.
        foreach (Transform child in transform)
        {
            if (child.GetComponent<SpriteRenderer>() != null)
            {
                outlineChildList.Add(child.GetComponent<SpriteRenderer>().material);
            }

        }
        for (int i = 0; i < outlineChildList.Count; i++)
        {
            outlineChildList[i].SetFloat("_OutlineThickness", 3f);

            if (objectElement == ElementType.fire)
            {
                // Debug.Log("SPAWNED FIRE ELEMENTAL ITEM, CHANGING SHADER.");
                Color fireColor = new Color(212, 139, 57, 255);
                outlineChildList[i].SetColor("_OutlineColor", Color.red);
            }
            else if (objectElement == ElementType.ice)
            {
                // Debug.Log("SPAWNED ICE ELEMENTAL ITEM, CHANGING SHADER.");
                Color iceColor = new Color(70, 219, 213, 255);
                outlineChildList[i].SetColor("_OutlineColor", Color.cyan);
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

        if(randNum == 0)
        {

            yield return new WaitForSeconds(0.3f);

            if(inPlayerVicinity)
            {
                inCoroutine = false;
                didStop = true;
                yield break;
            }
            stopping = true;
            float timeToWait = 0;

            //test
            timeToWait = 0.15f;

            float timePassed = 0f;
            bool toggleStuck = false;


            //Stop it.
            thisRigid.velocity = Vector2.zero;

            while (timePassed < timeToWait)
            {
                timePassed += Time.deltaTime;
                if(toggleStuck)
                {
                    thisRigid.velocity = Vector2.right * 3.0f;
                    toggleStuck = false;
                }
                else
                {
                    thisRigid.velocity = Vector2.left * 3.0f;
                    toggleStuck = true;
                }
           
                yield return new WaitForSeconds(0.1f);
            }

            if(objectElement == ElementType.fire)
            {
                objectElement = ElementType.ice;
                roverAnimator.Play(iceGo);

            }
            else
            {
                objectElement = ElementType.fire;
                roverAnimator.Play(fireGo);
            }
            changeOutline();


            //Get the animationClip time.
            //stopAni = roverAnimator.runtimeAnimatorController.animationClips[0];
            //timeToWait = stopAni.length;


            //Play animation.

            //yield return new WaitForSeconds(timeToWait);

            //Placeholder.
            // yield return new WaitForSeconds(1.5f);
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

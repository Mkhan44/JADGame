//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Wizard : Obstacle_Behaviour
{
    [SerializeField] GameObject fireTellParticlePrefab;
    [SerializeField] GameObject iceTellParticlePrefab;
    [SerializeField] GameObject fireBeamPrefab;
    [SerializeField] GameObject iceBeamPrefab;

    GameObject fireBeamInstance;
    GameObject iceBeamInstance;
    GameObject fireTellParticleInstance;
    GameObject iceTellParticleInstance;

    GameObject currentTellParticle;
    GameObject currentBeamInstance;
    [SerializeField] Animator wizardAnimator;
    bool hasTeledIn;
    bool hasCast;
    int randCast;

    Coroutine teleCo;

    //Animation clips & their lengths.
    AnimationClip teleAni;
    AnimationClip castUpAni;
    AnimationClip castDownAni;
    AnimationClip castUpIdleAni;
    AnimationClip castDownIdleAni;

    const string dynamitefadeName = "dynamitefade";
    const string dynamiteidleName = "dynamiteidle";
    const string dynaBool = "Explode";

    protected override void Awake()
    {
        base.Awake();
       
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        onScreenIndicator = false;
        teleCo = null;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
        hasTeledIn = false;
        hasCast = false;
        randCast = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Movement()
    {
        if(!onScreenIndicator)
        {
            base.Movement();
            Debug.Log("Moving using regular movement.");
        }
        else
        {
            if(!hasTeledIn)
            {
                teleCo = StartCoroutine(teleAniCo());
                Debug.Log("hasTeledIn is false, so we started the teleCoroutine.");
            }
            else
            {
                if(hasCast)
                {
                    if(teleCo != null)
                    {
                        Debug.Log("hasTeledIn is TRUE && hasCase is TRUE && teleCo is not null. So we are adding this back to the pool.");
                        hasTeledIn = false;
                        onScreenIndicator = false;
                        Wave_Spawner.Instance.updateEnemiesLeft(1);
                        Level_Manager.Instance.increaseEnemiesDodged();
                        Object_Pooler.Instance.AddToPool(gameObject);
                        return;
                    }
                    
                }
            }

            thisRigid.velocity = Vector2.zero;
        }
        
    }

    //Teleport in and then cast a spell OR teleport out and add back to pooler.
    IEnumerator teleAniCo()
    {
        float lengthOfAni;
        
        if(!hasTeledIn)
        {
            //Teleport Wizard in.
            teleAni = wizardAnimator.runtimeAnimatorController.animationClips[0];
           // Debug.Log(teleAni.name);
            wizardAnimator.Play("wizardtele");
            hasTeledIn = true;
        }
        else
        {
            //Teleport wizard out.
            teleAni = wizardAnimator.runtimeAnimatorController.animationClips[4];
           // Debug.Log(teleAni.name);
            wizardAnimator.Play("wizardteleout");
            lengthOfAni = teleAni.length;
            yield return new WaitForSeconds(lengthOfAni);
            this.transform.position = new Vector2(this.transform.position.x + 2, this.transform.position.y);
            hasCast = true;
            yield break;
        }
        lengthOfAni = teleAni.length;
        yield return new WaitForSeconds(lengthOfAni);

        //Have wizard in looping idle state.
        wizardAnimator.Play("wizardidle");

        //May change this depending on difficulty so there is less of a tell for harder difficulties.
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(castSpell());

        yield return null;
    }

    //Spawn tell on top or bottom, wait for x amount of seconds, then fire beam.
    IEnumerator castSpell()
    {
        randCast = Random.Range(1, 3);
        float castAniWait = 0f;
        if (randCast == 1)
        {
            fireTellParticleInstance = Instantiate(fireTellParticlePrefab);
            castUpAni = wizardAnimator.runtimeAnimatorController.animationClips[1];
            castAniWait = castUpAni.length;
            yield return new WaitForSeconds(1.5f);

            wizardAnimator.Play("wizardtopcast");
        }
        else
        {
            iceTellParticleInstance = Instantiate(iceTellParticlePrefab);
            castDownAni = wizardAnimator.runtimeAnimatorController.animationClips[2];
            castAniWait = castDownAni.length;
            yield return new WaitForSeconds(1.5f);

            wizardAnimator.Play("wizardbotcast");
        }

        yield return new WaitForSeconds(castAniWait);

        Destroy(fireTellParticleInstance);
        Destroy(iceTellParticleInstance);

       // fireBeamInstance = Instantiate(fireBeamPrefab);
      //  iceBeamInstance = Instantiate(iceBeamInstance);

        //This will probably change based on difficulty or be randomized slightly.
        yield return new WaitForSeconds(1.5f);

        //Should teleport out after calling this.
        teleCo = StartCoroutine(teleAniCo());

        yield return null;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner")
        {

            if (thisType == typeOfObstacle.obstacle)
            {
                Wave_Spawner.Instance.updateEnemiesLeft(1);
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
            inPlayerVicinity = false;

            //TURN ON WHEN WE ARE READY TO POOL
            thisRigid.velocity = Vector2.zero;
            Object_Pooler.Instance.AddToPool(gameObject);
            //Destroy(gameObject);
            return;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (thisType == typeOfObstacle.obstacle)
        {
            if (collision.gameObject.tag == "Player_Vicinity_Blocker")
            {
                inPlayerVicinity = true;
            }

            if (collision.gameObject.tag == "On_screen_Vicinity_Collider")
            {
                onScreenIndicator = true;
                //Debug.Log(gameObject.name + " Is now on screen!");
            }

            //In here we will make whatever arrow it is highlighted and then begin the coroutine to fade it out.
            if (collision.gameObject.tag == "Indicator_Vicinity_Collider")
            {
                inIndicatorVicinity = true;
                //    Debug.Log("Obstacle has been spawned, trigger indicator at spawnpoint: " + spawnPoint.ToString());
                Level_Manager.Instance.indicatorArrow(spawnPoint);
            }
        }
        if (thisType == typeOfObstacle.coin)
        {
            if (collision.gameObject.tag == "Despawner")
            {
                thisRigid.velocity = Vector2.zero;
                //  Debug.Log("Adding coin to pool since it collided with despawner!");
                Object_Pooler.Instance.AddToPool(gameObject);
                return;
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (thisType == typeOfObstacle.obstacle)
        {
            if (collision.gameObject.tag == "Indicator_Vicinity_Collider")
            {
                inIndicatorVicinity = false;

                // Debug.Log(gameObject.name + " Left the indicator collider!");
                Level_Manager.Instance.indicatorArrowOff(spawnPoint);
            }
        }
    }

}

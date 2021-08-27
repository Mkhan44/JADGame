//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Dynamite_Cart : Obstacle_Behaviour
{
    [SerializeField] GameObject explosionParticlePrefab;
    [SerializeField] GameObject boomParticlePrefab;
    [SerializeField] GameObject bangParticlePrefab;

    GameObject currentComicEffectParticleInstance;
    GameObject currentParticleInstance;
    [SerializeField] Animator dynaAnimator;

    const string dynamitefadeName = "dynamitefade";
    const string dynamiteidleName = "dynamiteidle";
    const string dynaBool = "Explode";

    protected override void Awake()
    {
        base.Awake();

        outlineMat = this.GetComponent<SpriteRenderer>().material;
        if (outlineMat == null && thisType != typeOfObstacle.obstacle)
        {
            Debug.LogError("We can't find the material of this object!");
        }

        outlineMat.SetFloat("_OutlineThickness", 3f);

        if (objectElement == ElementType.fire)
        {
            // Debug.Log("SPAWNED FIRE ELEMENTAL ITEM, CHANGING SHADER.");
            Color fireColor = new Color(212, 139, 57, 255);
            outlineMat.SetColor("_OutlineColor", Color.red);
        }
        else if (objectElement == ElementType.ice)
        {
            // Debug.Log("SPAWNED ICE ELEMENTAL ITEM, CHANGING SHADER.");
            Color iceColor = new Color(70, 219, 213, 255);
            outlineMat.SetColor("_OutlineColor", Color.cyan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        Destroy(currentParticleInstance);
        Destroy(currentComicEffectParticleInstance);
        dynaAnimator.SetBool(dynaBool, false);
    }

    protected override void Movement()
    {
        base.Movement();
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner" || collision.gameObject.tag == "Player")
        {


        }
        base.OnCollisionEnter2D(collision);
    }

    IEnumerator explosionAni()
    {
        AnimationClip dynaClip;

        dynaClip = dynaAnimator.runtimeAnimatorController.animationClips[0];

        float aniTime = dynaClip.length;


        dynaAnimator.SetBool(dynaBool, true);

       // yield return new WaitForSeconds(aniTime - 0.1f);

        //Vector2 particlePos = new Vector2(dynaAnimator.gameObject.transform.position.x, dynaAnimator.gameObject.transform.position.y + 0.2f);
        Vector2 particlePos = new Vector2(dynaAnimator.gameObject.transform.position.x, dynaAnimator.gameObject.transform.position.y + 0.35f);

        Vector2 particlePosComic = new Vector2(dynaAnimator.gameObject.transform.position.x, dynaAnimator.gameObject.transform.position.y + 0.25f);

        currentParticleInstance = Instantiate(explosionParticlePrefab, particlePos, this.transform.rotation);
        currentParticleInstance.transform.SetParent(gameObject.transform, true);
        currentParticleInstance.transform.SetSiblingIndex(0);

        {
            currentComicEffectParticleInstance = Instantiate(boomParticlePrefab, particlePos, this.transform.rotation);
            currentComicEffectParticleInstance.transform.SetParent(gameObject.transform, true);
            currentComicEffectParticleInstance.transform.SetSiblingIndex(0);
        }



        yield return new WaitForSeconds(1.0f);

        

        yield return null;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (thisType == typeOfObstacle.obstacle)
        {
            if (collision.gameObject.tag == "Player_Vicinity_Blocker")
            {
                int randNum = Random.Range(0, 2);

                //TEST
                randNum = 0;
                if(randNum == 0)
                {
                    Debug.Log("Explode!");
                    StartCoroutine(explosionAni());
                    playSoundExternally();
                }
                else
                {
                    Debug.Log("No explode.");
                }

                inPlayerVicinity = true;

            }

            if (collision.gameObject.tag == "On_screen_Vicinity_Collider")
            {
                onScreenIndicator = true;
                Debug.Log(gameObject.name + " Is now on screen!");
            }

                //In here we will make whatever arrow it is highlighted and then begin the coroutine to fade it out.
                if (collision.gameObject.tag == "Indicator_Vicinity_Collider")
                {
                    inIndicatorVicinity = true;
                    //    Debug.Log("Obstacle has been spawned, trigger indicator at spawnpoint: " + spawnPoint.ToString());
                    Level_Manager.Instance.indicatorArrow(spawnPoint);
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

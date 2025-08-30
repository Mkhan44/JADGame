//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shotgun : Obstacle_Behaviour
{
    [SerializeField] Animator shotgunAnimator;

    [Header("Big shell variables")]
    [SerializeField] GameObject shotgunShellPrefab;
    [SerializeField] BoxCollider2D shellCollider;
    [SerializeField] GameObject shotgunShellInstance;
    [SerializeField] Rigidbody2D bigShellRigid;

    [Header("Small shell variables")]
    [SerializeField] GameObject smallShellPrefab;
    GameObject smallShell1;
    GameObject smallShell2;
    [SerializeField] BoxCollider2D smallShellCollider1;
    [SerializeField] Rigidbody2D smallShellRigid1;
    [SerializeField] BoxCollider2D smallShellCollider2;
    [SerializeField] Rigidbody2D smallShellRigid2;

    [SerializeField] bool isFlipped;

    [SerializeField] float horizontalBulletSpeed;
    [SerializeField] float verticalBulletSpeed;

    [SerializeField] AudioClip splitSound;

    const string shootShot = "shotgun";

    enum shellSplitFormation
    {
        topMid,
        topBottom,
        midBottom
    }
    //Stuff local to cowboy.
    bool isStopped;
    bool hasShot;

    public override void OnObjectSpawn()
    {
        smallShell1 = null;
        smallShell2 = null;
        bigShellRigid = null;
        isFlipped = false;
        isStopped = false;
        hasShot = false;
        shellCollider.gameObject.SetActive(false);
        Vector3 rot = this.transform.rotation.eulerAngles;
        rot = new Vector3(0f, rot.y, rot.z);
        this.transform.rotation = Quaternion.Euler(rot);
        base.OnObjectSpawn();
    }
    protected override void Movement()
    {
        if (!hasShot)
        {
            if (thisRigid != null && !onScreenIndicator)
            {
                base.Movement();
            }

            if (onScreenIndicator)
            {
                if (!isStopped)
                {
                    isStopped = true;
                    thisRigid.linearVelocity = Vector2.zero;
                    // Debug.Log("onScreenIndicator in movement function!");
                     StartCoroutine(shootShell());
                }

            }
        }
        else
        {
            thisRigid.linearVelocity = Vector2.right * speed;
        }

        if(shotgunShellInstance != null)
        {
            bigShellRigid.linearVelocity = Vector2.left * 0.5f;
        }


        if(smallShell1 != null && smallShell2 != null)
        {
            if (thisObstacleDiff == obstacleDiff.easy)
            {
                smallShellRigid1.linearVelocity = Vector2.left * 2.8f;
                smallShellRigid2.linearVelocity = Vector2.left * 3.0f;
            }
            else if (thisObstacleDiff == obstacleDiff.medium)
            {
                smallShellRigid1.linearVelocity = Vector2.left * 3.5f;
                smallShellRigid2.linearVelocity = Vector2.left * 3.7f;
            }
            else
            {
                smallShellRigid1.linearVelocity = Vector2.left * 3.8f;
                smallShellRigid2.linearVelocity = Vector2.left * 4.0f;
            }

        }

        
    }

    IEnumerator shootShell()
    {
        //1 to 3.
        int randNum = Random.Range(1, 4);

        shellCollider.gameObject.SetActive(true);
        //Gotta figure out why offset is changing to something other than this at runtime.
        shellCollider.offset = new Vector2(-0.3811183f, 0.02361713f);
        AnimationClip shottyClip;
        //Since there is only 1 animation , this should be the clip for cocking and shooting the shotgun.
        shottyClip = shotgunAnimator.runtimeAnimatorController.animationClips[0];
        if (thisObstacleDiff == obstacleDiff.easy)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else if (thisObstacleDiff == obstacleDiff.medium)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }




        shotgunAnimator.Play(shootShot);

       // Debug.Log("The animation length of: " + shottyClip.name + " is: " + shottyClip.length);

        if(thisObstacleDiff == obstacleDiff.easy)
        {
            yield return new WaitForSeconds(shottyClip.length - 0.6f);
        }
        else if (thisObstacleDiff == obstacleDiff.medium)
        {
            yield return new WaitForSeconds(shottyClip.length - 0.4f);
        }
        else
        {
            yield return new WaitForSeconds(shottyClip.length - 0.2f);
        }


        Vector3 tempPos = new Vector3(shellCollider.gameObject.transform.position.x - 0.5f, shellCollider.gameObject.transform.position.y, shellCollider.gameObject.transform.position.z);
        // shellCollider.offset = new Vector2(-0.3811183f, 0.02361713f);
        shotgunShellInstance = Instantiate(shotgunShellPrefab, shellCollider.transform);
        bigShellRigid = shotgunShellInstance.GetComponent<Rigidbody2D>();

        if (thisObstacleDiff == obstacleDiff.easy)
        {
            yield return new WaitForSeconds(1.0f);
        }
        else if (thisObstacleDiff == obstacleDiff.medium)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        hasShot = true;

        //Let bullet travel for a bit before splitting.

        Transform spawnTrans = shotgunShellInstance.transform;
        Vector3 topSpawnPos = new Vector3(shotgunShellInstance.transform.position.x, shotgunShellInstance.transform.position.y + 1.7f);
        Vector3 botSpawnPos = new Vector3(shotgunShellInstance.transform.position.x, shotgunShellInstance.transform.position.y - 1.0f);
        Vector3 bigShellPos = new Vector3(shotgunShellInstance.transform.position.x, shotgunShellInstance.transform.position.y - 0.3f);

        switch(randNum)
        {
            //Top and mid.
            case 1:
                {
                    Debug.Log("Spawning top and mid for shotty shells.");
                    smallShell1 = Instantiate(smallShellPrefab, topSpawnPos, this.transform.rotation);
                    smallShell2 = Instantiate(smallShellPrefab, bigShellPos, this.transform.rotation);
                    break;
                }
                //Mid and bottom.
            case 2:
                {
                    Debug.Log("Spawning Mid and Bottom for shotty shells.");
                    smallShell1 = Instantiate(smallShellPrefab, botSpawnPos, this.transform.rotation);
                    smallShell2 = Instantiate(smallShellPrefab, bigShellPos, this.transform.rotation);
                    break;
                }
                //Top and Bottom.
            case 3:
                {
                    Debug.Log("Spawning Top and Bottom for shotty shells.");
                    smallShell1 = Instantiate(smallShellPrefab, topSpawnPos, this.transform.rotation);
                    smallShell2 = Instantiate(smallShellPrefab, botSpawnPos, this.transform.rotation);
                    break;
                }
        }
        Audio_Manager.Instance.playSFX(splitSound);

        smallShell1.GetComponent<Shotgun_Shell>().initializeShell(smallShell2, this);
        smallShell2.GetComponent<Shotgun_Shell>().initializeShell(smallShell1, this);

        Destroy(shotgunShellInstance);

        smallShellCollider1 = smallShell1.GetComponent<BoxCollider2D>();
        smallShellCollider2 = smallShell2.GetComponent<BoxCollider2D>();
        smallShellRigid1 = smallShell1.GetComponent<Rigidbody2D>();
        smallShellRigid2 = smallShell2.GetComponent<Rigidbody2D>();



        yield return null;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner" || collision.gameObject.tag == "Player")
        {


        }
        base.OnCollisionEnter2D(collision);
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
                Debug.Log(gameObject.name + " Is now on screen!");
            }

            if (!hasShot)
            {
                //In here we will make whatever arrow it is highlighted and then begin the coroutine to fade it out.
                if (collision.gameObject.tag == "Indicator_Vicinity_Collider")
                {
                    inIndicatorVicinity = true;
                    //    Debug.Log("Obstacle has been spawned, trigger indicator at spawnpoint: " + spawnPoint.ToString());
                    Level_Manager.Instance.indicatorArrow(spawnPoint);
                }
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

                if (!hasShot)
                {
                    // Debug.Log(gameObject.name + " Left the indicator collider!");
                    Level_Manager.Instance.indicatorArrowOff(spawnPoint);
                }

            }
        }


    }



}

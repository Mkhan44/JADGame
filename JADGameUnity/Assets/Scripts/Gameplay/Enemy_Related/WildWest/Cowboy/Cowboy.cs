//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cowboy : Obstacle_Behaviour
{
    [SerializeField] Animator cowboyAnimator;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] bool atStandingPoint;
    Coroutine teleTimeCo;
    [SerializeField] BoxCollider2D bulletCollider;
    [SerializeField] BoxCollider2D gunHitbox;
    [SerializeField] bool isFlipped;

    [SerializeField] float horizontalBulletSpeed;
    [SerializeField] float verticalBulletSpeed;
    //Stuff local to cowboy.
    bool isStoppedBool;
    bool hasShot;

    const string IsStopped = "IsStopped";
    const string IsSpinning = "IsSpinning";
    const string ShootDirection = "ShootDirection";

    protected override void Awake()
    {
        base.Awake();
        atStandingPoint = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    protected override void Movement()
    {
        if (thisRigid != null && !onScreenIndicator)
        {
            base.Movement();
        }

        if(onScreenIndicator)
        {
          
            if(!isStoppedBool)
            {
                isStoppedBool = true;
                thisRigid.velocity = Vector2.zero;
               // Debug.Log("onScreenIndicator in movement function!");
                StartCoroutine(shootGun());
            }
         
        }
    }

    public override void OnObjectSpawn()
    {
        isFlipped = false;
        isStoppedBool = false;
        hasShot = false;
        Vector3 rot = this.transform.rotation.eulerAngles;
        rot = new Vector3(0f, rot.y, rot.z);
        this.transform.rotation = Quaternion.Euler(rot);
        base.OnObjectSpawn();
    }

    //Shoot coroutine.
    IEnumerator shootGun()
    {
        bool animDone = false;
        cowboyAnimator.SetBool(IsSpinning, true);
        cowboyAnimator.SetBool(IsStopped, true);

        //1 or 2.
        int randNum = Random.Range(1, 3);

        //Duration of the animation.
        float animDuration = 0;

        yield return new WaitForSeconds(2f);

        
        //Shoot up.
        if(randNum == 1)
        {
            
            cowboyAnimator.SetInteger(ShootDirection, 1);

            while (!animDone)
            {
                if (cowboyAnimator.GetCurrentAnimatorStateInfo(0).IsName("cowboyshootup"))
                {
                    yield return new WaitForSeconds(0.2f);
                    animDone = true;
                    cowboyAnimator.SetInteger(ShootDirection, 3);
                    GameObject tempBullet = Instantiate(bulletPrefab, gunHitbox.transform);
                    tempBullet.transform.parent = null;

                    //Change the speed based on difficulty!
                    tempBullet.GetComponent<Revolver_Bullet>().initializeBullet(randNum, horizontalBulletSpeed, verticalBulletSpeed, this.gameObject);
                    bulletCollider = tempBullet.GetComponent<BoxCollider2D>();
                }
                yield return null;
            }
            animDone = false;
        }
        else
        {
            cowboyAnimator.SetInteger(ShootDirection, 2);

            while (!animDone)
            {
                if (cowboyAnimator.GetCurrentAnimatorStateInfo(0).IsName("cowboyshootdown"))
                {
                    yield return new WaitForSeconds(0.2f);
                    animDone = true;
                    cowboyAnimator.SetInteger(ShootDirection, 3);
                    GameObject tempBullet = Instantiate(bulletPrefab, gunHitbox.gameObject.transform);
                    tempBullet.transform.parent = null;

                    //Change the speed based on difficulty!
                    tempBullet.GetComponent<Revolver_Bullet>().initializeBullet(randNum, horizontalBulletSpeed, verticalBulletSpeed, this.gameObject);
                    bulletCollider = tempBullet.GetComponent<BoxCollider2D>();
                }
                yield return null;
            }
            animDone = false;
        }
       

        //While bullet has not hit the player or gone off screen yet...

        //After bullet hits/offscreens...

        yield return new WaitForSeconds(1f);

        flipSprite();
        hasShot = true;
        cowboyAnimator.SetBool(IsStopped, false);
        speed = 0f;

        //Need to refactor this code so that it lets the cowboy leave the screen completely.
        while (speed < maxSpeed+5)
        {
            speed = maxSpeed;

            if (thisRigid != null)
            {
                thisRigid.velocity = Vector2.right * speed;
            }

            if(inIndicatorVicinity)
            {
                break;
            }
            yield return null;
        }

     //   Debug.Log("FINISHED THE LOOP");

       
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

                if(!hasShot)
                {
                    // Debug.Log(gameObject.name + " Left the indicator collider!");
                    Level_Manager.Instance.indicatorArrowOff(spawnPoint);
                }
                
            }
        }


    }

    void flipSprite()
    {
        if (!isFlipped)
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            this.transform.rotation = Quaternion.Euler(rot);
            isFlipped = true;
        }
        else
        {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y - 180, rot.z);
            this.transform.rotation = Quaternion.Euler(rot);
            isFlipped = false;
        }


    }
}

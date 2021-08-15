//Code written by Mohamed Riaz Khan of BukuGames.
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
    [HideInInspector]
    public float speed = 0f;

    float ogSpeed;
    [Tooltip("How fast will this object go? This determines the speed increase rate.")]
    public float increaseRate;
    [Tooltip("Maximum speed this object should approach. If 0f , it will be set to increase rate by default.")]
    public float maxSpeed;
    //We will have spawn points set up in the Wave_Spawner script.
    [Tooltip("Spawnpoint1 = Mid, Spawnpoint2 = Bottom, Spawnpoint3 = hanging from ceiling")]
    public Wave_Spawner.spawnPointNum spawnPoint;

    [Tooltip("We will use this to determine if objects should flip and what not. Since player can only move verticaly, things shouldn't be able to drop on them.")]
    protected bool inPlayerVicinity;

    [Tooltip("This will be used for indicator arrows.")]
    protected bool inIndicatorVicinity;

    [Tooltip("This will indicate when the obstacle is on the screen.")]
    protected bool onScreenIndicator;

    [Tooltip("This is going to be an instance of the material for the outline that is created at runtime. We will use this to change outline shader on the fly.")]
    protected Material outlineMat;
    protected List<Material> outlineChildList = new List<Material>();

    [Tooltip("If there is a hitbox we don't want to count when despawning the object, assign it here.")]
    [SerializeField] protected BoxCollider2D extraCollider;

    private BoxCollider2D theDespawnerCollider;
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
    protected ElementType objectElement;

    public obstacleDiff thisObstacleDiff;

    [SerializeField]
    protected typeOfObstacle thisType;

    [Tooltip("The score value of this enemy. Should be in values of 100's.")]
    [SerializeField] protected int scoreValue;

    [Tooltip("ONLY USE FOR TIMEPORTALS!!!! Leave blank otherwise.")]
    public Level_Manager.timePeriod theEra;

    protected Rigidbody2D thisRigid;

    [Header("SFX Related")]
    [Tooltip("Sound effect used for this obstacle.")]
    [SerializeField] AudioClip soundToPlay;
    [Tooltip("Whether or not this sound will loop.")]
    [SerializeField] bool willLoop;
    [Tooltip("Does this sound effect play at a specific time or as soon as the obstacle is on screen?")]
    [SerializeField] bool playOnStart;
    [Tooltip("Does this need to be louder or softer than 0.1f? MAX VALUE SHOULD BE 1.")]
    [SerializeField] float volumeOverride;
    bool hasPlayedYet;
    int audioManagerReferenceNum;

    bool madeToEnd = false;
    protected virtual void Awake()
    {
        thisRigid = this.GetComponent<Rigidbody2D>();
        ogSpeed = speed;
        if(maxSpeed == 0)
        {
            maxSpeed = increaseRate;
        }
        inPlayerVicinity = false;
        onScreenIndicator = false;
        hasPlayedYet = false;
        audioManagerReferenceNum = -1;

        if (this.transform.childCount > 0)
        {
            foreach (Transform child in transform)
            {
                if(child.GetComponent<SpriteRenderer>() != null)
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

            if(this.GetComponent<SpriteRenderer>() != null)
            {
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

        }

        else
        {
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

    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
       
        endPos = new Vector2((startPos.x - 20), (startPos.y));
      //  Debug.Log("startPos is: " + startPos + " And endPos is: " + endPos);
    }

    //'start' method whenever this is reused via Pooling.
    public virtual void OnObjectSpawn()
    {
        speed = 0f;
        inPlayerVicinity = false;
        onScreenIndicator = false;
        speed = ogSpeed;
        startPos = this.transform.position;
        endPos = new Vector2((startPos.x - 20), (startPos.y));

        if(extraCollider != null)
        {
            theDespawnerCollider = GameObject.Find("Despawner").GetComponent<BoxCollider2D>();
            Physics2D.IgnoreCollision(extraCollider, theDespawnerCollider);
        }
        hasPlayedYet = false;
        audioManagerReferenceNum = -1;
       // Debug.Log("startPos is: " + startPos + " And endPos is: " + endPos);
    }
  

    // Update is called once per frame
    void Update()
    {
        Movement();

        if(!hasPlayedYet && playOnStart && !inIndicatorVicinity && soundToPlay != null)
        {
            if(volumeOverride == 0 || volumeOverride > 1 || volumeOverride < -1)
            {
                audioManagerReferenceNum = Audio_Manager.Instance.playSFX(soundToPlay, willLoop);
            }
            else
            {
                audioManagerReferenceNum = Audio_Manager.Instance.playSFX(soundToPlay, willLoop, volumeOverride);
            }
            
            hasPlayedYet = true;
          //  Debug.Log("Playing sound for obstacle!");
        }
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
            endPos = new Vector2((startPos.x - 5.5f), (startPos.y));
            speed = 7f;
            if(thisRigid != null)
            {
                if (this.transform.position.x > endPos.x)
                {
                    thisRigid.velocity = Vector2.left * speed;
                }
                else
                {
                    this.transform.position = new Vector2(endPos.x, this.transform.position.y);
                    thisRigid.velocity = Vector2.zero;
                }
                   
            }

            /*
            if(!madeToEnd)
            {
                endPos = new Vector2((startPos.x - 5.5f), (startPos.y));
                
                if (this.transform.position != new Vector3(endPos.x, endPos.y, this.transform.position.z))
                {
                    speed += 1.0f * Time.deltaTime;
                }
                

                this.transform.position = Vector2.Lerp(startPos, endPos, speed);
            }

            if(this.transform.position == new Vector3 (endPos.x, endPos.y))
            {
                madeToEnd = true;
            }
            */
         

        }

    }

    //Get the element from another script if needed.
    public ElementType getElement()
    {
        return objectElement;
    }

    //Get Score value.

    public int getScoreVal()
    {
        return scoreValue;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Despawner")
        {
            if(audioManagerReferenceNum != -1)
            {
                Audio_Manager.Instance.stopSFX(audioManagerReferenceNum);
            }

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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (thisType == typeOfObstacle.obstacle)
        {
            if (collision.gameObject.tag == "Player_Vicinity_Blocker")
            {
                inPlayerVicinity = true;
            }

            if(collision.gameObject.tag == "On_screen_Vicinity_Collider")
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
      if(thisType == typeOfObstacle.coin)
        {
            if(collision.gameObject.tag == "Despawner")
            {
                thisRigid.velocity = Vector2.zero;
              //  Debug.Log("Adding coin to pool since it collided with despawner!");
                Object_Pooler.Instance.AddToPool(gameObject);
                return;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
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

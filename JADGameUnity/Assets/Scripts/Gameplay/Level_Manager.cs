using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
public class Level_Manager : MonoBehaviour
{
    public GameObject player;
    public Button jumpButton;
    public Button duckButton;

    List<GameObject> powerUps;
    Player thePlayer;
    Animator playerAnimator;
    Rigidbody2D playerRigid2D;
    Vector2 playerInitialPos;

    [SerializeField]
    float gravityScale;
    [SerializeField]
    float jumpHeight;

    bool jumpComplete;

    int currentPlayerHealth;
    public TextMeshProUGUI healthText;

    //Obstacles coming towards the player.
    public List<GameObject> obstacles;

    //Time

    float elapsedTime;

    float fullTime;


    public float spawnTime;
    public float increaseSpawnThreshold;


    //Meters
    public Temperature_Manager heatMeter;
    public Temperature_Manager iceMeter;
    public bool meterFilled;

    private void Awake()
    {
        thePlayer = player.GetComponent<Player>();
        playerAnimator = player.GetComponent<Animator>();

        playerRigid2D = player.GetComponent<Rigidbody2D>();

        Input.multiTouchEnabled = false;
    }

    private void Start()
    {


        // currentPlayerHealth = thePlayer.getCurrentHealth();

        if (currentPlayerHealth == 0)
        {
            //Default = 3.
            currentPlayerHealth = 3;
           // healthText.text = "Health: " + currentPlayerHealth;
        }
        playerInitialPos = thePlayer.getInitialPos();

        gravityScale = thePlayer.getGravity();
        playerRigid2D.gravityScale = gravityScale;
        jumpHeight = thePlayer.getJumpHeight();

        

    }

    private void Update()
    {
        //Does stuff according to player's current state.

        
        checkState();
        
        elapsedTime += Time.deltaTime;
        fullTime += Time.deltaTime;

        if (elapsedTime >= spawnTime && player.activeInHierarchy)
        {
            spawnObstacles();
            elapsedTime = 0f;
            if (fullTime >= increaseSpawnThreshold && spawnTime >= 1)
            {
                spawnTime -= 0.1f;

                //Increase threshold by a set amount maybe?
                Debug.Log("GETTING FASTER!");
                increaseSpawnThreshold += 1;
            }
        }  
    }

    //Jump.
    public void Jump()
    {
        //Jump height is based off of the value we set for player jump height.
        playerRigid2D.velocity = Vector2.up * jumpHeight;

        //player.transform.position = currentPos;

        //  StartCoroutine(jumpUp(currentPos));
        jumpButton.interactable = false;
        thePlayer.setState(Player.playerState.jumping);
        duckButton.interactable = false;


    }

    //Player is hanging from the top.
    public void Hang()
    {
        playerRigid2D.gravityScale = 0f;
        jumpButton.interactable = false;
        duckButton.interactable = false;
    }

   
    public void duck()
    {
        // Debug.Log("Calling duck");
        thePlayer.setState(Player.playerState.ducking);

        playerAnimator.SetBool("IsCrouching", true);
        duckButton.interactable = false;
        jumpButton.interactable = false;
        // StartCoroutine(getUp());
    }

    public void getUpReg()
    {
        thePlayer.setState(Player.playerState.idle);
        playerAnimator.SetBool("IsCrouching", false);
        duckButton.interactable = true;
    }


    //Stuff happening in UPDATE//
    void checkState()
    {
        if (player.activeInHierarchy)
        {
            //checkMeter();
            switch (thePlayer.GetState())
            {
                case Player.playerState.ducking:
                    {
                        //Do ducking stuff...

                        //If we're holding the button.
                        if (Input.GetMouseButton(0) || Input.GetKey("down"))
                        {
                            //Stay crouched. Increase cool meter.
                            //Coolmeter += something...
                         //   StartCoroutine(iceMeter.fillConstant());
                         //   StartCoroutine(heatMeter.decreaseConstant());
                        }
                        else
                        {
                            getUpReg();
                        }
                        break;
                    }
                case Player.playerState.jumping:
                    {
                        //They hold the button here so hang.
                        if (Input.GetMouseButton(0) || Input.GetKey("up"))
                        {
                            //Hanging feature if we decide to implement it. Otherwise just count this as spam jumping maybe?
                            
                           thePlayer.setState(Player.playerState.hanging);
                           
                        }
                        //Not holding so don't hang.
                        else
                        {
                            if(player.transform.position.y <= 0.1)
                            {
                                thePlayer.setState(Player.playerState.idle);
                                jumpButton.interactable = true;
                            }
                        }
                        break;
                    }
                case Player.playerState.hanging:
                    {
                        // StartCoroutine(heatMeter.fillConstant());
                        // StartCoroutine(iceMeter.decreaseConstant());
                        if (Input.GetMouseButton(0) || Input.GetKey("up"))
                        {
                            Hang();
                        }
                        else
                        {
                            //Prolly have some variable to dictate this.
                            playerRigid2D.gravityScale = gravityScale;
                            thePlayer.setState(Player.playerState.idle);

                        }
                        
                        break;
                    }
                case Player.playerState.dead:
                    {
                        player.SetActive(false);
                        duckButton.interactable = false;
                        jumpButton.interactable = false;
                        break;
                    }
                case Player.playerState.burning:
                    {
                        Debug.Log("Player is burning!");
                        StartCoroutine(heatMeter.decreaseMeterFilled(meterFilled));

                        //Check if meter is depleted fully. If it is, then set player back to idle.
                        if (heatMeter.getMeterVal() <= 0)
                        {
                            thePlayer.setState(Player.playerState.idle);
                            meterFilled = false;
                        }
                        break;
                    }
                case Player.playerState.frozen:
                    {

                        Debug.Log("Player is frozen!");
                        StartCoroutine(iceMeter.decreaseMeterFilled(meterFilled));

                        //Check if meter is depleted fully. If it is, then set player back to idle.
                        if (heatMeter.getMeterVal() <= 0)
                        {
                            thePlayer.setState(Player.playerState.idle);
                            meterFilled = false;
                        }
                        break;
                    }
                default:
                    {
                        thePlayer.setState(Player.playerState.idle);
                        if (player.transform.position.y <= 0.1)
                        {
                            jumpButton.interactable = true;
                            duckButton.interactable = true;
                        }
                        break;
                    }
            }
        }


        if (currentPlayerHealth > 0)
        {
           // healthText.text = "Health: " + currentPlayerHealth;
        }
    }


    void checkMeter()
    {

        //Maybe check for a boolean to say if player is burning/frozen and don't change it to true until the bar is depleted...?
        //if(thePlayer.GetState() != Player.playerState.burning || thePlayer.GetState() != Player.playerState.frozen)
        if (!meterFilled)
        {
            if (heatMeter.getMeterVal() >= 1f)
            {
                thePlayer.setState(Player.playerState.burning);
                meterFilled = true;
                Debug.Log("Meter value is 100!");
                //Call a function that starts decreasing the meter gradually.
                //Make button to spam for getting out of heat mode available.
            }
            if (iceMeter.getMeterVal() >= 1f)
            {
                thePlayer.setState(Player.playerState.frozen);
                meterFilled = true;
                Debug.Log("Meter value is 100!");
                //Call a function that starts decreasing the meter gradually.
                //Make button to spam for getting out of frozen mode available.
            }
        }

    }

    //Stuff happening in UPDATE//


    //Player gameplay related functions//

    //We need a damage value, type of element, and then we need to gather all currently spawned 'obstacles' and get rid of them.
    //Also need to put a slight delay, have player begin blinking as well.
    //If the player's heat or frozen bars are too filled, execute heat/frozen mode.
    public void Damage()
    {
        //Create a list of the obstacles that are currently spawned:
        //Since palyer took damage, destroy all.

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i]);
        }
        currentPlayerHealth -= 1;
        if (currentPlayerHealth <= 0)
        {
            thePlayer.setState(Player.playerState.dead);
            //  player.SetActive(false);
           // healthText.text = "Health: 0";
            //Gameover but allow ad to be played for revive.
        }
    }

    //If jumping/holding up, meter rises at a fixed rate while ice meter drops.
    public void temperatureMetersManager(Obstacle_Behaviour.ElementType element)
    {
        if (!meterFilled)
        {
            //Maybe give every obstacle it's own amount to fill?
            //NEED TO MAKE A FORMULA FOR FILLING!!!!
            if (element == Obstacle_Behaviour.ElementType.fire)
            {

                heatMeter.fillMeter(20.0f);
            }
            else if (element == Obstacle_Behaviour.ElementType.ice)
            {
                iceMeter.fillMeter(20.0f);
            }
        }

    }


    //Player gameplay related functions//


    //Obstacle related functions//

    //Spawn a random item from the list. We'll probably have sub-functions that check what to spawn based on difficulty/how long player has survived.
    //Need to ensure that even if items are spawning fast, that they are still far apart enough that the player can jump and duck to avoid them with good timing.
    void spawnObstacles()
    {
        int randNum;

        int minNum = 0;
        int maxNum = (obstacles.Count);

        //In this case the range can be 0 , or the exact count because randomize needs to be 1 above whatever you want. EX: List has 2 items, count = 2, but only 2 index...So 2 won't ever be called.
        randNum = Random.Range(minNum, maxNum);

        Instantiate(obstacles[randNum]);

        //Between 1-3.
        // rndSeed = Random.Range(1, 4);

    }

    //Obstacle related functions//
}

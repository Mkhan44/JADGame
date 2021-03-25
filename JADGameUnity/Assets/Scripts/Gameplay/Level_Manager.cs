//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;
public class Level_Manager : MonoBehaviour
{
    [Header("Player related")]

    public GameObject player;
    public Button jumpButton;
    public Button duckButton;
    public Button coolDownButton;
    public Button heatUpButton;
    List<GameObject> powerUps;
    public Player thePlayer;
    Animator playerAnimator;
    Rigidbody2D playerRigid2D;
    Vector2 playerInitialPos;
    bool onGround;
    int currentPlayerHealth;
    public TextMeshProUGUI healthText;

    [Tooltip("The gravity for the player. Obtained via Player script.")]
    [SerializeField]
    float gravityScale;
    [Tooltip("The velocity that will be applied when the player jumps. Obtained via Player script.")]
    [SerializeField]
    float jumpHeight;

    [Header("Burning values")]
    [Tooltip("Jump height to be used when player is burning.")]
    public float burningJumpHeight;
    [Tooltip("Gravity to be used when player is burning.")]
    public float burningGravity;

    //Time
    [Header("Time related variables")]
    float elapsedTime;

    [Header("Meters")]

    //Meters
    public Temperature_Manager heatMeter;
    public Temperature_Manager iceMeter;
    public bool meterFilled;

    [Header("GameOver/Retry stuff")]
    //Game over/Retry
    //Debug for now.
    public Button retryButton;

    //CONSTANTS FOR ANIMATION!
    const string IsCrouching = "IsCrouching";
    const string Damaged = "Damaged";
    const string IsJumping = "IsJumping";
    const string IsGrounded = "IsGrounded";
    const string IsFalling = "IsFalling";
    const string IsHanging = "IsHanging";
    const string IsBurning = "IsBurning";
    const string IsFrozen = "IsFrozen";
    //CONSTANTS FOR ANIMATION!

    //Time period related.
    public enum timePeriod
    {
        None,
        Prehistoric,
        FeudalJapan,
        WildWest,
        Medieval,
        Future,
    }

    [Header("Time periods")]
    [Tooltip("Current time period. This will change throughout gameplay.")]
    [SerializeField]
    public timePeriod TimePeriod;

    int portalSelect;

    [Header("UI MISC related.")]
    [Tooltip("Text that displays the number of coins the user has collected during this play session.")]
    public TextMeshProUGUI coinText;
    int coinsCollected;

    [Header("Item related.")]
    public List<Item> itemsThisRun;

    Collect_Manager.typeOfItem currentItem;

    float useDuration;
    public TextMeshProUGUI useDurationText;

    [SerializeField]
    Shop_Item item1;
    [SerializeField]
    Shop_Item item2;
    [SerializeField]
    Shop_Item item3;

    public GameObject item1Holder;
    public GameObject item2Holder;
    public GameObject item3Holder;

    [Header("Treasure chest related")]
    [Tooltip("This variable is for checking whether or not player has selected a chest. 1 = top, 2 = bottom")]
    public int chestSelect;

    

    public static Level_Manager Instance;
    private void Awake()
    {
        Instance = this;
        thePlayer = player.GetComponent<Player>();
        playerAnimator = player.GetComponent<Animator>();

        playerRigid2D = player.GetComponent<Rigidbody2D>();

        Input.multiTouchEnabled = false;
        currentItem = Collect_Manager.typeOfItem.none;
       

        chestSelect = 0;
        portalSelect = 0;

    }

    private void Start()
    {
        currentPlayerHealth = thePlayer.getCurrentHealth();

        if (currentPlayerHealth == 0)
        {
            //Default = 3.
            currentPlayerHealth = 3;
            healthText.text = "Health: " + currentPlayerHealth;
        }
        playerInitialPos = thePlayer.getInitialPos();

        gravityScale = thePlayer.getGravity();
        playerRigid2D.gravityScale = gravityScale;
        jumpHeight = thePlayer.getJumpHeight();

        heatUpButton.gameObject.SetActive(false);
        coolDownButton.gameObject.SetActive(false);

        retryButton.gameObject.SetActive(false);

        coinsCollected = 0;
        coinText.text = "Coins: " + coinsCollected.ToString();

        useDurationText.text = "No item in use.";
        setMeterRates();
        setupItems();

    }

    /*
     *  SETUP FUNCTIONS
     * 
     */

    void setupItems()
    {
        //If player has items, populate them.
        if (Collect_Manager.instance.item1 >= 0)
        {
            foreach (Collect_Manager.typeOfItem items in System.Enum.GetValues(typeof(Collect_Manager.typeOfItem)))
            {
                if (Collect_Manager.instance.item1 == (int)items)
                {
                    item1 = Collect_Manager.instance.itemsToPick[(int)items];
                    Debug.LogWarning(item1);
                    Item theItem = item1Holder.transform.GetChild(0).GetComponent<Item>();
                    theItem.thisItemType = item1.theItem;
                    Image tempImg = item1Holder.transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = item1.itemImage;
                    if (item1.hasDuration)
                    {
                        theItem.itemDuration = item1.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }
                    break;
                }
            }
        }
        else
        {
            Item theItem = item1Holder.transform.GetChild(0).GetComponent<Item>();
            theItem.thisItemType = Collect_Manager.typeOfItem.none;
            Image tempImg = item1Holder.transform.GetChild(0).GetComponent<Image>();
            tempImg.sprite = null;
        }
        if (Collect_Manager.instance.item2 >= 0)
        {
            foreach (Collect_Manager.typeOfItem items in System.Enum.GetValues(typeof(Collect_Manager.typeOfItem)))
            {
                if (Collect_Manager.instance.item2 == (int)items)
                {
                    item2 = Collect_Manager.instance.itemsToPick[(int)items];
                    Item theItem = item2Holder.transform.GetChild(0).GetComponent<Item>();
                    theItem.thisItemType = item2.theItem;
                    Image tempImg = item2Holder.transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = item2.itemImage;
                    if (item2.hasDuration)
                    {
                        theItem.itemDuration = item2.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }
                    break;
                }
            }
        }
        else
        {
            Item theItem = item2Holder.transform.GetChild(0).GetComponent<Item>();
            theItem.thisItemType = Collect_Manager.typeOfItem.none;
            Image tempImg = item2Holder.transform.GetChild(0).GetComponent<Image>();
            tempImg.sprite = null;
        }
        if (Collect_Manager.instance.item3 >= 0)
        {
            foreach (Collect_Manager.typeOfItem items in System.Enum.GetValues(typeof(Collect_Manager.typeOfItem)))
            {
                if (Collect_Manager.instance.item3 == (int)items)
                {
                    item3 = Collect_Manager.instance.itemsToPick[(int)items];
                    Item theItem = item3Holder.transform.GetChild(0).GetComponent<Item>();
                    theItem.thisItemType = item3.theItem;
                    Image tempImg = item3Holder.transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = item3.itemImage;
                    if (item3.hasDuration)
                    {
                        theItem.itemDuration = item3.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }
                    break;
                }
            }
        }
        else
        {
            Item theItem = item3Holder.transform.GetChild(0).GetComponent<Item>();
            theItem.thisItemType = Collect_Manager.typeOfItem.none;
            Image tempImg = item3Holder.transform.GetChild(0).GetComponent<Image>();
            tempImg.sprite = null;
        }
    }

    /*
     * SETUP FUNCTIONS
     * 
     */

    private void Update()
    {
        //Does stuff according to player's current state.
        checkState();
        coinText.text = "Coins: " + coinsCollected.ToString();

    }

    //Jump.
    public void Jump()
    {
        //Jump height is based off of the value we set for player jump height.
        playerRigid2D.velocity = Vector2.up * jumpHeight;

        //player.transform.position = currentPos;

        //  StartCoroutine(jumpUp(currentPos));
        jumpButton.interactable = false;
        duckButton.interactable = false;
        thePlayer.setState(Player.playerState.jumping);
        playerAnimator.SetBool(IsGrounded, false);
        playerAnimator.SetBool(IsJumping, true);
       


    }

    //Player is hanging from the top.
    public void Hang()
    {
        playerRigid2D.gravityScale = 0f;
        jumpButton.interactable = false;
        duckButton.interactable = false;
        playerAnimator.SetBool(IsGrounded, false);
        playerAnimator.SetBool(IsJumping, false);
        playerAnimator.SetBool(IsHanging, true);
    }

   
    public void duck()
    {
        // Debug.Log("Calling duck");
        thePlayer.setState(Player.playerState.ducking);

        playerAnimator.SetBool(IsCrouching, true);
        duckButton.interactable = false;
        jumpButton.interactable = false;
        // StartCoroutine(getUp());
    }

    public void getUpReg()
    {
        thePlayer.setState(Player.playerState.idle);
        playerAnimator.SetBool(IsCrouching, false);
        duckButton.interactable = true;
    }

    public void frozenDuck()
    {
        //Add some type of particle effect to simulate frozen player.
        playerAnimator.SetBool(IsCrouching, true);
        playerAnimator.SetBool(IsFrozen, true);
        duckButton.interactable = false;
        jumpButton.interactable = false;
       // Debug.Log("We called frozenDuck()!");
    }

    //Jump function that will be called when player is burning.
    public void burningJump()
    {
        //We may want to randomize the jump height/gravity to make it seem crazier.
        playerRigid2D.velocity = Vector2.up * burningJumpHeight;
        //playerRigid2D.gravityScale = gravityScale;
    }

    //Wait period between jumps.
    public IEnumerator burningJumpWait()
    {
        playerAnimator.SetBool(IsBurning, true);
        jumpButton.interactable = false;
        duckButton.interactable = false;
        while (heatMeter.getMeterVal() > 0)
        {
            yield return new WaitForSeconds(0.1f);
            if(onGround)
            {
                burningJump();
            }
            yield return null;
        }
        
    }



    //Stuff happening in UPDATE//
    void checkState()
    {
        if (player.activeInHierarchy)
        {
            checkMeter();
           // Debug.Log(thePlayer.GetState());
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

                            if (currentItem != Collect_Manager.typeOfItem.HandWarmer && this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.bonus && this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                            {
                                StartCoroutine(iceMeter.fillConstant());
                            }
                            if(this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.bonus && this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                            {
                                StartCoroutine(heatMeter.decreaseConstant());
                            }
                            
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
                           
                            if (onGround)
                            {
                                thePlayer.setState(Player.playerState.idle);
                                jumpButton.interactable = true;
                            }
                        }
                        break;
                    }
                case Player.playerState.hanging:
                    {
                        if(currentItem != Collect_Manager.typeOfItem.FireVest && this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.bonus && this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                        {
                            StartCoroutine(heatMeter.fillConstant());
                        }
                        if (this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.bonus && this.GetComponent<Wave_Spawner>().getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                        {
                            StartCoroutine(iceMeter.decreaseConstant());
                        }
                        if (Input.GetMouseButton(0) || Input.GetKey("up"))
                        {
                            Hang();
                        }
                        else
                        {
                            //Prolly have some variable to dictate this.
                            playerRigid2D.gravityScale = gravityScale;
                            playerAnimator.SetBool(IsHanging, false);
                            playerAnimator.SetBool(IsFalling, true);
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
                       // Debug.Log("Player is burning!");
                        //Only call this once.
                        if(heatMeter.getMeterVal() == 100f)
                        {
                            playerRigid2D.gravityScale = burningGravity;
                            StartCoroutine(heatMeter.decreaseMeterFilled(meterFilled));
                            coolDownButton.gameObject.SetActive(true);
                            StartCoroutine(burningJumpWait());
                        }
                        //Call the burningJump function.

                        //Check if meter is depleted fully. If it is, then set player back to idle.
                        if (heatMeter.getMeterVal() <= 0)
                        {
                            playerRigid2D.gravityScale = gravityScale;
                            thePlayer.setState(Player.playerState.idle);
                            meterFilled = false;
                        }
                        break;
                    }
                case Player.playerState.frozen:
                    {

                       // Debug.Log("Player is frozen!");
                        //Only call this once.
                        if (iceMeter.getMeterVal() == 100f)
                        {
                            frozenDuck();
                            heatUpButton.gameObject.SetActive(true);
                            StartCoroutine(iceMeter.decreaseMeterFilled(meterFilled));
                        }

                        //Check if meter is depleted fully. If it is, then set player back to idle.
                        if (iceMeter.getMeterVal() <= 0)
                        {
                            thePlayer.setState(Player.playerState.idle);
                            playerAnimator.SetBool(IsCrouching, false);
                            meterFilled = false;
                        }
                        break;
                    }
                    //Idle state.
                default:
                    {
                        thePlayer.setState(Player.playerState.idle);
                        heatUpButton.gameObject.SetActive(false);
                        coolDownButton.gameObject.SetActive(false);
                        if (onGround)
                        {
                            playerAnimator.SetBool(IsGrounded, true);
                            playerAnimator.SetBool(IsFalling, false);
                            playerAnimator.SetBool(IsBurning, false);
                            playerAnimator.SetBool(IsFrozen, false);
                            jumpButton.interactable = true;
                            duckButton.interactable = true;
                        }
                        break;
                    }
            }
        }


        if (currentPlayerHealth > 0)
        {
           healthText.text = "Health: " + currentPlayerHealth;
        }

        //Debug, may need to take this out later.
        if (currentPlayerHealth <= 0 && thePlayer.GetState() != Player.playerState.dead)
        {
            GameOver();
        }

        //Debug
        if(useDuration > 0)
        {
            thePlayer.isPoweredUp = true;
        }
        else
        {
            thePlayer.isPoweredUp = false;
            currentItem = Collect_Manager.typeOfItem.none;
        }

    }


    void checkMeter()
    {

        //Maybe check for a boolean to say if player is burning/frozen and don't change it to true until the bar is depleted...?
        //if(thePlayer.GetState() != Player.playerState.burning || thePlayer.GetState() != Player.playerState.frozen)
        if (!meterFilled)
        {
            if (heatMeter.getMeterVal() >= 100f)
            {
                thePlayer.setState(Player.playerState.burning);
                meterFilled = true;
                Debug.Log("Meter value is 100!");
                //Call a function that starts decreasing the meter gradually.
                //Make button to spam for getting out of heat mode available.
            }
            if (iceMeter.getMeterVal() >= 100f)
            {
                thePlayer.setState(Player.playerState.frozen);
                meterFilled = true;
                Debug.Log("Meter value is 100!");
                //Call a function that starts decreasing the meter gradually.
                //Make button to spam for getting out of frozen mode available.
            }
        }

    }

    //Called from player script to check if player is grounded.
    public void checkGrounded(bool yesOrNah)
    {
        onGround = yesOrNah;
    }

    //Stuff happening in UPDATE//


    /*Player gameplay related functions
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    */

    //We need a damage value, type of element, and then we need to gather all currently spawned 'obstacles' and get rid of them.
    //Also need to put a slight delay, have player begin blinking as well.
    //If the player's heat or frozen bars are too filled, execute heat/frozen mode.
    public void Damage()
    {
        //Create a list of the obstacles that are currently spawned:
        //Since player took damage, destroy all.

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        

        for (int i = 0; i < obstacles.Length; i++)
        {
            //TURN ON WHEN WE ARE READY TO POOL.
            Object_Pooler.Instance.AddToPool(obstacles[i]);
            //Destroy(obstacles[i]);
        }

       
        currentPlayerHealth -= 1;


        if (currentPlayerHealth <= 0)
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            for (int i = 0; i < coins.Length; i++)
            {
                //TURN ON WHEN WE ARE READY TO POOL.
                Object_Pooler.Instance.AddToPool(coins[i]);
                // Destroy(coins[i]);

            }
            GameOver();
            //Gameover but allow ad to be played for revive.
        }
        else
        {
            StartCoroutine(damageAni());
        }
    }

    //How long player should be in hitstun after taking damage. If we want to add any type of invince, we needa add it here.
    IEnumerator damageAni()
    {
        //MAYBE ADD THE BUTTON INTERACTABLES HERE INSTEAD??? THAT WAY PLAYER CAN'T INTERRUPT ANIMATIONS.
     //   jumpButton.interactable = false;
    //    duckButton.interactable = false;
        playerAnimator.SetBool(Damaged, true);
        yield return new WaitForSeconds(0.3f);
        playerAnimator.SetBool(Damaged, false);
     //   jumpButton.interactable = true;
     //   duckButton.interactable = true;
    }

    public void collectCoin(int amount)
    {
        //Have player collect a coin and add it to a counter that we need to save.
        coinsCollected += amount;
        if(amount > 1)
        {
            Debug.Log("You collected: " + amount + " coins!");
        }
       // Debug.Log("Collected a coin!");

    }

    //We'll need this to send values over to the different meters based on what the Player's current fill rate is.
    public void setMeterRates()
    {
        iceMeter.setIce(thePlayer.getIceMeterFill());
        heatMeter.setHeat(thePlayer.getHeatMeterFill());
    }

    //Might wanna add a value for each obstacle based on their 'meter increase value'. That way harder obstacles increase it by more later on etc.
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

   //Just setting the Item to not be null here so we know the player is using an Item.
    public void setCurrentItem(Collect_Manager.typeOfItem tempPowerUp , float duration)
    {
        //If the duration == 0 we know that this is a one time use.
        if(duration == 0 )
        {
            //One time use.
            useDuration = 0;
            
        }
        else
        {
            useDuration = duration;
            StartCoroutine(durationCount());
        }
        Debug.Log("The current item being used is: " + tempPowerUp.ToString());
        currentItem = tempPowerUp;
        thePlayer.isPoweredUp = true;
    }

    //For other scripts accessing what power up we currently have.
    public Collect_Manager.typeOfItem getCurrentItem()
    {
        return currentItem;
    }

    public IEnumerator durationCount()
    {
        while(useDuration > 0)
        {
            useDuration -= Time.deltaTime;
            useDurationText.text = Mathf.Round(useDuration).ToString() + " Seconds left";
            yield return null;
        }
        useDuration = 0;
        useDurationText.text = "No item in use.";
        Debug.Log("Finished the duration count!");
    }

    //Use this to force player into Idle for anything that we need to show the player. Gameplay buttons should be disabled elsewhere.
    public void ResetAnimator()
    {
        playerAnimator.SetBool(IsCrouching, false);
        playerRigid2D.gravityScale = gravityScale;
        thePlayer.setState(Player.playerState.idle);
    }

    //Player is dead.
    //Probably will load a panel in with some stats and ask if they want to retry (Watch ad) or go back to menu/share.
    //Seperate function for 'retry' will be implemented.

    void GameOver()
    {
        meterFilled = false;
        //In this if/elseif , make sure that when we add particle effects that you disable those. Also, get rid of the unfreeze/unheat buttons.
        if (thePlayer.GetState() == Player.playerState.burning)
        {
            heatMeter.setMeterValExternally(0.0f);
        }
        else if (thePlayer.GetState() == Player.playerState.frozen)
        {
            iceMeter.setMeterValExternally(0.0f);
        }
        heatUpButton.gameObject.SetActive(false);
        coolDownButton.gameObject.SetActive(false);
        jumpButton.interactable = false;
        duckButton.interactable = false;
        thePlayer.setState(Player.playerState.dead);
        //  player.SetActive(false);
        healthText.text = "Health: 0";
        Collect_Manager.instance.totalCoins += coinsCollected;
        SaveCollectables();
        retryButton.gameObject.SetActive(true);
    }

    public void restartScene()
    {
        //Gonna need to change this later and have the scene number 
        //FOR DEBUG.
        SceneManager.LoadScene(0);
    }

    //Save
    void SaveCollectables()
    {
        Save_System.SaveCollectables(Collect_Manager.instance);
    }

    //Save


    /*Player gameplay related functions
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    */


    /*Obstacle  related functions
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    */

    void timeCount()
    {
        elapsedTime += Time.deltaTime;
    }


    /*Obstacle  related functions
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    */

    /*Treasure  related functions
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   */

    public int getChestSelect()
    {
        return chestSelect;
    }

    public void setChestSelect(int selectPass)
    {
        chestSelect = selectPass;
    }
    public IEnumerator pickAChest()
    {
        // duckButton.gameObject.SetActive(false);
        // jumpButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        while (chestSelect == 0)
        {
            if (thePlayer.GetState() == Player.playerState.hanging)
            {
                chestSelect = 2;
               // Debug.Log("You selected chest 2!");
                duckButton.gameObject.SetActive(false);
                jumpButton.gameObject.SetActive(false);
            }
            else if (thePlayer.GetState() == Player.playerState.ducking)
            {
                chestSelect = 1;
              //  Debug.Log("You selected chest 1!");
                duckButton.gameObject.SetActive(false);
                jumpButton.gameObject.SetActive(false);
            }
            yield return null;
        }
       
    }


    /*Treasure  related functions
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   */

    /*TimeSwap  related functions
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  */

    public timePeriod getTimePeriod()
    {
        return TimePeriod;
    }

    public void setTimePeriod(timePeriod eraPass) 
    {
        TimePeriod = eraPass;
    }

    public int getTimePortalSelection()
    {
        return portalSelect;
    }

    public void setTimePortalSelection(int selection)
    {
        portalSelect = selection;
    }

    public IEnumerator pickAPortal()
    {
        // duckButton.gameObject.SetActive(false);
        // jumpButton.gameObject.SetActive(false);

        while (portalSelect == 0)
        {
            if (thePlayer.GetState() == Player.playerState.hanging)
            {
                portalSelect = 2;
                // Debug.Log("You selected chest 2!");
                duckButton.gameObject.SetActive(false);
                jumpButton.gameObject.SetActive(false);
            }
            else if (thePlayer.GetState() == Player.playerState.ducking)
            {
                portalSelect = 1;
                //  Debug.Log("You selected chest 1!");
                duckButton.gameObject.SetActive(false);
                jumpButton.gameObject.SetActive(false);
            }
            yield return null;
        }

    }


    /*TimeSwap  related functions
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  */
}

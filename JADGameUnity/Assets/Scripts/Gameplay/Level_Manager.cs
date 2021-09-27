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
   
    public enum levelType
    {
        normal,
        tutorial
    }

    [Header("Level Type")]
    [SerializeField] levelType theLevelType;


    [Header("Player related")]

    public GameObject player;
    public Button jumpButton;
    public Button duckButton;
    Color jumpButtonAlpha;
    Color duckButtonAlpha;
    Button_Interact jumpButtonInteract;
    Button_Interact duckButtonInteract;
    public Button coolDownButton;
    public Button heatUpButton;
    List<GameObject> powerUps;
    public Player thePlayer;
    [SerializeField] Color damagedColor;
    Animator playerAnimator;
    Rigidbody2D playerRigid2D;
    Vector2 playerInitialPos;
    bool onGround;
    int currentPlayerHealth;
    public TextMeshProUGUI healthText;
    [SerializeField] Heart_System theHeartSystem;

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
    public Slider powerupMeter;
    public bool meterFilled;

    [Header("Indicators")]
    [SerializeField] Image indicatorArrowTop;
    [SerializeField] Image indicatorArrowMid;
    [SerializeField] Image indicatorArrowBot;

    [Header("GameOver/Retry stuff")]
    //Game over/Retry
    //Debug for now.
    public Button retryButton;
    public GameObject gameOverPanel;
    public GameObject gameOverAdP;
    public GameObject gameOverTallyP;
    public GameObject gameOverButtonsP;
    public GameObject skipTallyButtonP;
    [SerializeField] bool firstTimeDying;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI waveBonusText;
    public TextMeshProUGUI RPGameOverText;
    public TextMeshProUGUI coinsGameOverText;
    public TextMeshProUGUI boltsGameOverText;
    [SerializeField] GameObject playerGameoverSpin;
    [SerializeField] Animator playerGameoverSpinAnimator;
    int totalScore = 0;
    int waveBonus = 0;
    int finalCoins = 0;
    int finalBolts = 0;

    bool isPaused;


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
        tutorial,
    }

    [Header("Time periods")]
    [Tooltip("Current time period. This will change throughout gameplay.")]
    [SerializeField]
    public timePeriod TimePeriod;

    int portalSelect;

    [Header("UI MISC related.")]
    [Tooltip("Text that displays the number of coins the user has collected during this play session.")]
    public TextMeshProUGUI coinText;
    [Tooltip("Text that displays the number of bolts the user has collected during this play session.")]
    public TextMeshProUGUI boltText;
    int coinsCollected;
    int boltsCollected;
    [SerializeField] Sprite mutedSprite;
    [SerializeField] Sprite unmutedSprite;
    [SerializeField] Image muteButtonSprite;
    [SerializeField] GameObject pausePanel;

    //May make this into something that gets instantiated...dunno yet.
    [Header("Notice UI related")]
    [SerializeField] TextMeshProUGUI noticeText;
    Coroutine noticeCoroutine;
    [SerializeField] Queue<string> noticeQueue = new Queue<string>();

    [Header("Item related.")]
    public List<Item> itemsThisRun;

    Collect_Manager.typeOfItem currentItem;

    float useDuration;
    public TextMeshProUGUI useDurationText;

    [SerializeField]
    public Shop_Item item1;
    [SerializeField]
    public Shop_Item item2;
    [SerializeField]
    public Shop_Item item3;

    public GameObject item1Holder;
    public GameObject item2Holder;
    public GameObject item3Holder;

    public GameObject item1CDPanel;
    public GameObject item2CDPanel;
    public GameObject item3CDPanel;

    [Header("Treasure chest related")]
    [Tooltip("This variable is for checking whether or not player has selected a chest. 1 = top, 2 = bottom")]
    public int chestSelect;


    [Header("Coroutines")]
    Coroutine increaseHeatMeterRoutine;
    Coroutine decreaseHeatMeterRoutine;
    Coroutine increaseIceMeterRoutine;
    Coroutine decreaseIceMeterRoutine;
    Coroutine scoreCountRoutine;
    Coroutine coinCountRoutine;
    Coroutine boltCountRoutine;
    Coroutine finalTallyRoutine;
    Coroutine decreaseHeatIdleRoutine;
    Coroutine decreaseIceIdleRoutine;

    [Header("Score related")]
    [SerializeField] int currentScore;
    [SerializeField] int wavesSurvived;
    public TextMeshProUGUI scoreText;
    [SerializeField] int pointMultiplier;
    public TextMeshProUGUI multiplierText;
    [SerializeField] int enemiesDodgedSinceLastMulti;

    [Header("Player Audio Clips")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip duckSound;
    [SerializeField] AudioClip magnetizedSound;
    [SerializeField] AudioClip landingSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip burnEnterSound;
    [SerializeField] AudioClip frozenEnterSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip burningDissipateSound;
    [SerializeField] AudioClip frozenBrokenSound;

    [Header("Gameover SFX")]
    [SerializeField] AudioClip RPTallySound;
    [SerializeField] AudioClip coinTallySound;

    [Header("Misc SFX")]
    [SerializeField] AudioClip errorSound;



    public static Level_Manager Instance;
    private void Awake()
    {
        isPaused = false;

        Instance = this;
        thePlayer = player.GetComponent<Player>();
        playerAnimator = player.GetComponent<Animator>();

        playerRigid2D = player.GetComponent<Rigidbody2D>();

        Input.multiTouchEnabled = false;
        currentItem = Collect_Manager.typeOfItem.none;

        jumpButtonInteract = jumpButton.GetComponent<Button_Interact>();
        duckButtonInteract = duckButton.GetComponent<Button_Interact>();


        chestSelect = 0;
        portalSelect = 0;

        currentScore = 0;
        wavesSurvived = 0;

        if(Tutorial_Instance_Debug.instance != null)
        {
            if (Tutorial_Instance_Debug.instance.getTutorialVal())
            {
                theLevelType = levelType.tutorial;
                TimePeriod = timePeriod.tutorial;
                GameObject pauseButton = GameObject.Find("Pause_Button");
                GameObject muteButton = GameObject.Find("MuteButton");
                pauseButton.SetActive(false);
                muteButton.SetActive(false);
                item1CDPanel.SetActive(false);
                item2CDPanel.SetActive(false);
                item3CDPanel.SetActive(false);
            }
            else
            {
                theLevelType = levelType.normal;
                //Need to randomize this in the future. !!!!
                selectTimePeriod();


            }
        }
        else
        {
            theLevelType = levelType.normal;
            //Need to randomize this in the future. !!!!
            selectTimePeriod();
            Debug.LogWarning("The tutorial_instance_debug is null!");
        }
      


        if(theLevelType != levelType.tutorial)
        {
            int currentSkinInt = (Collect_Manager.instance.getCurrentSkin());

            //Pick player skin.
            for (int i = 0; i < Collect_Manager.instance.skinsToPick.Count; i++)
            {
                if (currentSkinInt == i)
                {

                    playerAnimator.runtimeAnimatorController = Collect_Manager.instance.skinsToPick[i].animationOverrideController;

                    break;
                }
            }
        }

    }

    void selectTimePeriod()
    {
        if (TimePeriod == timePeriod.None)
        {

            //Debug
            if(Collect_Manager.instance.getTimePeriod() != timePeriod.None)
            {
                TimePeriod = Collect_Manager.instance.getTimePeriod();
            }
            else
            {
                int randomTimePeriodToStart = 0;
                int numTimePeriods = System.Enum.GetValues(typeof(timePeriod)).Length;
                //Account for 'none' and 'tutorial'.
                //Debug.LogWarning(numTimePeriods.ToString());
                numTimePeriods -= 2;
                randomTimePeriodToStart = Random.Range(0, numTimePeriods);
                if(theLevelType == levelType.tutorial)
                {
                    randomTimePeriodToStart = 5;
                }
                // Debug.Log("The amount of time period's we're randomizing between are: " + randomTimePeriodToStart.ToString());
                //0 = prehistoric, 1 = feudalJapan, 2 = WildWest, 3 = Med, 4 = Future
                switch (randomTimePeriodToStart)
                {
                    case 0:
                        {
                            TimePeriod = timePeriod.Prehistoric;
                            break;
                        }
                    case 1:
                        {
                            TimePeriod = timePeriod.FeudalJapan;
                            break;
                        }
                    case 2:
                        {
                            TimePeriod = timePeriod.WildWest;
                            break;
                        }
                    case 3:
                        {
                            TimePeriod = timePeriod.Medieval;
                            break;
                        }
                    case 4:
                        {
                            TimePeriod = timePeriod.Future;
                            break;
                        }
                    case 5:
                        {
                            TimePeriod = timePeriod.tutorial;
                            break;
                        }
                    default:
                        {
                            TimePeriod = timePeriod.Prehistoric;
                            Debug.LogWarning("We could not find a corresponding time period!");
                            break;
                        }
                }
           
            }


        }
        else
        {
            Debug.Log("We're selecting the time period externally prolly through the editor.");
        }
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
        jumpButtonAlpha = jumpButton.GetComponent<Image>().color;
        duckButtonAlpha = duckButton.GetComponent<Image>().color;

        coinsCollected = 0;
        coinText.text = " : " + coinsCollected.ToString();
        boltText.text = " : " + boltsCollected.ToString();
        noticeText.text = "";

        useDurationText.text = "No item in use.";
        scoreText.text = "RP: 0";

        pointMultiplier = 1;
        multiplierText.text = pointMultiplier.ToString() + "x";
        enemiesDodgedSinceLastMulti = 0;

        gameOverPanel.SetActive(false);
        firstTimeDying = false;

        toggleMute(false);

        setMeterRates();
        powerupMeter.value = 0f;

        if (theLevelType != levelType.tutorial)
        {
            setupItems();
            checkScore();
        }

        theHeartSystem.initializeHealth(currentPlayerHealth);
    }

    /*
     *  SETUP FUNCTIONS
     * 
     */

    //DEBUG TAKE OUT LATER.

    public void pauseGame(bool changeIcon)
    {
        if(changeIcon)
        {
            //Change the icon.
        }
        

        if(isPaused == true)
        {
            Time.timeScale = 1f;
            if(theLevelType != levelType.tutorial)
            {
                pausePanel.SetActive(false);
            }
            isPaused = false;
        }
        else
        {
            if (theLevelType != levelType.tutorial)
            {
                pausePanel.SetActive(true);
            }
            isPaused = true;
            Time.timeScale = 0f;
        }

        Audio_Manager.Instance.togglePauseSFX();
    }

    public bool pauseStatus()
    {
        return isPaused;
    }

    //For game audio muting.
    public void toggleMute(bool togglingMute)
    {
        if (togglingMute)
        {
            Collect_Manager.instance.isMuted = !Collect_Manager.instance.isMuted;
        }
        Debug.Log("Value of mute is: " + Collect_Manager.instance.isMuted);


        if (Collect_Manager.instance.isMuted)
        {
            AudioListener.volume = 0;
            muteButtonSprite.sprite = mutedSprite;
        }
        else
        {
            AudioListener.volume = 1;
            muteButtonSprite.sprite = unmutedSprite;
        }
    }


    void setupItems()
    {
        item1CDPanel.SetActive(false);
        item2CDPanel.SetActive(false);
        item3CDPanel.SetActive(false);

        //If player has items, populate them.
        if (Collect_Manager.instance.item1 >= 0)
        {
            foreach (Collect_Manager.typeOfItem items in System.Enum.GetValues(typeof(Collect_Manager.typeOfItem)))
            {
                if (Collect_Manager.instance.item1 == (int)items)
                {
                    item1 = Collect_Manager.instance.itemsToPick[(int)items];
                   // Debug.LogWarning(item1);
                    Item theItem = item1Holder.transform.GetChild(0).GetComponent<Item>();
                    theItem.thisItemType = item1.theItem;
                    Image tempImg = item1Holder.transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = item1.itemImage;
                    theItem.audioToPlay = item1.useSound;
                    if (item1.hasDuration)
                    {
                        theItem.itemDuration = item1.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }
                    if(item1.waveCooldown > 0)
                    {
                        theItem.waveCooldownTime = item1.waveCooldown;
                    }
                    else
                    {
                        theItem.waveCooldownTime = 0;
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
           // tempImg.sprite = null;
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
                    theItem.audioToPlay = item2.useSound;
                    if (item2.hasDuration)
                    {
                        theItem.itemDuration = item2.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }
                    if (item2.waveCooldown > 0)
                    {
                        theItem.waveCooldownTime = item2.waveCooldown;
                    }
                    else
                    {
                        theItem.waveCooldownTime = 0;
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
           // tempImg.sprite = null;
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
                    theItem.audioToPlay = item3.useSound;
                    if (item3.hasDuration)
                    {
                        theItem.itemDuration = item3.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }
                    if (item3.waveCooldown > 0)
                    {
                        theItem.waveCooldownTime = item3.waveCooldown;
                    }
                    else
                    {
                        theItem.waveCooldownTime = 0;
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
          //  tempImg.sprite = null;
        }

        Collect_Manager.instance.item1 = -1;
        Collect_Manager.instance.item2 = -1;
        Collect_Manager.instance.item3 = -1;
    }

    /*
     * SETUP FUNCTIONS
     * 
     */

    private void Update()
    {
        //Does stuff according to player's current state.
        checkState();
        if (Input.GetKeyDown("space"))
        {
            pauseGame(false);
        }
      //  coinText.text = " : " + coinsCollected.ToString();

    }

    //Jump.
    public void Jump()
    {
        duckButtonInteract.theSpriteState.disabledSprite = duckButtonInteract.disabledImg;
        jumpButtonInteract.theSpriteState.disabledSprite = jumpButtonInteract.pressedImg;

        duckButton.spriteState = duckButtonInteract.theSpriteState;
        jumpButton.spriteState = jumpButtonInteract.theSpriteState;

        if (theLevelType == levelType.tutorial)
        {
            if(Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.jumpButton)
            {
                Tutorial_Manager.Instance.conditionComplete();
            }
        }

        //Jump height is based off of the value we set for player jump height.
        playerRigid2D.velocity = Vector2.up * jumpHeight;

        //player.transform.position = currentPos;

        //  StartCoroutine(jumpUp(currentPos));
        jumpButton.interactable = false;
        duckButton.interactable = false;
        thePlayer.setState(Player.playerState.jumping);
        onGround = false;
        playerAnimator.SetBool(IsGrounded, false);
        playerAnimator.SetBool(IsJumping, true);

        Audio_Manager.Instance.playSFX(jumpSound);

    }

    //Player is hanging from the top.
    public void Hang()
    {
        if (theLevelType == levelType.tutorial)
        {
            if (Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.hang)
            {
                Tutorial_Manager.Instance.conditionComplete();
            }
            StartCoroutine(Tutorial_Manager.Instance.disableJDButtons());
        }
        playerRigid2D.gravityScale = 0f;
        playerRigid2D.velocity = Vector2.up * jumpHeight;
        jumpButton.interactable = false;
        duckButton.interactable = false;
        playerAnimator.SetBool(IsGrounded, false);
        playerAnimator.SetBool(IsJumping, false);
        playerAnimator.SetBool(IsHanging, true);
    }

   
    public void duck()
    {
        duckButtonInteract.theSpriteState.disabledSprite = duckButtonInteract.pressedImg;
        jumpButtonInteract.theSpriteState.disabledSprite = jumpButtonInteract.disabledImg;

        duckButton.spriteState = duckButtonInteract.theSpriteState;
        jumpButton.spriteState = jumpButtonInteract.theSpriteState;

        if (theLevelType == levelType.tutorial)
        {
            if (Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.duckButton)
            {
                Tutorial_Manager.Instance.conditionComplete();
            }
        }

        //  Debug.Log("Calling duck from levelManager");
        thePlayer.setState(Player.playerState.ducking);

        playerAnimator.SetBool(IsCrouching, true);
        duckButton.interactable = false;
        jumpButton.interactable = false;

        Audio_Manager.Instance.playSFX(duckSound);
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
        Audio_Manager.Instance.playSFX(jumpSound);
        //playerRigid2D.gravityScale = gravityScale;
    }

    //Wait period between jumps.
    public IEnumerator burningJumpWait()
    {
        playerAnimator.SetBool(IsBurning, true);
        jumpButton.interactable = false;
        duckButton.interactable = false;
        while (heatMeter.getMeterVal() > 0 && thePlayer.GetState() == Player.playerState.burning)
        {
           // yield return new WaitForSeconds(0.1f);
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
        if (!thePlayer.getPlayerDeathVal())
        {
            checkMeter();
           // Debug.Log(thePlayer.GetState());
            switch (thePlayer.GetState())
            {
                case Player.playerState.ducking:
                    {
                        //Do ducking stuff...

                        //If we're holding the button.
                        if (duckButtonInteract.getHeldValue() && duckButton.enabled != false)
                        {
                            if(duckButtonInteract.getTimeHeld() >= 0.3f)
                            {
                                if (currentItem != Collect_Manager.typeOfItem.HandWarmers && Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.bonus && Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                                {
                                    if (theLevelType == levelType.tutorial)
                                    {
                                        if (Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.crouch)
                                        {
                                            Tutorial_Manager.Instance.conditionComplete();
                                           
                                        }
                                        StartCoroutine(Tutorial_Manager.Instance.disableJDButtons());
                                    }
                                    else
                                    {
                                        StartCoroutine(iceMeter.fillConstant());
                                    }

                                   
                                }
                                if (Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.bonus && Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                                {
                                    StartCoroutine(heatMeter.decreaseConstant());
                                }
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
                        if (jumpButtonInteract.getHeldValue())
                        {
                            if(jumpButtonInteract.getTimeHeld() >= 0.3f)
                            {
                                thePlayer.setState(Player.playerState.hanging);
                                Audio_Manager.Instance.playSFX(magnetizedSound, false, 0.5f);
                            }
                          

                        }
                        //Not holding so don't hang.
                        else
                        {
                            if (onGround)
                            {
                                thePlayer.setState(Player.playerState.idle);
                                // jumpButton.interactable = true;
                            }
                            else
                            {
                                //Player is falling.
                                playerAnimator.SetBool(IsJumping, false);
                                playerAnimator.SetBool(IsFalling, true);

                            }
                        }
                        break;
                    }
                case Player.playerState.hanging:
                    {
                        if(currentItem != Collect_Manager.typeOfItem.FireVest && Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.bonus && Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                        {
                            if (theLevelType != levelType.tutorial)
                            {
                                StartCoroutine(heatMeter.fillConstant());
                            }

       
                        }
                        if (Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.bonus && Wave_Spawner.Instance.getWaveType() != Wave_Spawner.typeOfWave.timeSwap)
                        {
                            StartCoroutine(iceMeter.decreaseConstant());
                        }
                        if (jumpButtonInteract.getHeldValue() && jumpButton.enabled != false)
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
                        heatMeter.setMeterValExternally(0f);
                        iceMeter.setMeterValExternally(0f);
                        thePlayer.setPlayerDeath(true);
                        //player.SetActive(false);
                        duckButton.interactable = false;
                        jumpButton.interactable = false;
                        break;
                    }
                case Player.playerState.burning:
                    {
                     
                        // Debug.Log("Player is burning!");
                        //Only call this once.
                        if (heatMeter.getMeterVal() == 100f)
                        {
                            duckButtonInteract.theSpriteState.disabledSprite = duckButtonInteract.disabledImg;
                            jumpButtonInteract.theSpriteState.disabledSprite = jumpButtonInteract.disabledImg;
                            duckButton.spriteState = duckButtonInteract.theSpriteState;
                            jumpButton.spriteState = jumpButtonInteract.theSpriteState;

                            playerRigid2D.gravityScale = burningGravity;
                            StartCoroutine(burningJumpWait());
                           
                         
                            coolDownButton.gameObject.SetActive(true);
                            disableAlphaOnButtons();
                            playerAnimator.SetBool(IsCrouching, false);
                            playerAnimator.SetBool(IsHanging, false);

                  
                            Audio_Manager.Instance.playSFX(burnEnterSound, false, 0.05f);
                            heatMeter.setMeterValExternally(99.9f);
                            if (theLevelType == levelType.tutorial)
                            {
                                
                            }
                            else
                            {
                                setupNoticeTextAnimation("You're Burning! Mash the cooldown button!", true);
                                StartCoroutine(heatMeter.decreaseMeterFilled(meterFilled));
                            }
                        }
                        //Call the burningJump function.

                        //Check if meter is depleted fully. If it is, then set player back to idle.
                        if (heatMeter.getMeterVal() <= 0)
                        {
                            Audio_Manager.Instance.playSFX(burningDissipateSound);
                            if (theLevelType == levelType.tutorial)
                            {
                                if (Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.burning)
                                {
                                    Tutorial_Manager.Instance.conditionComplete();
                                }
                            }

                            playerRigid2D.gravityScale = gravityScale;
                            thePlayer.setState(Player.playerState.idle);
                            if(decreaseHeatMeterRoutine != null)
                            {
                                decreaseHeatMeterRoutine = null;
                            }
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
                            duckButtonInteract.theSpriteState.disabledSprite = duckButtonInteract.disabledImg;
                            jumpButtonInteract.theSpriteState.disabledSprite = jumpButtonInteract.disabledImg;
                            duckButton.spriteState = duckButtonInteract.theSpriteState;
                            jumpButton.spriteState = jumpButtonInteract.theSpriteState;

                            frozenDuck();
                          
                            Audio_Manager.Instance.playSFX(frozenEnterSound, false, 0.05f);
                            heatUpButton.gameObject.SetActive(true);
                            disableAlphaOnButtons();
                            playerAnimator.SetBool(IsHanging, false);
                            iceMeter.setMeterValExternally(99.9f);
                            if (theLevelType == levelType.tutorial)
                            {
                                
                            }
                            else
                            {
                                StartCoroutine(iceMeter.decreaseMeterFilled(meterFilled));
                                setupNoticeTextAnimation("You're Frozen! Mash the heat up button!", true);
                            }
                          
                           
                        }

                        //Check if meter is depleted fully. If it is, then set player back to idle.
                        if (iceMeter.getMeterVal() <= 0)
                        {
                            Audio_Manager.Instance.playSFX(frozenBrokenSound);
                            if (theLevelType == levelType.tutorial)
                            {
                                if (Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.frozen)
                                {
                                    Tutorial_Manager.Instance.conditionComplete();
                                }
                            }

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
                        if (decreaseHeatIdleRoutine == null && decreaseIceIdleRoutine == null)
                        {
                            decreaseHeatIdleRoutine = StartCoroutine(heatMeter.decreaseIdle());
                            decreaseIceIdleRoutine = StartCoroutine(iceMeter.decreaseIdle());
                        }
                        heatUpButton.gameObject.SetActive(false);
                        coolDownButton.gameObject.SetActive(false);
                        enableAlphaOnButtons();
                        if (onGround)
                        {
                            duckButtonInteract.theSpriteState.disabledSprite = duckButtonInteract.pressedImg;
                            jumpButtonInteract.theSpriteState.disabledSprite = jumpButtonInteract.pressedImg;
                            duckButton.spriteState = duckButtonInteract.theSpriteState;
                            jumpButton.spriteState = jumpButtonInteract.theSpriteState;

                            playerAnimator.SetBool(IsGrounded, true);
                            playerAnimator.SetBool(IsJumping, false);
                            playerAnimator.SetBool(IsFalling, false);
                            playerAnimator.SetBool(IsBurning, false);
                            playerAnimator.SetBool(IsFrozen, false);
                            playerAnimator.SetBool(IsCrouching, false);
                            playerRigid2D.gravityScale = gravityScale;
                            jumpButton.interactable = true;
                            duckButton.interactable = true;
                        }
                        break;
                    }
            }
            if(thePlayer.GetState() != Player.playerState.idle)
            {
                if (decreaseHeatIdleRoutine != null && decreaseIceIdleRoutine != null)
                {
                    StopCoroutine(decreaseHeatIdleRoutine);
                    StopCoroutine(decreaseIceIdleRoutine);
                    decreaseHeatIdleRoutine = null;
                    decreaseIceIdleRoutine = null;
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
            gameOver();
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

        if(onGround && thePlayer.GetState() != Player.playerState.burning && !thePlayer.getPlayerDeathVal())
        {
            Audio_Manager.Instance.playSFX(landingSound, false, 0.3f);
        }
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
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        Wave_Spawner.Instance.updateEnemiesLeft(obstacles.Length);
        for (int i = 0; i < obstacles.Length; i++)
        {
            //TURN ON WHEN WE ARE READY TO POOL.
            Object_Pooler.Instance.AddToPool(obstacles[i]);
            //Destroy(obstacles[i]);
        }

        for (int j = 0; j < bullets.Length; j++)
        {
            Destroy(bullets[j]);
            //Destroy(obstacles[j]);
        }

        Audio_Manager.Instance.stopSFX();
        currentPlayerHealth -= 1;

        theHeartSystem.updateHealth(currentPlayerHealth);

        if (currentPlayerHealth <= 0)
        {
            GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
            for (int i = 0; i < coins.Length; i++)
            {
                //TURN ON WHEN WE ARE READY TO POOL.
                Object_Pooler.Instance.AddToPool(coins[i]);
                // Destroy(coins[i]);

            }

            GameObject[] bolts = GameObject.FindGameObjectsWithTag("Bolt");
            for (int i = 0; i < bolts.Length; i++)
            {
                //TURN ON WHEN WE ARE READY TO POOL.
                Object_Pooler.Instance.AddToPool(bolts[i]);
                // Destroy(coins[i]);

            }
            Audio_Manager.Instance.playSFX(deathSound);
            gameOver();
            //Gameover but allow ad to be played for revive.
        }
        else
        {
            Audio_Manager.Instance.playSFX(damageSound);
            StartCoroutine(damageAni());
        }

        resetMultiplier();
    }

    //How long player should be in hitstun after taking damage. If we want to add any type of invince, we needa add it here.
    IEnumerator damageAni()
    {
        playerAnimator.SetBool(Damaged, true);
        player.GetComponent<SpriteRenderer>().color = damagedColor;
        playerRigid2D.constraints &= ~RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        player.transform.position = new Vector2((player.transform.position.x + 0.1f), player.transform.position.y);
        yield return new WaitForSeconds(0.05f);
        player.transform.position = new Vector2((player.transform.position.x - 0.1f), player.transform.position.y);
        yield return new WaitForSeconds(0.05f);
        player.transform.position = new Vector2((player.transform.position.x + 0.1f), player.transform.position.y);
        yield return new WaitForSeconds(0.05f);
        player.transform.position = new Vector2((player.transform.position.x - 0.1f), player.transform.position.y);
        yield return new WaitForSeconds(0.05f);

        playerRigid2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        playerAnimator.SetBool(Damaged, false);
    }

    public void collectCoin(int amount)
    {
        //1 coin = 5 points. Do math to figure out how much to increase the score.
        int scoreIncrease = (amount * 5);
        if (theLevelType == levelType.tutorial)
        {
            scoreIncrease = 0;
        }
        
        updateScore(scoreIncrease);

        int tempCollected = coinsCollected;
        coinsCollected += amount;
        if(amount > 1)
        {
            if (coinCountRoutine != null)
            {
                StopCoroutine(coinCountRoutine);
                coinCountRoutine = null;
               // Debug.Log("New coin total = " + currentScore);
            }
            coinCountRoutine = StartCoroutine(coinAni(tempCollected, coinsCollected));
           // Debug.Log("You collected: " + amount + " coins!");
        }
        else
        {
            coinText.text = " : " + coinsCollected.ToString();
        }
       // Debug.Log("Collected a coin!");

    }

    IEnumerator coinAni(int incomingAmt, int newAmt)
    {
        int speedToCount = 1;
        if(newAmt - incomingAmt >= 100)
        {
            speedToCount = 5;
        }
        else if (newAmt - incomingAmt >= 1000)
        {
            speedToCount = 10;
        }
        coinText.text = " : " + incomingAmt.ToString();
        while (incomingAmt < newAmt)
        {
            yield return new WaitForSeconds(0.01f * Time.deltaTime);
            incomingAmt += speedToCount;
            if(incomingAmt > newAmt)
            {
                incomingAmt = newAmt;
            }
            coinText.text = " : " + incomingAmt.ToString();
        }

        yield return null;
    }

    public void collectBolt(int amount)
    {
        int scoreIncrease = (amount * 100);
        updateScore(scoreIncrease);

        int tempCollected = boltsCollected;
        boltsCollected += amount;
        if(amount > 1)
        {
            if(boltCountRoutine != null)
            {
                StopCoroutine(boltCountRoutine);
                boltCountRoutine = null;
            }
            boltCountRoutine = StartCoroutine(boltAni(tempCollected, boltsCollected));
        }
        else
        {
            boltText.text = " : " + boltsCollected.ToString();
        }

        if(amount == 1)
        {
            setupNoticeTextAnimation("You just collected: x" + amount.ToString() + " Bolt!");
        }
        else
        {
            setupNoticeTextAnimation("You just collected: x" + amount.ToString() + " Bolts!");
        }
        
    }

    IEnumerator boltAni(int incomingAmt, int newAmt)
    {
        boltText.text = " : " + incomingAmt.ToString();
        while (incomingAmt < newAmt)
        {
            yield return new WaitForSeconds(0.01f * Time.deltaTime);
            incomingAmt += 1;
            boltText.text = " : " + incomingAmt.ToString();
        }

        yield return null;
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

    //ITEM Functions

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
            Vector4 powerUpColor = new Vector4(191, 170, 34, 1.0f);
            thePlayer.outlineMaterialInstance.SetColor("_OutlineColor", Color.yellow);
            powerupMeter.value = 100f;
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

    public void itemCDUpdate()
    {
        Item theItem1 = item1Holder.transform.GetChild(0).GetComponent<Item>();
        if (theItem1.cooldownStatus())
        {
            theItem1.updateCDTime();
        }

        Item theItem2 = item2Holder.transform.GetChild(0).GetComponent<Item>();
        if (theItem2.cooldownStatus())
        {
            theItem2.updateCDTime();
        }

        Item theItem3 = item3Holder.transform.GetChild(0).GetComponent<Item>();
        if (theItem3.cooldownStatus())
        {
            theItem3.updateCDTime();
        }
    }

    public IEnumerator durationCount()
    {
        float fullDuration = useDuration;
        float timePassed = 0f;
   
        while (useDuration > 0)
        {
            if(Wave_Spawner.Instance.getWaveType() == Wave_Spawner.typeOfWave.normal)
            {
                //powerupMeter.value -= 0.6f;
                useDuration -= Time.deltaTime;

                powerupMeter.value = Mathf.Lerp(100, 0, timePassed / fullDuration);
                timePassed += Time.deltaTime;
                useDurationText.text = Mathf.Round(useDuration).ToString() + " Seconds left";
            }
         
            yield return null;
        }
        Vector4 regularColor = new Vector4(191, 137, 137, 1.0f);
        thePlayer.outlineMaterialInstance.SetColor("_OutlineColor", Color.white);
        useDuration = 0;
        powerupMeter.value = 0f;
        useDurationText.text = "No item in use.";
        Debug.Log("Finished the duration count!");
    }

    //ITEM Functions


    //Use this to force player into Idle for anything that we need to show the player. Gameplay buttons should be disabled elsewhere.
    public void ResetAnimator()
    {
        playerAnimator.SetBool(Damaged, false);
        playerAnimator.SetBool(IsJumping, false);
        playerAnimator.SetBool(IsFalling, false);
        playerAnimator.SetBool(IsHanging, false);
        playerAnimator.SetBool(IsBurning, false);
        playerAnimator.SetBool(IsFrozen, false);
        playerAnimator.SetBool(IsCrouching, false);
        playerAnimator.SetBool(IsGrounded, true);
        meterFilled = false;
        thePlayer.setState(Player.playerState.idle);
        playerRigid2D.gravityScale = 20f;
        
    }

    //Player is dead.
    //Probably will load a panel in with some stats and ask if they want to retry (Watch ad) or go back to menu/share.
    //Seperate function for 'retry' will be implemented.

    void gameOver()
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
        Debug.Log("You survived: " + wavesSurvived + " waves!");
        //Debug.Log("Your score is: " + currentScore);
        checkFinalWaves();
        gameOverPanelSpawn();
       
    }

    void gameOverPanelSpawn()
    {
        gameOverPanel.SetActive(true);
        skipTallyButtonP.SetActive(false);

        if (!firstTimeDying)
        {
            gameOverAdP.SetActive(true);
            gameOverTallyP.SetActive(false);
            playerGameoverSpin.SetActive(false);
            firstTimeDying = true;
        }
        else
        {
            gameOverAdP.SetActive(false);
            gameOverTally();
        }
        Time.timeScale = 1f;
    }

    public void setGameoverSkin()
    {
        int currentSkinInt = (Collect_Manager.instance.getCurrentSkin());
        //Populate the skin preview on the top based on what the user has currently equipped.
        for (int i = 0; i < Collect_Manager.instance.skinsToPick.Count; i++)
        {
            if (currentSkinInt == i)
            {
                // stopAnimator();


                // currentSkinHolderImage.sprite = skinsToPick[i].skinSprite;
                if (Collect_Manager.instance.skinsToPick[i].animationOverrideController == null)
                {
                    Debug.Log("Hey we're using the default animatorController...is something wrong?");
                    //playerGameoverSpinAnimator.runtimeAnimatorController = defaultAnimator;
                }
                else
                {
                    playerGameoverSpinAnimator.runtimeAnimatorController = Collect_Manager.instance.skinsToPick[i].animationOverrideController;
                }


                break;
            }
        }

        
        playerGameoverSpinAnimator.SetBool(IsFalling, true);
        playerGameoverSpinAnimator.SetBool(IsJumping, true);
        playerGameoverSpinAnimator.SetBool(IsGrounded, false);
       // playerGameoverSpinAnimator.Play("Guy1_Fall");
        StartCoroutine(spinPlayerSprite());

    }

    IEnumerator spinPlayerSprite()
    {
        Vector3 testVect = playerGameoverSpin.transform.eulerAngles;

        StartCoroutine(turnOnColorPlayerSpin());

        while (gameOverPanel.activeInHierarchy)
        {
            if(testVect.z > 360 || testVect.z < -360)
            {
                testVect.z = 0;
            }

            testVect.z += 5;
            playerGameoverSpin.transform.eulerAngles = testVect;
            yield return null;
        }

        yield return null;
    }

    IEnumerator turnOnColorPlayerSpin()
    {
        float timePassed = 0f;
        float duration = 50;
        Image playerSpinIMG = playerGameoverSpin.transform.GetChild(0).GetComponent<Image>();
        Color noAlphaColor = playerSpinIMG.color;
        Color fullAlpha = new Color(255, 255, 255, 255);

        while (timePassed < duration)
        {
            playerSpinIMG.color = Color.Lerp(noAlphaColor, fullAlpha, timePassed / duration);
            timePassed += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }


        // yield return new WaitForSeconds(0.3f);
        // playerGameoverSpin.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 255);

        yield return null;
    }

    public void playRespawnVid()
    {
        if(AdsManager.instance != null)
        {
            AdsManager.instance.playRewardedVideoAd();

        }
        else
        {
            Debug.LogWarning("Hey, the adsManager instance is null!");
        }
    }

    //Gets called if player watches an ad to respawn the player and continue the game.
    public void respawnPlayer()
    {
        gameOverPanel.SetActive(false);
        currentPlayerHealth = 1;
        theHeartSystem.updateHealth(currentPlayerHealth);
        thePlayer.setPlayerDeath(false);
        //player.SetActive(true);
        Wave_Spawner.Instance.respawnPlayer();
        thePlayer.setState(Player.playerState.idle);
      //  playerRigid2D.gravityScale = 20;
    }

    public void returnToMenu()
    {
        //Gonna need to change this later and have the scene number 
        
        Time.timeScale = 1f;
        Save_System.SaveCollectables(Collect_Manager.instance);
        //FOR DEBUG.
        SceneManager.LoadSceneAsync(0);
    }

    public void retryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1);
    }

    /*Level related functions
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  //***********************************************************************
  */
    
    //We'll call this function externally if the player clicks tutorial somewhere.
    public void setThisLevelType(levelType theSetType)
    {
        theLevelType = theSetType;
    }

    public levelType getThisLevelType()
    {
        return theLevelType;
    }

    public void disableAlphaOnButtons()
    {
        jumpButton.GetComponent<Image>().color = new Color32(255,255,255,20);
        duckButton.GetComponent<Image>().color = new Color32(255, 255, 255, 20);
    }

    public void enableAlphaOnButtons()
    {
        jumpButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        duckButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void setupNoticeTextAnimation(string message, bool skip = false)
    {
        if (noticeText.color.a > 0 && !skip)
        {
            //Add to queue, return for now.
            noticeQueue.Enqueue(message);
           // Debug.Log("lol returning");
            return;
        }

        if (noticeCoroutine != null)
        {
            StopCoroutine(noticeCoroutine);
        }

        noticeCoroutine = StartCoroutine(animateNoticeText(message, skip));
    }
    public IEnumerator animateNoticeText(string message = "", bool skip = false)
    {
        if(skip)
        {
            noticeText.text = message;
            noticeQueue.Clear();
        }
        else
        {
            noticeQueue.Enqueue(message);

            noticeText.text = noticeQueue.Dequeue();
        }
     

        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 255);
        Color32 endColor = new Color32(255, 255, 255, 0);

        rate = (1.0f / 4.5f) * 2.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            noticeText.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;
        }

        if(noticeQueue.Count > 0)
        {
            StartCoroutine(animateNoticeText(noticeQueue.Dequeue()));
        }

        
    }

        /*Level related functions
      //***********************************************************************
      //***********************************************************************
      //***********************************************************************
      //***********************************************************************
      //***********************************************************************
      */

        /*Save related functions
       //***********************************************************************
       //***********************************************************************
       //***********************************************************************
       //***********************************************************************
       //***********************************************************************
       */

        //save system!
        void SaveCollectables()
    {
        Save_System.SaveCollectables(Collect_Manager.instance);
    }

   

    /*Save related functions
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   //***********************************************************************
   */


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


    //Flash one of the indicator arrows and then have it slowly fade away to show the player which of the 3 lanes the enemy will attack from.
    public void indicatorArrow(Wave_Spawner.spawnPointNum theSpawn)
    {
        if (theSpawn == Wave_Spawner.spawnPointNum.spawnPoint3)
        {
            indicatorArrowTop.color = new Color32(255, 0, 0, 255);
          //  StartCoroutine(fadeIndicatorArrow(indicatorArrowTop));
        }
        else if (theSpawn == Wave_Spawner.spawnPointNum.spawnPoint2)
        {
            indicatorArrowBot.color = new Color32(255, 0, 0, 255);
           // StartCoroutine(fadeIndicatorArrow(indicatorArrowBot));
        }
        else
        {
            indicatorArrowMid.color = new Color32(255, 0, 0, 255);
          // StartCoroutine(fadeIndicatorArrow(indicatorArrowMid));
        }
    }

    public void indicatorArrowOff(Wave_Spawner.spawnPointNum theSpawn)
    {
        if (theSpawn == Wave_Spawner.spawnPointNum.spawnPoint3)
        {
            indicatorArrowTop.color = new Color32(255, 0, 0, 0);
            //  StartCoroutine(fadeIndicatorArrow(indicatorArrowTop));
        }
        else if (theSpawn == Wave_Spawner.spawnPointNum.spawnPoint2)
        {
            indicatorArrowBot.color = new Color32(255, 0, 0, 0);
            // StartCoroutine(fadeIndicatorArrow(indicatorArrowBot));
        }
        else
        {
            indicatorArrowMid.color = new Color32(255, 0, 0, 0);
            // StartCoroutine(fadeIndicatorArrow(indicatorArrowMid));
        }
    }

    IEnumerator fadeIndicatorArrow(Image indArrow)
    {
        /*
        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 255);
        Color32 endColor = new Color32(255, 255, 255, 0);


        rate = (1.0f / 4.5f) * 1.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            indArrow.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;
        }
        */
        yield return new WaitForSeconds(1.5f);
        indArrow.color = new Color(255, 0, 0, 0);

       // Color indArrowColor = indArrow.color;

       // indArrow.color = Color.Lerp(indArrowColor, new Color(0, 0, 0, 0), 0.5f);
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
        duckButtonInteract.resetHeldValue();
        jumpButtonInteract.resetHeldValue();

        while (chestSelect == 0)
        {
            if (thePlayer.GetState() == Player.playerState.hanging && jumpButtonInteract.getTimeHeld() > 1.0f)
            {
                duckButton.enabled = false;
                jumpButton.enabled = false;
                playerAnimator.SetBool(IsFalling, true);
                chestSelect = 2;
               // Debug.Log("You selected chest 2!");
                
                break;
            }
            else if (thePlayer.GetState() == Player.playerState.ducking && duckButtonInteract.getTimeHeld() > 1.0f)
            {
                duckButton.enabled = false;
                jumpButton.enabled = false;
                chestSelect = 1;
              //  Debug.Log("You selected chest 1!");

                break;
            }

            setupNoticeTextAnimation("Hold down the Jump or Duck button to make a choice!",true);
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
        duckButtonInteract.resetHeldValue();
        jumpButtonInteract.resetHeldValue();

        while (portalSelect == 0)
        {
            if (thePlayer.GetState() == Player.playerState.hanging && jumpButtonInteract.getTimeHeld() > 1.0f)
            {
                duckButton.enabled = false;
                jumpButton.enabled = false;
                playerAnimator.SetBool(IsFalling, true);
                portalSelect = 2;
                // Debug.Log("You selected chest 2!");
            }
            else if (thePlayer.GetState() == Player.playerState.ducking && duckButtonInteract.getTimeHeld() > 1.0f)
            {
                duckButton.enabled = false;
                jumpButton.enabled = false;
                portalSelect = 1;
                //  Debug.Log("You selected chest 1!");

            }

            setupNoticeTextAnimation("Hold down the Jump or Duck button to make a choice!",true);
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



        /*Scoring  related functions
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    //***********************************************************************
    */
    public void setWavesSurvived(int num)
    {
        wavesSurvived = num;
    }

    //This function updates the score periodically. We should be calling this in constantly to update the integer value.
    //Math that needs to be done for multipliers etc will be done elsewhere.
    public void updateScore(int increase)
    {
        if(thePlayer.GetState() != Player.playerState.dead)
        {
            if (currentScore > 9999999)
            {
                currentScore = 9999999;
                scoreText.text = "RP: " + currentScore.ToString();
            }
            else
            {
                int tempScore = currentScore;
                //We may need to change this logic if pointMultiplier isn't a blanket effect.
                currentScore += (increase * pointMultiplier);
                //Current score has been increased, so we pass that in as the target to get to.
                if (scoreCountRoutine != null)
                {
                    StopCoroutine(scoreCountRoutine);
                    scoreCountRoutine = null;
                   // Debug.Log("New score total = " + currentScore);
                }

                scoreCountRoutine = StartCoroutine(scoreAni(tempScore, currentScore));
            }

        }

    }

    public void increaseMultiplier()
    {
        pointMultiplier += 1;
        if(pointMultiplier > 5)
        {
            pointMultiplier = 5;
        }
        multiplierText.text = pointMultiplier.ToString() + "x";
    }

    public void resetMultiplier()
    {
        pointMultiplier = 1;
        multiplierText.text = pointMultiplier.ToString() + "x";
        enemiesDodgedSinceLastMulti = 0;
    }

    //If we need this via another script.
    public int getMultiplier()
    {
        return pointMultiplier;
    }

    public void increaseEnemiesDodged()
    {
        enemiesDodgedSinceLastMulti += 1;
        if(enemiesDodgedSinceLastMulti == 5)
        {
            increaseMultiplier();
            enemiesDodgedSinceLastMulti = 0;
        }
        
    }

    //This function uses a coroutine to update the score periodically.
    public void checkScore()
    {
        StartCoroutine(scoreTimer());
    }

    //Every few seconds update the score by 50 as a test.
    public IEnumerator scoreTimer()
    {
     
        float timePassed = 0f;
        yield return null;

        //Refactor the check here...don't wanna continously be calling this. !!!!!
        while(Wave_Spawner.Instance.getWaveType() == Wave_Spawner.typeOfWave.normal && thePlayer.GetState() != Player.playerState.dead && Wave_Spawner.Instance.getIntroFinishedStatus())
        {
            timePassed += Time.deltaTime;

            //May want to have the threshold as a variable so that we can edit it on the fly for multipliers and stuff?
            if(timePassed >= 2.5f)
            {
                updateScore(10);
                timePassed = 0f;

            }

            yield return null;
        }
    }

    //Animation for making the score count up like a ticker.
    public IEnumerator scoreAni(int incomingScore, int nextScore)
    {
        scoreText.text = "RP: " + incomingScore.ToString();
        int speedToCount = 1;
        if (nextScore - incomingScore >= 500)
        {
            speedToCount = 5;
        }
        else if (nextScore - incomingScore >= 1000)
        {
            speedToCount = 10;
        }

        while (incomingScore < nextScore)
        {
            yield return new WaitForSeconds(0.01f * Time.deltaTime);
            incomingScore += speedToCount;
            if(incomingScore > nextScore)
            {
                incomingScore = nextScore;
            }
            scoreText.text = "RP: " + incomingScore.ToString();
        }

        yield return null;

    }

    public int getScore()
    {
        Debug.Log("Score is: " + currentScore);
        return currentScore;
    }



    //Game over related score stuff.

    //Tally up the points you received, and the wave bonus. Convert those totals into coins. Add bonus coins to your currently collected coins. Right now we're doing 1000 points = 1 coin.
    public void gameOverTally()
    {

        gameOverTallyP.SetActive(true);
        gameOverAdP.SetActive(false);

        //Change game over sprite to match the sprite player is playing with.
        playerGameoverSpin.SetActive(true);
        setGameoverSkin();
    

        //RPGameOverText.text = "RP: " + currentScore.ToString();
        waveBonus = wavesSurvived * 50;
        //waveBonusText.text = "Wave Bonus: " + wavesSurvived + " X 50 =" +  waveBonus.ToString();
        totalScore = currentScore + waveBonus;
        //totalScoreText.text = "Total score: " + totalScore.ToString();
       
        int extraCoins = totalScore / 1000;

        finalCoins = coinsCollected + extraCoins;
        finalBolts = boltsCollected;

        //coinsGameOverText.text = "X " + finalCoins;

        finalTallyRoutine = StartCoroutine(finalTally(currentScore, waveBonus, coinsCollected, finalCoins, totalScore));
        skipTallyButtonP.SetActive(true);
        coinsCollected = finalCoins;
        Collect_Manager.instance.totalCoins += coinsCollected;
        Collect_Manager.instance.totalBolts += boltsCollected;
        currentScore = totalScore;
        checkFinalScore();
        SaveCollectables();

    }

    public void stopTally()
    {
        if(finalTallyRoutine != null)
        {
            StopCoroutine(finalTallyRoutine);
        }

        int oldCurrentScore = totalScore - waveBonus;
        RPGameOverText.text = "RP: " + oldCurrentScore.ToString();
        waveBonusText.text = "Wave Bonus: " + wavesSurvived + " X 50 = " + waveBonus.ToString();
        totalScoreText.text = "Total score: " + totalScore;
        coinsGameOverText.text = "X " + finalCoins;
        boltsGameOverText.text = "X " + boltsCollected;

        gameOverButtonsP.SetActive(true);
        Audio_Manager.Instance.stopSFX();
    }

    IEnumerator finalTally(int incomingScore, int finalWaveBonus, int incomingCoins, int finalCoins, int finalScore)
    {
        RPGameOverText.text = "RP: " + currentScore.ToString();
        waveBonusText.text = "Wave Bonus: " + wavesSurvived + " X 50 = 0";
        totalScoreText.text = "Total score: 0";
        boltsGameOverText.text = "X " + boltsCollected;
        coinsGameOverText.text = "X " + incomingCoins;

        int currentWaveBonus = 0;
        int tempCurrentScore = 0;

        playScoreTallySound();
        //Count up the current score player received during the run, not counting any bonuses yet.
        while (tempCurrentScore < incomingScore)
        {
            yield return new WaitForSeconds(0.001f * Time.deltaTime);

            //Increase speed of count based on points.
            if(incomingScore > 10000)
            {
                tempCurrentScore += 1000;
            }
            else if(incomingScore > 100000)
            {
                tempCurrentScore += 10000;
            }
            else
            {
                tempCurrentScore += 100;
            }
            
            if(tempCurrentScore > incomingScore)
            {
                tempCurrentScore = incomingScore;
            }
            RPGameOverText.text = "RP: " + tempCurrentScore.ToString();
            totalScoreText.text = "Total score: " + tempCurrentScore.ToString();
        }

        stopScoreTallySound();

        yield return new WaitForSeconds(3.0f * Time.deltaTime);



        //Count up the bonus points player received based on the amount of waves they survived.
        playScoreTallySound();

        while (currentWaveBonus < finalWaveBonus)
        {
            yield return new WaitForSeconds(0.01f * Time.deltaTime);
            currentWaveBonus += 5;
            if(currentWaveBonus > finalWaveBonus)
            {
                currentWaveBonus = finalWaveBonus;
            }
            waveBonusText.text = "Wave Bonus: " + wavesSurvived + " X 50 = " + currentWaveBonus;
        }
        stopScoreTallySound();

        yield return new WaitForSeconds(3.0f * Time.deltaTime);


        //Take the sum of the wave bonus and incoming points and add them to the score to make the final score.
        int tempSum = currentWaveBonus + incomingScore;
        playScoreTallySound();

        while (tempCurrentScore < tempSum)
        {
            yield return new WaitForSeconds(0.01f * Time.deltaTime);
            tempCurrentScore += 10;
            if(tempCurrentScore > tempSum)
            {
                tempCurrentScore = tempSum;
            }
            totalScoreText.text = "Total score: " + tempCurrentScore.ToString();
        }
        currentScore = tempCurrentScore;

        stopScoreTallySound();

        yield return new WaitForSeconds(3.0f * Time.deltaTime);



        //Calculate the amount of bonus coins the player gets based on their score.
        playCoinTallySound();
        while (incomingCoins < finalCoins)
        {
            yield return new WaitForSeconds(0.01f * Time.deltaTime);
            incomingCoins += 3;
            if(incomingCoins > finalCoins)
            {
                incomingCoins = finalCoins;
            }
            coinsGameOverText.text = "X " + incomingCoins;
        }

        stopCoinTallySound();
        skipTallyButtonP.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.0f * Time.deltaTime);

        gameOverButtonsP.SetActive(true);
        yield return null;
    }

    void playScoreTallySound()
    {
        Audio_Manager.Instance.playSFX(RPTallySound, true, 0.2f);
    }

    void stopScoreTallySound()
    {
        Audio_Manager.Instance.stopSFX(RPTallySound.name);
    }

    void playCoinTallySound()
    {
        Audio_Manager.Instance.playSFX(coinTallySound, true, 0.2f);
    }

    void stopCoinTallySound()
    {
        Audio_Manager.Instance.stopSFX(coinTallySound.name);
    }

    public void playErrorSound()
    {
        stopErrorSound();
        Audio_Manager.Instance.playSFX(errorSound);
    }

    void stopErrorSound()
    {
        Audio_Manager.Instance.stopSFX(errorSound.name);
    }



    void checkFinalWaves()
    {
        //Add to total waves survived. Might have some achievement for this.
        Collect_Manager.instance.totalWavesSurvived += wavesSurvived;

        if (wavesSurvived > Collect_Manager.instance.mostWavesSurvived)
        {
            Collect_Manager.instance.mostWavesSurvived = wavesSurvived;
            Debug.Log("You got a new record for most waves survived!");
        }
    }

    void checkFinalScore()
    {
        if (currentScore > Collect_Manager.instance.highScore)
        {
            Collect_Manager.instance.highScore = currentScore;

            //Submit to leaderboard here!
            if (Essential_GameServices.instance != null)
            {
                Essential_GameServices.instance.ReportScoreToLeaderboards("Mostresearchpoints", currentScore);
                Debug.Log("Submitting your high score to the leaderboard!");
            }
            else
            {
                Debug.LogWarning("Hey, we could not find the instance of the gameservices. Can't submit your high score!");
            }
        

            /*
            if(CloudOnce_Services.instance != null)
            {
                CloudOnce_Services.instance.submitLeaderboardScore(currentScore);
                Debug.Log("You got a new high score! Submitted it to the leaderboard.");
            }
            else
            {
                Debug.LogWarning("You got a new high score! Couldn't submit it to the leaderboard, though.");
            }
            */


        }
    }

    void playRespawnAd()
    {
        if (AdsManager.instance != null)
        {
            AdsManager.instance.playRewardedVideoAd();
        }
    }


    //Game over related score stuff.

    /*Scoring  related functions
//***********************************************************************
//***********************************************************************
//***********************************************************************
//***********************************************************************
//***********************************************************************
*/


}

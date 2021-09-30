//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Wave_Spawner : MonoBehaviour
{
    //Tells us the type of wave we're on. Normal = enemy spawn, bonus = treasure and other stuff, timeSwap = time portals appear.
    public enum typeOfWave
    {
        normal,
        bonus,
        timeSwap
    }

    //Where items/enemies will spawn. We need to make these adjustable for different enemy types.
    public enum spawnPointNum
    {
        spawnPoint1,
        spawnPoint2,
        spawnPoint3,
        random
    }

    //Difficulty of the waves. We'll have enemies that only spawn when we're at a certain difficulty.
    //The difficulty will start at easy when you enter a new time period, and then upgrade to medium, and then hardPause.
    public enum waveDiff
    {
        easy,
        medium,
        hardPause
    }

    [SerializeField] Level_Manager.levelType theLevelType;


    [Tooltip("The difficulty of the wave. Should be changeable on the fly.")]
    [SerializeField] waveDiff theWaveDiff;
    
    [Tooltip("The type of wave. We spawn on chests on bonus, portals on timeWarp")]
    typeOfWave waveType;

    [Tooltip("Display for what wave we're on.")]
    public TextMeshProUGUI waveText;

    [Tooltip("Display the time period we're currently in.")]
    public TextMeshProUGUI eraText;


    [SerializeField] int waveCount;
    [SerializeField] int wavesSinceBonus;
    [SerializeField] int minWavesTillBonus;
    [SerializeField] int minWavesTillTimeswap;
    [SerializeField] int wavesSinceTimeSwap;
    [SerializeField] int wavesSinceMeterIncrease;
    [SerializeField] bool hasSwapped;

    [Tooltip("Rate that enemies will spawn in at. This should decrease with each wave passed.")]
    public float spawnRate;
    [Tooltip("The time between waves. Probably want this to be constant.")]
    public float timeBetweenWaves;

    bool waveComplete;
    [Tooltip("Enemies we'll be using. This list will be swapped out based on the time period we're in. Enemies in slots 0-2 are easy, 3-5 are medium, and 6 onward are hardPause")]
    public List<GameObject> enemies;
    [Tooltip("Spawnpoints for enemies/items. We'll use these to determine where things spawn. Should be Gameobjects with 'spawnPoint' tag.")]
    public List<GameObject> spawnPoints;

    [SerializeField]
    [Tooltip("The current spawnpoint of the enemy.")]
    spawnPointNum currentSpawn;

    [Tooltip("How many enemies we want to spawn during this wave.")]
    public int enemyCount;
    [Tooltip("Enemies left. This starts at enemyCount and then goes down whenever an enemy de-spawns.")]
    public int enemiesLeft;

    [Tooltip("Prefab of a coin that we will be spawning in.")]
    public GameObject coinPrefab;

    [Tooltip("Prefab of bolt that we will be spawning in.")]
    public GameObject boltPrefab;

    [Tooltip("Prefabs for treasure chests that will spawn in.")]
    public GameObject chestPrefabGround;
    public GameObject chestPrefabAir;
    public Sprite coinImg;

    [Tooltip("Prefabs for different time periods.")]
    public List<GameObject> timePortalPrefabs;

    
    [SerializeField] List<GameObject> prehistoricBackgrounds;
    [SerializeField] List<GameObject> FeudalBackgrounds;
    [SerializeField] List<GameObject> WildWestBackgrounds;
    [SerializeField] List<GameObject> MedBackgrounds;
    [SerializeField] List<GameObject> FutureBackgrounds;
    [SerializeField] GameObject tutorialBG;

    [SerializeField] GameObject currentBG1;
    [SerializeField] GameObject currentBG2;


    [SerializeField] bool introTransitionFinished;
    [SerializeField] RectTransform fadePanel;
    [SerializeField] GameObject loadingIcon;
    [SerializeField] GameObject bgTransitionGraphic;
    [SerializeField] Button pauseButton;

    public Player thePlayer;
    Player.playerState currentPlayerState;

    Coroutine spawnRoutine;
    Coroutine chestRoutine;
    Coroutine timeRoutine;

    bool specialWaveOn;

    bool stopCo;

    bool lastEnemySpawnedCoins;

    public int wavesSinceCollectedBolt;
    bool attemptedToSpawnBoltThisWave;

    float bgSeperationVal = 16.79999f;
    //Variance on waves related things.

    int wavesSinceDifficultyChange;

    public static Wave_Spawner Instance;

    [Header("SFX")]
    [SerializeField] AudioClip itemRouletteSound;
    [SerializeField] AudioClip itemGetSound;
    [SerializeField] AudioClip warpInSound;
    [SerializeField] AudioClip countdownSound;
    [SerializeField] AudioClip startSound;
    [SerializeField] AudioClip retryWarpSound;
    [SerializeField] AudioClip returnToLabSound;
    [SerializeField] AudioClip switchingErasSound;

    private void Awake()
    {
        Instance = this;
        introTransitionFinished = false;
    }
    void Start()
    {
        initializeWaveSpawner();
    }

    // Update is called once per frame
    void Update()
    {
      //  currentPlayerState = thePlayer.GetState();

        if(waveType == typeOfWave.normal)
        {
            
        }
        else if(waveType == typeOfWave.bonus)
        {
            waveText.text = "Bonus!";
        }
        else if(waveType == typeOfWave.timeSwap)
        {
            waveText.text = "Timeswap!";
        }

        eraText.text = "Current era: " + Level_Manager.Instance.getTimePeriod().ToString();
        //Messy...Might want to reorganize.
        if((Level_Manager.Instance.thePlayer.getPlayerDeathVal() || Level_Manager.Instance.getThisLevelType() == Level_Manager.levelType.tutorial) && introTransitionFinished)
        {
            if(stopCo == false)
            {
                if(spawnRoutine != null)
                {
                    StopCoroutine(spawnRoutine);
                }
                enemiesLeft = enemyCount;
                stopCo = true;
            }
            
        }
        else
        {
            if (waveComplete == true && introTransitionFinished)
            {
                stopCo = false;
                spawnRoutine = StartCoroutine(waveSpawner(waveType));
            }
        }

    }

    void initializeWaveSpawner()
    {
        theLevelType = Level_Manager.Instance.getThisLevelType();
        currentPlayerState = thePlayer.GetState();
        waveType = typeOfWave.normal;
        waveComplete = true;
        waveCount = 1;
        wavesSinceBonus = 0;
        minWavesTillBonus = 2;
        minWavesTillTimeswap = 1;
        wavesSinceTimeSwap = 0;
        wavesSinceCollectedBolt = 0;
        attemptedToSpawnBoltThisWave = false;
        stopCo = false;
        lastEnemySpawnedCoins = false;
        theWaveDiff = waveDiff.easy;
        hasSwapped = false;
        // Level_Manager.Instance = this.GetComponent<Level_Manager>();
        specialWaveOn = false;
        wavesSinceDifficultyChange = 0;
        enemiesLeft = enemyCount;
        SetCurrentEnemies();

        List<GameObject> BGsToLoad = new List<GameObject>();
        //Setup the BGs, this will be based on the era from LevelManager!
        //Will also setup music tracks here.
        switch (Level_Manager.Instance.getTimePeriod())
        {
            case Level_Manager.timePeriod.Prehistoric:
                {
                    BGsToLoad = prehistoricBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.FeudalJapan:
                {
                    BGsToLoad = FeudalBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.WildWest:
                {
                    BGsToLoad = WildWestBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.Medieval:
                {
                    BGsToLoad = MedBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.Future:
                {
                    BGsToLoad = FutureBackgrounds;

                    break;
                }
            case Level_Manager.timePeriod.tutorial:
                {
                    BGsToLoad.Add(tutorialBG);

                    break;
                }
            default:
                {
                    BGsToLoad = prehistoricBackgrounds;
                    break;
                }
        }
        currentBG1 = Instantiate(BGsToLoad[0]);
        currentBG2 = Instantiate(BGsToLoad[0]);

        currentBG2.transform.position = new Vector3(BGsToLoad[0].transform.position.x + bgSeperationVal, BGsToLoad[0].transform.position.y);

        //Play intro transition.
        StartCoroutine(introTransition());
    }

    IEnumerator introTransition()
    {
        loadingIcon.SetActive(true);
        fadePanel.gameObject.SetActive(true);
        waveText.text = waveCount.ToString();
        yield return new WaitForSeconds(0.3f);

        Audio_Manager.Instance.playSFX(warpInSound,false,0.3f);

        if (Level_Manager.Instance.getThisLevelType() == Level_Manager.levelType.tutorial)
        {
            Level_Manager.Instance.setupNoticeTextAnimation("Tutorial Start!", true);
        }
        else
        {
            Level_Manager.Instance.setupNoticeTextAnimation("Get ready!", true);
        }
        loadingIcon.SetActive(false);

        Vector3 currentScale = fadePanel.localScale;
        Vector3 decreaseScaleRate = new Vector3(1, 1, 1);

        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 150);
        Color32 endColor = new Color32(255, 255, 255, 0);
        Image fadePanelImg = fadePanel.GetComponent<Image>();

        rate = (1.0f / 2.5f) * 1.0f;


        while (i < 1.0f)
        {
            currentScale -= decreaseScaleRate;
           // fadePanel.localScale = currentScale;
            i += Time.deltaTime * rate;

            fadePanel.localScale = Vector3.Lerp(fadePanel.localScale, new Vector3(0, 0, 0), (i));

            fadePanelImg.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;

            //  yield return new WaitForSeconds(0.001f);
        }
      

        if (Level_Manager.Instance.getThisLevelType() == Level_Manager.levelType.tutorial)
        {
            fadePanel.transform.parent.gameObject.SetActive(false);
            fadePanel.gameObject.SetActive(false);
            introTransitionFinished = true;
            yield break;
        }

      //  yield return new WaitForSeconds(1.0f);
        Audio_Manager.Instance.playSFX(countdownSound);
        Level_Manager.Instance.setupNoticeTextAnimation("3", true);
        yield return new WaitForSeconds(1.0f);
        Audio_Manager.Instance.playSFX(countdownSound);
        Level_Manager.Instance.setupNoticeTextAnimation("2", true);
        yield return new WaitForSeconds(1.0f);
        Audio_Manager.Instance.playSFX(countdownSound);
        Level_Manager.Instance.setupNoticeTextAnimation("1", true);
        yield return new WaitForSeconds(1.0f);
        Audio_Manager.Instance.playSFX(startSound);

       
        fadePanel.transform.parent.gameObject.SetActive(false);
        fadePanel.gameObject.SetActive(false);
        introTransitionFinished = true;
        yield return null;
    }

    public IEnumerator respawnIntroTransition()
    {
        introTransitionFinished = false;
        fadePanel.transform.parent.gameObject.SetActive(true);
        fadePanel.gameObject.SetActive(true);

        Level_Manager.Instance.setupNoticeTextAnimation("Get ready!", true);
        yield return new WaitForSeconds(0.8f);

        Audio_Manager.Instance.playSFX(countdownSound);
        Level_Manager.Instance.setupNoticeTextAnimation("3", true);
        yield return new WaitForSeconds(1.0f);
        Audio_Manager.Instance.playSFX(countdownSound);
        Level_Manager.Instance.setupNoticeTextAnimation("2", true);
        yield return new WaitForSeconds(1.0f);
        Audio_Manager.Instance.playSFX(countdownSound);
        Level_Manager.Instance.setupNoticeTextAnimation("1", true);
        yield return new WaitForSeconds(1.0f);
        Audio_Manager.Instance.playSFX(startSound);
        Level_Manager.Instance.setupNoticeTextAnimation("Go!", true);
        Time.timeScale = Level_Manager.Instance.GetTimeScale();

        fadePanel.transform.parent.gameObject.SetActive(false);
        fadePanel.gameObject.SetActive(false);
        introTransitionFinished = true;
        stopCo = false;
        spawnRoutine = StartCoroutine(waveSpawner(waveType));
        yield return null;
    }


    //Spawn for the regular waves.
    //We can potentially use this same function for bonus + time swap...Just need to pass in the current waveState and do something based on what it is.
    IEnumerator waveSpawner(typeOfWave theWaveType)
    {
        int randNum;

        int minNum = 0;

        //Since we don't want all enemies in the pool to be chosen from [some may be more difficult]
        //We'll need to make maxNum change depending on the wave number we're on. [i.e. Harder enemies only appear on wave 5+, etc.]

        int maxNum = (enemies.Count);

        Transform enemySpawnPlacement;

        waveComplete = false;

        //use this as a failsafe so we don't end up in an infinite loop when checking for difficulties of enemies. If easy mode pick only easy enemies, medium can pick easy or medium, hardPause can pick all etc.
        List<GameObject> diffOptions = new List<GameObject>();

        //Regular spawn.
        if (theWaveType == typeOfWave.normal)
        {
            if (waveCount == 1)
            {
                Level_Manager.Instance.setupNoticeTextAnimation("Wave " + waveCount + " start!", true);
            }
            else
            {
                Level_Manager.Instance.setupNoticeTextAnimation("Wave " + waveCount + " start!");

                float currentHeatMeterFillRate = Level_Manager.Instance.thePlayer.getHeatMeterFill();
                float currentIceMeterFillRate = Level_Manager.Instance.thePlayer.getIceMeterFill();

                if (currentHeatMeterFillRate < 0.8f && currentIceMeterFillRate < 0.8f)
                {
                    bool changedMeterRates = false;

                    if (wavesSinceMeterIncrease >= 0)
                    {
                        currentHeatMeterFillRate += 0.05f;
                        currentIceMeterFillRate += 0.05f;
                        changedMeterRates = true;
                    }
                    /*
                    if (currentHeatMeterFillRate < 0.50f)
                    {
                      //  Debug.Log("Hey we're testing less than 0.5!");
                        if (wavesSinceMeterIncrease >= 0)
                        {
                            currentHeatMeterFillRate += 0.05f;
                            currentIceMeterFillRate += 0.05f;
                            changedMeterRates = true;
                        }
                    }
                    else if(currentHeatMeterFillRate >= 0.50f && currentHeatMeterFillRate < 1.10f)
                    {
                      //  Debug.Log("Hey we're testing between 0.5 and 0.6!");
                        if (wavesSinceMeterIncrease > 1)
                        {
                            currentHeatMeterFillRate += 0.05f;
                            currentIceMeterFillRate += 0.05f;
                            changedMeterRates = true;
                        }
                    }
                    */
                    /*
                    else if (currentHeatMeterFillRate >= 0.60f && currentHeatMeterFillRate < 0.85f)
                    {
                      //  Debug.Log("Hey we're testing between 0.6 and 0.85!");
                        if (wavesSinceMeterIncrease > 2)
                        {
                            currentHeatMeterFillRate += 0.05f;
                            currentIceMeterFillRate += 0.05f;
                            changedMeterRates = true;
                        }
                    }
                    else
                    {
                       // Debug.Log("Hey we're testing between 0.85 and 0.9 (LAST)!");
                        if (wavesSinceMeterIncrease > 3)
                        {
                            currentHeatMeterFillRate += 0.05f;
                            currentIceMeterFillRate += 0.05f;
                            changedMeterRates = true;
                        }
                    }
                    */

                    if(changedMeterRates)
                    {
                        Level_Manager.Instance.thePlayer.setHeatMeterFillVal(currentHeatMeterFillRate);
                        Level_Manager.Instance.thePlayer.setIceMeterFillVal(currentIceMeterFillRate);

                        Level_Manager.Instance.setMeterRates();
                        Level_Manager.Instance.iceMeter.setFillRate();
                        Level_Manager.Instance.heatMeter.setFillRate();

                        Level_Manager.Instance.setupNoticeTextAnimation("Temperature's getting more intense!");
                        wavesSinceMeterIncrease = 0;
                    }
                    
                }

               
            }
            waveText.text = waveCount.ToString();


            //Update item cooldowns.
            Level_Manager.Instance.itemCDUpdate();

            //Test values for changing difficulty. Will need some formula later on.
            //if (wavesSinceDifficultyChange == 3)
           //DEBUG , USE THE ONE ABOVE FOR REAL!
            if(hasSwapped)
            {
                if (wavesSinceDifficultyChange == 1)
                {
                    theWaveDiff = waveDiff.medium;
                    setBGs(theWaveDiff);
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
                  //  Debug.Log("The difficulty of the wave is: " + theWaveDiff);
                }
                if (wavesSinceDifficultyChange == 2)
                //if (wavesSinceDifficultyChange == 5)
                {
                    theWaveDiff = waveDiff.hardPause;
                    setBGs(theWaveDiff);
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
                  //  Debug.Log("The difficulty of the wave is: " + theWaveDiff);
                }
            }
            else
            {
                if (wavesSinceDifficultyChange == 3)
                {
                    theWaveDiff = waveDiff.medium;
                    setBGs(theWaveDiff);
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
                  //  Debug.Log("The difficulty of the wave is: " + theWaveDiff);
                }
                if (wavesSinceDifficultyChange == 6)
                //if (wavesSinceDifficultyChange == 5)
                {
                    theWaveDiff = waveDiff.hardPause;
                    setBGs(theWaveDiff);
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
                  //  Debug.Log("The difficulty of the wave is: " + theWaveDiff);
                }
            }

           

            int tempLeftNum = 0;
            tempLeftNum = enemiesLeft;
            for (int i = 0; i < enemyCount; i++)
            {

                //In this case the range can be 0 , or the exact count because randomize needs to be 1 above whatever you want. EX: List has 2 items, count = 2, but only 2 index...So 2 won't ever be called.
                //randNum = Random.Range(minNum, maxNum);

                float timePassed = 0f;
                while (tempLeftNum <= enemiesLeft)
                {
                   // Debug.Log("tempLeftNum = " + tempLeftNum + " enemies left = " + enemiesLeft);
                    if(i == 0)
                    {
                        break;
                    }
                    /*
                    //Failsafe.
                    timePassed += Time.deltaTime;
                    if(timePassed > spawnRate)
                    {
                        //Debug.Log("Broke out of the loop, got stuck!");
                        break;
                    }
                    */
                    yield return null;
                }
                if(i != 0)
                {
                    tempLeftNum = enemiesLeft;
                }
                
                
                switch(theWaveDiff)
                {
                    case waveDiff.easy:
                        {
                            for(int j = 0; j < maxNum; j++)
                                {
                                    if(enemies[j].GetComponent<Obstacle_Behaviour>().thisObstacleDiff == Obstacle_Behaviour.obstacleDiff.easy)
                                    {
                                        diffOptions.Add(enemies[j]);
                                    }
                                }
                            int newMax = diffOptions.Count;
                            randNum = Random.Range(minNum, newMax);
                            break;
                        }
                    case waveDiff.medium:
                        {
                            for (int j = 0; j < maxNum; j++)
                            {
                                if(hasSwapped)
                                {
                                    if (enemies[j].GetComponent<Obstacle_Behaviour>().thisObstacleDiff == Obstacle_Behaviour.obstacleDiff.medium)
                                    {
                                        diffOptions.Add(enemies[j]);
                                    }
                                }
                                else
                                {
                                    //Just check for not being hardPause because anything else is spawnable.
                                    if (enemies[j].GetComponent<Obstacle_Behaviour>().thisObstacleDiff != Obstacle_Behaviour.obstacleDiff.hardPause)
                                    {
                                        diffOptions.Add(enemies[j]);
                                    }
                                }
                               
                            }
                            int newMax = diffOptions.Count;
                            randNum = Random.Range(minNum, newMax);
                            break;
                        }
                    case waveDiff.hardPause:
                        {
                            for (int j = 0; j < maxNum; j++)
                            {
                                //Just check for not being easy because anything else is spawnable. UNLESS we've swapped already.
                                if(hasSwapped)
                                {
                                    if (enemies[j].GetComponent<Obstacle_Behaviour>().thisObstacleDiff == Obstacle_Behaviour.obstacleDiff.hardPause)
                                    {
                                        diffOptions.Add(enemies[j]);
                                        // Debug.Log("Added enemy number: " + j + " from the list of enemies since it's not an easy enemy.");
                                    }
                                }
                                else
                                {
                                    if (enemies[j].GetComponent<Obstacle_Behaviour>().thisObstacleDiff != Obstacle_Behaviour.obstacleDiff.easy)
                                    {
                                        diffOptions.Add(enemies[j]);
                                        // Debug.Log("Added enemy number: " + j + " from the list of enemies since it's not an easy enemy.");
                                    }
                                }
                               
                            }
                            int newMax = diffOptions.Count;
                            randNum = Random.Range(minNum, newMax);
                          
                            break;
                        }
                    default:
                        {
                            //Shouldn't ever reach here but default just in case.
                            Debug.LogWarning("Hey, we didn't find the difficulty!");
                            randNum = Random.Range(minNum, maxNum);
                            break;
                        }
                }


               
                //Get spawnpoint based on enemy's script, spawn them there.
                currentSpawn = diffOptions[randNum].GetComponent<Obstacle_Behaviour>().spawnPoint;


                //Add to this switch statement if we add more spawn points. May want to revise this to make it easier for futureproofing.
                switch(currentSpawn)
                {
                    case spawnPointNum.spawnPoint1:
                        {
                            //First spawn point.
                            enemySpawnPlacement = spawnPoints[0].transform;
                            break;
                        }
                    case spawnPointNum.spawnPoint2:
                        {
                            //Second spawn point.
                            enemySpawnPlacement = spawnPoints[1].transform;
                            break;
                        }
                    case spawnPointNum.spawnPoint3:
                        {
                            //Third spawn point.
                            enemySpawnPlacement = spawnPoints[2].transform;
                            break;
                        }
                    case spawnPointNum.random:
                        {
                            int randomSpawn;
                            randomSpawn = Random.Range(0, spawnPoints.Count);
                            enemySpawnPlacement = spawnPoints[randomSpawn].transform;
                            break;
                        }
                     default:
                        {
                            //Just use the first one if we don't have anything assigned for some reason.
                            enemySpawnPlacement = spawnPoints[0].transform;
                            break;
                        }
                       
                }


                //Debug.Log("We picked: " + diffOptions[randNum].gameObject.name);
                GameObject enemyClone = Object_Pooler.Instance.SpawnFromPool(diffOptions[randNum].name, enemySpawnPlacement.position, enemySpawnPlacement.rotation);
                // GameObject enemyClone = Instantiate(diffOptions[randNum], enemySpawnPlacement);
                //Reset the list.
                diffOptions.Clear();


                //Spawning in Bolt for player to try and pick up.

                Debug.Log("wavesSinceCollectedBolt = " + wavesSinceCollectedBolt);
                if (wavesSinceCollectedBolt >= 2 && enemiesLeft >= 2)
                {

                    int rndBoltSpawn = Random.Range(0, 3);
                    int chanceToSpawn = Random.Range(0, 3);


                    //Debug
                    chanceToSpawn = 2;

                    //Make sure lastEnemySpawnedCoins so that we don't overlap with coins with the bolt.
                    if (chanceToSpawn > 0 && !attemptedToSpawnBoltThisWave && lastEnemySpawnedCoins)
                    {
                        attemptedToSpawnBoltThisWave = true;
                        StartCoroutine(boltSpawn(rndBoltSpawn));
                    }


                }

                //Spawning in coins.

                if (!lastEnemySpawnedCoins && enemiesLeft >= 2)
                {
                    int spawnCoinRnd = Random.Range(0, 500);
                    if (spawnCoinRnd >= 250)
                    {
                        //Debug.Log("We're spawning coins! Value of RNG was: "+ spawnCoinRnd.ToString());

                        spawnCoinRnd = Random.Range(0, 2);
                        int amountofCoinsToSpawn = Random.Range(1, 5);
                        StartCoroutine(CoinSpawn(spawnCoinRnd, amountofCoinsToSpawn));
                        lastEnemySpawnedCoins = true;
                    }
                   
                }
                else
                {
                   // Debug.Log("Not spawning in coins since we spawned them on the last enemy.");
                    lastEnemySpawnedCoins = false;
                }



                //If there are too many enemies on screen maybe have some delay here instead of just static at the spawnRate value???
                yield return new WaitForSeconds(spawnRate);
            }

            while(enemiesLeft > 0)
            {
                //Debug.Log("There's still an enemy on screen!");
                yield return null;
            }

            
            //Only increase spawnrate , enemy count and wavecount after normal waves. Though we may need a hidden waveCount counter for achievements, etc.
            if (spawnRate > 0.4f)
            {
                spawnRate -= 0.1f;

                //DEBUG
              //  spawnRate = 1.5f;
            }
            
            
            enemyCount += 1;
            if(enemyCount > 999)
            {
                enemyCount = 999;
            }
            enemiesLeft = enemyCount;
            waveCount += 1;
           
            if(Level_Manager.Instance.GetTimeScale() < 1.7f && waveCount >= 12)
            {
                Level_Manager.Instance.setupNoticeTextAnimation("Time's speeding up!", true);
                Level_Manager.Instance.SetTimescale(0.05f);
              //  Debug.LogWarning("The timescale is now: " + Level_Manager.Instance.GetTimeScale());
            }
            Level_Manager.Instance.setWavesSurvived((waveCount - 1));
            wavesSinceDifficultyChange += 1;
            wavesSinceMeterIncrease += 1;
            wavesSinceCollectedBolt += 1;
            attemptedToSpawnBoltThisWave = false;
        }
        else if(theWaveType == typeOfWave.bonus)
        {
            //Insert treasure chest spawn here. Will need some way to calculate the treasure box items as well.

            //Spawn in chests, give player chance to jump or duck, play animation, give item.
            //Chests depsawn, wave over.

            if (!specialWaveOn)
            {
                specialWaveOn = true;
                chestRoutine = StartCoroutine(chestSpawn());
            }
        
        }
        else if(theWaveType == typeOfWave.timeSwap)
        {
            //Insert timeportal spawn here. Will need another list of diff time portals.

            if(!specialWaveOn)
            {
                specialWaveOn = true;
                List<GameObject> tempPortals = new List<GameObject>();
                for (int f = 0; f < timePortalPrefabs.Count; f++)
                {
                    tempPortals.Add(timePortalPrefabs[f]);
                }

                //These NEED to assosciate with both the timePortalPrefabs on the WaveSpawner & on the LevelManager's timePeriod.
                switch(Level_Manager.Instance.TimePeriod)
                {
                    case Level_Manager.timePeriod.Prehistoric:
                        {
                            tempPortals.Remove(tempPortals[0]);
                            break;
                        }
                    case Level_Manager.timePeriod.FeudalJapan:
                        {
                            tempPortals.Remove(tempPortals[1]);
                            break;
                        }
                    case Level_Manager.timePeriod.WildWest:
                        {
                            tempPortals.Remove(tempPortals[2]);
                            break;
                        }
                    case Level_Manager.timePeriod.Medieval:
                        {
                            tempPortals.Remove(tempPortals[3]);
                            break;
                        }
                    case Level_Manager.timePeriod.Future:
                        {
                            tempPortals.Remove(tempPortals[4]);
                            break;
                        }
                }
                //for (int g = 0; g < tempPortals.Count; g++)
                //{
                //    Debug.LogWarning("Name " + g + " in the portal list is: " + tempPortals[g]);
                //}

                int random1 = Random.Range(0, (tempPortals.Count));
                int random2 = Random.Range(0, (tempPortals.Count));
               //Make sure both are different.
                if(random1 == random2)
                {
                    if(random2 != 0)
                    {
                        random2 -= 1;
                    }
                    else if(random2 != tempPortals.Count)
                    {
                        random2 += 1;
                    }
                }
       
                //Debug for testing purposes.
               // random1 = 2;
                //random2 = 3;
                timeRoutine = StartCoroutine(SpawnTimePortal(random1, random2, tempPortals));
            }
            

        }

        
        //Bonus wave spawn.
        if(!specialWaveOn && waveType != typeOfWave.timeSwap)
        {   
            wavesSinceBonus++;
           // Debug.Log("Waves since bonus is: " + wavesSinceBonus.ToString());
            if (wavesSinceBonus > minWavesTillBonus)
            {
                
               // int doWeBonus;
               // doWeBonus = Random.Range(1, 7);
               ////DEBUGGING.
               // //doWeBonus = 8;
               // if(doWeBonus >= 2)
               // {
                  //  Debug.Log("Next wave is a bonus wave! RNG was: " + doWeBonus);
                    //Max we can wait should be 9 waves.
                    if(minWavesTillBonus < 9)
                    {
                        minWavesTillBonus += 1;
                    }
                    
                    wavesSinceBonus = 0;
                    waveType = typeOfWave.bonus;
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, true);
               // }

            }
        }
        


        //TimerPortal wave spawn.
        if (!specialWaveOn && waveType != typeOfWave.bonus)
        {
            //wavesSinceTimeSwap++;
            if (theWaveDiff == waveDiff.hardPause)
            {
                wavesSinceTimeSwap++;
             //   Debug.Log("Waves since TimeSwap is: " + wavesSinceTimeSwap.ToString());
            }


            if (wavesSinceTimeSwap > minWavesTillTimeswap && theWaveDiff == waveDiff.hardPause)
            {
                //int doWeTimeSwap;
                //doWeTimeSwap = Random.Range(1, 7);
                ////DEBUGGING.
                ////doWeTimeSwap = 8;
                //if (doWeTimeSwap >= 2)
                //{
                   // Debug.Log("Next wave is a timeswap wave! RNG was: " + doWeTimeSwap);
                    if (minWavesTillTimeswap < 5)
                    {
                        minWavesTillTimeswap += 1;
                    }

                    wavesSinceTimeSwap = 0;
                    waveType = typeOfWave.timeSwap;
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, true);
                //}

            }
        }



        if (waveType == typeOfWave.normal)
        {
           // Debug.Log("Special wave is: " + specialWaveOn);
            yield return new WaitForSeconds(timeBetweenWaves);
        }

       // Debug.Log("The wave is complete, new wave type is: " + waveType.ToString());

        waveComplete = true;
    }

    //Test coRoutine for spawning coins.
    public IEnumerator CoinSpawn(int rndSpawn, int amountToSpawn)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 tempSpawn;
        Quaternion tempRot;

        //Mid, so spawn on top or bottom.
        if (currentSpawn == spawnPointNum.spawnPoint1)
        {
            if (rndSpawn == 0)
            {
                tempSpawn = spawnPoints[2].transform.position;
                tempRot = spawnPoints[2].transform.rotation;
                Vector3 tempSpawnYChange = tempSpawn;
                tempSpawnYChange.y += 0.5f;
                tempSpawn = tempSpawnYChange;
            }
            else
            {
                tempSpawn = spawnPoints[1].transform.position;
                tempRot = spawnPoints[1].transform.rotation;
            }


        }
        //Bottom so spawn on mid or top.
        else if (currentSpawn == spawnPointNum.spawnPoint2)
        {
            if (rndSpawn == 0)
            {
                tempSpawn = spawnPoints[0].transform.position;
                tempRot = spawnPoints[0].transform.rotation;
                Vector3 tempSpawnYChange = tempSpawn;
                tempSpawnYChange.y -= 0.2f;
                tempSpawn = tempSpawnYChange;
            }
            else
            {
                tempSpawn = spawnPoints[2].transform.position;
                tempRot = spawnPoints[2].transform.rotation;
                Vector3 tempSpawnYChange = tempSpawn;
                tempSpawnYChange.y += 0.5f;
                tempSpawn = tempSpawnYChange;
            }
        }
        //Top so spawn on bottom always.
        else
        {
            tempSpawn = spawnPoints[1].transform.position;
            tempRot = spawnPoints[1].transform.rotation;
        }

        //Spawn X amount of coins based on the RNG.
      //  Debug.Log("Spawning in: " + amountToSpawn.ToString() + " coins!");
        for(int i = 0; i <= (amountToSpawn-1); i++)
        {
            Object_Pooler.Instance.SpawnFromPool(coinPrefab.name, tempSpawn, tempRot); 
            yield return new WaitForSeconds(0.25f);
           
        }
        
    }

    public IEnumerator boltSpawn(int rndSpawn)
    {
       // Debug.Log("Spawning in a bolt!");

        yield return new WaitForSeconds(0.5f);

        //Mid, so spawn on top or bottom.
        if(currentSpawn == spawnPointNum.spawnPoint1)
        {
            if(rndSpawn == 0)
            {
                Vector3 tempSpawnYChange = spawnPoints[2].transform.position;
                tempSpawnYChange.y += 0.1f;
                Object_Pooler.Instance.SpawnFromPool(boltPrefab.name, tempSpawnYChange, spawnPoints[2].transform.rotation);
            }
            else
            {
                Object_Pooler.Instance.SpawnFromPool(boltPrefab.name, spawnPoints[1].transform.position, spawnPoints[1].transform.rotation);
            }
            
            
        }
        //Bottom so spawn on mid or top.
        else if(currentSpawn == spawnPointNum.spawnPoint2)
        {
            if (rndSpawn == 0)
            {
                Vector3 tempSpawnYChange = spawnPoints[2].transform.position;
                tempSpawnYChange.y += 0.1f;
                Object_Pooler.Instance.SpawnFromPool(boltPrefab.name, tempSpawnYChange, spawnPoints[2].transform.rotation);
            }
            else
            {
                Vector3 tempSpawnYChange = spawnPoints[2].transform.position;
                tempSpawnYChange.y -= 0.2f;
                Object_Pooler.Instance.SpawnFromPool(boltPrefab.name, tempSpawnYChange, spawnPoints[0].transform.rotation);
            }
        }
        //Top so spawn on bottom always.
        else
        {
            Object_Pooler.Instance.SpawnFromPool(boltPrefab.name, spawnPoints[1].transform.position, spawnPoints[1].transform.rotation);
        }
         

        yield return null;
    }

    //For bonus wave.
    public IEnumerator chestSpawn()
    {
        Time.timeScale = 1f;
        AnimationClip chestOpenClip;
        float aniTime = 0f;

        GameObject chest1 = Instantiate(chestPrefabGround, spawnPoints[1].transform);
        //This 2nd prefab will probably be the top graphic that Timour has.
        GameObject chest2 = Instantiate(chestPrefabAir, spawnPoints[2].transform);
        chest2.GetComponent<Rigidbody2D>().gravityScale = -10f;

        //Basic randomization for prizes. This will have to be changed and have different stipulations based on waves passed, and other stuff etc.
        int randPrizeNum = 0;
        Sprite itemToReceive;
        GameObject chestSpriteChild;
        SpriteRenderer itemSpriteToChange;
        Collect_Manager.typeOfItem itemToGive = Collect_Manager.typeOfItem.none;

        int theSelection = Level_Manager.Instance.getChestSelect();


        Level_Manager.Instance.duckButton.enabled = false;
        Level_Manager.Instance.jumpButton.enabled = false;
       // Level_Manager.Instance.duckButton.gameObject.SetActive(false);
       // Level_Manager.Instance.jumpButton.gameObject.SetActive(false);

      //  Level_Manager.Instance.ResetAnimator();

        yield return new WaitForSeconds(1.5f);

        Level_Manager.Instance.duckButton.enabled = true;
        Level_Manager.Instance.jumpButton.enabled = true;

        StartCoroutine(Level_Manager.Instance.pickAChest());
        //Play animation for "Jump or Duck" option, then make the buttons available.

       
        Debug.Log("Waiting for response from the player...");

        while (theSelection == 0)
        {
            theSelection = Level_Manager.Instance.getChestSelect();
            yield return null;
        }

        if(theSelection == 1)
        {
            Animator tempAnimator = chest1.GetComponent<Animator>();
            chestOpenClip = tempAnimator.runtimeAnimatorController.animationClips[0];
            aniTime = chestOpenClip.length;
            chestSpriteChild = chest1.transform.GetChild(0).gameObject;
            itemSpriteToChange = chestSpriteChild.GetComponent<SpriteRenderer>();
            tempAnimator.SetBool("Chest_Open", true);
            chest1.GetComponent<Obstacle_Behaviour>().playSoundExternally(0.4f);
        }
        else
        {
            Animator tempAnimator = chest2.GetComponent<Animator>();
            chestOpenClip = tempAnimator.runtimeAnimatorController.animationClips[0];
            aniTime = chestOpenClip.length;
            chestSpriteChild = chest2.transform.GetChild(0).gameObject;
            itemSpriteToChange = chestSpriteChild.GetComponent<SpriteRenderer>();
            tempAnimator.SetBool("Chest_Open", true);
            chest2.GetComponent<Obstacle_Behaviour>().playSoundExternally(0.4f);
        }

        

        // Debug.Log("Ani time = " + aniTime);
        /*
        CURRENT ENUM
        HandWarmers,
        Defroster,
        FireVest,
        LiquidNitrogenCanister,
        NeutralTablet,
        none
        CURRENT ENUM
        randPrizeNum needs to be randomized to be between 0 (First index of the enum) and 1 less than the total number of elements in the enum. EX: 5 elements in enum, so highLimit would be 5-1 or 4.
        However, we are using itemsToPick as the list to count based on. So highLimit = the count of itemsToPick or everything except for 'none' from the enum.
        Since Random.Range is inclusive for the high end, it needs to be our value + 1...So highLimit = 5 , we do 5 + 1 or 6. This means that it can randomize between 0 and 5.
        Finally, since we're testing for coins as well, we add an extra value. So instead of 5 + 1, we'll do 5 + 2. This means it's randomizing between the 5 elements + 1 extra for coins, or 0-6.
         */

        int highLimit = Collect_Manager.instance.itemsToPick.Count;
        randPrizeNum = Random.Range(0, (highLimit+2));
        if(randPrizeNum != Collect_Manager.instance.itemsToPick.Count+1)
        {
            foreach (Collect_Manager.typeOfItem theItem in System.Enum.GetValues(typeof(Collect_Manager.typeOfItem)))
            {
                //Cast the enum to an integer to compare it.
                if (randPrizeNum == (int)theItem)
                {
                  //  Debug.Log("Current item number is: " + theItem + " Which corresponds to: " + randPrizeNum);
                    itemToGive = theItem;
                    break;
                }
            }
        }
        else
        {
            itemToGive = Collect_Manager.typeOfItem.none;
        }

        bool isAnItem = true;
        switch(itemToGive)
        {
            case Collect_Manager.typeOfItem.HandWarmers:
                {
                    itemToReceive = Collect_Manager.instance.itemsToPick[(int)Collect_Manager.typeOfItem.HandWarmers].itemImage;
                    break;
                }
            case Collect_Manager.typeOfItem.Defroster:
                {
                    itemToReceive = Collect_Manager.instance.itemsToPick[(int)Collect_Manager.typeOfItem.Defroster].itemImage;
                    break;
                }
            case Collect_Manager.typeOfItem.FireVest:
                {
                    itemToReceive = Collect_Manager.instance.itemsToPick[(int)Collect_Manager.typeOfItem.FireVest].itemImage;
                    break;
                }
            case Collect_Manager.typeOfItem.LiquidNitrogenCanister:
                {
                    itemToReceive = Collect_Manager.instance.itemsToPick[(int)Collect_Manager.typeOfItem.LiquidNitrogenCanister].itemImage;
                    break;
                }
            case Collect_Manager.typeOfItem.NeutralTablet:
                {
                    itemToReceive = Collect_Manager.instance.itemsToPick[(int)Collect_Manager.typeOfItem.NeutralTablet].itemImage;
                    break;
                }
            //For 'none'.
            default:
                {
                    itemToReceive = coinImg;
                    isAnItem = false;
                    break;
                }
        }
       
        yield return new WaitForSeconds(aniTime - 0.1f);

        chestSpriteChild.SetActive(true);
        Vector2 oldTrans = chestSpriteChild.transform.position;
        float duration = 0.4f;
        float time = 0f;
        int randItemImg;

        Audio_Manager.Instance.playSFX(itemRouletteSound, true);
        while (time < duration)
        {
            randItemImg = Random.Range(-1, Collect_Manager.instance.itemsToPick.Count);
            if(randItemImg == -1)
            {
                itemSpriteToChange.sprite = coinImg;
            }
            else
            {
                itemSpriteToChange.sprite = Collect_Manager.instance.itemsToPick[randItemImg].itemImage;
            }
              
            chestSpriteChild.transform.position = Vector2.Lerp(oldTrans, new Vector2(oldTrans.x, oldTrans.y + 0.25f), time/duration);
            time += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        Audio_Manager.Instance.stopSFX(itemRouletteSound.name);
        Audio_Manager.Instance.playSFX(itemGetSound, false, 0.4f);


        itemSpriteToChange.sprite = itemToReceive;

        //Play animation of item rising out of the chest and randomizing.
        yield return new WaitForSeconds(1.0f);


        if (isAnItem)
        {
            Collect_Manager.instance.purchaseItemConfirm(itemToGive);
            Level_Manager.Instance.setupNoticeTextAnimation("You just got: " + itemToGive.ToString() + "!");

            //if (theWaveDiff == waveDiff.easy)
            //{
            //    Collect_Manager.instance.purchaseItemConfirm(itemToGive);
            //    //Debug.Log("Received: " + itemToGive.ToString() + " from the treasure chest!");
            //    Level_Manager.Instance.setupNoticeTextAnimation("You just got: " + itemToGive.ToString() + "!");
            //}
            //else if(theWaveDiff == waveDiff.medium)
            //{
            //    Collect_Manager.instance.purchaseItemConfirm(itemToGive);
            //    Collect_Manager.instance.purchaseItemConfirm(itemToGive);
            //    //Debug.Log("Received: " + itemToGive.ToString() + " from the treasure chest!");
            //    Level_Manager.Instance.setupNoticeTextAnimation("You just got: x2 " + itemToGive.ToString() + "!");
            //}
            //else
            //{
            //    Collect_Manager.instance.purchaseItemConfirm(itemToGive);
            //    Collect_Manager.instance.purchaseItemConfirm(itemToGive);
            //    Collect_Manager.instance.purchaseItemConfirm(itemToGive);
            //    //Debug.Log("Received: " + itemToGive.ToString() + " from the treasure chest!");
            //    Level_Manager.Instance.setupNoticeTextAnimation("You just got: x3 " + itemToGive.ToString() + "!");
            //}
            
        }
        else
        {
            if(theWaveDiff == waveDiff.easy)
            {
                Level_Manager.Instance.collectCoin(50);
                //Debug.Log("Received: 50 coins from the treasure chest!");
                Level_Manager.Instance.setupNoticeTextAnimation("You just got: 50 coins!");
            }
            else if(theWaveDiff == waveDiff.medium)
            {
                Level_Manager.Instance.collectCoin(100);
                //Debug.Log("Received: 100 coins from the treasure chest!");
                Level_Manager.Instance.setupNoticeTextAnimation("You just got: 100 coins!");
            }
            else
            {
                Level_Manager.Instance.collectCoin(150);
                //Debug.Log("Received: 150 coins from the treasure chest!");
                Level_Manager.Instance.setupNoticeTextAnimation("You just got: 150 coins!");
            }
            
        }

        Destroy(chest1);
        Destroy(chest2);


        //We finished the bonus wave.
        Level_Manager.Instance.duckButton.enabled = true;
        Level_Manager.Instance.jumpButton.enabled = true;

        waveType = typeOfWave.normal;
        Level_Manager.Instance.checkScore();
        specialWaveOn = false;
        Level_Manager.Instance.setChestSelect(0);


        Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
        Time.timeScale = Level_Manager.Instance.GetTimeScale();
        waveComplete = true;


    }
    //Spawn in 2 time era portals at random. Same concept as chest spawn.
    public IEnumerator SpawnTimePortal(int portalIndex1, int portalIndex2, List<GameObject> theTempPortals)
    {

        Time.timeScale = 1f;
        GameObject portal1 = Instantiate(theTempPortals[portalIndex1], spawnPoints[1].transform);
        GameObject portal2 = Instantiate(theTempPortals[portalIndex2], spawnPoints[2].transform);


        //portal2.GetComponent<SpriteRenderer>().flipY = true;
        portal2.GetComponent<Rigidbody2D>().gravityScale = -10f;
        int theSelection = Level_Manager.Instance.getTimePortalSelection();

        Level_Manager.Instance.duckButton.enabled = false;
        Level_Manager.Instance.jumpButton.enabled = false;

        yield return new WaitForSeconds(1.5f);

        Level_Manager.Instance.duckButton.enabled = true;
        Level_Manager.Instance.jumpButton.enabled = true;
        //Play animation for "Jump or Duck" option, then make the buttons available.

        StartCoroutine(Level_Manager.Instance.pickAPortal());

        Debug.Log("Waiting for response from the player...");

        while (theSelection == 0)
        {
            theSelection = Level_Manager.Instance.getTimePortalSelection();
            yield return null;
        }

        Level_Manager.timePeriod tempTimePeriod;
        if (theSelection == 1)
        {
            tempTimePeriod = portal1.GetComponent<Obstacle_Behaviour>().theEra;
         
        }
        else
        {
            tempTimePeriod = portal2.GetComponent<Obstacle_Behaviour>().theEra;
        }

        Level_Manager.Instance.setTimePeriod(tempTimePeriod);
        Object_Pooler.Instance.SetTimePeriodList(Level_Manager.Instance.getTimePeriod());

        yield return new WaitForSeconds(0.1f);
        //After swapping time periods, we'll swap the currently used list in ObjectPooler thus affecting the list here.
        SetCurrentEnemies();


        yield return new WaitForSeconds(1.0f);

        Destroy(portal1);
        Destroy(portal2);

        //We finished the timeswap  wave.
        Level_Manager.Instance.duckButton.enabled = true;
        Level_Manager.Instance.jumpButton.enabled = true;
        waveType = typeOfWave.normal;
        Level_Manager.Instance.checkScore();
        specialWaveOn = false;
        Level_Manager.Instance.setTimePortalSelection(0);
        wavesSinceDifficultyChange = 0;

        //Switch back to easy when we swap waves...But we'll keep the spawn rate quick. Like warioware does kinda.
        theWaveDiff = waveDiff.easy;
        hasSwapped = true;
        Audio_Manager.Instance.setMusicTracks(Level_Manager.Instance.getTimePeriod());


        //Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
        //setBGs(theWaveDiff);
        StartCoroutine(AnimateToNextBG());

        waveComplete = true;
    }

    IEnumerator AnimateToNextBG()
    {
        pauseButton.interactable = false;

        introTransitionFinished = false;

        SpriteRenderer tempRef = bgTransitionGraphic.GetComponent<SpriteRenderer>();
        bgTransitionGraphic.transform.localScale = new Vector3(0, 0, 0);

        Vector3 currentScale = bgTransitionGraphic.transform.localScale;
        Vector3 increaseScaleRate = new Vector3(1, 1, 1);
        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 0);
        Color32 endColor = new Color32(255, 255, 255, 220);

        Audio_Manager.Instance.playSFX(switchingErasSound);
        rate = (1.0f / 2.5f) * 2.0f;

        //Animate the scale out to cover the transition.
        while (i < 1.0f)
        {
            currentScale += increaseScaleRate;
            i += Time.deltaTime * rate;

            bgTransitionGraphic.transform.localScale = Vector3.Lerp(bgTransitionGraphic.transform.localScale, new Vector3(7, 7, 7), (i));

            tempRef.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;

            //  yield return new WaitForSeconds(0.001f);
        }

        //220 = alpha at beginning.

        setBGs(theWaveDiff);
      
        //Reverse the animation.
        i = 0;

        while (i < 1.0f)
        {
            currentScale += increaseScaleRate;
            i += Time.deltaTime * rate;

            tempRef.color = Color32.Lerp(endColor, startColor, (i));
            yield return null;

            //  yield return new WaitForSeconds(0.001f);
        }
        bgTransitionGraphic.transform.localScale = new Vector3(0, 0, 0);

        yield return null;

        introTransitionFinished = true;
        pauseButton.interactable = true;
        Time.timeScale = Level_Manager.Instance.GetTimeScale();
    }



    //Getters/setters

    public typeOfWave getWaveType()
    {
        return waveType;
    }

    public waveDiff getWaveDiff()
    {
        return theWaveDiff;
    }

    //Fill in the current enemy list with enemies that are appropriate to the current time period we are in.
    void SetCurrentEnemies()
    {
        enemies.Clear();
        int tempCount = Object_Pooler.Instance.GetCurrentListCount();
        for (int i = 0; i < tempCount; i++)
        {
            GameObject tempObj = Object_Pooler.Instance.GetCurrentlistIndex(i);

            if (tempObj != null)
            {
                enemies.Add(tempObj);
            }
        }
    }


    //If swapping difficulties.
    void setBGs(waveDiff difficulty)
    {
        List<GameObject> BGsToLoad = new List<GameObject>();

        switch (Level_Manager.Instance.TimePeriod)
        {
            case Level_Manager.timePeriod.Prehistoric:
                {
                    BGsToLoad = prehistoricBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.FeudalJapan:
                {
                    BGsToLoad = FeudalBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.WildWest:
                {
                    BGsToLoad = WildWestBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.Medieval:
                {
                    BGsToLoad = MedBackgrounds;
                    break;
                }
            case Level_Manager.timePeriod.Future:
                {
                    BGsToLoad = FutureBackgrounds;

                    break;
                }
            default:
                {
                    BGsToLoad = FutureBackgrounds;
                    break;
                }
        }


        int indexInList = 0;
        if(difficulty == waveDiff.easy)
        {
            indexInList = 0;
        }
        else if(difficulty == waveDiff.medium)
        {
            indexInList = 1;
        }
        else
        {
            indexInList = 2;
        }

        Transform bg1Trans = currentBG1.transform;
        Destroy(currentBG1);
        Destroy(currentBG2);
        currentBG1 = null;
        currentBG2 = null;
        currentBG1 = Instantiate(BGsToLoad[indexInList], bg1Trans.position , this.transform.rotation);
        currentBG2 = Instantiate(BGsToLoad[indexInList], bg1Trans.position, this.transform.rotation);
        currentBG1.transform.position = new Vector2(currentBG1.transform.position.x, currentBG1.transform.position.y);
        currentBG2.transform.position = new Vector2(currentBG1.transform.position.x + bgSeperationVal, currentBG1.transform.position.y);
    }

    public void updateEnemiesLeft(int num)
    {
        enemiesLeft -= num;
        Debug.Log("Enemies left are: " + enemiesLeft);
    }

    public bool getIntroFinishedStatus()
    {
        return introTransitionFinished;
    }

    //Getters/Setters


    //Respawn from LevelManager function
    public void respawnPlayer()
    {
        StartCoroutine(respawnIntroTransition());

    }

    //Tutorial functions.

    public void tutorialSpawn()
    {
        switch(Tutorial_Manager.Instance.getCurrentStep())
        {
            //Spawn boulder.
            case 10:
                {
                    Transform enemySpawnPlacement;

                    currentSpawn = enemies[0].GetComponent<Obstacle_Behaviour>().spawnPoint;
                    //Add to this switch statement if we add more spawn points. May want to revise this to make it easier for futureproofing.
                    switch (currentSpawn)
                    {
                        case spawnPointNum.spawnPoint1:
                            {
                                //First spawn point.
                                enemySpawnPlacement = spawnPoints[0].transform;
                                break;
                            }
                        case spawnPointNum.spawnPoint2:
                            {
                                //Second spawn point.
                                enemySpawnPlacement = spawnPoints[1].transform;
                                break;
                            }
                        case spawnPointNum.spawnPoint3:
                            {
                                //Third spawn point.
                                enemySpawnPlacement = spawnPoints[2].transform;
                                break;
                            }
                        case spawnPointNum.random:
                            {
                                int randomSpawn;
                                randomSpawn = Random.Range(0, spawnPoints.Count);
                                enemySpawnPlacement = spawnPoints[randomSpawn].transform;
                                break;
                            }
                        default:
                            {
                                //Just use the first one if we don't have anything assigned for some reason.
                                enemySpawnPlacement = spawnPoints[0].transform;
                                break;
                            }

                    }

                    GameObject enemyClone = Object_Pooler.Instance.SpawnFromPool(enemies[0].name, enemySpawnPlacement.position, enemySpawnPlacement.rotation);
                    break;
                }
            //Spawn vine.
            case 12:
                {
                    Transform enemySpawnPlacement;

                    currentSpawn = enemies[1].GetComponent<Obstacle_Behaviour>().spawnPoint;
                    //Add to this switch statement if we add more spawn points. May want to revise this to make it easier for futureproofing.
                    switch (currentSpawn)
                    {
                        case spawnPointNum.spawnPoint1:
                            {
                                //First spawn point.
                                enemySpawnPlacement = spawnPoints[0].transform;
                                break;
                            }
                        case spawnPointNum.spawnPoint2:
                            {
                                //Second spawn point.
                                enemySpawnPlacement = spawnPoints[1].transform;
                                break;
                            }
                        case spawnPointNum.spawnPoint3:
                            {
                                //Third spawn point.
                                enemySpawnPlacement = spawnPoints[2].transform;
                                break;
                            }
                        case spawnPointNum.random:
                            {
                                int randomSpawn;
                                randomSpawn = Random.Range(0, spawnPoints.Count);
                                enemySpawnPlacement = spawnPoints[randomSpawn].transform;
                                break;
                            }
                        default:
                            {
                                //Just use the first one if we don't have anything assigned for some reason.
                                enemySpawnPlacement = spawnPoints[0].transform;
                                break;
                            }

                    }

                    GameObject enemyClone = Object_Pooler.Instance.SpawnFromPool(enemies[1].name, enemySpawnPlacement.position, enemySpawnPlacement.rotation);
                    break;
                }
            case 30:
                {
                    Transform enemySpawnPlacement;

                    currentSpawn = enemies[0].GetComponent<Obstacle_Behaviour>().spawnPoint;
                    //Add to this switch statement if we add more spawn points. May want to revise this to make it easier for futureproofing.
                    switch (currentSpawn)
                    {
                        case spawnPointNum.spawnPoint1:
                            {
                                //First spawn point.
                                enemySpawnPlacement = spawnPoints[0].transform;
                                break;
                            }
                        case spawnPointNum.spawnPoint2:
                            {
                                //Second spawn point.
                                enemySpawnPlacement = spawnPoints[1].transform;
                                break;
                            }
                        case spawnPointNum.spawnPoint3:
                            {
                                //Third spawn point.
                                enemySpawnPlacement = spawnPoints[2].transform;
                                break;
                            }
                        case spawnPointNum.random:
                            {
                                int randomSpawn;
                                randomSpawn = Random.Range(0, spawnPoints.Count);
                                enemySpawnPlacement = spawnPoints[randomSpawn].transform;
                                break;
                            }
                        default:
                            {
                                //Just use the first one if we don't have anything assigned for some reason.
                                enemySpawnPlacement = spawnPoints[0].transform;
                                break;
                            }

                    }

                    GameObject enemyClone = Object_Pooler.Instance.SpawnFromPool(enemies[0].name, enemySpawnPlacement.position, enemySpawnPlacement.rotation);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    //Gameover functions
    public void CallTransitionOut(int typeOfTransition)
    {
        StartCoroutine(transitionOut(typeOfTransition));
    }

    IEnumerator transitionOut(int num)
    {
        loadingIcon.SetActive(true);
        fadePanel.transform.parent.gameObject.SetActive(true);
        fadePanel.gameObject.SetActive(true);
        introTransitionFinished = false;
        thePlayer.setHealth(99);
        Time.timeScale = 1f;
        Level_Manager.Instance.player.SetActive(false);
        Audio_Manager.Instance.muteCurrentTrack();
        Audio_Manager.Instance.togglePauseSFX();
        if (num == 1)
        {
            Audio_Manager.Instance.playSFX(retryWarpSound);
        }
        else
        {
            Audio_Manager.Instance.playSFX(returnToLabSound);
        }

       // fadePanel.gameObject.SetActive(true);

        Vector3 currentScale = fadePanel.localScale;
        Vector3 decreaseScaleRate = new Vector3(1, 1, 1);

        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 150);
        Color32 endColor = new Color32(255, 255, 255, 255);
        Image fadePanelImg = fadePanel.GetComponent<Image>();

        rate = (1.0f / 2.5f) * 1.0f;


        while (i < 1.0f)
        {
            currentScale += decreaseScaleRate;
            // fadePanel.localScale = currentScale;
            i += Time.deltaTime * rate;

            fadePanel.localScale = Vector3.Lerp(fadePanel.localScale, new Vector3(1, 1, 1), (i));

            fadePanelImg.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;

            //  yield return new WaitForSeconds(0.001f);
        }

        if (num == 1)
        {
            Level_Manager.Instance.retryLevel();
        }
        else
        {
            Level_Manager.Instance.returnToMenu();
        }
    }
    //Gameover functions
}

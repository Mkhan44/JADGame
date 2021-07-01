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
    [SerializeField] int wavesSinceTimeSwap;

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

    [Tooltip("Prefab treasure chest that will spawn in.")]
    public GameObject chestPrefab;

    [Tooltip("Prefabs for different time periods.")]
    public List<GameObject> timePortalPrefabs;

    
    [SerializeField] List<GameObject> prehistoricBackgrounds;
    [SerializeField] List<GameObject> FeudalBackgrounds;
    [SerializeField] List<GameObject> WildWestBackgrounds;
    [SerializeField] List<GameObject> MedBackgrounds;
    [SerializeField] List<GameObject> FutureBackgrounds;

    [SerializeField] GameObject currentBG1;
    [SerializeField] GameObject currentBG2;

    public Player thePlayer;
    Player.playerState currentPlayerState;

    Coroutine spawnRoutine;
    Coroutine chestRoutine;
    Coroutine timeRoutine;

    bool specialWaveOn;

    bool stopCo;


    //Variance on waves related things.

    int wavesSinceDifficultyChange;

    public static Wave_Spawner Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        theLevelType = Level_Manager.Instance.getThisLevelType();
        currentPlayerState = thePlayer.GetState();
        waveType = typeOfWave.normal;
        waveComplete = true;
        waveCount = 1;
        wavesSinceBonus = 0;
        wavesSinceTimeSwap = 0;
        stopCo = false;
        theWaveDiff = waveDiff.easy;
       // Level_Manager.Instance = this.GetComponent<Level_Manager>();
        specialWaveOn = false;
        wavesSinceDifficultyChange = 0;
        enemiesLeft = enemyCount;
        SetCurrentEnemies();

        List<GameObject> BGsToLoad = prehistoricBackgrounds;
        //Setup the BGs, this will be based on the era from LevelManager!
        //Will also setup music tracks here.
        switch(Level_Manager.Instance.getTimePeriod())
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
                    BGsToLoad = prehistoricBackgrounds;
                    break;
                }
        }
        currentBG1 = Instantiate(BGsToLoad[0]);
        currentBG2 = Instantiate(BGsToLoad[0]);
        currentBG2.transform.position = new Vector3(BGsToLoad[0].transform.position.x + +16.8f, BGsToLoad[0].transform.position.y);


     //   Audio_Manager.Instance.setMusicTracks(Level_Manager.Instance.getTimePeriod());
    //    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);

    }

    // Update is called once per frame
    void Update()
    {
      //  currentPlayerState = thePlayer.GetState();

        if(waveType == typeOfWave.normal)
        {
            waveText.text = "Wave: " + waveCount.ToString();
        }
        else if(waveType == typeOfWave.bonus)
        {
            waveText.text = "Wave: Bonus!";
        }
        else if(waveType == typeOfWave.timeSwap)
        {
            waveText.text = "Wave: Timeswap!";
        }

        eraText.text = "Current era: " + Level_Manager.Instance.getTimePeriod().ToString();
        //Messy...Might want to reorganize.
        if(!Level_Manager.Instance.player.activeInHierarchy || Level_Manager.Instance.getThisLevelType() == Level_Manager.levelType.tutorial)
        {
            if(stopCo == false)
            {
                if(spawnRoutine != null)
                {
                    StopCoroutine(spawnRoutine);
                }
              
                stopCo = true;
            }
            
        }
        else
        {
            if (waveComplete == true)
            {
                stopCo = false;
                spawnRoutine = StartCoroutine(waveSpawner(waveType));
            }
        }

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
            //Update item cooldowns.
            Level_Manager.Instance.itemCDUpdate();

            //Test values for changing difficulty. Will need some formula later on.
            //if (wavesSinceDifficultyChange == 3)
           //DEBUG , USE THE ONE ABOVE FOR REAL!
            if (wavesSinceDifficultyChange == 2)
            {
                theWaveDiff = waveDiff.medium;
                setBGs(theWaveDiff);
                Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
                Debug.Log("The difficulty of the wave is: " + theWaveDiff);
            }
            if (wavesSinceDifficultyChange == 3)
            //if (wavesSinceDifficultyChange == 5)
            {
                theWaveDiff = waveDiff.hardPause;
                setBGs(theWaveDiff);
                Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
                Debug.Log("The difficulty of the wave is: " + theWaveDiff);
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
                                //Just check for not being hardPause because anything else is spawnable.
                                if (enemies[j].GetComponent<Obstacle_Behaviour>().thisObstacleDiff != Obstacle_Behaviour.obstacleDiff.hardPause)
                                {
                                    diffOptions.Add(enemies[j]);
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
                                //Just check for not being easy because anything else is spawnable.
                                if (enemies[j].GetComponent<Obstacle_Behaviour>().thisObstacleDiff != Obstacle_Behaviour.obstacleDiff.easy)
                                {
                                    diffOptions.Add(enemies[j]);
                                   // Debug.Log("Added enemy number: " + j + " from the list of enemies since it's not an easy enemy.");
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


                //Test for spawning in coins.
                int spawnCoinRnd = Random.Range(0, 500);
                if (spawnCoinRnd >= 250)
                {
                    //Debug.Log("We're spawning coins! Value of RNG was: "+ spawnCoinRnd.ToString());

                    spawnCoinRnd = Random.Range(0, 2);
                    int amountofCoinsToSpawn = Random.Range(1, 6);
                    StartCoroutine(CoinSpawn(spawnCoinRnd, amountofCoinsToSpawn));
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
            if(spawnRate > 1.5f)
            {
                spawnRate -= 0.5f;
            }
            
            enemyCount += 2;
            enemiesLeft = enemyCount;
            waveCount += 1;
            Level_Manager.Instance.setWavesSurvived((waveCount - 1));
            wavesSinceDifficultyChange += 1;
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
                int random1 = Random.Range(0, (timePortalPrefabs.Count));
                int random2 = Random.Range(0, (timePortalPrefabs.Count));
               //Make sure both are different.
                if(random1 == random2)
                {
                    if(random2 != 0)
                    {
                        random2 -= 1;
                    }
                    else if(random2 != timePortalPrefabs.Count)
                    {
                        random2 += 1;
                    }
                }
                //Debug for testing purposes.
                if(random1 == 2 || random1 == 4)
                {
                    random1 = 1;
                    random2 = 0;
                }
                else if(random2 == 2 || random2 == 4)
                {
                    random1 = 1;
                    random2 = 0;
                }
                //Debug for testing purposes.
                //random1 = 1;
                //random2 = 3;
                timeRoutine = StartCoroutine(SpawnTimePortal(random1, random2));
            }
            

        }

        
        //Bonus test.
        if(!specialWaveOn && waveType != typeOfWave.timeSwap)
        {   
            wavesSinceBonus++;
            Debug.Log("Waves since bonus is: " + wavesSinceBonus.ToString());
            if (wavesSinceBonus > 2)
            {
                
                int doWeBonus;
                doWeBonus = Random.Range(1, 7);
               //DEBUGGING.
               // doWeBonus = 8;
                if(doWeBonus >= 2)
                {
                    Debug.Log("Next wave is a bonus wave! RNG was: " + doWeBonus);
                    wavesSinceBonus = 0;
                    waveType = typeOfWave.bonus;
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, true);
                }

            }
        }
        


        //TimerPortal test.
        if (!specialWaveOn && waveType != typeOfWave.bonus)
        {
            wavesSinceTimeSwap++;
            Debug.Log("Waves since TimeSwap is: " + wavesSinceTimeSwap.ToString());
            if (wavesSinceTimeSwap > 0)
            {
                
                int doWeTimeSwap;
                doWeTimeSwap = Random.Range(1, 7);
                //DEBUGGING.
                doWeTimeSwap = 8;
                if (doWeTimeSwap >= 2)
                {
                    Debug.Log("Next wave is a timeswap wave! RNG was: " + doWeTimeSwap);
                    wavesSinceTimeSwap = 0;
                    waveType = typeOfWave.timeSwap;
                    Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, true);
                }

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

        //Spawn X amount of coins based on the RNG.
        //Debug.Log("Spawning in: " + amountToSpawn.ToString() + " coins!");
        for(int i = 0; i <= (amountToSpawn-1); i++)
        {
            if(rndSpawn == 0)
            {
                Object_Pooler.Instance.SpawnFromPool(coinPrefab.name, spawnPoints[1].transform.position,spawnPoints[1].transform.rotation);
            }
            else
            {
                Object_Pooler.Instance.SpawnFromPool(coinPrefab.name, spawnPoints[2].transform.position, spawnPoints[2].transform.rotation);
            }
            yield return new WaitForSeconds(0.2f);
           
        }
        
    }

    //For bonus wave.
    public IEnumerator chestSpawn()
    {

        GameObject chest1 = Instantiate(chestPrefab, spawnPoints[1].transform);
        GameObject chest2 = Instantiate(chestPrefab, spawnPoints[2].transform);
        //Flip chest that's spawning on the top.
        chest2.GetComponent<SpriteRenderer>().flipY = true;
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
            chest1.GetComponent<Animator>().SetBool("Chest_Open", true);
        }
        else
        {
            chest2.GetComponent<Animator>().SetBool("Chest_Open", true);
        }

        //Basic randomization for prizes. This will have to be changed and have different stipulations based on waves passed, and other stuff etc.
        int randPrizeNum = 0;
        randPrizeNum = Random.Range(1, 3);
        switch(randPrizeNum)
        {
            case 1:
                {
                    Level_Manager.Instance.collectCoin(50);
                    break;
                }
            case 2:
                {
                    Level_Manager.Instance.collectCoin(20);
                    break;
                }
        }

        yield return new WaitForSeconds(1.0f);

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
        waveComplete = true;


    }
    //Spawn in 2 time era portals at random. Same concept as chest spawn.
    public IEnumerator SpawnTimePortal(int portalIndex1, int portalIndex2)
    {
        GameObject portal1 = Instantiate(timePortalPrefabs[portalIndex1], spawnPoints[1].transform);
        GameObject portal2 = Instantiate(timePortalPrefabs[portalIndex2], spawnPoints[2].transform);


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
        Audio_Manager.Instance.setMusicTracks(Level_Manager.Instance.getTimePeriod());
        //Audio_Manager.Instance.changeMusicDifficulty(theWaveDiff, false);
        setBGs(theWaveDiff);


        waveComplete = true;
    }



    //Getters/setters

    public typeOfWave getWaveType()
    {
        return waveType;
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
        List<GameObject> BGsToLoad = FeudalBackgrounds;

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
        //DEBUG, GOTTA MAKE SURE THAT WE DON'T NEED THIS 0.2F INCREASE IN THE FUTURE!
        currentBG1.transform.position = new Vector2(currentBG1.transform.position.x, currentBG1.transform.position.y);
        currentBG2.transform.position = new Vector2(currentBG1.transform.position.x + +16.8f, currentBG1.transform.position.y);
    }

    public void updateEnemiesLeft(int num)
    {
        enemiesLeft -= num;
        Debug.Log("Enemies left are: " + enemiesLeft);
    }

    //Getters/Setters


    //Respawn from LevelManager function
    public void respawnPlayer()
    {
        stopCo = false;
        spawnRoutine = StartCoroutine(waveSpawner(waveType));
    }

    //Tutorial functions.

    public void tutorialSpawn()
    {
        switch(Tutorial_Manager.Instance.getCurrentStep())
        {
            //Spawn boulder.
            case 12:
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
            case 14:
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
            case 35:
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
}

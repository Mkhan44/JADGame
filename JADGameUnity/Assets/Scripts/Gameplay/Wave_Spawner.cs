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

    Level_Manager levMan;

    [Tooltip("The difficulty of the wave. Should be changeable on the fly.")]
    waveDiff theWaveDiff;
    
    [Tooltip("The type of wave. We spawn on chests on bonus, portals on timeWarp")]
    typeOfWave waveType;

    [Tooltip("Display for what wave we're on.")]
    public TextMeshProUGUI waveText;

    [SerializeField]
    int waveCount;
    int wavesSinceBonus;
    int wavesSinceTimeSwap;

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

    [Tooltip("Prefab of a coin that we will be spawning in.")]
    public GameObject coinPrefab;

    [Tooltip("Prefab treasure chest that will spawn in.")]
    public GameObject chestPrefab;

    public Player thePlayer;
    Player.playerState currentPlayerState;

    Coroutine spawnRoutine;
    Coroutine chestRoutine;

    bool bonusTest;

    bool stopCo;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = thePlayer.GetState();
        waveType = typeOfWave.normal;
        waveComplete = true;
        waveCount = 1;
        wavesSinceBonus = 0;
        wavesSinceTimeSwap = 0;
        stopCo = false;
        theWaveDiff = waveDiff.easy;
        levMan = this.GetComponent<Level_Manager>();
        bonusTest = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerState = thePlayer.GetState();

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


        //Messy...Might want to reorganize.
        if(currentPlayerState == Player.playerState.dead)
        {
            if(stopCo == false)
            {
                StopCoroutine(spawnRoutine);
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
            if (waveCount == 3)
            {
                theWaveDiff = waveDiff.medium;
                Debug.Log("The difficulty of the wave is: " + theWaveDiff);
            }
            if (waveCount == 5)
            {
                theWaveDiff = waveDiff.hardPause;
                Debug.Log("The difficulty of the wave is: " + theWaveDiff);
            }

            for (int i = 0; i < enemyCount; i++)
            {
                //In this case the range can be 0 , or the exact count because randomize needs to be 1 above whatever you want. EX: List has 2 items, count = 2, but only 2 index...So 2 won't ever be called.
                //randNum = Random.Range(minNum, maxNum);
                
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
                GameObject enemyClone = Instantiate(diffOptions[randNum], enemySpawnPlacement);
                //Reset the list.
                diffOptions.Clear();


                //Test for spawning in coins.
                int spawnCoinRnd = Random.Range(0, 500);
                if (spawnCoinRnd >= 250)
                {
                    Debug.Log("We're spawning coins! Value of RNG was: "+ spawnCoinRnd.ToString());

                    spawnCoinRnd = Random.Range(0, 2);
                    StartCoroutine(waitSpawn(spawnCoinRnd));
                }


                yield return new WaitForSeconds(spawnRate);
            }


            //Only increase spawnrate , enemy count and wavecount after normal waves. Though we may need a hidden waveCount counter for achievements, etc.
            spawnRate -= 0.5f;
            enemyCount += 2;
            waveCount += 1;
        }
        else if(theWaveType == typeOfWave.bonus)
        {
            //Insert treasure chest spawn here. Will need some way to calculate the treasure box items as well.

            //Spawn in chests, give player chance to jump or duck, play animation, give item.
            //Chests depsawn, wave over.

            if (!bonusTest)
            {
                bonusTest = true;
                chestRoutine = StartCoroutine(chestSpawn());
            }
        
        }
        else if(theWaveType == typeOfWave.timeSwap)
        {
            //Insert timeportal spawn here. Will need another list of diff time portals.
            
        }


        //Test.
        /*
        if(waveCount == 3)
        {
            theWaveDiff = waveDiff.medium;
            Debug.Log("The difficulty of the wave is: " + theWaveDiff);
        }
        if(waveCount == 5)
        {
            theWaveDiff = waveDiff.hardPause;
            Debug.Log("The difficulty of the wave is: " + theWaveDiff);
        }
        */

        //Bonus test.
        if(!bonusTest)
        {
            if (waveCount == 1)
            {
                wavesSinceBonus++;
            }
            wavesSinceBonus++;
            Debug.Log("Waves since bonus is: " + wavesSinceBonus.ToString());
            if (wavesSinceBonus == 1)
            {
                /*
                int doWeBonus;
                doWeBonus = Random.Range(1, 5);
                if(doWeBonus >= 3)
                {
                    Debug.Log("Next wave is a bonus wave! RNG was: " + doWeBonus);
                    wavesSinceBonus = 0;
                    waveType = typeOfWave.bonus;
                }
                */
                waveType = typeOfWave.bonus;

                //MAKE THIS ACTIVE WHEN WE ARE REALLY TESTING OTHERWISE BONUS WILL ONLY COME ONCE!!!
                //wavesSinceBonus = 0;
            }
        }
       

     
        if(waveType == typeOfWave.normal)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
        }
  

        waveComplete = true;
    }

    //Test coRoutine for spawning coins.
    public IEnumerator waitSpawn(int rndSpawn)
    {
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i <= 4; i++)
        {
            if(rndSpawn == 0)
            {
                Instantiate(coinPrefab, spawnPoints[1].transform);
            }
            else
            {
                Instantiate(coinPrefab, spawnPoints[2].transform);
            }
            yield return new WaitForSeconds(0.2f);
           
        }
        
    }

    //For bonus wave.
    public IEnumerator chestSpawn()
    {
       // yield return new WaitForSeconds(0.2f);

        GameObject chest1 = Instantiate(chestPrefab, spawnPoints[1].transform);
        GameObject chest2 = Instantiate(chestPrefab, spawnPoints[2].transform);
        //Flip chest that's spawning on the top.
        chest2.GetComponent<SpriteRenderer>().flipY = true;
        int theSelection = levMan.getChestSelect();

        StartCoroutine(levMan.pickAChest());
        levMan.duckButton.gameObject.SetActive(false);
        levMan.jumpButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        //Play animation for "Jump or Duck" option, then make the buttons available.

        levMan.duckButton.gameObject.SetActive(true);
        levMan.jumpButton.gameObject.SetActive(true);
        Debug.Log("Waiting for response from the player...");

        while (theSelection == 0)
        {
            theSelection = levMan.getChestSelect();
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
                    levMan.collectCoin(50);
                    break;
                }
            case 2:
                {
                    levMan.collectCoin(20);
                    break;
                }
        }

        yield return new WaitForSeconds(1.0f);

        //Use obejct pooling instead.
        Destroy(chest1);
        Destroy(chest2);

        //We finished the bonus wave.
        levMan.duckButton.gameObject.SetActive(true);
        levMan.jumpButton.gameObject.SetActive(true);
        waveType = typeOfWave.normal;
        bonusTest = false;
        levMan.setChestSelect(0);


    }

    //TBA.
    //Call this function from LevelManager.
    //When swapping time periods, new lists of enemies will need to be updated.
    public void changeEnemyList()
    {
        
    }
    //Use this function for when we swap time periods.
    public void timeSwap()
    {

    }

    //Getters/setters

    public typeOfWave getWaveType()
    {
        return waveType;
    }


    //Getters/Setters
}

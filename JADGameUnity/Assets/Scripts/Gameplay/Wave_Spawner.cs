using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Wave_Spawner : MonoBehaviour
{
    public enum typeOfWave
    {
        normal,
        bonus,
        timeSwap
    }

    [Tooltip("The type of wave. We spawn on chests on bonus, portals on timeWarp")]
    typeOfWave waveType;

    [Tooltip("What wave we're on.")]
    public TextMeshProUGUI waveText;

    [SerializeField]
    int waveCount = 0;

    [Tooltip("Rate that enemies will spawn in at. This should decrease with each wave passed.")]
    public float spawnRate;
    [Tooltip("The time between waves. Probably want this to be constant.")]
    public float timeBetweenWaves;

    bool waveComplete;

    public List<GameObject> enemies;

    [Tooltip("How many enemies we want to spawn during this wave.")]
    public int enemyCount;

    public Player thePlayer;
    Player.playerState currentPlayerState;

    Coroutine spawnRoutine;

    bool stopCo;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayerState = thePlayer.GetState();
        waveType = typeOfWave.normal;
        waveComplete = true;
        waveCount = 1;
        stopCo = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerState = thePlayer.GetState();

        waveText.text = "Wave: " + waveCount.ToString();


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


        waveComplete = false;

        //Regular spawn.
        if(theWaveType == typeOfWave.normal)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                //In this case the range can be 0 , or the exact count because randomize needs to be 1 above whatever you want. EX: List has 2 items, count = 2, but only 2 index...So 2 won't ever be called.
                randNum = Random.Range(minNum, maxNum);

                Debug.Log("We picked enemy number: " + randNum.ToString());
                GameObject enemyClone = Instantiate(enemies[randNum]);

                yield return new WaitForSeconds(spawnRate);
            }
        }
        else if(theWaveType == typeOfWave.bonus)
        {
            //Insert treasure chest spawn here. Will need some way to calculate the treasure box items as well.
        }
        else if(theWaveType == typeOfWave.timeSwap)
        {
            //Insert timeportal spawn here. Will need another list of diff time portals.
        }
       


        //Need some logic on increasing this stuff.
        spawnRate -= 0.1f;
        enemyCount += 3;
        waveCount += 1;

        yield return new WaitForSeconds(timeBetweenWaves);
        waveComplete = true;
    }

    //TBA.
    //Call this function from LevelManager.
    //When swapping time periods, new lists of enemies will need to be updated.
    public void changeEnemyList()
    {
        
    }
}

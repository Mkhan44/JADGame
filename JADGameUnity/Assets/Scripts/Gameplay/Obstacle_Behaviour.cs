//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour , IPooled_Object
{
    Vector2 startPos;
    Vector2 endPos;
    //Speed
    [Tooltip("Speed of the obstacle moving from right to left.")]
    public float speed = 0f;

    float ogSpeed;

    //We will have spawn points set up in the Wave_Spawner script.
    [Tooltip("Spawnpoint1 = Top, Spawnpoint2 = Bottom, Spawnpoint3 = hanging from ceiling")]
    public Wave_Spawner.spawnPointNum spawnPoint;
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
        bonus
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
    ElementType objectElement;

    public obstacleDiff thisObstacleDiff;

    [SerializeField]
    typeOfObstacle thisType;

    private void Awake()
    {
       
        ogSpeed = speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
       
        endPos = new Vector2((startPos.x - 20), (startPos.y));
        Debug.Log("startPos is: " + startPos + " And endPos is: " + endPos);
    }

    //'start' method whenever this is reused via Pooling.
    public void OnObjectSpawn()
    {
        speed = ogSpeed;
        startPos = this.transform.position;
        endPos = new Vector2((startPos.x - 20), (startPos.y));
        Debug.Log("startPos is: " + startPos + " And endPos is: " + endPos);
    }
  

    // Update is called once per frame
    void Update()
    {
        //If it's anything that's not a chest.
        if(thisType != typeOfObstacle.chest)
        {
            speed += 0.15f * Time.deltaTime;

            this.transform.position = Vector2.Lerp(startPos, endPos, speed);

            if (this.transform.position.x == endPos.x)
            {
                //TURN ON WHEN WE ARE READY TO POOL
                Object_Pooler.Instance.AddToPool(gameObject);
                //Destroy(gameObject);
                return;
            }
        }
        //If it's a chest.
        else
        {
            endPos = new Vector2((startPos.x - 5), (startPos.y));

            speed += 1.0f * Time.deltaTime;
            this.transform.position = Vector2.Lerp(startPos, endPos, speed);
        }
        
    }

    //Get the element from another script if needed.
    public ElementType getElement()
    {
        return objectElement;
    }
}

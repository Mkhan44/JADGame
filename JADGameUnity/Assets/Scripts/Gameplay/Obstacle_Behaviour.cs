//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Behaviour : MonoBehaviour
{
    Vector2 startPos;
    Vector2 endPos;
    //Speed
    [Tooltip("Speed of the obstacle moving from right to left.")]
    public float speed = 0f;

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
        hardPause
    }

    public bool isCoin;

    [SerializeField]
    ElementType objectElement;

    public obstacleDiff thisObstacleDiff;

    private void Awake()
    {
        startPos = this.transform.position;
        endPos = new Vector2((startPos.x - 50), (startPos.y));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed += 0.05f * Time.deltaTime;

        this.transform.position = Vector2.Lerp(startPos, endPos, speed);

        if(this.transform.position.x == endPos.x)
        {
            Destroy(gameObject);
            return;
        }
    }

    //Get the element from another script if needed.
    public ElementType getElement()
    {
        return objectElement;
    }
}

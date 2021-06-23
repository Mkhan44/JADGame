//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Dynamite_Cart : Obstacle_Behaviour
{
    protected override void Awake()
    {
        base.Awake();

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

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
    }

    protected override void Movement()
    {
        base.Movement();
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner" || collision.gameObject.tag == "Player")
        {


        }
        base.OnCollisionEnter2D(collision);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (thisType == typeOfObstacle.obstacle)
        {
            if (collision.gameObject.tag == "Player_Vicinity_Blocker")
            {
                int randNum = Random.Range(0, 2);

                if(randNum == 0)
                {
                    Debug.Log("Explode!");
                }
                else
                {
                    Debug.Log("No explode.");
                }

                inPlayerVicinity = true;

            }

            if (collision.gameObject.tag == "On_screen_Vicinity_Collider")
            {
                onScreenIndicator = true;
                Debug.Log(gameObject.name + " Is now on screen!");
            }

                //In here we will make whatever arrow it is highlighted and then begin the coroutine to fade it out.
                if (collision.gameObject.tag == "Indicator_Vicinity_Collider")
                {
                    inIndicatorVicinity = true;
                    //    Debug.Log("Obstacle has been spawned, trigger indicator at spawnpoint: " + spawnPoint.ToString());
                    Level_Manager.Instance.indicatorArrow(spawnPoint);
                }
       
        }

    }

    protected override void OnTriggerExit2D(Collider2D collision)
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

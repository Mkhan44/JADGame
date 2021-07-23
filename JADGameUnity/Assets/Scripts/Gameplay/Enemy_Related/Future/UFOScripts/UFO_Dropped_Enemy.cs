//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO_Dropped_Enemy : MonoBehaviour
{
    float increaseRate;
    float maxSpeed;
    float speed;
    UFO ufoParentScript;
    GameObject ufoParent;
    Rigidbody2D thisRigid;

    bool onGround;

    private void Awake()
    {
        onGround = false;
    }

    private void Update()
    {
        if(onGround)
        {
            Movement();
        }
    }

    void Movement()
    {
        if (increaseRate != 0f)
        {
            if (speed < maxSpeed)
            {
                speed += increaseRate * Time.deltaTime;
            }
            else
            {
                speed = maxSpeed;
            }
        }
        else
        {
            if (speed < maxSpeed)
            {
                speed += 0.15f * Time.deltaTime;
            }
            else
            {
                speed = maxSpeed;
            }

        }

        if (thisRigid != null)
        {
            thisRigid.velocity = Vector2.left * speed;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Despawner")
        {
            Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            if (ufoParentScript.getScoreVal() <= 0)
            {
                Level_Manager.Instance.updateScore(100);
            }
            else
            {
                Level_Manager.Instance.updateScore(ufoParentScript.getScoreVal());
            }

            //TURN ON WHEN WE ARE READY TO POOL
            thisRigid.velocity = Vector2.zero;
            Object_Pooler.Instance.AddToPool(ufoParent.gameObject);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }

        if (collision.gameObject.tag == "Player")
        {
            //Wave_Spawner.Instance.updateEnemiesLeft(1);
            Level_Manager.Instance.increaseEnemiesDodged();
            Level_Manager.Instance.Damage();
            Object_Pooler.Instance.AddToPool(ufoParent.gameObject);
            Destroy(gameObject);
        }
    }

}

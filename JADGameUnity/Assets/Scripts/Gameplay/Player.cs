//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use to house the variables related to the player.
public class Player : MonoBehaviour
{
    int currentHealth;
    int maxHealth;

    Vector2 initialPos;
    public Level_Manager levMan;

    public enum playerState
    {
        idle,
        jumping,
        ducking,
        hanging,
        burning,
        frozen,
        dead
    }

    public enum powerUp
    {
        //will be filled in later...
    }

    playerState currentState;

    [Tooltip("The gravity for the player. This determines how fast the player will fall after jumping.")]
    [SerializeField]
    float gravityScale;
    [Tooltip("The velocity that will be applied when the player jumps. Higher # = higher jump height.")]
    [SerializeField]
    float jumpHeight;
    [Tooltip("Amount that the heat meter will fill. Will be changed via items and external factors.")]
    [SerializeField]
    float heatMeterFillVal;
    [Tooltip("Amount that the ice meter will fill. Will be changed via items and external factors.")]
    [SerializeField]
    float iceMeterFillVal;





    private void Awake()
    {
        currentState = playerState.idle;
        initialPos = gameObject.transform.position;


    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public playerState GetState()
    {
        return currentState;
    }

    public void setState(playerState setter)
    {
        currentState = setter;
    }

    public void setHealth(int incomingHealth)
    {
        //Use this to take damage
        currentHealth = incomingHealth;
    }

    //Make damage + heal functions here if needed.

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public Vector2 getInitialPos()
    {
        initialPos = gameObject.transform.position;

        return initialPos;
    }


    public float getGravity()
    {
        return gravityScale;
    }
    public float getJumpHeight()
    {
        return jumpHeight;
    }

    public float getHeatMeterFill()
    {
        return heatMeterFillVal;
    }

    public float getIceMeterFill()
    {
        return iceMeterFillVal;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("We collided with something");
        //If the tag is 'obstacle'  , check if we're invincible, if not then go to hurt mode and call damage from levelmanager.
        if (other.gameObject.tag == "Obstacle")
        {
            // Debug.Log("Obstacle touched player!");
            Debug.Log("The element of this obstacle is: " + other.gameObject.GetComponent<Obstacle_Behaviour>().getElement());
            switch (other.gameObject.GetComponent<Obstacle_Behaviour>().getElement())
            {
                case Obstacle_Behaviour.ElementType.fire:
                    {
                        levMan.temperatureMetersManager(Obstacle_Behaviour.ElementType.fire);
                        break;
                    }
                case Obstacle_Behaviour.ElementType.ice:
                    {
                        levMan.temperatureMetersManager(Obstacle_Behaviour.ElementType.ice);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            levMan.Damage();
        }
    }
}

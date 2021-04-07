//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_Interact : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    public enum buttonType
    {
        jump,
        duck,
        coolDown,
        heatUp
    }

    public buttonType typeOfButton;

    float timeHeld;

    public bool isHeld;

    Coroutine heldRoutine;

    // Start is called before the first frame update
    void Start()
    {
        timeHeld = 0f;
        isHeld = false;
        switch (typeOfButton)
        {
            case buttonType.duck:
                {
                    //Assign if needed.
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Player.playerState checkState;
        //checkState = levelMan.player.GetComponent<Player>().GetState();

        if (Level_Manager.Instance.player.activeInHierarchy || Level_Manager.Instance.meterFilled)
        {
            if (Input.GetKey("up") && (Level_Manager.Instance.player.GetComponent<Player>().GetState() != Player.playerState.ducking))
            {
                if (typeOfButton == buttonType.jump)
                {
                    if (Level_Manager.Instance.jumpButton.interactable != false)
                    {
                        //Assign if needed.
                        Level_Manager.Instance.Jump();
                    }
                }
                //    Debug.Log("up arrow key is held down");
            }

            if (Input.GetKey("down") && (Level_Manager.Instance.player.GetComponent<Player>().GetState() != Player.playerState.jumping))
            {
                if (typeOfButton == buttonType.duck)
                {
                    //Assign if needed.
                    Level_Manager.Instance.duck();
                }
                //   Debug.Log("down arrow key is held down");
            }
        }

        
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        Player.playerState checkState;
        checkState = Level_Manager.Instance.player.GetComponent<Player>().GetState();

        if (Level_Manager.Instance.player.activeInHierarchy || !Level_Manager.Instance.meterFilled)
        {
            if(heldRoutine == null && gameObject.GetComponent<Button>().IsInteractable())
            {
                heldRoutine = StartCoroutine(heldButtonTimer());
               // Debug.Log("Started heldButtonTimer coroutine!");
            }


            switch (typeOfButton)
            {
                case buttonType.duck:
                    {
                        if(Level_Manager.Instance.duckButton.interactable != false && checkState != Player.playerState.jumping && checkState != Player.playerState.hanging)
                        {
                            Level_Manager.Instance.duck(); 
                        }
                        break;

                    }
                case buttonType.jump:
                    {
                        if (Level_Manager.Instance.jumpButton.interactable != false && checkState != Player.playerState.ducking && checkState != Player.playerState.hanging)
                        {
                            Level_Manager.Instance.Jump();
                        }
                        break;
                    }
                    //Could make decreaseMeter values different based on modifiers if we want.
                case buttonType.coolDown:
                    {
                        Level_Manager.Instance.heatMeter.decreaseMeter(5.0f);
                        break;
                    }
                case buttonType.heatUp:
                    {
                        Level_Manager.Instance.iceMeter.decreaseMeter(5.0f);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Need some type of timer here...Reset it here and turn isHeld to false.
        if(heldRoutine != null)
        {
            StopCoroutine(heldRoutine);
            heldRoutine = null;
        }
        timeHeld = 0f;
        isHeld = false;
    }


    //Tracks how long the player has been holding down the button.
    public IEnumerator heldButtonTimer()
    {
        isHeld = true;

        while (isHeld)
        {
            timeHeld += Time.deltaTime;
            yield return null;
        }
    }

    public bool getHeldValue()
    {
        return isHeld;
    }

    public float getTimeHeld()
    {
        return timeHeld;
    }

    public void OnEnable()
    {
        timeHeld = 0f;
        isHeld = false;
    }

    void OnDisable()
    {

    }

}

//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Interact : MonoBehaviour , IPointerDownHandler
{
    public enum buttonType
    {
        jump,
        duck,
        coolDown,
        heatUp
    }

    public buttonType typeOfButton;

    // Start is called before the first frame update
    void Start()
    {
        switch(typeOfButton)
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


}

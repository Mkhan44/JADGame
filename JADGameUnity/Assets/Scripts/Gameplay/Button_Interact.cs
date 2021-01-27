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

    public Level_Manager levelMan;
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

        if (levelMan.player.activeInHierarchy || !levelMan.meterFilled)
        {
            if (Input.GetKey("up") && (levelMan.player.GetComponent<Player>().GetState() != Player.playerState.ducking))
            {
                if (typeOfButton == buttonType.jump)
                {
                    if (levelMan.jumpButton.interactable != false)
                    {
                        //Assign if needed.
                        levelMan.Jump();
                    }
                }
                //    Debug.Log("up arrow key is held down");
            }

            if (Input.GetKey("down") && (levelMan.player.GetComponent<Player>().GetState() != Player.playerState.jumping))
            {
                if (typeOfButton == buttonType.duck)
                {
                    //Assign if needed.
                    levelMan.duck();
                }
                //   Debug.Log("down arrow key is held down");
            }
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        Player.playerState checkState;
        checkState = levelMan.player.GetComponent<Player>().GetState();

        if (levelMan.player.activeInHierarchy || !levelMan.meterFilled)
        {
            switch (typeOfButton)
            {
                case buttonType.duck:
                    {
                        if(levelMan.duckButton.interactable != false && checkState != Player.playerState.jumping && checkState != Player.playerState.hanging)
                        {
                            levelMan.duck(); 
                        }
                        break;

                    }
                case buttonType.jump:
                    {
                        if (levelMan.jumpButton.interactable != false && checkState != Player.playerState.ducking && checkState != Player.playerState.hanging)
                        {
                            levelMan.Jump();
                        }
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

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

    public Sprite disabledImg;
    public Sprite pressedImg;

    public SpriteState theSpriteState;

    [Header("Sounds")]
    [SerializeField] AudioClip burningMashSound;
    [SerializeField] AudioClip frozenMashSound;

    // Start is called before the first frame update
    void Start()
    {
        theSpriteState = this.GetComponent<Button>().spriteState;

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
                        if(Level_Manager.Instance.duckButton.interactable != false && Level_Manager.Instance.duckButton.enabled != false && checkState != Player.playerState.jumping && checkState != Player.playerState.hanging)
                        {
                            Level_Manager.Instance.duck();
                            //Debug.Log("Duck button was pressed!");
                        }
                        break;

                    }
                case buttonType.jump:
                    {
                        if (Level_Manager.Instance.jumpButton.interactable != false && Level_Manager.Instance.jumpButton.enabled != false && checkState != Player.playerState.ducking && checkState != Player.playerState.hanging)
                        {
                            Level_Manager.Instance.Jump();
                        }
                        break;
                    }
                    //Could make decreaseMeter values different based on modifiers if we want.
                case buttonType.coolDown:
                    {
                        Level_Manager.Instance.heatMeter.decreaseMeter(5.0f);
                        Audio_Manager.Instance.playSFX(burningMashSound, false, 0.05f);
                        break;
                    }
                case buttonType.heatUp:
                    {
                        Level_Manager.Instance.iceMeter.decreaseMeter(5.0f);
                        Audio_Manager.Instance.playSFX(frozenMashSound);
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
        resetHeldValue();
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

    public void resetHeldValue()
    {
        if (heldRoutine != null)
        {
            StopCoroutine(heldRoutine);
            heldRoutine = null;
        }
        timeHeld = 0f;
        isHeld = false;
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

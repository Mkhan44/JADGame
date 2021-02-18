//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour , IPointerDownHandler
{
    public enum itemType
    {
        FireVest,
        HandWarmers,
        Clock,
        Defroster,
        LiquidNitrogen,
        //debug
        none
    }

    public itemType thisItemType;

    bool hasBeenUsed;

    [Tooltip("If the item has a duration till it runs out, edit this value in seconds. Otherwise keep it at 0.")]
    public float itemDuration;

    private void Awake()
    {
        //There should only be 1 of these.
        GameObject tempLevMan = GameObject.Find("Level_Manager"); 

        if(tempLevMan != null)
        {
            Level_Manager.Instance = tempLevMan.GetComponent<Level_Manager>();
        }
        else
        {
            Debug.LogWarning("Couldn't find the level manager via: " + gameObject.name + " !");
        }

        hasBeenUsed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Activate power up.

        //If player is not powered up already, and this object isn't interactable (opactiy not at 100%) we can activate the power up.
        if(!hasBeenUsed)
        {
            if (!Level_Manager.Instance.thePlayer.isPoweredUp)
            {
                if(itemDuration == 0)
                {
                    //We know this is an item that is used once and it's over. No duration.

                    switch (thisItemType)
                    {
                        case itemType.Defroster:
                            {
                                if (Level_Manager.Instance.thePlayer.GetState() == Player.playerState.frozen && Level_Manager.Instance.thePlayer.GetState() != Player.playerState.dead)
                                {
                                    Level_Manager.Instance.iceMeter.setMeterValExternally(0);
                                    //Item has been used!
                                    disableItem();
                                }
                                else
                                {
                                    //Play error SFX when we implement sound.
                                  //  Debug.Log("Can't use the item, player isn't frozen!");
                                }

                                break;
                            }
                    }


                }
                else
                {
                    if(Level_Manager.Instance.thePlayer.GetState() != Player.playerState.burning && Level_Manager.Instance.thePlayer.GetState() != Player.playerState.frozen && Level_Manager.Instance.thePlayer.GetState() != Player.playerState.dead)
                    {
                        //Go through the check for which item with a duration it is.
                        switch (thisItemType)
                        {
                            case itemType.FireVest:
                                {
                                    Level_Manager.Instance.setCurrentItem(thisItemType, itemDuration);
                                    //  fireVest();
                                    break;
                                }
                            case itemType.HandWarmers:
                                {
                                    Level_Manager.Instance.setCurrentItem(thisItemType, itemDuration);
                                    break;
                                }
                            default:
                                {
                                    break;
                                }

                        }
                        //Item has been used!
                        disableItem();
                    }
                    
                    else
                    {
                       // Debug.Log("Can't use items, player is burning/frozen/dead!");
                    }
                    
                }
                
            }
            else
            {
               // Debug.Log("Hey you can't use this power up right now!");
            }
        }
        else
        {
           // Debug.Log("Item has already been used!");
        }
       
    }


    /*
     * ***************************************************************
     *  ITEM FUNCTIONS.
     * ***************************************************************
     */


    /*
     *****************************************************************
     * Items WITHOUT duration (one time use).
     ******************************************************************
     */


    /*
   *****************************************************************
   * Items WITHOUT duration (one time use).
   ******************************************************************
   */


    /*
     *****************************************************************
     * Items WITH duration.
     * call setCurrentItem() from levelmanager.
     ******************************************************************
     */



    /*
     *****************************************************************
     * Items WITH duration.
     * call setCurrentItem() from levelmanager.
     ******************************************************************
     */

    /*
     * ***************************************************************
     *  ITEM FUNCTIONS.
     * 
     * ***************************************************************
     */


    //Once we use the item don't let it be used again.
    void disableItem()
    {
        hasBeenUsed = true;
        gameObject.SetActive(false);
    }


}

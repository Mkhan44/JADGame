//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
public class Item : MonoBehaviour , IPointerDownHandler
{

    public Collect_Manager.typeOfItem thisItemType;

    bool hasBeenUsed;
    bool inCooldown;

    [SerializeField] GameObject CDPanel;
    [SerializeField] TextMeshProUGUI CDText;

    [Tooltip("If the item has a duration till it runs out, edit this value in seconds. Otherwise keep it at 0.")]
    public float itemDuration;

    [Tooltip("How many waves to wait for cooldown. If 0 , item is gone after one use.")]
    public int waveCooldownTime;

    [SerializeField] int tempCoolDownCounter;
    [SerializeField] int wavesTillCDDone;
    [SerializeField] int numTimesUsed;

    [SerializeField] public AudioClip audioToPlay;

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
        inCooldown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Activate power up.

        //If player is not powered up already, and this object isn't interactable (opactiy not at 100%) we can activate the power up.
        if(!hasBeenUsed && this.thisItemType != Collect_Manager.typeOfItem.none)
        {
            if (!Level_Manager.Instance.thePlayer.isPoweredUp)
            {
                //We know this is an item that is used once and it's over. No duration.
                if (itemDuration == 0)
                {
                   

                    switch (thisItemType)
                    {
                        case Collect_Manager.typeOfItem.Defroster:
                            {
                                if (Level_Manager.Instance.thePlayer.GetState() == Player.playerState.frozen && Level_Manager.Instance.thePlayer.GetState() != Player.playerState.dead)
                                {
                                    if (Level_Manager.Instance.getThisLevelType() == Level_Manager.levelType.tutorial)
                                    {
                                        if (Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.useDefroster)
                                        {
                                            Tutorial_Manager.Instance.conditionComplete();
                                        }
                                    }

                                    Level_Manager.Instance.iceMeter.setMeterValExternally(0);
                                    //Item has been used!
                                    disableItem();
                                }
                                else
                                {
                                    //Play error SFX when we implement sound.
                                    Debug.Log("Can't use the item, player isn't frozen!");
                                }

                                break;
                            }
                        case Collect_Manager.typeOfItem.LiquidNitrogenCanister:
                            {
                                if (Level_Manager.Instance.thePlayer.GetState() == Player.playerState.burning && Level_Manager.Instance.thePlayer.GetState() != Player.playerState.dead)
                                {
                                    Level_Manager.Instance.heatMeter.setMeterValExternally(0);
                                    //Item has been used!
                                    disableItem();
                                }
                                else
                                {
                                    //Play error SFX when we implement sound.
                                     Debug.Log("Can't use the item, player isn't burning!");
                                }

                                break;
                            }
                    }


                }
                //Items with duration fall under this else.
                else
                {
                    if(Level_Manager.Instance.thePlayer.GetState() != Player.playerState.burning && Level_Manager.Instance.thePlayer.GetState() != Player.playerState.frozen && Level_Manager.Instance.thePlayer.GetState() != Player.playerState.dead)
                    {
                        //Go through the check for which item with a duration it is.
                        switch (thisItemType)
                        {
                            case Collect_Manager.typeOfItem.FireVest:
                                {
                                    Level_Manager.Instance.setCurrentItem(thisItemType, itemDuration);
                                    //  fireVest();
                                    break;
                                }
                            case Collect_Manager.typeOfItem.HandWarmer:
                                {
                                    if(Level_Manager.Instance.getThisLevelType() == Level_Manager.levelType.tutorial)
                                    {
                                        if(Tutorial_Manager.Instance.getStepType() == Tutorial_Step.stepType.useHandwarmer)
                                        {
                                            Tutorial_Manager.Instance.conditionComplete();
                                        }
                                    }
                                    Level_Manager.Instance.setCurrentItem(thisItemType, itemDuration);
                                    break;
                                }
                            case Collect_Manager.typeOfItem.NeutralTablet:
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
        if(audioToPlay != null)
        {
            Audio_Manager.Instance.playSFX(audioToPlay,false,0.2f);
        }
        hasBeenUsed = true;
        if(waveCooldownTime > 0)
        {
            inCooldown = true;
            CDPanel.SetActive(true);
            //If this is the first time you've used the item.
            if (numTimesUsed == 0)
            {
                tempCoolDownCounter = waveCooldownTime;
            }

            //Otherwise...
            else
            {
                tempCoolDownCounter = waveCooldownTime+numTimesUsed;
           
            }
            numTimesUsed += 1;
            wavesTillCDDone = tempCoolDownCounter;
            CDText.text = wavesTillCDDone.ToString();
            
        }
        else
        {
            CDPanel.SetActive(true);
            CDText.text = "No Use!";
            gameObject.SetActive(false);
        }
        
    }

    public void updateCDTime()
    {
        wavesTillCDDone -= 1;
        CDText.text = wavesTillCDDone.ToString();
        if(wavesTillCDDone == 0)
        {
            inCooldown = false;
            hasBeenUsed = false;
            disableCDPanel();
        }
    }

    public int getCDTime()
    {
        return tempCoolDownCounter;
    }

    public bool cooldownStatus()
    {
        return inCooldown;
    }

    void disableCDPanel()
    {
        CDPanel.SetActive(false);
    }

    public void changeItem(Collect_Manager.typeOfItem item)
    {
        thisItemType = item;
    }
}

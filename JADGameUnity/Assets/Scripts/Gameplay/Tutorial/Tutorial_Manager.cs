//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Tutorial_Manager : MonoBehaviour
{
    int numSteps;
    int currentStep;

    bool conditionMet;

    public TextMeshProUGUI stepText;
    public Button nextButton;
    public GameObject tutPanel;
    public GameObject textPanelHolder;
    public GameObject bottomUIPanelBlocker;
    public GameObject jumpButtonBlocker;
    public GameObject duckButtonBlocker;
    public GameObject HUCDButtonBlocker;
    public GameObject itemHolderBLocker;

    [Tooltip("Text that the steps actually say in game. stepText will display this.")]
    [TextArea]  public List<string> phrases = new List<string>();

    [Tooltip("Should be the same amount as phrases. Use this to check if a condition is needed.")]
    [SerializeField] List<Tutorial_Step> conditionCheck = new List<Tutorial_Step>();

    public static Tutorial_Manager Instance;
    private void Awake()
    {
        Instance = this;

        stepText.text = phrases[0];
        nextButton.gameObject.SetActive(true);
        currentStep = 0;
        numSteps = phrases.Count-1;
        conditionMet = true;
        if(Level_Manager.Instance.getThisLevelType() != Level_Manager.levelType.tutorial)
        {
            tutPanel.SetActive(false);
        }

        bottomUIPanelBlocker.SetActive(true);
        jumpButtonBlocker.SetActive(false);
        duckButtonBlocker.SetActive(false);
        HUCDButtonBlocker.SetActive(false);
        itemHolderBLocker.SetActive(false);
    }

    public void nextStep()
    {
        //We're done, don't go any further.
        if(currentStep == numSteps)
        {
            Debug.Log("Hey we cleared everything!");
          //  tutPanel.SetActive(false);
            SceneManager.LoadScene(0);
            return;
        }

        if(conditionCheck[currentStep+1].isSpecial)
        {
            bottomUIPanelBlocker.SetActive(true);
            doSpecial();
            return;
        }
        else
        {
            currentStep += 1;
        }
   
        if (currentStep <= numSteps && conditionMet != false)
        {
            stepText.text = phrases[currentStep];
            Debug.Log("We are on step " + currentStep);
            //If we need to meet a condition before getting to the next step after.
 
            if (conditionCheck[currentStep].hasStep)
            {
                nextButton.gameObject.SetActive(false);
                bottomUIPanelBlocker.SetActive(false);
                doStep();
                conditionMet = false;
                Debug.Log("This step requires a condition to be met.");
                //Player needs to complete the next step before being able to move onto the next instruction.
            }
            else
            {
                bottomUIPanelBlocker.SetActive(true);
                jumpButtonBlocker.SetActive(false);
                duckButtonBlocker.SetActive(false);
                HUCDButtonBlocker.SetActive(false);
                itemHolderBLocker.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Hey we cleared everything!");
          //  tutPanel.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }

    public void conditionComplete()
    {
        Debug.Log("Condition complete called!");
        Time.timeScale = 1f;
        conditionMet = true;
        nextButton.gameObject.SetActive(true);
        nextStep();
    }

    public int getCurrentStep()
    {
        return currentStep;
    }

    public Tutorial_Step.stepType getStepType()
    {
        return conditionCheck[currentStep].thisStepType;
    }

    void doStep()
    {
        switch(conditionCheck[currentStep].thisStepType)
        {
            case Tutorial_Step.stepType.burning:
                {
                    Level_Manager.Instance.heatMeter.GetComponent<Temperature_Manager>().fillMeter(100f);

                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(true);
                    duckButtonBlocker.SetActive(true);
                    HUCDButtonBlocker.SetActive(false);
                    itemHolderBLocker.SetActive(true);

                    break;
                }
            case Tutorial_Step.stepType.frozen:
                {
                    Level_Manager.Instance.iceMeter.GetComponent<Temperature_Manager>().fillMeter(100f);

                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(true);
                    duckButtonBlocker.SetActive(true);
                    HUCDButtonBlocker.SetActive(false);
                    itemHolderBLocker.SetActive(true);

                    break;
                }
            case Tutorial_Step.stepType.useHandwarmer:
                {
                    //Assign item1 as handwarmer.
                    Level_Manager.Instance.item1 = Collect_Manager.instance.itemsToPick[0];
                    Item theItem = Level_Manager.Instance.item1Holder.transform.GetChild(0).GetComponent<Item>();
                    theItem.thisItemType = Level_Manager.Instance.item1.theItem;
                    Image tempImg = Level_Manager.Instance.item1Holder.transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = Level_Manager.Instance.item1.itemImage;
                    if (Level_Manager.Instance.item1.hasDuration)
                    {
                        theItem.itemDuration = Level_Manager.Instance.item1.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }

                    theItem.waveCooldownTime = 2;

                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(true);
                    duckButtonBlocker.SetActive(true);
                    HUCDButtonBlocker.SetActive(true);
                    itemHolderBLocker.SetActive(false);

                    break;
                }
            case Tutorial_Step.stepType.useDefroster:
                {
                    Level_Manager.Instance.iceMeter.GetComponent<Temperature_Manager>().fillMeter(100f);
                    //Assign item2 as defroster.
                    Level_Manager.Instance.item2 = Collect_Manager.instance.itemsToPick[1];
                    Item theItem = Level_Manager.Instance.item2Holder.transform.GetChild(0).GetComponent<Item>();
                    theItem.thisItemType = Level_Manager.Instance.item2.theItem;
                    Image tempImg = Level_Manager.Instance.item2Holder.transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = Level_Manager.Instance.item2.itemImage;
                    if (Level_Manager.Instance.item2.hasDuration)
                    {
                        theItem.itemDuration = Level_Manager.Instance.item2.duration;
                    }
                    else
                    {
                        theItem.itemDuration = 0f;
                    }

                    theItem.waveCooldownTime = 3;

                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(true);
                    duckButtonBlocker.SetActive(true);
                    HUCDButtonBlocker.SetActive(true);
                    itemHolderBLocker.SetActive(false);

                    break;
                }
            default:
                {
                    Debug.Log("This is not a burning or frozen or item step.");
                    break;
                }
        }

        switch(conditionCheck[currentStep].thisStepType)
        {
            case Tutorial_Step.stepType.jumpButton:
                {
                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(false);
                    duckButtonBlocker.SetActive(true);
                    HUCDButtonBlocker.SetActive(true);
                    itemHolderBLocker.SetActive(true);
                    break;
                }
            case Tutorial_Step.stepType.duckButton:
                {
                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(true);
                    duckButtonBlocker.SetActive(false);
                    HUCDButtonBlocker.SetActive(true);
                    itemHolderBLocker.SetActive(true);
                    break;
                }
            case Tutorial_Step.stepType.hang:
                {
                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(false);
                    duckButtonBlocker.SetActive(true);
                    HUCDButtonBlocker.SetActive(true);
                    itemHolderBLocker.SetActive(true);
                    break;
                }
            case Tutorial_Step.stepType.crouch:
                {
                    bottomUIPanelBlocker.SetActive(false);
                    jumpButtonBlocker.SetActive(true);
                    duckButtonBlocker.SetActive(false);
                    HUCDButtonBlocker.SetActive(true);
                    itemHolderBLocker.SetActive(true);
                    break;
                }

        }
    }


    void doSpecial()
    {
        nextButton.gameObject.SetActive(false);
        StartCoroutine(waitText(conditionCheck[currentStep+1].waitTime));

    }

    public void incrementCurrentStepExternally()
    {
        currentStep += 1;
    }

    public IEnumerator disableJDButtons()
    {
        yield return new WaitForSeconds(1.5f);
        Level_Manager.Instance.jumpButton.enabled = false;
        Level_Manager.Instance.duckButton.enabled = false;
        yield return new WaitForSeconds(1.0f);
        Level_Manager.Instance.jumpButton.enabled = true;
        Level_Manager.Instance.duckButton.enabled = true;
    }

    IEnumerator waitText(float waitTime)
    {
        incrementCurrentStepExternally();
        stepText.text = phrases[currentStep];
        float elapsedTime = 0f;
        while(elapsedTime < 2.0f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
     
      //  yield return new WaitForSecondsRealtime(2.0f);
        //Debug.Log("We have waited 2.0 seconds, shutting off the text box.");
        textPanelHolder.SetActive(false);
        //Debug.Log("The current step is: " + currentStep.ToString() + " IN WAITTEXT");
        switch(currentStep)
        {
            //Spawn enemy.
            case 12:
                {
                    Wave_Spawner.Instance.tutorialSpawn();
                    break;
                }
            case 14:
                {
                    Wave_Spawner.Instance.tutorialSpawn();
                    break;
                }
            case 35:
                {
                    Wave_Spawner.Instance.tutorialSpawn();
                    textPanelHolder.SetActive(true);
                    break;
                }
            default:
                {
                    Debug.LogWarning("Hey we didn't find a step that has a special condition in the switch statement!");
                    break;
                }
        }

        elapsedTime = 0f;
        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
            yield return null;
        }

       // yield return new WaitForSecondsRealtime(waitTime);
       if(currentStep != 35)
        {
            Time.timeScale = 0f;
        }
        
       textPanelHolder.SetActive(true);
       nextButton.gameObject.SetActive(true);
       //incrementCurrentStepExternally();
       nextStep();


    }
}

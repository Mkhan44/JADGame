//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Tutorial_Manager : MonoBehaviour
{
    int numSteps;
    int currentStep;

    bool conditionMet;

    public TextMeshProUGUI stepText;
    public Button nextButton;
    public GameObject tutPanel;
    public GameObject textPanelHolder;

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
    }

    public void nextStep()
    {
        //We're done, don't go any further.
        if(currentStep == conditionCheck.Count-1)
        {
            Debug.Log("Hey we cleared everything!");
            tutPanel.SetActive(false);
            return;
        }

        if(conditionCheck[currentStep+1].isSpecial)
        {
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
                conditionMet = false;
                Debug.Log("This step requires a condition to be met.");
                //Player needs to complete the next step before being able to move onto the next instruction.
            }
        }
        else
        {
            Debug.Log("Hey we cleared everything!");
            tutPanel.SetActive(false);
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


    void doSpecial()
    {
        nextButton.gameObject.SetActive(false);
        StartCoroutine(waitText(conditionCheck[currentStep+1].waitTime));

    }

    public void incrementCurrentStepExternally()
    {
        currentStep += 1;
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
        Time.timeScale = 0f;
        textPanelHolder.SetActive(true);
        nextButton.gameObject.SetActive(true);
        //incrementCurrentStepExternally();
        nextStep();


    }
}

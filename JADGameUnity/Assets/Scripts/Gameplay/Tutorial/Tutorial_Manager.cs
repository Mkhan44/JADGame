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

    [Tooltip("Text that the steps actually say in game. stepText will display this.")]
    [TextArea]  public List<string> phrases = new List<string>();

    [Tooltip("Should be the same amount as phrases. Use this to check if a condition is needed.")]
    [SerializeField] List<bool> conditionCheck = new List<bool>();

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
        currentStep += 1;
        if (currentStep <= numSteps && conditionMet != false)
        {
            stepText.text = phrases[currentStep];
            Debug.Log("We are on step " + currentStep);
            //If we need to meet a condition before getting to the next step after.
            if (conditionCheck[currentStep])
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
        conditionMet = true;
        nextButton.gameObject.SetActive(true);
        nextStep();
    }

    public int getCurrentStep()
    {
        return currentStep;
    }

}

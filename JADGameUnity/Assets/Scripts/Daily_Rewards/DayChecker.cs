//Code written by Mohamed Riaz Khan of Bukugames.
//All code is written by me (Above name) unless otherwise stated via comments below.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;
public class DayChecker : MonoBehaviour
{
    public Button menuButton;
    public TextMeshProUGUI menuText;
    public TextMeshProUGUI dialougeText;

    int currentCoins;

    string dateKey = "LastClaimDateKey";

    string dateStored;
    DateTime oldDate;
    DateTime newDate;
    TimeSpan difference;

    bool firstTime = false;

    [Header("Vars we can change")]

    [Tooltip("The minimum reward the user can obtain when claiming.")]
    public int rewardVarianceMin;
    [Tooltip("The maximum reward the user can obtain when claiming.")]
    public int rewardVarianceMax; //Remember to make it this + 1 when randomizing...

    [Tooltip("Hours between rewards. (Must be less than 24)")]
    public int hoursToWait;

    //Converting this into a percentage so it's not a full 24 hours to wait.
    float percentageToWait;

    // Start is called before the first frame update
    void Awake()
    {
        if (hoursToWait > 24)
        {
            hoursToWait = 24;
        }
        else if(hoursToWait < 1)
        {
            hoursToWait = 1;
        }

        percentageToWait = (float)hoursToWait / 24f;

 


        newDate = System.DateTime.Now;
        if(PlayerPrefs.GetString(dateKey) == "")
        {
            string settingDate = Convert.ToString(newDate);
            Debug.Log("There is no data: Setting the date to - " + settingDate);
            PlayerPrefs.SetString(dateKey, settingDate);
            firstTime = true;
        }
        else
        {
            firstTime = false;
        }

        dateStored = PlayerPrefs.GetString(dateKey);
        oldDate = Convert.ToDateTime(dateStored);

//        Debug.Log("Last date: " + oldDate);
  //      Debug.Log("Current date: " + newDate);


        
    }
    void Start()
    {
        currentCoins = Collect_Manager.instance.totalCoins;

        //Debug.Log(PlayerPrefs.GetString(dateKey));
        canWeClaim();
       

    }

    // Update is called once per frame
    void Update()
    {
      // canWeClaim();
    }

    bool canWeClaim()
    {
        newDate = System.DateTime.Now;

        //Checks if the current date is at least 1 higher than previous one we stored.
        TimeSpan difference = newDate.Subtract(oldDate);

       // Debug.Log("The difference is: " + difference.TotalDays);
       // Debug.Log("Percentage to wait is: " + percentageToWait + " Which is hrs: " + hoursToWait);
        if(firstTime)
        {
            menuText.text = "Daily reward!";
            menuButton.interactable = true;
            PlayerPrefs.SetString(dateKey, "");
            return true;
        }
        else
        {
       
            if (difference.TotalDays >=  percentageToWait)
            {
                menuText.text = "Daily reward!";
                menuButton.interactable = true;
                return true;
            }
            else
            {
                menuText.text = "Claimed already";
                menuButton.interactable = false;
                return false;
            }
        }
       
    }



    public void claimReward()
    {

        //Give player random num of hints between the variance.
        int coinsToGive;

        coinsToGive = Random.Range(rewardVarianceMin , (rewardVarianceMax+1));

        float roundedNum = Mathf.Round(coinsToGive / 100);

        Debug.Log("Rounded coins are: " + roundedNum);

        coinsToGive = (int)roundedNum * 100;

        //Debug.Log("coins we are giving to the player is: " + coinsToGive);

        dialougeText.text = "You just got x" + coinsToGive + " Coins! Come back tomorrow for more rewards!";

        currentCoins += coinsToGive;

        Collect_Manager.instance.totalCoins = currentCoins;

        Save_System.SaveCollectables(Collect_Manager.instance);

        firstTime = false;

        onRewardClaimed();

    }

    void onRewardClaimed()
    {
        //Makes it so the current date we are on is now the "previous date" for check.

        string newStringDate = Convert.ToString(newDate);

        PlayerPrefs.SetString(dateKey, newStringDate);

        oldDate = Convert.ToDateTime(newStringDate);
        //Call to make sure that the messages reset.
        canWeClaim();
    }
}

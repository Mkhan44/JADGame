using System;

using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using TMPro;

using Random = UnityEngine.Random;


public class DailyReward : MonoBehaviour {

	//UI

	public TextMeshProUGUI timeLabel; //only use if your timer uses a label

	public Button timerButton; //used to disable button when needed

	public Image _progress;

    public TextMeshProUGUI timeLeft;

    public TextMeshProUGUI timeLeftMenu;

	//TIME ELEMENTS

	public int hours; //to set the hours

	public int minutes; //to set the minutes

	public int seconds; //to set the seconds

	private bool _timerComplete = false;

    private bool _timerIsReady;

	private TimeSpan _startTime;

	private TimeSpan _endTime;

	private TimeSpan _remainingTime;

	//progress filler

	private float _value = 1f;

	//reward to claim

	public int RewardToEarn;



    //MY VARIABLES

    string numHintsKey = "remainingHints";
    int numHintsLeft;

    string theTimeLeftStr;
    public Button closeButton;
    public TextMeshProUGUI rewardsNumTextMesh;
    public Button rewardMenuButton;

    //Format:

     TimeSpan timeLeftFormat;
     float randomFloat;

	//startup

	void Start()

	{

		if (PlayerPrefs.GetString ("_timer") == "")

		{ 

			Debug.Log ("==> Enableing button");

			enableButton ();

		} else 

		{

			disableButton ();

			StartCoroutine ("CheckTime");

		}

	}







	//update the time information with what we got some the internet

	private void updateTime()

	{

		if (PlayerPrefs.GetString ("_timer") == "Standby") {

            PlayerPrefs.SetInt ("_date", TimeManager.sharedInstance.getCurrentDateNow());

            PlayerPrefs.SetString("_timer", TimeManager.sharedInstance.getCurrentTimeNow());

        }else if (PlayerPrefs.GetString ("_timer") != "" && PlayerPrefs.GetString ("_timer") != "Standby")

        {

            int _old = PlayerPrefs.GetInt("_date");

            int _now = TimeManager.sharedInstance.getCurrentDateNow();

          
            

            //check if a day as passed

            if(_now > _old)

            {//day as passed

                Debug.Log("Day has passed");

                enableButton ();

                return;

            }else if (_now == _old)

            {//same day

                Debug.Log("Same Day - configuring now");

                _configTimerSettings();

                return;

            }else

            {

                Debug.Log("error with date");
                timeLeftMenu.text = "Can't connect to server!";
                rewardMenuButton.interactable = false;
                return;

            }

        }

         Debug.Log("Day had passed - configuring now");

         _configTimerSettings();

	}



//setting up and configureing the values

//update the time information with what we got some the internet

private void _configTimerSettings()

{

    _startTime = TimeSpan.Parse (PlayerPrefs.GetString ("_timer"));

    _endTime = TimeSpan.Parse (hours + ":" + minutes + ":" + seconds);

    TimeSpan temp = TimeSpan.Parse (TimeManager.sharedInstance.getCurrentTimeNow ());

    TimeSpan diff = temp.Subtract (_startTime);

    _remainingTime = _endTime.Subtract (diff);

    //start timmer where we left off

    setProgressWhereWeLeftOff ();

    

    if(diff >= _endTime)

    {

        _timerComplete = true;

        enableButton ();

    }else

    {

        _timerComplete = false;

        disableButton();

        _timerIsReady = true;

    }

}



//initializing the value of the timer

	private void setProgressWhereWeLeftOff()

	{

		float ah = 1f / (float)_endTime.TotalSeconds;

		float bh = 1f / (float)_remainingTime.TotalSeconds;

		_value = ah / bh;

        timeLeftFormat = _remainingTime;

		//_progress.fillAmount = _value;

	}







	//enable button function

	private void enableButton()

	{
        timeLeft.text = "Click below to claim your reward!";
        timeLeftMenu.text = "Reward Ready!";

        closeButton.interactable = false;

        timerButton.interactable = true;

        rewardMenuButton.interactable = true;

	}







	//disable button function

	private void disableButton()

	{
        rewardMenuButton.interactable = false;
        

		timerButton.interactable = false;

        closeButton.interactable = true;
		//timeLabel.text = "Close";

	}





	//use to check the current time before completely any task. use this to validate

	private IEnumerator CheckTime()

	{

		disableButton ();

       // timeLabel.text = "Checking the time...";

        timeLeft.text = "Checking the time...";
        timeLeftMenu.text = "Checking the time...";

		Debug.Log ("==> Checking for new time");

		yield return StartCoroutine (

			TimeManager.sharedInstance.getTime()



		);

		updateTime ();

		Debug.Log ("==> Time check complete!");



	}





	//trggered on button click

	public void rewardClicked()

	{

		Debug.Log ("==> Claim Button Clicked");

        claimReward (RewardToEarn);

		PlayerPrefs.SetString ("_timer", "Standby");

		StartCoroutine ("CheckTime");

	}







	//update method to make the progress tick

	void Update()

	{
        

        if(_timerIsReady)

        {

            if (!_timerComplete && PlayerPrefs.GetString ("_timer") != "")

                {
                    _value -= Time.deltaTime * 1f / (float)_endTime.TotalSeconds;

                    double newVal = ((double)_value * 100);
                   // _value -= Time.deltaTime * 1f;

                  
                    timeLeftMenu.text = theTimeLeftStr + "Already claimed today!";

                //    _progress.fillAmount = _value;

                

                    //this is called once only

                    if (_value <= 0 && !_timerComplete) {

                        //when the timer hits 0, let do a quick validation to make sure no speed hacks.

                    validateTime ();

                    _timerComplete = true;

                }

            }

        }

	}







	//validator

	private void validateTime()

	{

		Debug.Log ("==> Validating time to make sure no speed hack!");

		StartCoroutine ("CheckTime");

	}





	private void claimReward(int x)

	{
        numHintsLeft = PlayerPrefs.GetInt(numHintsKey);
        //Do randomization with x
        //between 3 and x-1 ...In the current case x = 11 so between 3-10.
        int randomNum = Random.Range(3, x);

        PlayerPrefs.SetInt(numHintsKey, (numHintsLeft + randomNum));

		Debug.Log ("You got: "+ randomNum +" Hints!");
        rewardsNumTextMesh.text = "Congratulations! You just got x" + randomNum + " hints!"; 


	}



}


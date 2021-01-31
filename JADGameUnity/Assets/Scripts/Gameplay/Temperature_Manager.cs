//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

//Attach this cscript to any temperature bar.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temperature_Manager : MonoBehaviour
{

    enum meterType
    {
        heatMeter,
        iceMeter
    }

    [Tooltip("Is this an ice meter or heat meter?")]
    [SerializeField]
    meterType typeOfMeter;

    public Slider theMeter;

    float maxMeterVal = 100f;
    float currentMeterVal;

    [Header("Meter fill related variables.")]
    //Amount the heat meter will fill. Get from player script.
    float heatFillRate;
    //Amount the heat meter will fill. Get from player script.
    float iceFillRate;
    //Will use this based on what type of meter this script is attached to.
    float realFillRate;
    //If any modifiers need to be applied to the bar fill rate, they will happen here.
    float modifer;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        currentMeterVal = 0.0f;
        theMeter.value = currentMeterVal;
        setFillRate();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Setters/Getters.
    public void setHeat(float val)
    {
        heatFillRate = val;
    }
    public void setIce(float val)
    {
        iceFillRate = val;
    }
    public void setFillRate()
    {
        if(typeOfMeter == meterType.heatMeter)
        {
            realFillRate = heatFillRate;
            return;
        }
        else
        {
            realFillRate = iceFillRate;
            return;
        }

        //Failsafe.
        if(realFillRate == 0f)
        {
            realFillRate = 1f;
        }
    }


    //Setters/Getters.

    //If we get hit by an obstacle, fill it by x amount (x being whatever obstacle gives)
    public void fillMeter(float amount)
    {
       
        currentMeterVal += amount;
        if(currentMeterVal >= maxMeterVal)
        {
            currentMeterVal = maxMeterVal;
            
        }
        theMeter.value = currentMeterVal;
      
    }

    public void decreaseMeter(float amount)
    {
        currentMeterVal -= amount;
        if (currentMeterVal <= 0f)
        {
            currentMeterVal = 0f;
        }
        theMeter.value = currentMeterVal;
    }

    //Depending on the type of bar...We need to call this and fill the corresponding bar with each passing second by a set amount.
    public IEnumerator fillConstant()
    {
        //We need a variable in the inspector just in case we have any modifers to the speed at which bar fills.

        yield return new WaitForSeconds(0.1f);

        if (typeOfMeter == meterType.heatMeter)
        {
            //Fill heat
            currentMeterVal += realFillRate;
            if(currentMeterVal >= maxMeterVal)
            {
                currentMeterVal = maxMeterVal;
            }
            theMeter.value = currentMeterVal;
            //Debug.Log("Filling heat...");
        }
        else
        {
            currentMeterVal += realFillRate;
            if (currentMeterVal >= maxMeterVal)
            {
                currentMeterVal = maxMeterVal;
            }
            theMeter.value = currentMeterVal;
            //Fill cold
            //Debug.Log("Filling cold...");
        }

        
        
    }

    //Use this when ducking to decrease heat meter or when jumping/hanging to decrease ice meter.
    public IEnumerator decreaseConstant()
    {
        yield return new WaitForSeconds(0.1f);

        currentMeterVal -= realFillRate;
        if (currentMeterVal <= 0.0f)
        {
            currentMeterVal = 0.0f;
        }
        theMeter.value = currentMeterVal;
    }

    public float getMeterVal()
    {
        return currentMeterVal;
    }

    //Sets the value via the levelmanger or something else if player is in heat/cold mode.
    public void setMeterValExternally(float value)
    {
        currentMeterVal = value;
        if(currentMeterVal >= 100)
        {
            currentMeterVal = 100;
        }
        else if (currentMeterVal <= 0)
        {
            currentMeterVal = 0;
        }
    }

    //Once meter is filled, it should decrease automatically over a set period using this function.
    public IEnumerator decreaseMeterFilled(bool filledMeter)
    {
        while(filledMeter && currentMeterVal > 0)
        {
            yield return new WaitForSeconds(0.1f);
            currentMeterVal -= 0.5f;
            if (currentMeterVal <= 0.0f)
            {
                currentMeterVal = 0.0f;
                filledMeter = false;
            }
            theMeter.value = currentMeterVal;

            yield return null;
        }
    }
}

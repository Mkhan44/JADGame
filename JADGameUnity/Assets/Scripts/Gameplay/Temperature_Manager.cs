

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

    [SerializeField]
    meterType typeOfMeter;

    public Slider theMeter;

    private float maxMeterVal = 1f;
    private float currentMeterVal;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        currentMeterVal = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //If we get hit by an obstacle, fill it by x amount (x being whatever obstacle gives)
    public void fillMeter(float amount)
    {
       
        currentMeterVal += amount/100;
        if(currentMeterVal >= maxMeterVal)
        {
            currentMeterVal = maxMeterVal;
            
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
            currentMeterVal += 0.002f;
            if(currentMeterVal >= maxMeterVal)
            {
                currentMeterVal = maxMeterVal;
            }
            theMeter.value = currentMeterVal;
            //Debug.Log("Filling heat...");
        }
        else
        {
            currentMeterVal += 0.002f;
            if (currentMeterVal >= maxMeterVal)
            {
                currentMeterVal = maxMeterVal;
            }
            theMeter.value = currentMeterVal;
            //Fill cold
            //Debug.Log("Filling cold...");
        }

        
        
    }

    public IEnumerator decreaseConstant()
    {
        yield return new WaitForSeconds(0.1f);

        currentMeterVal -= 0.002f;
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

    public IEnumerator decreaseMeterFilled(bool filledMeter)
    {
        while(filledMeter && currentMeterVal > 0)
        {
            yield return new WaitForSeconds(0.1f);
            currentMeterVal -= 0.002f;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary;

public class Cloud_Saving : MonoBehaviour
{
    public static Cloud_Saving instance;
    public Cloud_Data myCloudData;

    public int cloudTotalCoinsTest;

    [Header("DO NOT CHANGE THIS!")]
    [SerializeField] public string cloudDataKey;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);

        if(CloudServices.IsAvailable())
        {
            CloudServices.Synchronize();
        }
        else
        {
            Universal_Dialouge_Box.instance.activatePopup("Cloud services not available! Cannot Synchronize.");
        }


    }

    private void OnEnable()
    {
        // register for events
        CloudServices.OnUserChange += OnUserChange;
        CloudServices.OnSavedDataChange += OnSavedDataChange;
        CloudServices.OnSynchronizeComplete += OnSynchronizeComplete;
    }

    private void OnDisable()
    {
        // unregister from events
        CloudServices.OnUserChange -= OnUserChange;
        CloudServices.OnSavedDataChange -= OnSavedDataChange;
        CloudServices.OnSynchronizeComplete -= OnSynchronizeComplete;
    }

    // Register for the CloudServices.OnSyncronizeComplete event
    // ...
    private void OnSynchronizeComplete(CloudServicesSynchronizeResult result)
    {
        Debug.Log("Received synchronize finish callback.");
        Debug.Log("Status: " + result.Success);
        Universal_Dialouge_Box.instance.activatePopup("Received synchronize finish callback. Status: " + result.Success.ToString());
        // By this time, you have the latest data from cloud and you can start reading.
        myCloudData = GetCloudData(cloudDataKey);
        cloudTotalCoinsTest = loadCloudData();

        /*

        if (myCloudData == null)
        {
            Universal_Dialouge_Box.instance.activatePopup("myCloudData is null!");
        }
        else
        {
              Universal_Dialouge_Box.instance.activatePopup("myCloudData is not null. Your current coins from the cloud are: " + myCloudData.totalCoins);
        }
        */
        
        //Debug.Log(myCloudData);
    }

    public void SaveCloudData(string key, Cloud_Data data)
    {
        string json = JsonUtility.ToJson(data);

        CloudServices.SetString(key, json);
    }

    //Test version for updating highscore.
    public void SaveCloudData(int num)
    {
        CloudServices.SetInt("testSuperIntRPRT", num);
    }

    public int loadCloudData()
    {
      return  CloudServices.GetInt("testSuperIntRPRT");
    }
    //test

    public Cloud_Data GetCloudData(string key)
    {
        string json = CloudServices.GetString(key);

        return JsonUtility.FromJson<Cloud_Data>(json);
    }

    public void manualSync()
    {
        CloudServices.Synchronize();
    }

    private void OnUserChange(CloudServicesUserChangeResult result, Error error)
    {
        Debug.Log(result.User);
    }

    //This function is where we check the user local data versus what's on the cloud. Make sure things are synced.
    private void OnSavedDataChange(CloudServicesSavedDataChangeResult result)
    {
        for(int i = 0; i< result.ChangedKeys.Length; i ++)
        {
            if(result.ChangedKeys[i] == cloudDataKey)
            {
                Cloud_Data serverCopy = GetCloudData(cloudDataKey);

                //CHECK ALL CLOUD VS LOCAL STUFF HERE.

                if(serverCopy.totalCoins > myCloudData.totalCoins)
                {
                    //Test, we don't actually want to do this in the full game because the user might have bought something.
                    Debug.Log("The server copy had: " + serverCopy.totalCoins + " coins and the local copy had: " + myCloudData.totalCoins + " coins. We are updating the local copy.");
                    myCloudData.totalCoins = serverCopy.totalCoins;
                    //Update the collect manager as well.
                    int tempCoins = (int)myCloudData.totalCoins;
                    Collect_Manager.instance.totalCoins = tempCoins;
                }
                else
                {
                    Debug.Log("The server copy had: " + serverCopy.totalCoins + " coins and the local copy had: " + myCloudData.totalCoins + " coins. We are NOT updating the local copy.");
                }

                SaveCloudData(cloudDataKey, myCloudData);

                //CHECK ALL CLOUD VS LOCAL STUFF HERE.
            }

            if(result.ChangedKeys[i] == "testSuperIntRPRT")
            {
                int serverCopyTotalCoins = loadCloudData();

                if(serverCopyTotalCoins > cloudTotalCoinsTest)
                {
                    Universal_Dialouge_Box.instance.activatePopup("We changed the number of coins. On server coin value was: " + serverCopyTotalCoins + " and on local copy we had " + cloudTotalCoinsTest + " Which has now been updated to match the server.");
                    cloudTotalCoinsTest = serverCopyTotalCoins;

                }
                else
                {
                    Universal_Dialouge_Box.instance.activatePopup("We DID NOT change the number of coins. On server coin value was: " + serverCopyTotalCoins + " and on local copy we had " + cloudTotalCoinsTest + " Which is NOT bigger than server.");
                }
            }
        }
    }

}

//All data we need to save. For a list we may need to do some tweaking...
public class Cloud_Data
{
    public long totalCoins;
    public long highScore;
    public long highestWave;
    public long mostWavesSurvived;
    public long totalWavesSurvived;

}
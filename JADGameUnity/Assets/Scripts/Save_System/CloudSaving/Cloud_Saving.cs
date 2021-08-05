using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary;

public class Cloud_Saving : MonoBehaviour
{
    public static Cloud_Saving instance;
    public Cloud_Data myCloudData;

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
        // By this time, you have the latest data from cloud and you can start reading.
        myCloudData = GetCloudData(cloudDataKey);
        //Debug.Log(myCloudData);
    }

    public void SaveCloudData(string key, Cloud_Data data)
    {
        string json = JsonUtility.ToJson(data);

        CloudServices.SetString(key, json);
    }

    public Cloud_Data GetCloudData(string key)
    {
        string json = CloudServices.GetString(key);

        return JsonUtility.FromJson<Cloud_Data>(json);
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
                    Collect_Manager.instance.totalCoins = myCloudData.totalCoins;
                }
                else
                {
                    Debug.Log("The server copy had: " + serverCopy.totalCoins + " coins and the local copy had: " + myCloudData.totalCoins + " coins. We are NOT updating the local copy.");
                }

                SaveCloudData(cloudDataKey, myCloudData);

                //CHECK ALL CLOUD VS LOCAL STUFF HERE.
            }
        }
    }

}

//All data we need to save. For a list we may need to do some tweaking...
public class Cloud_Data
{
    public int totalCoins;
    public int highScore;
    public int highestWave;
    public int mostWavesSurvived;
    public int totalWavesSurvived;

}
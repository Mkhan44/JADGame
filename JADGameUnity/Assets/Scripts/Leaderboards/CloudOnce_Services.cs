//This script uses services for Google play and App store connect to submit information such as achivements, leaderboards, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;

public class CloudOnce_Services : MonoBehaviour
{
    public static CloudOnce_Services instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            Cloud.OnInitializeComplete += CloudOnceInitializeComplete;
            Cloud.Initialize(true, true);
            Cloud.OnCloudLoadComplete += CloudOnceLoadComplete;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    void CloudOnceInitializeComplete()
    {
        Cloud.OnInitializeComplete -= CloudOnceInitializeComplete;
        Cloud.Storage.Load();
    }

   


    public void submitLeaderboardScore(int score)
    {
        Leaderboards.Most_research_points.SubmitScore(score);
    }

    //For loading/saving data from the cloud.

    void CloudOnceLoadComplete(bool success)
    {
        //Place the loaded data into the local save. This is mostly used for IOS.

        if (Collect_Manager.instance != null)
        {
            Collect_Manager.instance.totalCoins = CloudVariables.TotalCoins;
        }
    }
    public void saveCloud()
    {
        Cloud.Storage.Save();
        //Also save to collect_Manager.
    }

    public void deleteData()
    {
        Cloud.Storage.DeleteAll();
    }


    //For loading/saving data from the cloud.
}

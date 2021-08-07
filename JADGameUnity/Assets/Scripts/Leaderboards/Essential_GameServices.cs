using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class Essential_GameServices : MonoBehaviour
{
    public static Essential_GameServices instance;

    public static bool isAuthenticated;

    static ILocalPlayer localPlayer;

    private void OnEnable()
    {
        // register for events
        GameServices.OnAuthStatusChange += OnAuthStatusChange;
    }

    private void OnDisable()
    {
        //base.OnDisable();

        // unregister from events
        GameServices.OnAuthStatusChange -= OnAuthStatusChange;
    }



    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
     

        if(isAuthenticated)
        {
            return;
        }
        if(GameServices.IsAvailable())
        {
            GameServices.Authenticate();
        }
    }


    private void OnAuthStatusChange(GameServicesAuthStatusChangeResult result, Error error)
    {
        if (error == null)
        {
            Debug.Log("Received auth status change event");
            Debug.Log("Auth status: " + result.AuthStatus);

            if (result.AuthStatus == LocalPlayerAuthStatus.Authenticated)
            {
                Debug.Log("Local player: " + result.LocalPlayer);
                isAuthenticated = true;
                localPlayer = result.LocalPlayer;
            }


        }
        else
        {
            Debug.LogError("Failed login with error : " + error);
        }
    }


    public void ReportScoreToLeaderboards(string leaderboardId, long score)
    {

        GameServices.ReportScore(leaderboardId, score, (error) =>
        {
            if (error == null)
            {
                Debug.Log("Request to submit score finished successfully.");
            }
            else
            {
                Debug.Log("Request to submit score failed with error: " + error.Description);
            }
        });
    }

    public void displayAllLeaderboards()
    {
        GameServices.ShowLeaderboards(callback: (result, error) =>
        {
            Debug.Log("Leaderboards UI closed");
        });
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

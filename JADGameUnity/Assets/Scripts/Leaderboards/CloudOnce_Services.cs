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
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }


    public void submitLeaderboardScore(int score)
    {
        Leaderboards.Most_research_points.SubmitScore(score);
    }
}

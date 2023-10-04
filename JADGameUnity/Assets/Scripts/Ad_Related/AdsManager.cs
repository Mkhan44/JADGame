using UnityEngine;
using UnityEngine.Advertisements;
using Hellmade.Sound;

public class AdsManager : MonoBehaviour , IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    private string noAdsKey = "noAdsKey";
    public int adsPurchasedCheck;

    private string playStoreID = "3985667";
    private string appStoreID = "3985666";

    private string interstitialAd = "video";
    private string rewardedVideoAd = "rewardedVideo";

    public bool isTargetPlayStore;
    public bool isTestAd;

    string globalVolumeKey = "Global_Volume";
    string MusicVolumeKey = "Music_Volume";
    string SFXVolumeKey = "SFX_Volume";

    float globalVolumePref;
    float MusicVolumePref;
    float SFXVolumePref;


    public static AdsManager instance;
    void Awake()
    {
        adsPurchasedCheck = PlayerPrefs.GetInt(noAdsKey);
       
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
    void Start()
    {

        // Advertisement.AddListener(this);
        initializeAds();

        globalVolumePref = PlayerPrefs.GetFloat(globalVolumeKey);
        MusicVolumePref = PlayerPrefs.GetFloat(MusicVolumeKey);
        SFXVolumePref = PlayerPrefs.GetFloat(SFXVolumeKey);
    }

    private void initializeAds()
    {
        if(isTargetPlayStore)
        {
            Advertisement.Initialize(playStoreID, isTestAd, this);
            return;
        }
        else
        {
            Advertisement.Initialize(appStoreID, isTestAd, this);
        }
    }

    public void playInterstitialAd()
    {
        Advertisement.Load(interstitialAd, this);

        //if(!Advertisement.IsReady(interstitialAd))
        //{

        //    return;
        //}
        //else
        //{
        //    Advertisement.Show(interstitialAd);
        //}
    }

    public void playRewardedVideoAd()
    {
        Advertisement.Load(rewardedVideoAd, this);

        //if (!Advertisement.IsReady(rewardedVideoAd))
        //{

        //    return;
        //}
        //else
        //{
        //    Advertisement.Show(rewardedVideoAd);
        //}
    }

    public void OnUnityAdsDidError(string message)
    {
    //    throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //    throw new System.NotImplementedException();
        switch (showResult)
        {
            case ShowResult.Failed:
                {
                    break;
                }
            case ShowResult.Skipped:
                {
                    break;
                }
            case ShowResult.Finished:
                {
                    //User watched rewarded video, give them something.
                    if(placementId == rewardedVideoAd)
                    {
                        Debug.Log("Rewarded video!");
                        //Let player respawn during gameplay.
                        if (Level_Manager.Instance != null)
                        {
                            Level_Manager.Instance.respawnPlayer();
                        }
                        else
                        {
                            Debug.LogWarning("Hey, the level manager instance is null!!! Are we in gameplay mode?");
                        }
                       
                    }
                    else if(placementId == interstitialAd)
                    {
                        Debug.Log("Interstitial video!");
                    }
                    break;
                }
        }

        if(Audio_Manager.Instance != null)
        {
            Audio_Manager.Instance.unmuteCurrentTrack();
        }

        //globalVolumePref = PlayerPrefs.GetFloat(globalVolumeKey);
        //     EazySoundManager.GlobalVolume = globalVolumePref;
    }


    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (Audio_Manager.Instance != null)
        {
            Audio_Manager.Instance.muteCurrentTrack();
        }
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        if (Audio_Manager.Instance != null)
        {
            Audio_Manager.Instance.muteCurrentTrack();
        }
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //    throw new System.NotImplementedException();
        switch (showCompletionState)
        {
            case UnityAdsShowCompletionState.UNKNOWN:
                {
                    break;
                }
            case UnityAdsShowCompletionState.SKIPPED:
                {
                    break;
                }
            case UnityAdsShowCompletionState.COMPLETED:
                {
                    //User watched rewarded video, give them something.
                    if (placementId == rewardedVideoAd)
                    {
                        Debug.Log("Rewarded video!");
                        //Let player respawn during gameplay.
                        if (Level_Manager.Instance != null)
                        {
                            Level_Manager.Instance.respawnPlayer();
                        }
                        else
                        {
                            Debug.LogWarning("Hey, the level manager instance is null!!! Are we in gameplay mode?");
                        }

                    }
                    else if (placementId == interstitialAd)
                    {
                        Debug.Log("Interstitial video!");
                    }
                    break;
                }
        }

        if (Audio_Manager.Instance != null)
        {
            Audio_Manager.Instance.unmuteCurrentTrack();
        }
    }

    public void OnInitializationComplete()
    {
        
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }
}

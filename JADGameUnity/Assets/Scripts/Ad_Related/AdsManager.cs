using UnityEngine;
using UnityEngine.Advertisements;
using Hellmade.Sound;

public class AdsManager : MonoBehaviour , IUnityAdsListener
{
    private string noAdsKey = "noAdsKey";
    public int adsPurchasedCheck;

    private string playStoreID = "3633238";
    private string appStoreID = "3633239";

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

    string numHintsKey = "remainingHints";
    int numHintsLeft;


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
       
        Advertisement.AddListener(this);
        initializeAds();

        globalVolumePref = PlayerPrefs.GetFloat(globalVolumeKey);
        MusicVolumePref = PlayerPrefs.GetFloat(MusicVolumeKey);
        SFXVolumePref = PlayerPrefs.GetFloat(SFXVolumeKey);
    }

    private void initializeAds()
    {
        if(isTargetPlayStore)
        {
            Advertisement.Initialize(playStoreID, isTestAd);
            return;
        }
        else
        {
            Advertisement.Initialize(appStoreID, isTestAd);
        }
    }

    public void playInterstitialAd()
    {
        if(!Advertisement.IsReady(interstitialAd))
        {
           
            return;
        }
        else
        {
            Advertisement.Show(interstitialAd);
        }
    }

    public void playRewardedVideoAd()
    {
        numHintsLeft = PlayerPrefs.GetInt(numHintsKey);
        if (!Advertisement.IsReady(rewardedVideoAd))
        {

            return;
        }
        else
        {
            Advertisement.Show(rewardedVideoAd);
        }
    }

    public void OnUnityAdsDidError(string message)
    {
    //    throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
    //    throw new System.NotImplementedException();
        switch(showResult)
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
                        
                        PlayerPrefs.SetInt(numHintsKey, (numHintsLeft + 1));
                    }
                    else if(placementId == interstitialAd)
                    {
                        Debug.Log("Interstitial video!");
                    }
                    break;
                }
        }

        globalVolumePref = PlayerPrefs.GetFloat(globalVolumeKey);
        EazySoundManager.GlobalVolume = globalVolumePref;
    }

    public void OnUnityAdsDidStart(string placementId)
    {
     //   throw new System.NotImplementedException();
        EazySoundManager.GlobalVolume = 0f;
    }

    public void OnUnityAdsReady(string placementId)
    {
    //    throw new System.NotImplementedException();
    }

}

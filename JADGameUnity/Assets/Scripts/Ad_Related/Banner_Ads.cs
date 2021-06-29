using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Hellmade.Sound;

public class Banner_Ads : MonoBehaviour
{

    private string playStoreID = "3985667";
    private string appStoreID = "3985666";
    private string theID;

    private string banner = "Banner";
    public bool isTargetPlayStore;
    public bool isTestAd;

    string globalVolumeKey = "Global_Volume";
    string MusicVolumeKey = "Music_Volume";
    string SFXVolumeKey = "SFX_Volume";

    float globalVolumePref;
    float MusicVolumePref;
    float SFXVolumePref;

    private string noAdsKey = "noAdsKey";
    int adsPurchasedCheck;

    void Awake()
    {
        adsPurchasedCheck = PlayerPrefs.GetInt(noAdsKey);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {

        if (isTargetPlayStore)
        {
            Advertisement.Initialize(playStoreID, isTestAd);
        }
        else
        {
            Advertisement.Initialize(appStoreID, isTestAd);
        }

        while(!Advertisement.IsReady(banner))
        {
            yield return null;
        }

        hideBanner();
    }

    public void hideBanner()
    {
        Advertisement.Banner.Hide();
    }

    public void showBanner()
    {
        Advertisement.Banner.Show(banner);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPos()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }
}

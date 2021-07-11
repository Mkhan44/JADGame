using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager Instance;

    [Header("Era music tracks")]
    [SerializeField] List<AudioClip> prehistoricMusic;
    [SerializeField] List<AudioClip> feudalJapanMusic;
    [SerializeField] List<AudioClip> wildWestMusic;
    [SerializeField] List<AudioClip> futureMusic;
    [SerializeField] List<AudioClip> medMusic;
    [SerializeField] AudioClip tutorialMusic;


    [Header("Gameobject music holders")]
    [SerializeField] AudioSource bonusSource;
    [SerializeField] AudioSource easySource;
    [SerializeField] AudioSource mediumSource;
    [SerializeField] AudioSource hardPauseSource;
    [SerializeField] AudioSource tutorialSource;

    [Header("Swap music variables during transitions.")]
    [SerializeField] AudioSource currentlyPlayingTrack;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(Level_Manager.Instance.getThisLevelType() == Level_Manager.levelType.normal)
        {
            tutorialSource.gameObject.SetActive(false);
            currentlyPlayingTrack = easySource;
        }
        else
        {
            easySource.gameObject.SetActive(false);
            mediumSource.gameObject.SetActive(false);
            bonusSource.gameObject.SetActive(false);
            hardPauseSource.gameObject.SetActive(false);
            currentlyPlayingTrack = tutorialSource;
       
        }

        setMusicTracks(Level_Manager.Instance.getTimePeriod());

    }

    public void setMusicTracks(Level_Manager.timePeriod theEra)
    {
        switch(theEra)
        {
            case Level_Manager.timePeriod.Prehistoric:
                {
                    bonusSource.clip = prehistoricMusic[0];
                    easySource.clip = prehistoricMusic[1];
                    mediumSource.clip = prehistoricMusic[2];
                    hardPauseSource.clip = prehistoricMusic[3];
                    break;
                }
            case Level_Manager.timePeriod.FeudalJapan:
                {
                    bonusSource.clip = feudalJapanMusic[0];
                    easySource.clip = feudalJapanMusic[1];
                    mediumSource.clip = feudalJapanMusic[2];
                    hardPauseSource.clip = feudalJapanMusic[3];
                    break;
                }
            case Level_Manager.timePeriod.WildWest:
                {
                    bonusSource.clip = wildWestMusic[0];
                    easySource.clip = wildWestMusic[1];
                    mediumSource.clip = wildWestMusic[2];
                    hardPauseSource.clip = wildWestMusic[3];
                    break;
                }
            case Level_Manager.timePeriod.Medieval:
                {
                    bonusSource.clip = medMusic[0];
                    easySource.clip = medMusic[1];
                    mediumSource.clip = medMusic[2];
                    hardPauseSource.clip = medMusic[3];
                    break;
                }
            case Level_Manager.timePeriod.tutorial:
                {
                    tutorialSource.clip = tutorialMusic;
                    tutorialSource.volume = 1f;
                    tutorialSource.Play();
                    return;
                    break;
                }
            default:
                {
                    break;  
                }
        }

        //DEBUG.
        
        //Prolly need to call this function BEFORE swapping the tracks...
        StartCoroutine(fadeBetweenTimeswaps(currentlyPlayingTrack, 1f));

        /*
        easySource.volume = 0f;
        mediumSource.volume = 0f;
        hardPauseSource.volume = 0f;
        bonusSource.volume = 0f;
        */
        

        //Have coroutine fade out the current tracks and then play the new ones.
        //Fade out should probably be before the swap above.
   
    }

    //Change the music based on the difficulty we are in. If it's a bonus/timeswap wave, use the Bonus music track.
    public void changeMusicDifficulty(Wave_Spawner.waveDiff theDifficulty, bool isSpecial)
    {
        if(isSpecial)
        {
            StartCoroutine(fadeBetweenDifficultyTracks(currentlyPlayingTrack, bonusSource, 1f));
            /*
            easySource.volume = 0f;
            mediumSource.volume = 0f;
            hardPauseSource.volume = 0f;
            bonusSource.volume = 1f;
            */
            //  currentlyPlayingTrack = bonusSource;
            return;
        }

        switch(theDifficulty)
        {
            case Wave_Spawner.waveDiff.easy:
                {
                    StartCoroutine(fadeBetweenDifficultyTracks(currentlyPlayingTrack, easySource, 1f));

                    /*
                    easySource.volume = 1f;
                    mediumSource.volume = 0f;
                    hardPauseSource.volume = 0f;
                    bonusSource.volume = 0f;
                    */
                    // currentlyPlayingTrack = easySource;

                    break;
                }
            case Wave_Spawner.waveDiff.medium:
                {
                    StartCoroutine(fadeBetweenDifficultyTracks(currentlyPlayingTrack, mediumSource, 1f));

                    /*
                    easySource.volume = 0f;
                    mediumSource.volume = 1f;
                    hardPauseSource.volume = 0f;
                    bonusSource.volume = 0f;
                    */
                   // currentlyPlayingTrack = mediumSource;
                    break;
                }
            case Wave_Spawner.waveDiff.hardPause:
                {
                    StartCoroutine(fadeBetweenDifficultyTracks(currentlyPlayingTrack, hardPauseSource, 1f));

                    /*
                    easySource.volume = 0f;
                    mediumSource.volume = 0f;
                    hardPauseSource.volume = 1f;
                    bonusSource.volume = 0f;
                    */
                    //currentlyPlayingTrack = hardPauseSource;
                    break;
                }
            default:
                {
                    Debug.LogWarning("We couldn't find the difficulty in the switch!");
                    break;
                }
        }

     
    }

    IEnumerator fadeBetweenTimeswaps(AudioSource currentTrackToFade, float fadeInTargetVolume)
    {
        //Fade out current track.
        
        while(currentTrackToFade.volume > 0)
        {
            Debug.Log("During Timeswap loop: " + currentTrackToFade.volume + " = the volume of current track");
            if(currentTrackToFade.volume > 0)
            {
                currentTrackToFade.volume -= 0.1f;
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1.0f);

        bonusSource.Play();
        easySource.Play();
        mediumSource.Play();
        hardPauseSource.Play();

        //Fade in new track.
        while (easySource.volume < fadeInTargetVolume)
        {
            easySource.volume += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        currentlyPlayingTrack = easySource;
        yield return null;
    }
    IEnumerator fadeBetweenDifficultyTracks(AudioSource currentTrackToFadeOut, AudioSource nextTrackToFadeIn, float fadeInTargetVolume)
    {
        currentlyPlayingTrack = nextTrackToFadeIn;

        //Debug.Log("BEFORE LOOP: " + currentTrackToFadeOut.volume + " = the volume of the current track & " + nextTrackToFadeIn.volume + " = the volume of the next track!");
        while(currentTrackToFadeOut.volume > 0 && nextTrackToFadeIn.volume < fadeInTargetVolume)
        {
          //  Debug.Log("DURING LOOP: " + currentTrackToFadeOut.volume + " = the volume of the current track & " + nextTrackToFadeIn.volume + " = the volume of the next track!");
            //Failsafe for if one is completed and the other is not.
            if (currentTrackToFadeOut.volume > 0)
            {
                currentTrackToFadeOut.volume -= 0.1f;
            }

            if(nextTrackToFadeIn.volume < fadeInTargetVolume)
            {
                nextTrackToFadeIn.volume += 0.1f;
            }

            yield return new WaitForSeconds(0.1f);
            //yield return null;
        }

        //Ensure that they are at the proper settings.
       // Debug.Log("The current track we are fading out is: " + currentTrackToFadeOut.clip.name + " And the next track is: " + nextTrackToFadeIn.clip.name);
        currentTrackToFadeOut.volume = 0f;
        nextTrackToFadeIn.volume = fadeInTargetVolume;

        //currentlyPlayingTrack = nextTrackToFadeIn;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void muteCurrentTrack()
    {
        currentlyPlayingTrack.volume = 0f;
    }

    public void unmuteCurrentTrack()
    {
        currentlyPlayingTrack.volume = 1f;
    }
}

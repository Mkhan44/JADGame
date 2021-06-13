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
        }
        else
        {
            easySource.gameObject.SetActive(false);
            mediumSource.gameObject.SetActive(false);
            bonusSource.gameObject.SetActive(false);
            hardPauseSource.gameObject.SetActive(false);
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
            default:
                {
                    break;  
                }
        }

        //DEBUG.
        easySource.volume = 1f;
        mediumSource.volume = 0f;
        hardPauseSource.volume = 0f;
        bonusSource.volume = 0f;
        currentlyPlayingTrack = easySource;

        //Have coroutine fade out the current tracks and then play the new ones.
        //Fade out should probably be before the swap above.
        bonusSource.Play();
        easySource.Play();
        mediumSource.Play();
        hardPauseSource.Play();
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

    IEnumerator fadeBetweenDifficultyTracks(AudioSource currentTrackToFadeOut, AudioSource nextTrackToFadeIn, float fadeInTargetVolume)
    {
        Debug.Log("BEFORE LOOP: " + currentlyPlayingTrack.volume + " = the volume of the current track & " + nextTrackToFadeIn.volume + " = the volume of the next track!");
        while(currentTrackToFadeOut.volume > 0 && nextTrackToFadeIn.volume < fadeInTargetVolume)
        {
            Debug.Log("DURING LOOP: " + currentlyPlayingTrack.volume + " = the volume of the current track & " + nextTrackToFadeIn.volume + " = the volume of the next track!");
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
        currentTrackToFadeOut.volume = 0f;
        nextTrackToFadeIn.volume = fadeInTargetVolume;

        currentlyPlayingTrack = nextTrackToFadeIn;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

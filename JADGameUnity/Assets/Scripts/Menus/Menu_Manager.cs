//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu_Manager : MonoBehaviour
{
    [Header("Singleton")]
    public static Menu_Manager instance;

    [Header("Fade to gameplay stuff")]
    [SerializeField] GameObject fadePanel;
    [SerializeField] GameObject loadingIcon;
    [SerializeField] GameObject noInputPanel;
    [SerializeField] GameObject playPopupMenu;
    [SerializeField] GameObject customizePlayPopupMenu;

    [Header("Mute related stuff.")]
    [SerializeField] Sprite unmutedSprite;
    [SerializeField] Sprite mutedSprite;
    [SerializeField] Image muteButtonSprite;

    [Header("Character switching")]
    [SerializeField] Animator skinAnimator;
    public RuntimeAnimatorController defaultAnimator;

    [Header("Shop related")]
    public TextMeshProUGUI shopNoticeText;
    public Coroutine shopNoticeAnimateRoutine;
    public TextMeshProUGUI coinTotalText;
    public TextMeshProUGUI boltTotalText;

    [Header("Credits related")]
    public RectTransform mainCreditsLayout;
    public RectTransform addCreditsLayout;

    [Header("SFX")]
    [SerializeField] AudioClip startupSound;
    [SerializeField] AudioClip buttonPressSound;
    [SerializeField] AudioClip closeButtonSound;

    [Header("Splash screen related")]
    [SerializeField] GameObject splashScreenParent;
    [SerializeField] Image splashImg;

    [Header("IOS Specific")]
    [SerializeField] GameObject iosDisclaimerButton;
    [SerializeField] GameObject disclaimerIOSPopup;

    [Header("Extra popups")]
    [SerializeField] GameObject firstTimeTutorialPopup;

    Scene gameplayScene;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //gameplayScene = SceneManager.GetSceneByName("Gameplay");
        toggleMute(false);
        setSkin();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        shopNoticeText.text = "";
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            iosDisclaimerButton.SetActive(true);
        }
        else
        {
            iosDisclaimerButton.SetActive(false);
        }
        if(splashScreenParent.activeInHierarchy)
        {
            if (Collect_Manager.instance.getBootupStatus())
            {
                splashScreenParent.SetActive(false);
            }
            else
            {
                StartCoroutine(fadeSplash());
                //Coroutine.
            }
        }
      
    }

    IEnumerator fadeSplash()
    {
       // Color32 splashScreenGraphic = splashScreenParent.transform.GetChild(0).GetComponent<Image>().color;

        yield return new WaitForSeconds(2.5f);
        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 255);
        Color32 endColor = new Color32(255, 255, 255, 0);

        rate = (1.0f / 4.5f) * 5.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            //   Debug.LogWarning(splashScreenGraphic);
            splashImg.color = Color32.Lerp(startColor, endColor, (i));
            //splashScreenParent.transform.GetChild(0).GetComponent<Image>().color = splashScreenGraphic;
            yield return null;
        }

        splashScreenParent.SetActive(false);
        Collect_Manager.instance.setBootupStatus(true);

        //Prompt player for tutorial.
        if(!Collect_Manager.instance.firstTimePlaying)
        {
            firstTimeTutorialPopup.SetActive(true);
            Collect_Manager.instance.firstTimePlaying = true;
            Save_System.SaveCollectables(Collect_Manager.instance);
        }

        //If on IOS, show disclaimer on first bootup.
        if (Application.platform == RuntimePlatform.IPhonePlayer && !Collect_Manager.instance.disclaimerDisplayIOSDone)
        {
            if (!Collect_Manager.instance.disclaimerDisplayIOSDone)
        {
            disclaimerIOSPopup.SetActive(true);
            Collect_Manager.instance.disclaimerDisplayIOSDone = true;
            Save_System.SaveCollectables(Collect_Manager.instance);
        }
       
        }

    }

    public void setSkin()
    {
        int currentSkinInt = (Collect_Manager.instance.getCurrentSkin());

        //Populate the skin preview on the top based on what the user has currently equipped.
        for (int i = 0; i < Collect_Manager.instance.skinsToPick.Count; i++)
        {
            if (currentSkinInt == i)
            {
               // stopAnimator();


                // currentSkinHolderImage.sprite = skinsToPick[i].skinSprite;
                if (Collect_Manager.instance.skinsToPick[i].animationOverrideController == null)
                {
                    Debug.Log("Hey we're using the default animatorController...is something wrong?");
                    skinAnimator.runtimeAnimatorController = defaultAnimator;
                }
                else
                {
                    skinAnimator.runtimeAnimatorController = Collect_Manager.instance.skinsToPick[i].animationOverrideController;
                }


                break;
            }
        }

    }

    public void toggleMute(bool togglingMute)
    {
        if(togglingMute)
        {
            Collect_Manager.instance.isMuted = !Collect_Manager.instance.isMuted;
        }
        Debug.Log("Value of mute is: " + Collect_Manager.instance.isMuted);
    

        if (Collect_Manager.instance.isMuted)
        {
            AudioListener.volume = 0;
            muteButtonSprite.sprite = mutedSprite;
        }
        else
        {
            AudioListener.volume = 1;
            muteButtonSprite.sprite = unmutedSprite;
        }
    }

    public void PlayGame()
    {
        Save_System.SaveCollectables(Collect_Manager.instance);

        //Might want to make sure that this string works.

        StartCoroutine(fadeToGameplay());
        //Make it so player can't spam the button.
        noInputPanel.SetActive(true);

    }

    public void tutorialToggle(bool toggleVal)
    {
        if(Tutorial_Instance_Debug.instance != null)
        {
            Tutorial_Instance_Debug.instance.setTutorial(toggleVal);
        }
    }
    
    IEnumerator fadeToGameplay()
    {

        Audio_Manager.Instance.playSFX(startupSound,false,0.3f);

        yield return new WaitForSeconds(0.8f);

        RectTransform fadePanelTransform = fadePanel.GetComponent<RectTransform>();
        Vector3 currentScale = fadePanelTransform.localScale;
        Vector3 increaseScaleRate = new Vector3(1, 1, 1);

        loadingIcon.SetActive(true);

        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 100);
        Color32 endColor = new Color32(255, 255, 255, 255);
        Image fadePanelImg = fadePanel.GetComponent<Image>();

        rate = (1.0f / 2.5f) * 1.0f;
   

        while (i < 1.0f)
        {
            currentScale += increaseScaleRate;
            //fadePanelTransform.localScale = currentScale;
            i += Time.deltaTime * rate;

            fadePanelTransform.localScale = Vector3.Lerp(fadePanelTransform.localScale, new Vector3(20, 20, 20), (i));

            fadePanelImg.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;

          //  yield return new WaitForSeconds(0.001f);
        }

        //Debug.Log("Loading the gameplay scene...");
        SceneManager.LoadSceneAsync("Gameplay");

    }

    //Shop text

    public void setupShopTextAnimation(string message)
    {
        if(shopNoticeAnimateRoutine != null)
        {
            StopCoroutine(shopNoticeAnimateRoutine);
        }

        shopNoticeText.text = message;
        shopNoticeAnimateRoutine = StartCoroutine(animateShopText());
    }
    public IEnumerator animateShopText()
    {
        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 255);
        Color32 endColor = new Color32(255, 255, 255, 0);


        rate = (1.0f / 4.5f) * 1.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            shopNoticeText.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;
        }
    }
    //Shop text


    //Credits scaling
    public void scaleCredits()
    {


        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            mainCreditsLayout.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            addCreditsLayout.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

    //Credits scaling

    public void ScrollToTop(ScrollRect scrollArea)
    {
       scrollArea.normalizedPosition = new Vector2(0, 1);
    }


    //Sound playing.

    public void playCloseButtonSFX()
    {
        Audio_Manager.Instance.stopSFX(closeButtonSound.name);
        Audio_Manager.Instance.playSFX(closeButtonSound);
    }

    //Sound Playing

    //Play game popup
    public void checkIfItemsEquipped()
    {
        if(Collect_Manager.instance.item1 == -1 && Collect_Manager.instance.item2 == -1 && Collect_Manager.instance.item3 == -1)
        {
            playPopupMenu.SetActive(true);
        }
        else
        {
            tutorialToggle(false);
            PlayGame();
        }
    }

    public void checkIfItemsEquippedCustomizeScreen()
    {
        if (Collect_Manager.instance.item1 == -1 && Collect_Manager.instance.item2 == -1 && Collect_Manager.instance.item3 == -1)
        {
            customizePlayPopupMenu.SetActive(true);
        }
        else
        {
            tutorialToggle(false);
            PlayGame();
        }
    }
    //Play game popup
}

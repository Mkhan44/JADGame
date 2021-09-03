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
    }

    void Update()
    {
        
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

        Debug.Log("Loading the gameplay scene...");
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


}

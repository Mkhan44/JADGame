//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Manager : MonoBehaviour
{
    [Header("Singleton")]
    public static Menu_Manager instance;

    [Header("Fade to gameplay stuff")]
    [SerializeField] GameObject fadePanel;
    [SerializeField] Color panelColor;

    Scene gameplayScene;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //gameplayScene = SceneManager.GetSceneByName("Gameplay");
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        
    }

    public void PlayGame()
    {
        Save_System.SaveCollectables(Collect_Manager.instance);

        //Might want to make sure that this string works.

        StartCoroutine(fadeToGameplay());


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

        float i = 0.0f;
        float rate = 0.0f;
        Color32 startColor = new Color32(255, 255, 255, 10);
        Color32 endColor = new Color32(255, 255, 255, 255);
        Image fadePanelImg = fadePanel.GetComponent<Image>();

        rate = (1.0f / 2.5f) * 1.0f;
   

        while (i < 1.0f)
        {
            currentScale += increaseScaleRate;
            fadePanelTransform.localScale = currentScale;
            i += Time.deltaTime * rate;

            fadePanelTransform.localScale = Vector3.Lerp(fadePanelTransform.localScale, new Vector3(20, 20, 20), (i));

            fadePanelImg.color = Color32.Lerp(startColor, endColor, (i));
            yield return null;

          //  yield return new WaitForSeconds(0.001f);
        }

        Debug.Log("Loading the gameplay scene...");
        SceneManager.LoadSceneAsync("Gameplay");

    }


}

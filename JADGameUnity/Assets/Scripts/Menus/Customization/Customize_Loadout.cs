//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

//This script handles customization for player picking their skins/item loadouts.
//Attach this to the customization menu.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Animations;

public class Customize_Loadout : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject skinSelectPrefab;
    public GameObject itemHolderPrefab;

    [Header("Skin related stuff")]
    //These should be scriptables that have skin info (Animator, sprite, etc) attached.
    //At runtime we'll determine which skins the player has and keep those in this list to spawn in.
    //MAKE SURE THIS LIST = IN THE RIGHT ORDER.
    [Tooltip("Ensure this list is in the same order as the information in Collect_Manager script!!!!")]
    public List<SkinInfo> skinsToPick;

    public ScrollRect skinScroll;

    //This is a constant.
    public GameObject currentSkinHolder;

    //We'll be using this to parent the prefabs.
    public HorizontalLayoutGroup skinScrollableArea;

    public Sprite currentSkinHolderSprite;
    public Animator currentSkinHolderAnimator;
    Coroutine animateRoutine;

    bool enteredOnce;


    // Start is called before the first frame update
    void Start()
    {
        enteredOnce = false;
        initializeSkinCustomization();
        enteredOnce = true;
    }

    private void OnEnable()
    {
       if(enteredOnce)
        {
            resetScrollableArea();
            StopCoroutine(animateRoutine);
            animateRoutine = StartCoroutine(animationRandomizer());
            loadSkins();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     **************************************************************
     * SKIN RELATED FUNCTIONS
     ************************************************************** 
     */

    //This function populates the current skin area as well as all skins the player currently has unlocked to select from.
    public void initializeSkinCustomization()
    {
        
        currentSkinHolderAnimator = currentSkinHolder.GetComponent<Animator>();
        currentSkinHolderSprite = currentSkinHolder.GetComponent<Image>().sprite;

        //switchCurrentSkin();

        loadSkins();

        animateRoutine = StartCoroutine(animationRandomizer());

        //currentSkinHolderAnimator.SetBool("IsCrouching", true);



    }

    public void loadSkins()
    {
        for(int i = 0; i < skinsToPick.Count; i++)
        {
            GameObject tempObj = Instantiate(skinSelectPrefab);
            tempObj.transform.SetParent(skinScrollableArea.gameObject.transform, false);
            tempObj.GetComponent<Image>().sprite = skinsToPick[i].skinIcon;
            Button tempButton = tempObj.transform.GetChild(0).GetComponent<Button>();
            int tempNum = i;
            tempButton.onClick.AddListener(() => setCurrentSkin(tempNum));
            tempObj.SetActive(false);

            for (int j = 0; j < Collect_Manager.instance.skinsUnlocked.Count; j++)
            {
                //Do i +1 here because obviously 0 won't ever be a value (In other words, the value of 0 itself) that is in skinsUnlocked.
                if ((i) == Collect_Manager.instance.skinsUnlocked[j])
                {
                    tempObj.SetActive(true);
                    break;
                }
            }
        }

       // Debug.Log("The skinsToPick list has: " + skinsToPick.Count + " elements in it.");

        switchCurrentSkin();
    }

    //Whenever we re-enter the menu, we want to reset the selectable skins because the player might have bought/unlocked some more.
    public void resetScrollableArea()
    {
        foreach(Transform child in skinScrollableArea.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }

    //Sets the skin the Collect_Manager once the user has picked a skin.
    //This is what will be called when clicking on a portrait/skin in the horizontal scroll group.
    public void setCurrentSkin(int currentSkinNum)
    {
        //NEED TO SET THE SKIN IN THE COLLECT_MANAGER SO IT SAVES FOR THE PLAYER!!!!
        foreach (Collect_Manager.skinTypes theType in System.Enum.GetValues(typeof(Collect_Manager.skinTypes)))
        {
            if((int)theType == currentSkinNum)
            {
                Collect_Manager.instance.setCurrentSkin(theType);
                Debug.Log("Switching skin to: " + theType);
                break;
            }
        }
       
        switchCurrentSkin();
    }

    public void switchCurrentSkin()
    {
        //We do minus 1 here because the array of SkinInfo starts at 0.
        int currentSkinInt = (Collect_Manager.instance.getCurrentSkin() - 1);

        //Populate the skin preview on the top based on what the user has currently equipped.
        for (int i = 0; i < skinsToPick.Count; i++)
        {
            if (currentSkinInt == i)
            {
                currentSkinHolderSprite = skinsToPick[i].skinSprite;
                currentSkinHolderAnimator.runtimeAnimatorController = skinsToPick[i].animationController;

                break;
            }
        }

    }

    //Play random animations from the skin we have selected indefinitely.
    public IEnumerator animationRandomizer()
    {
        int whichAniToPlay = 1;

        while (true)
        {
            if (whichAniToPlay == 1)
            {
                whichAniToPlay = 2;
                currentSkinHolderAnimator.SetBool("IsCrouching", true);
            }
            else
            {
                whichAniToPlay = 1;
                currentSkinHolderAnimator.SetBool("IsCrouching", false);
            }

            yield return new WaitForSeconds(1.0f);
        }

    }

    /*
     **************************************************************
     * SKIN RELATED FUNCTIONS
     ************************************************************** 
     */



}

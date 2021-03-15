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

    //This will be used at runtime to be modified to fit whatever skins are currently unlocked by the player.
    List<SkinInfo> modifiedList = new List<SkinInfo>();

    public ScrollRect skinScroll;

    //This is a constant.
    public GameObject currentSkinHolder;

    //We'll be using this to parent the prefabs.
    public HorizontalLayoutGroup skinScrollableArea;

    public Sprite currentSkinHolderSprite;
    public Animator currentSkinHolderAnimator;

    Collect_Manager.skinTypes currentSkinType;

    // Start is called before the first frame update
    void Start()
    {
        initializeSkinCustomization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //This function populates the current skin area as well as all skins the player currently has unlocked to select from.
    public void initializeSkinCustomization()
    {
        
        currentSkinHolderAnimator = currentSkinHolder.GetComponent<Animator>();
        currentSkinHolderSprite = currentSkinHolder.GetComponent<Image>().sprite;

        //We do minus 1 here because the array of SkinInfo starts at 0.
        int currentSkinInt = (Collect_Manager.instance.getCurrentSkin() - 1);
    
        //Populate the skin preview on the top based on what the user has currently equipped.
        for (int i = 0; i < skinsToPick.Count; i++)
        {
            if(currentSkinInt == i)
            {
                currentSkinHolderSprite = skinsToPick[i].skinSprite;
                currentSkinHolderAnimator.runtimeAnimatorController = skinsToPick[i].animationController;

                break;
            }
        }

        loadSkins();
      

        //currentSkinHolderAnimator.SetBool("IsCrouching", true);

        StartCoroutine(animationRandomizer());

    }

    public void loadSkins()
    {
        modifiedList.Clear();
        for(int i = 0; i < skinsToPick.Count; i++)
        {
            //Find the skins we have (i.e. skinsUnlocked[0] = default skin) , and only add those skins to the modified list.
            //Example: We have default (0) , female (1) , supernoob (3) , but not proness (2) ...So add the indexes of 0, 1, and 3 to modified list but get rid of 2.

            for(int j = 0; j < Collect_Manager.instance.skinsUnlocked.Count; j++)
            {
                //Do i +1 here because obviously 0 won't ever be a value that is in skinsUnlocked.
                if((i+1) == Collect_Manager.instance.skinsUnlocked[j])
                {
                    modifiedList.Add(skinsToPick[i]);
                    break;
                }
            }
        }

        Debug.Log("The modified list has: " + modifiedList.Count + " elements in it.");
        Debug.Log("The skinsToPick list has: " + skinsToPick.Count + " elements in it.");


        //Populate the scrollable area.
        for (int i = 0; i < modifiedList.Count; i++)
        {
            GameObject tempObj = Instantiate(skinSelectPrefab);
            tempObj.transform.SetParent(skinScrollableArea.gameObject.transform, false);

            tempObj.GetComponent<Image>().sprite = modifiedList[i].skinIcon;

            Button tempButton = tempObj.transform.GetChild(0).GetComponent<Button>();

           // tempButton.onClick.AddListener(() => changeText(tempNum));

        }
    }


    //Play random animations from the skin we have selected indefinitely.
    public IEnumerator animationRandomizer()
    {
        int whichAniToPlay = 1;

        while (true)
        {
            if(whichAniToPlay == 1)
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
}

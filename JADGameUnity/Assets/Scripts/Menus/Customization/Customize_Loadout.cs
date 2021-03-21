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

    //This is a constant.
    public GameObject currentSkinHolder;

    //We'll be using this to parent the prefabs.
    public HorizontalLayoutGroup skinScrollableArea;

    public Animator currentSkinHolderAnimator;
    public RuntimeAnimatorController defaultAnimator;
    Coroutine animateRoutine;

    [Header("Loadout related stuff")]
    public List<Shop_Item> itemsToPick;

    //Player's current loadout.
    [SerializeField]
    Shop_Item loadoutItem1;
    [SerializeField]
    Shop_Item loadoutItem2;
    [SerializeField]
    Shop_Item loadoutItem3;

    [SerializeField]
    GameObject invLayout;
    //This will be the 3 item holders...making this a list in case we need to make more of them.
    [SerializeField]
    List<GameObject> itemLoadoutHolders;

    bool enteredOnce;


    // Start is called before the first frame update
    void Start()
    {
        enteredOnce = false;
        initializeSkinCustomization();
        initializeLoadoutCustomization();
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


        //switchCurrentSkin();

        loadSkins();

       // animateRoutine = StartCoroutine(animationRandomizer());

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
        if(currentSkinNum == Collect_Manager.instance.getCurrentSkin())
        {
            Debug.Log("hey, we are already wearing this skin!");
            return;
        }

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
        int currentSkinInt = (Collect_Manager.instance.getCurrentSkin());

        //Populate the skin preview on the top based on what the user has currently equipped.
        for (int i = 0; i < skinsToPick.Count; i++)
        {
            if (currentSkinInt == i)
            {
                if(animateRoutine != null)
                {
                    StopCoroutine(animateRoutine);
                }

              
                // currentSkinHolderImage.sprite = skinsToPick[i].skinSprite;
                if(skinsToPick[i].animationOverrideController == null)
                {
                    Debug.Log("Hey we're using the default animatorController...is something wrong?");
                    currentSkinHolderAnimator.runtimeAnimatorController = defaultAnimator;
                }
                else
                {
                    currentSkinHolderAnimator.runtimeAnimatorController = skinsToPick[i].animationOverrideController;
                }
               
 
                break;
            }
        }

        animateRoutine = StartCoroutine(animationRandomizer());

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

    /*
     * ******************************
     * LOADOUT RELATED FUNCTIONS
     * ******************************
     */

    public void initializeLoadoutCustomization()
    {
       //Grab the 3 items from Collect_Manager. If they are null, leave them null and don't update the boxes/turn off the - buttons. If they are filled...Basically call equip in the order of what's filled.
        if(Collect_Manager.instance.item1 >= 0)
        {
            Collect_Manager.typeOfItem tempItem = getItemFromList(Collect_Manager.instance.item1);

            for (int i = 0; i < itemsToPick.Count; i++)
            {
                if(tempItem == itemsToPick[i].theItem)
                {
                    loadoutItem1 = itemsToPick[i];
                    Image tempImg = itemLoadoutHolders[0].transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = loadoutItem1.itemImage;
                }
            }
        }
  
        if (Collect_Manager.instance.item2 >= 0)
        {
            Collect_Manager.typeOfItem tempItem = getItemFromList(Collect_Manager.instance.item2);

            for (int i = 0; i < itemsToPick.Count; i++)
            {
                if (tempItem == itemsToPick[i].theItem)
                {
                    loadoutItem2 = itemsToPick[i];
                    Image tempImg = itemLoadoutHolders[1].transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = loadoutItem2.itemImage;
                }
            }


        }
        if (Collect_Manager.instance.item3 >= 0)
        {
            Collect_Manager.typeOfItem tempItem = getItemFromList(Collect_Manager.instance.item3);

            for (int i = 0; i < itemsToPick.Count; i++)
            {
                if (tempItem == itemsToPick[i].theItem)
                {
                    loadoutItem3 = itemsToPick[i];
                    Image tempImg = itemLoadoutHolders[2].transform.GetChild(0).GetComponent<Image>();
                    tempImg.sprite = loadoutItem3.itemImage;
                }
            }


        }

        //Load the item list below.
        for(int i = 0; i < itemsToPick.Count; i++)
        {
            GameObject tempObj = Instantiate(itemHolderPrefab);
            tempObj.transform.SetParent(invLayout.transform, false);
            Image tempIMG = tempObj.transform.GetChild(0).GetComponent<Image>();
            tempIMG.sprite = itemsToPick[i].itemImage;
            TextMeshProUGUI tempName = tempObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            tempName.text = itemsToPick[i].theName;
            TextMeshProUGUI tempDesc = tempObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            tempDesc.text = itemsToPick[i].description;
            TextMeshProUGUI tempOwnedText = tempObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            tempOwnedText.text = "Owned: \n X " + Collect_Manager.instance.numPlayerOwns(itemsToPick[i].theItem).ToString();
            Button tempButton = tempObj.GetComponent<Button>();
            int tempNum = i;
            tempButton.onClick.AddListener(() => equipItem(itemsToPick[tempNum], tempOwnedText));
        }
    }

   

    public void equipItem(Shop_Item theEquip, TextMeshProUGUI theText)
    {

        if(Collect_Manager.instance.numPlayerOwns(theEquip.theItem) <= 0)
        {
            Debug.Log("You don't own any of this item!");
            return;
        }
        if(loadoutItem1 == null)
        {
            //First item = 0th index.
            Image tempImg = itemLoadoutHolders[0].transform.GetChild(0).GetComponent<Image>();
            tempImg.sprite = theEquip.itemImage;
            loadoutItem1 = theEquip;
            int itemNum = (int)loadoutItem1.theItem;
            Collect_Manager.instance.equipItem(loadoutItem1.theItem);
            theText.text = "Owned: \n X " + Collect_Manager.instance.numPlayerOwns(theEquip.theItem).ToString();
            Collect_Manager.instance.item1 = (int)getItemFromList(itemNum);

            /*
            foreach (Collect_Manager.typeOfItem theItems in System.Enum.GetValues(typeof(Collect_Manager.typeOfItem)))
            {
                if(itemNum == (int)theItems)
                {
                    Collect_Manager.instance.item1 = itemNum;
                    Debug.Log("The item we matched with was: " + theItems + " We are assigning that to " + loadoutItem1.name);
                }
            }
            */
        }
        else if(loadoutItem2 == null)
        {

        }
        else if(loadoutItem3 == null)
        {

        }
        else
        {
            //Don't load anything because we filled all 3 slots.
        }

    }

    //Grabs the item from the enum list we have created in Collect_Manager.
    public Collect_Manager.typeOfItem getItemFromList(int num)
    {
        foreach (Collect_Manager.typeOfItem theItems in System.Enum.GetValues(typeof(Collect_Manager.typeOfItem)))
        {
            if (num == (int)theItems)
            {
                return theItems;
            }
        }
        Debug.LogWarning("We got to the end of the loop and couldn't find the corresponding item...Is something wrong?");
        return Collect_Manager.typeOfItem.Defroster;
    }


    /*
    * ******************************
    * LOADOUT RELATED FUNCTIONS
    * ******************************
    */


}

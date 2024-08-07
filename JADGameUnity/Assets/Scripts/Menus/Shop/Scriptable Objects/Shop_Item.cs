//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

//This is to set up the scriptable objects for shop items for the user to buy.


using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu]
public class Shop_Item : ScriptableObject
{
    [Header("Items")]
    [Tooltip("The image we will use to show the user what the item looks like.")]
    public Sprite itemImage;

    [Tooltip("The name of the item.")]
    public string theName;

    [Tooltip("Describe what the item does.")]
    public string description;

    [Tooltip("The price of the item.")]
    public int coinPrice;

    [Tooltip("What item is this?")]
    public Collect_Manager.typeOfItem theItem;

    [Header("Skins")]
    [Tooltip("If it costs bolt, the price in bolt of the item. Leave this as 0 otherwise.")]
    public int boltPrice;

    [Tooltip("If this is a skin, we need a value corresponding to it. THESE MUST BE UNIQUE!!!! Otherwise leave this as 0.")]
    public Collect_Manager.skinTypes thisSkinType;


    [Header("Currency")]
    [Tooltip("For currency only. What is the price in USD going to be? [We should determine this ourselves]")]
    public float currencyUSD;


    //GAMEPLAY VARIABLES!

    [Header("Gameplay related")]
    [Tooltip("ONLY APPLICABLE TO GAMEPLAY RELATED ITEMS: If the item has a duration and isn't an instant use, check this.")]
    public bool hasDuration;
    [Tooltip("If the item has a duration , this is where we set it: Otherwise leave this as 0.")]
    public float duration;
    [Tooltip("Cooldown. The item will have a cooldown that means it needs to wait x amount of waves before being used again. If this is 0, item is gone after 1 use.")]
    public int waveCooldown;
    [Tooltip("Item sound to be played when item is used in gameplay.")]
    public AudioClip useSound;

    //GAMEPLAY VARIABLES!
    //Might want a way to eliminate stuff we can only buy once like no ads after player has bought it.
}

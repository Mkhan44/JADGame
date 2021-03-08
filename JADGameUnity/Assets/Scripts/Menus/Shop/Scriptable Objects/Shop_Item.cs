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
    [Tooltip("The image we will use to show the user what the item looks like.")]
    public Sprite itemImage;

    [Tooltip("The name of the item.")]
    public string theName;

    [Tooltip("Describe what the item does.")]
    public string description;

    [Tooltip("The price of the item.")]
    public int coinPrice;

    [Tooltip("If it costs gems, the price in gems of the item. Leave this as 0 otherwise.")]
    public int gemPrice;

    [Tooltip("This string corresponds to the one that is being used in Collect_Manager script to tell how many of the item the player currently has. NEEDS TO MATCH EXACTLY!!!!")]
    public string collectManagerNumString;
}

//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

//This scriptable holds the information we will need to swap out skins on the fly.


using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu]
public class SkinInfo : ScriptableObject
{
    [Tooltip("Sprite that the skin will use.")]
    public Sprite skinSprite;

    [Tooltip("There should be an override controller that this skin will be using. This is for the preview.")]
    public AnimatorOverrideController animationOverrideController;

    public Collect_Manager.skinTypes thisSkinType;

    [Tooltip("The portrait that will be used for the button.")]
    public Sprite skinIcon;

    [Tooltip("Shop info for this skin.")]
    public Shop_Item shopInfo;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EXT_Links : MonoBehaviour
{
    public string thePlatform;
    public TextMeshProUGUI noticeText;

    public void setPlat(string platform)
    {
        thePlatform = platform;
        //loadPage();
        noticeText.text = "Would you like to visit our " + platform + " page?";
    }

    public void loadPage()
    {
        switch (thePlatform)
        {
            case "Facebook":
                {
                    Application.OpenURL("https://www.facebook.com/Bukugames");
                    break;
                }
            case "Twitter":
                {
                    Application.OpenURL("https://twitter.com/buku_games");
                    break;
                }
            case "Instagram":
                {
                    Application.OpenURL("https://www.instagram.com/bukugames/");
                    break;
                }
            case "YouTube":
                {
                    Application.OpenURL("https://www.youtube.com/channel/UCVMp9BDVF6Bh7R8jG2R9Wqg?");
                    break;
                }
            case "website":
                {
                    Application.OpenURL("https://buku-games.com/");
                    break;
                }
            default:
                {
                    Debug.LogWarning("Invalid string!");
                    break;
                }

        }
    }

    public void loadLicenses()
    {
        Application.OpenURL("https://github.com/Mkhan44/JADGame/blob/main/RPRT_Licenses");
    }
}

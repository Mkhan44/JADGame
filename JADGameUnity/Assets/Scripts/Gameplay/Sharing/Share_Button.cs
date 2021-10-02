using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Share_Button : MonoBehaviour
{
    private string shareMessage;
  

    public void clickShareButton()
    {
        int score = Level_Manager.Instance.getScore();
        //WILL NEED TO GET THE LINKS FOR IOS AND ANDROID STORES.
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            shareMessage = "Sweet! I got " + score + " Research Points (RP) in Right Place Right Time! \n download on the App store: https://apps.apple.com/us/app/right-place-right-time/id1565277143";
        }
        else
        {
            shareMessage = "Sweet! I got " + score + " Research Points (RP) in Right Place Right Time! \n download on Google Play: https://play.google.com/store/apps/details?id=com.BukuGames.RightPlaceRightTime";
        }
        
        StartCoroutine(TakeScreenshotAndShare());
    }
    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Life's a drag").SetText(shareMessage)
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();

        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }
}

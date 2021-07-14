//Code written by Mohamed Riaz Khan of BukuGames.
//All code is written by me (Above name) unless otherwise stated via comments below.
//Not authorized for use outside of the Github repository of this Mobile game developed by BukuGames.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Canvas_Resolution : MonoBehaviour
{



    public float resoX;
    public float resoY;

    private CanvasScaler can;
    
    // Start is called before the first frame update
    void Start()
    {
        can = GetComponent<CanvasScaler>();
        setInfo();
    }


    void setInfo()
    {
        /*
#if UNITY_EDITOR
        resoX = 1440f;
        resoY = 2560f;
        can.referenceResolution = new Vector2(resoX, resoY);
        return;
#endif
        */
        resoX = (float)Screen.currentResolution.width;
        resoY = (float)Screen.currentResolution.height;

        can.referenceResolution = new Vector2(resoX, resoY);
    }
  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child_Sound : MonoBehaviour
{
   public Obstacle_Behaviour myParent;

    // Start is called before the first frame update
    void Start()
    {
        if(myParent == null)
        {
            myParent = this.transform.parent.GetComponent<Obstacle_Behaviour>();
        }
       
    }


    public void playSoundFromParent(float overrideVol)
    {
        if(overrideVol == 0)
        {
            myParent.playSoundExternally(0f);
        }
        else
        {
            myParent.playSoundExternally(overrideVol);
        }
    }

    public void playExtraSoundFromParent(AudioClip theSoundToPlay)
    {
        myParent.playExtraSound(theSoundToPlay);
    }
}

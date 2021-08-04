using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.Editor.Android
{
    public abstract class AppComponent : Element
    {
        public void Add(IntentFilter element)
        {
            base.Add(element);
        }

        public void Add(MetaData element)
        {
            base.Add(element);
        }
    }
}
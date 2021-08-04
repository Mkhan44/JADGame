using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.Editor.Android
{
    public class Feature : Element
    {
        protected override string GetName()
        {
            return "uses-feature";
        }
    }
}

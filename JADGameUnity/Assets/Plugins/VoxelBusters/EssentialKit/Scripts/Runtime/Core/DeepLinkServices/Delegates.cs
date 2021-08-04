using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore
{
    public delegate void DynamicLinkOpenInternalCallback(string url);

    public delegate bool CanHandleDynamicLinkInternal(string url);
}
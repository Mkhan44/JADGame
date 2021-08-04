#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.CloudServicesCore.iOS
{
    internal enum CKAccountStatus : long
    {
        CKAccountStatusCouldNotDetermine    = 0,

        CKAccountStatusAvailable            = 1,

        CKAccountStatusRestricted           = 2,

        CKAccountStatusNoAccount            = 3,
    }
}
#endif
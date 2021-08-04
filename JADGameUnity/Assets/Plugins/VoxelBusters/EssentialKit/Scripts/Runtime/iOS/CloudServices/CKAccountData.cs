#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.CloudServicesCore.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct CKAccountData
    {
        #region Properties

        public IntPtr AccountIdentifierPtr
        { 
            get; 
            private set; 
        }

        #endregion
    }
}
#endif
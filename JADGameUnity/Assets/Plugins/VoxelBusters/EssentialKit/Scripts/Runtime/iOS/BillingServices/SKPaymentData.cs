#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SKPaymentData
    {
        #region Properties

        public IntPtr NativeObjectPtr
        {
            get;
            internal set;
        }

        public IntPtr ProductIdentifierPtr
        {
            get;
            internal set;
        }

        public int Quantity
        {
            get;
            internal set;
        }

        public IntPtr ApplicationUsernamePtr
        {
            get;
            internal set;
        }

        #endregion
    }
}
#endif
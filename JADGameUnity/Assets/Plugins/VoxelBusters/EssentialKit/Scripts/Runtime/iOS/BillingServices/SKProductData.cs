#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SKProductData
    {
        #region Properties

        public IntPtr NativeObjectPtr
        {
            get;
            internal set;
        }

        public IntPtr IdentifierPtr
        {
            get;
            internal set;
        }

        public IntPtr LocalizedTitlePtr
        {
            get;
            internal set;
        }

        public IntPtr LocalizedDescriptionPtr
        {
            get;
            internal set;
        }

        public double Price
        {
            get;
            internal set;
        }

        public IntPtr LocalizedPricePtr
        {
            get;
            internal set;
        }

        public IntPtr CurrencyCodePtr
        {
            get;
            internal set;
        }

        public IntPtr CurrencySymbolPtr
        {
            get;
            internal set;
        }

        #endregion
    }
}
#endif
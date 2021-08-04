#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SKPaymentTransactionData
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

        public SKPaymentData PaymentData
        {
            get;
            internal set;
        }

        public IntPtr DatePtr
        {
            get;
            internal set;
        }

        public SKPaymentTransactionState TransactionState
        {
            get;
            internal set;
        }

        public IntPtr ReceiptDataPtr
        {
            get;
            internal set;
        }

        public IntPtr ErrorPtr
        {
            get;
            internal set;
        }

        #endregion
    }
}
#endif
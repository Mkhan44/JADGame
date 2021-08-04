using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeArray
    {
        #region Properties

        public IntPtr Pointer
        {
            get;
            set;
        }

        public int Length
        {
            get;
            set;
        }

        #endregion
    }
}
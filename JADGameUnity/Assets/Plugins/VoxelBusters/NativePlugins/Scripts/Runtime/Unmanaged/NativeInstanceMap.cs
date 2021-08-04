using System;
using System.Collections.Generic;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    internal static class NativeInstanceMap
    {
        #region Static fields

        private static Dictionary<IntPtr, object> instanceMap;

        #endregion

        #region Constructors

        static NativeInstanceMap()
        {
            // set properties
            instanceMap = new Dictionary<IntPtr, object>(capacity: 4);
        }

        #endregion

        #region Static methods

        public static void AddInstance(IntPtr nativePtr, object owner)
        {
            instanceMap.Add(nativePtr, owner);
        }

        public static void RemoveInstance(IntPtr nativePtr)
        {
            instanceMap.Remove(nativePtr);
        }

        public static T GetOwner<T>(IntPtr nativePtr) where T : class
        {
            object owner;
            instanceMap.TryGetValue(nativePtr, out owner);

            return owner as T;
        }

        #endregion
    }
}
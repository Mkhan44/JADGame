using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal static class ManagedObjectReferencePool
    {
        #region Static fields

        private     static      List<object>        s_objectList        = new List<object>(capacity: 8);

        #endregion

        #region Static methods

        public static void Retain(object obj)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(obj, "obj");

            s_objectList.Add(obj);
        }

        public static void Release(object obj)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(obj, "obj");

            s_objectList.Remove(obj);
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class Assertions
    {
        #region Static methods

        public static void AssertIfNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfNotNull(object obj, string message)
        {
            if (obj != null)
            {
                throw new VBException(message);
            }
        }
        
        public static void AssertIfPropertyIsNull(object obj, string property)
        {
            if (obj == null)
            {
                throw new VBException(string.Format("{0} is null.", property));
            }
        }

        public static void AssertIfArgIsNull(object obj, string argName)
        {
            if (obj == null)
            {
                throw new VBException(string.Format("Arg {0} is null.", argName));
            }
        }

        public static void AssertIfSame(object obj1, object obj2, string message)
        {
            if (obj1 == obj2)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfEqual<T>(T value, T target, string message)
        {
            if (EqualityComparer<T>.Default.Equals(value, target))
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfNotEqual<T>(T value, T target, string message)
        {
            if (!EqualityComparer<T>.Default.Equals(value, target))
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfTrue(bool status, string message)
        {
            if (status)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfFalse(bool status, string message)
        {
            if (!status)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfLess(int value, int target, string message)
        {
            if (value < target)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfZero(int value, string message)
        {
            AssertIfEqual(value, 0, message);
        }

        public static void AssertIfEqual(int value, int target, string message)
        {
            if (value == target)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfLessThanOrEqual(int value, int target, string message)
        {
            if (value <= target)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfGreaterThanOrEqual(int value, int target, string message)
        {
            if (value >= target)
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfStringIsNullOrEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new VBException(message);
            }
        }

        public static void AssertIfArrayIsNull<T>(T[] array, string name)
        {
            if (array == null)
            {
                throw new VBException(string.Format("{0} is null.", name));
            }
        }
            
        public static void AssertIfArrayIsNullOrEmpty<T>(T[] array, string name)
        {
            if (array == null)
            {
                throw new VBException(string.Format("{0} is null.", name));
            }
            if (0 == array.Length)
            {
                throw new VBException(string.Format("{0} is empty.", name));
            }
        }

        #endregion
    }
}
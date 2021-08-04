using System;
using System.Reflection;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class ReflectionUtility
    {
        #region Constants

        public  const   string  kCSharpFirstPassAssembly    = "Assembly-CSharp-firstpass";

        public  const   string  kCSharpAssembly             = "Assembly-CSharp";

        #endregion

        #region Static methods

        public static Type GetTypeFromCSharpAssembly(string typeName)
        {
            return GetType(kCSharpAssembly, typeName);
        }

        public static Type GetTypeFromCSharpFirstPassAssembly(string typeName)
        {
            return GetType(kCSharpFirstPassAssembly, typeName);
        }

        public static Type GetType(string assemblyName, string typeName)
        {
            var targetAssembly  = FindAssemblyWithName(assemblyName);
            if (targetAssembly != null)
            {
                return targetAssembly.GetType(typeName, false);
            }

            return null;
        }

        public static Assembly FindAssemblyWithName(string assemblyName)
        {
            return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), (item) =>
            {
                return string.Equals(item.GetName().Name, assemblyName);
            });
        }

        public static T InvokeStaticMethod<T>(this Type type, string method, params object[] parameters)
        {
            return (T)type.GetMethod(method, BindingFlags.Public | BindingFlags.Static).Invoke(null, parameters);
        }

        #endregion
    }
}
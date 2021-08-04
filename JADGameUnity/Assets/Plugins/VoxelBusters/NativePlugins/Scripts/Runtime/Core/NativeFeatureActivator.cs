using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    public class NativeFeatureActivator
    {
        #region Static properties

        public static INativeFeatureInterfaceProvider InterfaceProvider { get; set; }

        #endregion

        #region Static methods

        public static TFeatureInterface CreateInterface<TFeatureInterface>(NativeFeaturePackageConfiguration packageConfiguration, bool isAvailable, params object[] args) where TFeatureInterface : INativeFeatureInterface
        {
            Assertions.AssertIfArgIsNull(packageConfiguration, "packageConfiguration");

            object  interfaceObject         = null;
            if (isAvailable)
            {
                var     currentPlatform     = Application.platform;
                
                // try creating interface object using custom interface provider
                if (InterfaceProvider != null)
                {
                    interfaceObject         = InterfaceProvider.CreateInterface(typeof(TFeatureInterface), currentPlatform);
                }

                // incase if no implementation is found, system retracts to default activation methods
                if (interfaceObject == null)
                {
                    var     packageType     = packageConfiguration.GetPackageForPlatform(currentPlatform);
                    if (packageType != null)
                    {
                        interfaceObject     = CreateInstance(packageType.Assembly, packageType.NativeInterfaceType, args);
                    }
                }
            }

            // fallback case, create default type object incase if specified type is not found
            if (interfaceObject == null)
            {
                var     packageType     = packageConfiguration.FallbackPackage;
                interfaceObject         = CreateInstance(packageType.Assembly, packageType.NativeInterfaceType, args);
            }

            DebugLogger.LogFormat("Created native interface of type: {0}", interfaceObject);
            return (TFeatureInterface)interfaceObject;
        }

        #endregion

        #region Private static methods

        private static object CreateInstance(string assemblyName, string typeName, object[] arguments)
        {
            try
            {
                Type    targetType  = ReflectionUtility.GetType(assemblyName, typeName);
                if (targetType == null)
                {
                    targetType      = ReflectionUtility.GetTypeFromCSharpFirstPassAssembly(typeName);
                    if (targetType == null)
                    {
                        targetType  = ReflectionUtility.GetTypeFromCSharpAssembly(typeName);
                    }
                }

                if (arguments == null)
                {
                    return Activator.CreateInstance(targetType);
                }
                else
                {
                    return Activator.CreateInstance(targetType, arguments);
                }
            }
            catch (Exception e)
            {
                DebugLogger.LogError(string.Format("Failed when creating instance [Assembly : {0}] [Type : {1}] [Error : {2}", assemblyName, typeName, e.Message));
                return null;
            }
        }

        #endregion
    }
}

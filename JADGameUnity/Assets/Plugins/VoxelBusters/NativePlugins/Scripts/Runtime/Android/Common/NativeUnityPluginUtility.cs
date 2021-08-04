#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using System;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeUnityPluginUtility
    {
        private static AndroidJavaObject    s_context           = null;
        private static NativeContext        s_nativeContext     = null;
        private static NativeActivity       s_nativeActivity    = null;

        /*private static Dictionary<string, AndroidJavaObject> sSingletonInstances = new Dictionary<string, AndroidJavaObject>();

        public static AndroidJavaObject GetSingletonInstance(string _className, string _methodName = "getInstance") //Assuming the class follows standard naming- "INSTANCE" for singleton objects
        {
            AndroidJavaObject _instance;

            sSingletonInstances.TryGetValue(_className, out _instance);

            if (_instance == null)
            {
                //Create instance
                AndroidJavaClass _class = new AndroidJavaClass(_className);

                if (_class != null) //If it doesn't exist, throw an error
                {
                    _instance = _class.CallStatic<AndroidJavaObject>(_methodName);

                    //Add the new instance value for this class name key
                    sSingletonInstances.Add(_className, _instance);
                }
                else
                {
                    DebugLogger.LogError("Class=" + _className + " not found!");
                    return null;
                }

            }

            return _instance;
        }

        public static AndroidJavaClass CreateJavaClass(string className)
        {
            AndroidJavaClass javaClass;

            //Create instance
            javaClass = new AndroidJavaClass(className);

            if (javaClass == null) //If it doesn't exist, throw an error
            {
                DebugLogger.LogError("Class=" + className + " not found!");
            }

            return javaClass;
        }

        public static AndroidJavaObject CreateJavaInstance(string className, bool passContext = true, params object[] arguments)
        {
            AndroidJavaObject instance;

            //Create instance
            if (passContext)
            {
                instance = new AndroidJavaObject(className, GetContext(), arguments);
            }
            else
            {
                instance = new AndroidJavaObject(className, arguments);
            }

            if (instance == null) //If it doesn't exist, throw an error
            {
                DebugLogger.LogError("Unable to create instance for class : "+ className);
            }

            return instance;
        }*/

        public static NativeActivity GetActivity()
        {
            if(s_nativeContext == null)
            {
                s_nativeActivity = new NativeActivity(GetUnityActivity());
            }
            return s_nativeActivity;
        }

        public static NativeContext GetContext()
        {
            if (s_nativeContext == null)
            {
                s_nativeContext = new NativeContext(GetUnityActivity());
            }
            return s_nativeContext;
        }

        public static To[] Map<From, To>(List<From> fromList)
        {
            List<To> list = new List<To>();
            foreach (From each in fromList)
            {
                list.Add((To)Activator.CreateInstance(typeof(To), new object[] { each }));
            }

            return list.ToArray();
        }

        private static AndroidJavaObject GetUnityActivity()
        {
            if (s_context == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                s_context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return s_context;
        }
    }
}
#endif
      
#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore.Android
{
    public class NativeDeepLinkRequestListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnOpenDelegate(string url);

        #endregion

        #region Public callbacks

        public OnOpenDelegate  onOpenCallback;

        #endregion


        #region Constructors

        public NativeDeepLinkRequestListener() : base("com.voxelbusters.android.essentialkit.features.deeplinkservices.IDeepLinkServices$IDeepLinkRequestListener")
        {
        }

        #endregion


        #region Public methods
#if NATIVE_PLUGINS_DEBUG_ENABLED
        public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
        {
            DebugLogger.Log("**************************************************");
            DebugLogger.Log("[Generic Invoke : " +  methodName + "]" + " Args Length : " + (javaArgs != null ? javaArgs.Length : 0));
            if(javaArgs != null)
            {
                System.Text.StringBuilder builder = new System.Text.StringBuilder();

                foreach(AndroidJavaObject each in javaArgs)
                {
                    if(each != null)
                    {
                        builder.Append(string.Format("[Type : {0} Value : {1}]", each.Call<AndroidJavaObject>("getClass").Call<string>("getName"), each.Call<string>("toString")));
                        builder.Append("\n");
                    }
                    else
                    {
                        builder.Append("[Value : null]");
                        builder.Append("\n");
                    }
                }

                DebugLogger.Log(builder.ToString());
            }
            DebugLogger.Log("-----------------------------------------------------");
            return base.Invoke(methodName, javaArgs);
        }
#endif

        public void onOpen(string url)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onOpen"  + " " + "[" + "url" + " : " + url +"]");
#endif
            if(onOpenCallback != null)
            {
                onOpenCallback(url);
            }
        }

        #endregion
    }
}
#endif
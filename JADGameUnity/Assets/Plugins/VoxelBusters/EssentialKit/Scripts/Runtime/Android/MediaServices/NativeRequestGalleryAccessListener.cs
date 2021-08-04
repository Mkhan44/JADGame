#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Android
{
    public class NativeRequestGalleryAccessListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnCompleteDelegate(NativeGalleryAccessStatus status);

        #endregion

        #region Public callbacks

        public OnCompleteDelegate  onCompleteCallback;

        #endregion


        #region Constructors

        public NativeRequestGalleryAccessListener() : base("com.voxelbusters.android.essentialkit.features.mediaservices.IMediaServices$IRequestGalleryAccessListener")
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

        public void onComplete(AndroidJavaObject status)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onComplete"  + " " + "[" + "status" + " : " + status +"]");
#endif
            if(onCompleteCallback != null)
            {
                onCompleteCallback(NativeGalleryAccessStatusHelper.ReadFromValue(status));
            }
        }

        #endregion
    }
}
#endif
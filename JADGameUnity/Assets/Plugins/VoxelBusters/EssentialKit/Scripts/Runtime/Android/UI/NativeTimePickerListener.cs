#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.UICore.Android
{
    public class NativeTimePickerListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnSuccessDelegate(int hourOfDay, int minutes);
        public delegate void OnCancelDelegate();

        #endregion

        #region Public callbacks

        public OnSuccessDelegate  onSuccessCallback;
        public OnCancelDelegate  onCancelCallback;

        #endregion


        #region Constructors

        public NativeTimePickerListener() : base("com.voxelbusters.android.essentialkit.features.uiviews.IUiViews$ITimePickerListener")
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

        public void onSuccess(int hourOfDay, int minutes)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onSuccess"  + " " + "[" + "hourOfDay" + " : " + hourOfDay +"]" + " " + "[" + "minutes" + " : " + minutes +"]");
#endif
            if(onSuccessCallback != null)
            {
                onSuccessCallback(hourOfDay, minutes);
            }
        }
        public void onCancel()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onCancel" );
#endif
            if(onCancelCallback != null)
            {
                onCancelCallback();
            }
        }

        #endregion
    }
}
#endif
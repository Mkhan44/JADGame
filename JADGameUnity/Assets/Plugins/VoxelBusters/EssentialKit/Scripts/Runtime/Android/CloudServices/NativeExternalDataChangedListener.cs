#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.CloudServicesCore.Android
{
    public class NativeExternalDataChangedListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnChangeDelegate(NativeExternalChangeReason reason, string[] keys, NativeJSONObject localCopy);

        #endregion

        #region Public callbacks

        public OnChangeDelegate  onChangeCallback;

        #endregion


        #region Constructors

        public NativeExternalDataChangedListener() : base("com.voxelbusters.android.essentialkit.features.cloudservices.ICloudServices$IExternalDataChangedListener")
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

        public void onChange(AndroidJavaObject reason, string[] keys, AndroidJavaObject localCopy)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onChange"  + " " + "[" + "reason" + " : " + reason +"]" + " " + "[" + "keys" + " : " + keys +"]" + " " + "[" + "localCopy" + " : " + localCopy +"]");
#endif
            if(onChangeCallback != null)
            {
                onChangeCallback(NativeExternalChangeReasonHelper.ReadFromValue(reason), keys, new NativeJSONObject(localCopy));
            }
        }

        #endregion
    }
}
#endif
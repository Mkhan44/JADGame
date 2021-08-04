#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public class NativeLoadServerCredentials : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnSuccessDelegate(string serverAuthCode, string idToken, string email);
        public delegate void OnFailureDelegate(string error);

        #endregion

        #region Public callbacks

        public OnSuccessDelegate  onSuccessCallback;
        public OnFailureDelegate  onFailureCallback;

        #endregion


        #region Constructors

        public NativeLoadServerCredentials() : base("com.voxelbusters.android.essentialkit.features.gameservices.IGameServices$ILoadServerCredentials")
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

        public void onSuccess(string serverAuthCode, string idToken, string email)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onSuccess"  + " " + "[" + "serverAuthCode" + " : " + serverAuthCode +"]" + " " + "[" + "idToken" + " : " + idToken +"]" + " " + "[" + "email" + " : " + email +"]");
#endif
            if(onSuccessCallback != null)
            {
                onSuccessCallback(serverAuthCode, idToken, email);
            }
        }
        public void onFailure(string error)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onFailure"  + " " + "[" + "error" + " : " + error +"]");
#endif
            if(onFailureCallback != null)
            {
                onFailureCallback(error);
            }
        }

        #endregion
    }
}
#endif
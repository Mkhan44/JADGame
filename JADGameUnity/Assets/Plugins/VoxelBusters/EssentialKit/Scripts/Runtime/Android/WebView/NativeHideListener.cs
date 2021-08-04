#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.WebViewCore.Android
{
    public class NativeHideListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnHideDelegate();

        #endregion

        #region Public callbacks

        public OnHideDelegate  onHideCallback;

        #endregion


        #region Constructors

        public NativeHideListener() : base("com.voxelbusters.essentialkit.features.webview.IHideListener")
        {
        }

        #endregion


        #region Public methods

        public override AndroidJavaObject Invoke(string methodName, AndroidJavaObject[] javaArgs)
        {
            Debug.Log("**************************************************");
            Debug.Log("[Generic Invoke : " +  methodName + "]" + " Args Length : " + (javaArgs != null ? javaArgs.Length : 0));
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

                Debug.Log(builder.ToString());
            }
            Debug.Log("-----------------------------------------------------");
            return base.Invoke(methodName, javaArgs);
        }

        public void onHide()
        {
            Debug.Log("[Proxy : Callback] : " + "onHide" );
            if(onHideCallback != null)
            {
                onHideCallback();
            }
        }

        #endregion
    }
}
#endif
#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.WebViewCore.Android
{
    public class NativeWebViewListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnPageLoadStartedDelegate();
        public delegate void OnPageLoadFinishedDelegate(string url);
        public delegate void OnPageLoadErrorDelegate(string failingUrl, string description);
        public delegate void OnMessageReceivedDelegate(NativeWebViewMessage message);
        public delegate void OnHideDelegate();
        public delegate void OnShowDelegate();

        #endregion

        #region Public callbacks

        public OnPageLoadStartedDelegate  onPageLoadStartedCallback;
        public OnPageLoadFinishedDelegate  onPageLoadFinishedCallback;
        public OnPageLoadErrorDelegate  onPageLoadErrorCallback;
        public OnMessageReceivedDelegate  onMessageReceivedCallback;
        public OnHideDelegate  onHideCallback;
        public OnShowDelegate  onShowCallback;

        #endregion


        #region Constructors

        public NativeWebViewListener() : base("com.voxelbusters.android.essentialkit.features.webview.IWebViewListener")
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

        public void onPageLoadStarted()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onPageLoadStarted" );
#endif
            if(onPageLoadStartedCallback != null)
            {
                onPageLoadStartedCallback();
            }
        }
        public void onPageLoadFinished(string url)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onPageLoadFinished"  + " " + "[" + "url" + " : " + url +"]");
#endif
            if(onPageLoadFinishedCallback != null)
            {
                onPageLoadFinishedCallback(url);
            }
        }
        public void onPageLoadError(string failingUrl, string description)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onPageLoadError"  + " " + "[" + "failingUrl" + " : " + failingUrl +"]" + " " + "[" + "description" + " : " + description +"]");
#endif
            if(onPageLoadErrorCallback != null)
            {
                onPageLoadErrorCallback(failingUrl, description);
            }
        }
        public void onMessageReceived(AndroidJavaObject message)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onMessageReceived"  + " " + "[" + "message" + " : " + message +"]");
#endif
            if(onMessageReceivedCallback != null)
            {
                onMessageReceivedCallback(new NativeWebViewMessage(message));
            }
        }
        public void onHide()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onHide" );
#endif
            if(onHideCallback != null)
            {
                onHideCallback();
            }
        }
        public void onShow()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Proxy : Callback] : " + "onShow" );
#endif
            if(onShowCallback != null)
            {
                onShowCallback();
            }
        }

        #endregion
    }
}
#endif
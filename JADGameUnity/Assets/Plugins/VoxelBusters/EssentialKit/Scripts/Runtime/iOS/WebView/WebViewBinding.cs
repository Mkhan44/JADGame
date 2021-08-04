#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.WebViewCore.iOS
{
    internal static class WebViewBinding 
    {
        [DllImport("__Internal")]
        public static extern void NPWebViewRegisterCallbacks(WebViewNativeCallback onShowCallback,
                                                            WebViewNativeCallback onHideCallback,
                                                            WebViewNativeCallback loadStartCallback,
                                                            WebViewNativeCallback loadFinishCallback,
                                                            WebViewRunJavaScriptNativeCallback runJavaScriptFinishCallback,
                                                            WebViewURLSchemeMatchFoundNativeCallback urlMatchFoundCallback);

        [DllImport("__Internal")]
        public static extern IntPtr NPWebViewCreate();

        [DllImport("__Internal")]
        public static extern string NPWebViewGetURL(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern string NPWebViewGetTitle(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern UnityRect NPWebViewGetNormalizedRect(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewSetNormalizedRect(IntPtr nativePtr, UnityRect rect);

        [DllImport("__Internal")]
        public static extern WebViewStyle NPWebViewGetStyle(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewSetStyle(IntPtr nativePtr, WebViewStyle style);

        [DllImport("__Internal")]
        public static extern bool NPWebViewGetScalesPageToFit(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewSetScalesPageToFit(IntPtr nativePtr, bool value);

        [DllImport("__Internal")]
        public static extern bool NPWebViewGetCanBounce(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewSetCanBounce(IntPtr nativePtr, bool value);

        [DllImport("__Internal")]
        public static extern UnityColor NPWebViewGetBackgroundColor(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewSetBackgroundColor(IntPtr nativePtr, UnityColor color);

        [DllImport("__Internal")]
        public static extern double NPWebViewGetProgress(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern bool NPWebViewGetIsLoading(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern bool NPWebViewGetJavaScriptEnabled(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewSetJavaScriptEnabled(IntPtr nativePtr, bool enabled);

        [DllImport("__Internal")]
        public static extern void NPWebViewShow(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewHide(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewLoadURL(IntPtr nativePtr, string urlStr);

        [DllImport("__Internal")]
        public static extern void NPWebViewLoadHtmlString(IntPtr nativePtr, string htmlStr, string baseUrl);

        [DllImport("__Internal")]
        public static extern void NPWebViewLoadData(IntPtr nativePtr, UnityAttachment attachmentData, string encodingName, string baseUrl);

        [DllImport("__Internal")]
        public static extern void NPWebViewReload(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewStopLoading(IntPtr nativePtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewRunJavaScript(IntPtr nativePtr, string script, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPWebViewAddURLScheme(IntPtr nativePtr, string scheme);

        [DllImport("__Internal")]
        public static extern void NPWebViewClearCache();
    }
}
#endif
#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.WebViewCore.iOS
{
    internal delegate void WebViewNativeCallback(IntPtr nativePtr, string error);

    internal delegate void WebViewRunJavaScriptNativeCallback(IntPtr nativePtr, string result, string error, IntPtr tagPtr);

    internal delegate void WebViewURLSchemeMatchFoundNativeCallback(IntPtr nativePtr, string url);
}
#endif
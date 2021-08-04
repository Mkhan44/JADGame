#if UNITY_ANDROID
using System;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.WebViewCore.Android
{
    internal static class Converter
    {
        public static NativeWebKitWebViewStyle from(WebViewStyle style)
        {
            switch(style)
            {
                case WebViewStyle.Default:
                    return NativeWebKitWebViewStyle.Default;
                case WebViewStyle.Popup:
                    return NativeWebKitWebViewStyle.Popup;
                case WebViewStyle.Browser:
                    return NativeWebKitWebViewStyle.ToolBar;
                default:
                    return NativeWebKitWebViewStyle.Default;
            }
        }
    }
}
#endif

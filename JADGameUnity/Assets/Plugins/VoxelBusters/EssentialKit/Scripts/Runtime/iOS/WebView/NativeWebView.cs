#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.WebViewCore.iOS
{
    public sealed class NativeWebView : NativeWebViewBase, INativeWebView
    {
        #region Constructors

        static NativeWebView()
        {
            // register for events
            WebViewBinding.NPWebViewRegisterCallbacks(HandleOnShowNativeCallback, HandleOnHideNativeCallback, HandleLoadStartNativeCallback, HandleLoadFinishNativeCallback, HandleRunJavaScriptNativeCallback, HandleURLSchemeMatchFoundNativeCallback);
        }

        public NativeWebView()
            : base(isAvailable: true)
        {
            // create instance
            var     nativePtr   = WebViewBinding.NPWebViewCreate();

            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);

            // add to collection to map action
            NativeInstanceMap.AddInstance(nativePtr, this);
        }

        ~NativeWebView()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        public override string GetURL()
        {
            return WebViewBinding.NPWebViewGetURL(AddrOfNativeObject());
        }

        public override string GetTitle()
        {
            return WebViewBinding.NPWebViewGetTitle(AddrOfNativeObject());
        }

        public override void SetFrame(Rect value)
        {
            UnityRect   rect    = (UnityRect)value;
            rect.X             /= Screen.width;
            rect.Y             /= Screen.height;
            rect.Width         /= Screen.width;
            rect.Height        /= Screen.height;
            WebViewBinding.NPWebViewSetNormalizedRect(AddrOfNativeObject(), rect);
        }

        public override void SetStyle(WebViewStyle style)
        {
            WebViewBinding.NPWebViewSetStyle(AddrOfNativeObject(), style);
        }

        public override void SetScalesPageToFit(bool value)
        {
            WebViewBinding.NPWebViewSetScalesPageToFit(AddrOfNativeObject(), value);
        }

        public override void SetCanBounce(bool value)
        {
            WebViewBinding.NPWebViewSetCanBounce(AddrOfNativeObject(), value);
        }

        public override void SetBackgroundColor(Color value)
        {
            WebViewBinding.NPWebViewSetBackgroundColor(AddrOfNativeObject(), (UnityColor)value);
        }

        public override double GetProgress()
        {
            return WebViewBinding.NPWebViewGetProgress(AddrOfNativeObject());
        }

        public override bool GetIsLoading()
        {
            return WebViewBinding.NPWebViewGetIsLoading(AddrOfNativeObject());
        }

        public override void SetJavaScriptEnabled(bool value)
        { 
            WebViewBinding.NPWebViewSetJavaScriptEnabled(AddrOfNativeObject(), value);
        }

        public override void Show()
        {
            WebViewBinding.NPWebViewShow(AddrOfNativeObject());
        }

        public override void Hide()
        {
            WebViewBinding.NPWebViewHide(AddrOfNativeObject());
        }

        public override void LoadURL(string url)
        {
            WebViewBinding.NPWebViewLoadURL(AddrOfNativeObject(), url);
        }

        public override void LoadHtmlString(string htmlString, string baseURL)
        {
            WebViewBinding.NPWebViewLoadHtmlString(AddrOfNativeObject(), htmlString, baseURL);
        }

        public override void LoadData(byte[] data, string mimeType, string textEncodingName, string baseURL)
        {
            GCHandle            handle      = GCHandle.Alloc(data, GCHandleType.Pinned);
            UnityAttachment     attachment  = new UnityAttachment()
            {
                DataArrayLength = data.Length,
                DataArrayPtr    = handle.AddrOfPinnedObject(),
                MimeTypePtr     = Marshal.StringToHGlobalAuto(mimeType),
            };

            WebViewBinding.NPWebViewLoadData(AddrOfNativeObject(), attachment, textEncodingName, baseURL);

            // release pinned data object
            handle.Free();
        }

        public override void Reload()
        {
            WebViewBinding.NPWebViewReload(AddrOfNativeObject());
        }

        public override void StopLoading()
        {
            WebViewBinding.NPWebViewStopLoading(AddrOfNativeObject());
        }

        public override void RunJavaScript(string script, RunJavaScriptInternalCallback callback)
        {
            IntPtr  tagPtr  = MarshalUtility.GetIntPtr(callback);
            WebViewBinding.NPWebViewRunJavaScript(AddrOfNativeObject(), script, tagPtr);
        }

        public override void AddURLScheme(string urlScheme)
        {
            WebViewBinding.NPWebViewAddURLScheme(AddrOfNativeObject(), urlScheme);
        }

        public override void ClearCache()
        {
            WebViewBinding.NPWebViewClearCache();
        }

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed)
            {
                return;
            }

            // release all unmanaged type objects
            var     nativePtr   = AddrOfNativeObject();
            if (nativePtr != IntPtr.Zero)
            {
                NativeInstanceMap.RemoveInstance(nativePtr);
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(WebViewNativeCallback))]
        private static void HandleOnShowNativeCallback(IntPtr nativePtr, string error)
        {
            var     owner       = NativeInstanceMap.GetOwner<NativeWebView>(nativePtr);
            Assertions.AssertIfNull(owner, "owner");

            // invoke event
            var     errorObj    = Error.CreateNullableError(description: error);
            owner.SendShowEvent(errorObj);
        }

        [MonoPInvokeCallback(typeof(WebViewNativeCallback))]
        private static void HandleOnHideNativeCallback(IntPtr nativePtr, string error)
        {
            var     owner       = NativeInstanceMap.GetOwner<NativeWebView>(nativePtr);
            Assertions.AssertIfNull(owner, "owner");

            // invoke event
            var     errorObj    = Error.CreateNullableError(description: error);
            owner.SendHideEvent(errorObj);
        }

        [MonoPInvokeCallback(typeof(WebViewNativeCallback))]
        private static void HandleLoadStartNativeCallback(IntPtr nativePtr, string error)
        {
            var     owner       = NativeInstanceMap.GetOwner<NativeWebView>(nativePtr);
            Assertions.AssertIfNull(owner, "owner");

            // invoke event
            var     errorObj    = Error.CreateNullableError(description: error);
            owner.SendLoadStartEvent(errorObj);
        }

        [MonoPInvokeCallback(typeof(WebViewNativeCallback))]
        private static void HandleLoadFinishNativeCallback(IntPtr nativePtr, string error)
        {
            var     owner       = NativeInstanceMap.GetOwner<NativeWebView>(nativePtr);
            Assertions.AssertIfNull(owner, "owner");

            // invoke event
            var     errorObj    = Error.CreateNullableError(description: error);
            owner.SendLoadFinishEvent(errorObj);
        }

        [MonoPInvokeCallback(typeof(WebViewRunJavaScriptNativeCallback))]
        private static void HandleRunJavaScriptNativeCallback(IntPtr nativePtr, string result, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);
            
            try
            {
                // invoke event
                var     errorObj    = Error.CreateNullableError(description: error);
                ((RunJavaScriptInternalCallback)tagHandle.Target).Invoke(result, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }
            
        [MonoPInvokeCallback(typeof(WebViewURLSchemeMatchFoundNativeCallback))]
        private static void HandleURLSchemeMatchFoundNativeCallback(IntPtr nativePtr, string url)
        {
            var     owner       = NativeInstanceMap.GetOwner<NativeWebView>(nativePtr);
            Assertions.AssertIfNull(owner, "owner");

            // invoke event
            owner.SendURLSchemeMatchFoundEvent(url);
        }

        #endregion
    }
}
#endif
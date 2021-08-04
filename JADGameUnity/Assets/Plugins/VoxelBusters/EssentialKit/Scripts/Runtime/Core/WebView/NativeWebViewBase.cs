using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.WebViewCore
{
    public abstract class NativeWebViewBase : NativeFeatureInterfaceBase, INativeWebView
    {
        #region Constructors

        protected NativeWebViewBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeWebViewInterface implementation

        public event WebViewInternalCallback OnShow;

        public event WebViewInternalCallback OnHide;

        public event WebViewInternalCallback OnLoadStart;

        public event WebViewInternalCallback OnLoadFinish;

        public event URLSchemeMatchFoundInternalCallback OnURLSchemeMatchFound;

        public abstract string GetURL();

        public abstract string GetTitle();

        public abstract void SetFrame(Rect value);

        public abstract void SetStyle(WebViewStyle style);

        public abstract void SetScalesPageToFit(bool value);

        public abstract void SetCanBounce(bool value);

        public abstract void SetBackgroundColor(Color value);

        public abstract double GetProgress();

        public abstract bool GetIsLoading();

        public abstract void SetJavaScriptEnabled(bool value);

        public abstract void Show();

        public abstract void Hide();

        public abstract void LoadURL(string url);

        public abstract void LoadHtmlString(string htmlString, string baseURL);

        public abstract void LoadData(byte[] data, string mimeType, string textEncodingName, string baseURL);

        public abstract void Reload();

        public abstract void StopLoading();

        public abstract void RunJavaScript(string script, RunJavaScriptInternalCallback callback);

        public abstract void AddURLScheme(string urlScheme);

        public abstract void ClearCache();

        #endregion

        #region Private methods

        protected void SendShowEvent(Error error)
        {
            if (OnShow != null)
            {
                CallbackDispatcher.InvokeOnMainThread(() => OnShow(error));
            }
        }

        protected void SendHideEvent(Error error)
        {
            if (OnHide != null)
            {
                CallbackDispatcher.InvokeOnMainThread(() => OnHide(error));
            }
        }

        protected void SendLoadStartEvent(Error error)
        {
            if (OnLoadStart != null)
            {
                CallbackDispatcher.InvokeOnMainThread(() => OnLoadStart(error));
            }
        }

        protected void SendLoadFinishEvent(Error error)
        {
            if (OnLoadFinish != null)
            {
                CallbackDispatcher.InvokeOnMainThread(() => OnLoadFinish(error));
            }
        }

        protected void SendURLSchemeMatchFoundEvent(string url)
        {
            if (OnURLSchemeMatchFound != null)
            {
                CallbackDispatcher.InvokeOnMainThread(() => OnURLSchemeMatchFound(url));
            }
        }

        #endregion
    }
}
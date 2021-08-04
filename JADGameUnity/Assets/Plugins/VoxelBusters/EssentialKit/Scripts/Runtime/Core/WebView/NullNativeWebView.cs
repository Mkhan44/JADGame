using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.WebViewCore
{
    internal sealed class NullNativeWebView : NativeWebViewBase, INativeWebView
    {
        #region Fields

        private     string      m_url;

        #endregion

        #region Constructors

        public NullNativeWebView()
            : base(isAvailable: false)
        {
            LogNotSupported();
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("WebView");
        }

        #endregion

        #region Base class methods

        public override string GetURL()
        {
            LogNotSupported();

            return m_url;
        }

        public override string GetTitle()
        {
            LogNotSupported();

            return string.Empty;
        }

        public override void SetFrame(Rect value)
        {
            LogNotSupported();
        }

        public override void SetStyle(WebViewStyle style)
        { 
            LogNotSupported();
        }

        public override void SetScalesPageToFit(bool value)
        { 
            LogNotSupported();
        }

        public override void SetCanBounce(bool value)
        { 
            LogNotSupported();
        }

        public override void SetBackgroundColor(Color value)
        { 
            LogNotSupported();
        }

        public override double GetProgress()
        {
            LogNotSupported();

            return 0d;
        }

        public override bool GetIsLoading()
        {
            LogNotSupported();

            return false;
        }

        public override void SetJavaScriptEnabled(bool value)
        { 
            LogNotSupported();
        }

        public override void Show()
        {
            LogNotSupported();

            SendShowEvent(Diagnostics.kFeatureNotSupported);
        }

        public override void Hide()
        {
            LogNotSupported();

            SendHideEvent(Diagnostics.kFeatureNotSupported);
        }

        public override void LoadURL(string url)
        {
            // set value
            m_url   = url;

            LogNotSupported();

            SendLoadEvents();
        }

        public override void LoadHtmlString(string htmlString, string baseURL)
        {
            // set value
            m_url   = "blank";

            LogNotSupported();

            SendLoadEvents();
        }

        public override void LoadData(byte[] data, string mimeType, string textEncodingName, string baseURL)
        {
            // set value
            m_url   = "blank";

            LogNotSupported();

            SendLoadEvents();
        }

        public override void Reload()
        {
            LogNotSupported();
        }

        public override void StopLoading()
        {
            LogNotSupported();
        }

        public override void RunJavaScript(string script, RunJavaScriptInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        public override void AddURLScheme(string urlScheme)
        {
            LogNotSupported();
        }

        public override void ClearCache()
        {
            LogNotSupported();
        }

        #endregion

        #region Private methods

        private void SendLoadEvents()
        {
            SendLoadStartEvent(Diagnostics.kFeatureNotSupported);

            SendLoadFinishEvent(Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
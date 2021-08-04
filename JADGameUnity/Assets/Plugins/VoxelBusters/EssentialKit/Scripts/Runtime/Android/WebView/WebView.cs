#if UNITY_ANDROID
using System;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using UnityEngine;

namespace VoxelBusters.EssentialKit.WebViewCore.Android
{
    public sealed class WebView : NativeWebViewBase, INativeWebView
    {
        #region Fields

        private NativeWebkitWebView m_instance;

        #endregion

        #region Constructors

        public WebView()
            : base(isAvailable: true)
        {
            m_instance = new NativeWebkitWebView(NativeUnityPluginUtility.GetContext());
            m_instance.SetViewListener(new NativeWebViewListener()
            {
                onPageLoadStartedCallback = () => SendLoadStartEvent(null),
                onPageLoadErrorCallback = (failingUrl, description) =>
                {
                    DebugLogger.Log(string.Format("Failing Url : {0} Description : {1}", failingUrl, description));
                    SendLoadFinishEvent(new Error(description));
                },
                onPageLoadFinishedCallback = (url) => SendLoadFinishEvent(null),
                onMessageReceivedCallback = (message) =>
                {
                    SendURLSchemeMatchFoundEvent(message.Url);
                },
                onHideCallback = () => SendHideEvent(null),
                onShowCallback = () => SendShowEvent(null)
            }) ;
        }

        ~WebView()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        public override string GetURL()
        {
            return m_instance.GetUrl();
        }

        public override string GetTitle()
        {
            return m_instance.GetTitle();
        }

        public override void SetFrame(Rect value)
        {
            UnityRect   rect    = (UnityRect)value;
            rect.X             /= Screen.width;
            rect.Y             /= Screen.height;
            rect.Width         /= Screen.width;
            rect.Height        /= Screen.height;
            m_instance.SetFrame(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public override void SetStyle(WebViewStyle style)
        {
            m_instance.SetStyle(Converter.from(style));
        }

        public override void SetScalesPageToFit(bool value)
        {
            m_instance.SetScalesPageToFit(value);
        }

        public override void SetCanBounce(bool value)
        {
            m_instance.SetBounce(value);
        }

        public override void SetBackgroundColor(Color value)
        {
            m_instance.SetBackgroundColor(value.r, value.g, value.b, value.a);
        }

        public override double GetProgress()
        {
            return m_instance.GetProgress();
        }

        public override bool GetIsLoading()
        {
            return m_instance.IsLoading();
        }

        public override void SetJavaScriptEnabled(bool value)
        {
            m_instance.SetJavaScriptEnabled(value);
        }

        public override void Show()
        {
            m_instance.Show();
        }

        public override void Hide()
        {
            m_instance.Hide();
        }

        public override void LoadURL(string url)
        {
            m_instance.LoadUrl(url);
        }

        public override void LoadHtmlString(string htmlString, string baseURL)
        {
            m_instance.LoadHtmlString(htmlString, baseURL);
        }

        public override void LoadData(byte[] data, string mimeType, string textEncodingName, string baseURL)
        {
            m_instance.LoadData(new NativeByteBuffer(data), data.Length, mimeType, textEncodingName, baseURL);
        }

        public override void Reload()
        {
            m_instance.Reload();
        }

        public override void StopLoading()
        {
            m_instance.StopLoading();
        }

        public override void RunJavaScript(string script, RunJavaScriptInternalCallback callback)
        {
            m_instance.EvaluateJavaScriptFromString(script, new NativeEvaluateJavaScriptListener()
            {
                onSuccessCallback = (result)    => callback(result, null),
                onFailureCallback = (error)     => callback(null, new Error(error))
            });
        }

        public override void AddURLScheme(string urlScheme)
        {
            m_instance.AddNewScheme(urlScheme);
        }

        public override void ClearCache()
        {
            m_instance.ClearCache();
            //TODO: Add clear cookies - Required for oauth examples
        }

        #endregion
    }
}
#endif
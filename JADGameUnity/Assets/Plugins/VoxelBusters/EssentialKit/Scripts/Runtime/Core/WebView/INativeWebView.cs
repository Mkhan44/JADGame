using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.WebViewCore
{
    public interface INativeWebView : INativeFeatureInterface
    {
        #region Events

        event WebViewInternalCallback OnShow;

        event WebViewInternalCallback OnHide;

        event WebViewInternalCallback OnLoadStart;

        event WebViewInternalCallback OnLoadFinish;

        event URLSchemeMatchFoundInternalCallback OnURLSchemeMatchFound;
        
        #endregion

        #region Methods

        string GetURL();

        string GetTitle();

        void SetFrame(Rect value);

        void SetStyle(WebViewStyle style);

        void SetScalesPageToFit(bool value);

        void SetCanBounce(bool value);

        void SetBackgroundColor(Color value);

        double GetProgress();

        bool GetIsLoading();

        void SetJavaScriptEnabled(bool value);

        void Show();

        void Hide();

        void LoadURL(string url);

        void LoadHtmlString(string htmlString, string baseURL);

        void LoadData(byte[] data, string mimeType, string textEncodingName, string baseURL);

        void Reload();

        void StopLoading();

        void RunJavaScript(string script, RunJavaScriptInternalCallback callback);

        void AddURLScheme(string urlScheme);

        void ClearCache();

        #endregion
    }
}
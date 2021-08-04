using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class WebViewDemo : DemoActionPanelBase<WebViewDemoAction, WebViewDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     RectTransform[]     m_instanceDependentObjects          = null;

        [SerializeField]
        private     Dropdown            m_styleDropdown                     = null;

        [SerializeField]
        private     InputField          m_urlInputField                     = null;

        [SerializeField]
        private     Toggle              m_isLocalPathToggle                 = null;

        [SerializeField]
        private     InputField          m_htmlStringInputField              = null;

        [SerializeField]
        private     InputField          m_jsInputField                      = null;

        private     WebView             m_activeWebView;

        #endregion

        #region Base class methods

        protected override void Start()
        {
            base.Start();

            // set default properties
            SetActiveWebView(null);
            m_styleDropdown.options = new List<Dropdown.OptionData>(Array.ConvertAll(Enum.GetNames(typeof(WebViewStyle)), (item) => new Dropdown.OptionData(item)));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // register for events
            WebView.OnShow          += OnWebViewShow;
            WebView.OnHide          += OnWebViewHide;;
            WebView.OnLoadStart     += OnWebViewLoadStart;
            WebView.OnLoadFinish    += OnWebViewLoadFinish;
            WebView.OnURLSchemeMatchFound += OnURLSchemeMatchFound;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // register for events
            WebView.OnShow          -= OnWebViewShow;
            WebView.OnHide          -= OnWebViewHide;;
            WebView.OnLoadStart     -= OnWebViewLoadStart;
            WebView.OnLoadFinish    -= OnWebViewLoadFinish;
            WebView.OnURLSchemeMatchFound -= OnURLSchemeMatchFound;
        }

        protected override void OnActionSelectInternal(WebViewDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case WebViewDemoActionType.Create:
                    var     newInstance     = WebView.CreateInstance();
                    newInstance.SetNormalizedFrame(new Rect(0f, 0f, 1f, 0.4f));
                    SetActiveWebView(newInstance);
                    break;
        
                case WebViewDemoActionType.SetControls:
                    var     selectedStyle   = GetSelectedStyle();
                    m_activeWebView.Style   = selectedStyle;
                    Log("Setting style property: " + selectedStyle);
                    break;

                case WebViewDemoActionType.SetAutoShowOnLoadFinish:
                    bool    autoShow        = selectedAction.GetComponent<Toggle>().isOn;
                    m_activeWebView.AutoShowOnLoadFinish    = autoShow;
                    Log("Setting auto show on load property: " + autoShow);
                    break;
        
                case WebViewDemoActionType.SetScalesPageToFit:
                    bool    scalesPageToFit         = selectedAction.GetComponent<Toggle>().isOn;
                    m_activeWebView.ScalesPageToFit = scalesPageToFit;
                    Log("Setting scales page to fit property: " + scalesPageToFit);
                    break;

                case WebViewDemoActionType.SetCanBounce:
                    bool    canBounce               = selectedAction.GetComponent<Toggle>().isOn;
                    m_activeWebView.CanBounce       = canBounce;
                    Log("Setting can bounce property: " + canBounce);
                    break;

                case WebViewDemoActionType.SetJavaScriptEnabled:
                    bool    jsEnabled                   = selectedAction.GetComponent<Toggle>().isOn;
                    m_activeWebView.JavaScriptEnabled   = jsEnabled;
                    Log("Setting js enabled property: " + jsEnabled);
                    break;

                case WebViewDemoActionType.GetURL:
                    string  url1        = m_activeWebView.URL;
                    Log("URL: " + url1);
                    break;

                case WebViewDemoActionType.GetTitle:
                    string  title1      = m_activeWebView.Title;
                    Log("Title: " + title1);
                    break;

                case WebViewDemoActionType.GetProgress:
                    double  progress1   = m_activeWebView.Progress;
                    Log("Progress: " + progress1);
                    break;

                case WebViewDemoActionType.GetIsLoading:
                    bool    isLoading1  = m_activeWebView.IsLoading;
                    Log("Is loading: " + isLoading1);
                    break;

                case WebViewDemoActionType.Show:
                    m_activeWebView.Show();
                    break;

                case WebViewDemoActionType.Hide:
                    m_activeWebView.Hide();
                    break;

                case WebViewDemoActionType.LoadURL:
                    string  urlStr      = m_urlInputField.text;
                    if (string.IsNullOrEmpty(urlStr))
                    {
                        Log("[Error] Specify url.");
                        return;
                    }
                    else
                    {
                        Log("Loading url.");
                        var     urlString   = m_isLocalPathToggle.isOn 
                            ? URLString.FileURLWithPath(m_urlInputField.text) 
                            : URLString.URLWithPath(m_urlInputField.text);
                        m_activeWebView.LoadURL(urlString);
                    }
                    break;

                case WebViewDemoActionType.LoadHtmlString:
                    string  htmlStr     = m_htmlStringInputField.text;
                    if (string.IsNullOrEmpty(htmlStr))
                    {
                        Log("[Error] Provide html string.");
                        return;
                    }
                    else
                    {
                        Log("Loading html string.");
                        m_activeWebView.LoadHtmlString(htmlStr);
                    }
                    break;

                case WebViewDemoActionType.LoadData:
                    Log("Loading random image data.");
                    var     image1  = DemoResources.GetRandomImage();
                    string  mimeType1, textEncodingName1;
                    byte[]  data1   = image1.Encode(TextureEncodingFormat.JPG, out mimeType1, out textEncodingName1);
                    m_activeWebView.LoadData(data1, mimeType1, textEncodingName1);
                    break;

                case WebViewDemoActionType.LoadTexture:
                    Log("Loading random image data.");
                    var     image2  = DemoResources.GetRandomImage();
                    m_activeWebView.LoadTexture(image2, TextureEncodingFormat.JPG); 
                    break;

                case WebViewDemoActionType.Reload:
                    Log("Reloading webpage.");
                    m_activeWebView.Reload();
                    break;
                
                case WebViewDemoActionType.StopLoading:
                    Log("Stopping load request.");
                    m_activeWebView.StopLoading();
                    break;
                
                case WebViewDemoActionType.RunJavaScript:
                    string  js  = m_jsInputField.text;
                    if (string.IsNullOrEmpty(js))
                    {
                        Log("Provide javascript.");
                        return;
                    }
                    else
                    {
                        Log("Running javascript.");
                        m_activeWebView.RunJavaScript(js, (result, error) =>
                        {
                            if (error == null)
                            {
                                Log("Javascript was executed successfully. Result: " + result.Result);
                            }
                            else
                            {
                                Log("Could not run javascript. Error: " + error);
                            }
                        });
                    }
                    break;

                case WebViewDemoActionType.SetFullScreenRect:
                    Log("Changing frame to full screen.");
                    m_activeWebView.SetFullScreen();
                    break;

                case WebViewDemoActionType.ClearCache:
                    Log("Clearing cache.");
                    m_activeWebView.ClearCache();
                    break;

                case WebViewDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kWebView);
                    break;
            }
        }

        #endregion

        #region Plugin callback methods

        private void OnWebViewShow(WebView result)
        {
            if (result == m_activeWebView)
            {
                Log("Webview is being displayed.");
            }
        }

        private void OnWebViewHide(WebView result)
        {
            if (result == m_activeWebView)
            {
                Log("Webview is being dismissed.");
            }
        }

        private void OnWebViewLoadStart(WebView result)
        {
            if (result == m_activeWebView)
            {
                Log("Webview did start loading request. URL: "+ m_activeWebView.URL);
            }
        }

        private void OnWebViewLoadFinish(WebView result, Error error)
        {
            if (result == m_activeWebView)
            {
                if (error == null)
                {
                    Log("Webview finished loading requested content successfully.");
                }
                else
                {
                    Log("Webview failed to load requested content. Error: " + error);
                }
            }
        }


        private void OnURLSchemeMatchFound(string result)
        {
            Log("Found a url scheme match : " + result);
        }

        #endregion

        #region Private methods

        private void SetActiveWebView(WebView webView)
        {
            // set property
            m_activeWebView     = webView;

            // update ui
            SetInstanceDependentObjectState(webView != null);
        }

        private void SetInstanceDependentObjectState(bool active)
        {
            foreach (var rect in m_instanceDependentObjects)
            {
                rect.gameObject.SetActive(active);
            }

            // update state
            if (active)
            {
                var     actions    = GetComponentsInChildren<WebViewDemoAction>(includeInactive: true);
                foreach (var action in actions)
                {
                    switch (action.ActionType)
                    {
                        case WebViewDemoActionType.SetCanBounce:
                            var toggle1     = action.GetComponent<Toggle>();
                            toggle1.isOn    = m_activeWebView.CanBounce;
                            break;

                        case WebViewDemoActionType.SetScalesPageToFit:
                            var toggle2     = action.GetComponent<Toggle>();
                            toggle2.isOn    = m_activeWebView.ScalesPageToFit;
                            break;

                        case WebViewDemoActionType.SetJavaScriptEnabled:
                            var toggle3     = action.GetComponent<Toggle>();
                            toggle3.isOn    = m_activeWebView.JavaScriptEnabled;
                            break;

                        case WebViewDemoActionType.SetAutoShowOnLoadFinish:
                            var toggle4     = action.GetComponent<Toggle>();
                            toggle4.isOn    = m_activeWebView.AutoShowOnLoadFinish;
                            break;

                        case WebViewDemoActionType.SetControls:
                            var dropdown1   = action.GetComponent<Dropdown>();
                            dropdown1.value = GetStyleIndex(m_activeWebView.Style);
                            break;
                    }
                }
            }
        }

        private WebViewStyle GetSelectedStyle()
        {
            return (WebViewStyle)Enum.GetValues(typeof(WebViewStyle)).GetValue(m_styleDropdown.value);
        }

        private int GetStyleIndex(WebViewStyle style)
        {
            var     array   = Enum.GetValues(typeof(WebViewStyle));
            for (int iter = 0; iter < array.Length; iter++)
            {
                if ((WebViewStyle)array.GetValue(iter) == style)
                {
                    return iter;
                }
            }

            return 0;
        }

        #endregion
    }
}

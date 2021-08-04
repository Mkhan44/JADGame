using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.WebViewCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to display web contents inside your application.
    /// </summary>
    /// <description>
    /// To do so, drag and drop the WebView prefab into scene heirarchy, which is placed under folder <i>Assets/VoxelBusters/NativePlugins/Prefab</i>. 
    /// And then send it a request to display web content. 
    /// You can also use this class to move back and forward in the history, just like web browser by setting control type to <c>eWebviewControlType.TOOLBAR</c>.
    /// </description>  
    /// <example>
    /// The following code illustrates how to load webpage using web view.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     public WebView m_webView;
    /// 
    ///     private void Start()
    ///     {
    ///         // set web view properties
    ///         m_webView     = WebView.CreateInstance();
    ///         m_webView.SetFullScreen();
    /// 
    ///         // start request
    ///         m_webView.LoadRequest("http://www.google.com");
    ///     }
    /// 
    ///     private void OnEnable()
    ///     {
    ///         // registering for event
    ///         WebView.OnShow          += OnShow;
    ///         WebView.OnHide          += OnHide;
    ///         WebView.OnLoadStart     += OnLoadStart;
    ///         WebView.OnLoadFinish    += OnLoadFinish;
    ///     }
    /// 
    ///     private void OnDisable()
    ///     {
    ///         // unregistering event
    ///         WebView.OnShow          -= OnShow;
    ///         WebView.OnHide          -= OnHide;
    ///         WebView.OnLoadStart     -= OnLoadStart;
    ///         WebView.OnLoadFinish    -= OnLoadFinish;
    ///     }
    /// 
    ///     private void OnShow(WebView result)
    ///     {
    ///         if (m_webView == result)
    ///         {
    ///             Debug.Log("Showing webview.");
    ///         }
    ///     }
    /// 
    ///     private void OnHide(WebView result)
    ///     {
    ///         if (m_webView == result)
    ///         {
    ///             Debug.Log("Hiding webview.");
    ///         }
    ///     }
    ///     
    ///     private void OnLoadStart(WebView result)
    ///     {
    ///         if (m_webView == result)
    ///         {
    ///             Debug.Log("Started loading request with url:" + m_webView.URL);
    ///         }
    ///     }
    ///     
    ///     private OnWebViewLoadFinish(WebView result, Error error)
    ///     {
    ///         if (m_webView == result)
    ///         {
    ///             if (error == null)
    ///             {
    ///                 Debug.Log("Webview did finish loading request successfully.");
    ///             }   
    ///             else
    ///             {
    ///                 Debug.Log("Webview did fail to load request. Error: " + error.Description);
    ///             }
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public sealed class WebView : NativeFeatureBehaviour
    {
        #region Fields

        [SerializeField]
        private     bool                    m_autoShowOnLoadFinish  = true;                

        [SerializeField]
        private     bool                    m_scalesPageToFit       = true;

        [SerializeField]
        private     bool                    m_canBounce             = true;  

        [SerializeField]
        private     bool                    m_javascriptEnabled     = true;

        [SerializeField]
        private     WebViewStyle            m_style                 = WebViewStyle.Popup;

        [SerializeField]
        private     Rect                    m_frame                 = new Rect();

        [SerializeField]
        private     Color                   m_backgroundColor       = Color.white; 

        private     INativeWebView          m_nativeInterface;

        #endregion

        #region Static properties
        
        public static WebViewUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.WebViewSettings;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The active URL. (read-only)
        /// </summary>
        public string URL
        {
            get
            {
                try
                {
                    return m_nativeInterface.GetURL();
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                    return null;
                }
            }
        }

        /// <summary>
        /// The page title. (read-only)
        /// </summary>
        public string Title
        {
            get
            {
                try
                {
                    return m_nativeInterface.GetTitle();
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                    return null;
                }
            }
        }

        /// <summary>
        /// The frame rectangle describes the webview’s position and size.
        /// </summary>
        public Rect Frame
        {
            get
            {
                return m_frame;
            }
            set
            {
                try
                {
                    // set new value
                    m_frame = value;

                    // apply changes to native object
                    m_nativeInterface.SetFrame(value);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
            }
        }

        /// <summary>
        /// An enum value that determines the appearence of webview.
        /// </summary>
        public WebViewStyle Style
        {
            get
            {
                return m_style;
            }
            set
            {
                try
                {
                    // set new value
                    m_style = value;

                    // apply changes to native object
                    m_nativeInterface.SetStyle(value);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
            }
        }

        /// <summary>
        /// A boolean value indicating whether webview can auto show itself when load request is finished.
        /// </summary>
        public bool AutoShowOnLoadFinish
        {
            get
            {
                return m_autoShowOnLoadFinish;
            }
            set
            {
                m_autoShowOnLoadFinish  = value;
            }
        }

        /// <summary>
        /// A boolean value indicating whether web view scales webpages to fit the view and the user can change the scale.
        /// </summary>
        public bool ScalesPageToFit
        {
            get
            {
                return m_scalesPageToFit;
            }
            set
            {
                try
                {
                    // set new value
                    m_scalesPageToFit   = value;

                    // apply changes to native object
                    m_nativeInterface.SetScalesPageToFit(value);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
            }
        }

        /// <summary>
        /// A Boolean value that controls whether the web view bounces past the edge of content and back again.
        /// </summary>
        public bool CanBounce
        {
            get
            {
                return m_canBounce;
            }
            set
            {
                try
                {
                    // set new value
                    m_canBounce = value;

                    // apply changes to native object
                    m_nativeInterface.SetCanBounce(value);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
            }
        }

        /// <summary>
        /// The background color of the webview.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return m_backgroundColor;
            }
            set
            {
                try
                {
                    // set new value
                    m_backgroundColor   = value;

                    // apply changes to native object
                    m_nativeInterface.SetBackgroundColor(value);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
            }
        }

        /// <summary>
        /// The value indicates the progress of load request.
        /// </summary>
        public double Progress
        {
            get
            {
                try
                {
                    return m_nativeInterface.GetProgress();
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                    return 0d;
                }
            }
        }

        /// <summary>
        /// A boolean value indicating whether this webview is loading content.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                try
                {
                    return m_nativeInterface.GetIsLoading();
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                    return false;
                }
            }
        }

        /// <summary>
        /// A boolean value indicating whether this webview allows java script execution.
        /// </summary>
        public bool JavaScriptEnabled
        {
            get
            {
                return m_javascriptEnabled;
            }
            set
            {
                try
                {
                    // set new value
                    m_javascriptEnabled = value;

                    // apply changes to native object
                    m_nativeInterface.SetJavaScriptEnabled(value);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
            }
        }

        #endregion

        #region Static events

        /// <summary>
        /// Event that will be called when webview is first displayed.
        /// </summary>
        public static event Callback<WebView> OnShow;

        /// <summary>
        /// Event that will be called when webview is dismissed.
        /// </summary>
        public static event Callback<WebView> OnHide;

        /// <summary>
        /// Event that will be called when web view begins load request.
        /// </summary>
        public static event Callback<WebView> OnLoadStart;

        /// <summary>
        /// Event that will be called when web view has finished loading.
        /// </summary>
        public static event EventCallback<WebView> OnLoadFinish;

        /// <summary>
        /// Event that will be called when web view has finished loading.
        /// </summary>
        public static event Callback<string> OnURLSchemeMatchFound;

        #endregion

        #region Create methods

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageComposer"/> class.
        /// </summary>
        public static WebView CreateInstance()
        {
            return CreateInstanceInternal<WebView>("WebView");
        }

        #endregion

        #region Lifecycle methods

        protected override void AwakeInternal(object[] args)
        {
            base.AwakeInternal(args);

            // initialise component
            m_nativeInterface   = NativeFeatureActivator.CreateInterface<INativeWebView>(ImplementationBlueprint.WebView, UnitySettings.IsEnabled);

            RegisterForEvents();
        }

        protected override void StartInternal()
        {
            base.StartInternal();

            // apply available settings values
            ScalesPageToFit     = m_scalesPageToFit;
            CanBounce           = m_canBounce;
            JavaScriptEnabled   = m_javascriptEnabled;
            BackgroundColor     = m_backgroundColor;
            Style               = m_style;
            Frame               = m_frame;
        }

        protected override void DestroyInternal()
        {
            base.DestroyInternal();

            UnregisterFromEvents();
            
            // invalidate native object
            if (m_nativeInterface != null)
            {
                m_nativeInterface.Hide();
                m_nativeInterface.Dispose();
            }
        }

        #endregion

        #region Behaviour methodss

        public override bool IsAvailable()
        {
            return (m_nativeInterface != null) && m_nativeInterface.IsAvailable;
        }

        protected override string GetFeatureName()
        {
            return "WebView";
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Displays the webview on the top of Unity view.
        /// </summary>
        public void Show()
        {
            try
            {
                m_nativeInterface.Show();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Hides the web view.
        /// </summary>
        public void Hide()
        {
            try
            {
                m_nativeInterface.Hide();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Connects to a given URL and asynchronously loads the content.
        /// </summary>
        /// <param name="url">A URL identifying the location of the content to load.</param>
        /// <remarks>
        /// \note Don’t use this method to load local HTML files, instead use <see cref="LoadHtmlString(string, URLString)"/>.
        /// </remarks>
        public void LoadURL(URLString url)
        {
            // validate arguments
            if (false == url.IsValid)
            {
                DebugLogger.LogWarning("Specified url is invalid.");
                return;
            }

            try
            {
                m_nativeInterface.LoadURL(url.ToString());
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Loads the webpage contents.
        /// </summary>
        /// <param name="htmlString">The contents of the webpage.</param>
        /// <param name="baseURL">The base URL for the content.</param>
        public void LoadHtmlString(string htmlString, URLString? baseURL = null)
        {
            // validate arguments
            if (null == htmlString)
            {
                DebugLogger.LogWarning("Html string is null.");
                return;
            }
            if (baseURL.HasValue && !baseURL.Value.IsValid)
            {
                DebugLogger.LogWarning("Specified base url is invalid.");
                return;
            }

            try
            {
                m_nativeInterface.LoadHtmlString(htmlString, baseURL.ToString());
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Loads the webpage contents from specified file.
        /// </summary>
        /// <param name="data">The data to use as the contents of the webpage.</param>
        /// <param name="mimeType">The MIME type of the content. Please check <see cref="MimeType">.</param>
        /// <param name="textEncodingName">The content's character encoding name.</param>
        /// <param name="baseURL">The base URL for the content.</param>
        public void LoadData(byte[] data, string mimeType, string textEncodingName, URLString? baseURL = null)
        {
            // validate arguments
            if (null == data)
            {
                DebugLogger.LogWarning("Data array is null.");
                return;
            }
            if (string.IsNullOrEmpty(mimeType))
            {
                DebugLogger.LogWarning("Mime type is null.");
                return;
            }
            if (string.IsNullOrEmpty(textEncodingName))
            {
                DebugLogger.LogWarning("Text encoding name is null.");
                return;
            }
            if (baseURL.HasValue && !baseURL.Value.IsValid)
            {
                DebugLogger.LogWarning("Specified base url is invalid.");
                return;
            }

            try
            {
                m_nativeInterface.LoadData(data, mimeType, textEncodingName, baseURL.ToString());
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Reloads the current page.
        /// </summary>
        public void Reload()
        {
            try
            {
                m_nativeInterface.Reload();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Stops loading the current page contents.
        /// </summary>
        public void StopLoading()
        {
            try
            {
                m_nativeInterface.StopLoading();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Executes a JavaScript string.
        /// </summary>
        /// <param name="script">The JavaScript string to evaluate.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public void RunJavaScript(string script, EventCallback<WebViewRunJavaScriptResult> callback)
        {
            // validate arguments
            if (null == callback)
            {
                DebugLogger.LogWarning("Callback is null.");
                return;
            }
            if (script == null)
            {
                DebugLogger.LogWarning("Script is null.");
                return;
            }

            try
            {
                // make request
                m_nativeInterface.RunJavaScript(script, (jsResult, jsError) =>
                {
                    // send result to caller object
                    var     result  = new WebViewRunJavaScriptResult()
                    {
                        Result      = jsResult,
                    };
                    CallbackDispatcher.InvokeOnMainThread(callback, result, jsError);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Registers the specified scheme, after which web view will start to listen for custom URL.
        /// </summary>
        /// <description>
        /// This approach is used for communicating web view with Unity. 
        /// When web view starts loading contents, it will check against registered schemes. 
        /// And incase if a match is found, web view will raise <c>DidReceiveMessageEvent</c> along with URL information.
        /// </description>
        /// <param name="urlScheme">The scheme name of the URL.</param>
        public void AddURLScheme(string urlScheme)
        {
            try
            {
                m_nativeInterface.AddURLScheme(urlScheme);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Clears all stored cached URL responses.
        /// </summary>
        public void ClearCache()
        {
            try
            {
                m_nativeInterface.ClearCache();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Private methods

        private void RegisterForEvents()
        {
            m_nativeInterface.OnShow       += HandleOnWebViewShow;
            m_nativeInterface.OnHide       += HandleOnWebViewHide;
            m_nativeInterface.OnLoadStart  += HandleOnWebViewLoadStart;
            m_nativeInterface.OnLoadFinish += HandleOnWebViewLoadFinish;
            m_nativeInterface.OnURLSchemeMatchFound += HandleOnURLSchemeMatchFound;
        }

        private void UnregisterFromEvents()
        {
            m_nativeInterface.OnShow       -= HandleOnWebViewShow;
            m_nativeInterface.OnHide       -= HandleOnWebViewHide;
            m_nativeInterface.OnLoadStart  -= HandleOnWebViewLoadStart;
            m_nativeInterface.OnLoadFinish -= HandleOnWebViewLoadFinish;
            m_nativeInterface.OnURLSchemeMatchFound -= HandleOnURLSchemeMatchFound;
        }

        #endregion

        #region Event callback methods

        private void HandleOnWebViewShow(Error error)
        {
            // notify listeners
            CallbackDispatcher.InvokeOnMainThread(OnShow, this);
        }

        private void HandleOnWebViewHide(Error error)
        {
            // notify listeners
            CallbackDispatcher.InvokeOnMainThread(OnHide, this);
        }

        private void HandleOnWebViewLoadStart(Error error)
        {
            // notify listeners
            CallbackDispatcher.InvokeOnMainThread(OnLoadStart, this);
        }

        private void HandleOnWebViewLoadFinish(Error error)
        {
            // notify listeners
            CallbackDispatcher.InvokeOnMainThread(OnLoadFinish, this, error);

            // check whether webview needs to be displayed on completing request
            if ((null == error) && AutoShowOnLoadFinish)
            {
                Show();
            }
        }

        private void HandleOnURLSchemeMatchFound(string url)
        {
            // notify listeners
            CallbackDispatcher.InvokeOnMainThread(OnURLSchemeMatchFound, url);
        }

        #endregion
    }
}
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.DeepLinkServicesCore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface to handle deep links.    
    /// </summary>
    /// <description>
    public static class DeepLinkServices
    {
        #region Static fields

        private     static  INativeDeepLinkServicesInterface    s_nativeInterface;

        #endregion

        #region Static properties

        public static DeepLinkServicesUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.DeepLinkServicesSettings;
            }
        }

        public static IDeepLinkServicesDelegate Delegate
        {
            get;
            set;
        }

        #endregion

        #region Static events

        /// <summary>
        /// Event that will be called when url scheme is opened.
        /// </summary>
        public static event Callback<DeepLinkServicesDynamicLinkOpenResult> OnCustomSchemeUrlOpen;

        /// <summary>
        /// Event that will be called when universal link is opened.
        /// </summary>
        public static event Callback<DeepLinkServicesDynamicLinkOpenResult> OnUniversalLinkOpen;

        #endregion

        #region Setup methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        internal static void Initialize()
        {
            // set properties
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeDeepLinkServicesInterface>(ImplementationBlueprint.DeepLinkServices, UnitySettings.IsEnabled);
            s_nativeInterface.SetCanHandleCustomSchemeUrl(handler: CanHandleCustomSchemeUrl);
            s_nativeInterface.SetCanHandleUniversalLink(handler: CanHandleUniversalLink);
            s_nativeInterface.OnCustomSchemeUrlOpen    += HandleOnCustomSchemeUrlOpen;
            s_nativeInterface.OnUniversalLinkOpen      += HandleOnUniversalLinkOpen;

            // init object
            s_nativeInterface.Init();
        }

        private static bool CanHandleCustomSchemeUrl(string url)
        {
            return (Delegate == null) || Delegate.CanHandleCustomSchemeUrl(new Uri(url));
        }

        private static bool CanHandleUniversalLink(string url)
        {
            return (Delegate == null) || Delegate.CanHandleUniversalLink(new Uri(url));
        }

        #endregion

        #region Callback methods

        private static void HandleOnCustomSchemeUrlOpen(string url)
        {
            DebugLogger.Log("Detected url scheme " + url);

            // notify listeners
            var     result      = new DeepLinkServicesDynamicLinkOpenResult() 
            { 
                Url             = new Uri(url), 
                RawUrlString    = url,
            };
            CallbackDispatcher.InvokeOnMainThread(OnCustomSchemeUrlOpen, result);
        }

        private static void HandleOnUniversalLinkOpen(string url)
        {
            DebugLogger.Log("Detected universal link " + url);

            // notify listeners
            var     result      = new DeepLinkServicesDynamicLinkOpenResult() 
            { 
                Url             = new Uri(url), 
                RawUrlString    = url,
            };
            CallbackDispatcher.InvokeOnMainThread(OnUniversalLinkOpen, result);
        }

        #endregion
    }
}
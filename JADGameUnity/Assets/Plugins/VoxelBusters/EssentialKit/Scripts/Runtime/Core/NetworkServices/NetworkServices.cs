using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.NetworkServicesCore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface to check network connectivity status.
    /// </summary>
    /// <example>
    /// The following example illustrates how to use network service related events.
    /// <code>
    /// using UnityEngine;
    /// using System.Collections;
    /// using VoxelBusters.EssentialKit;
    /// 
    /// public class ExampleClass : MonoBehaviour 
    /// {
    ///     private void OnEnable()
    ///     {
    ///         // registering for event
    ///         NetworkServices.OnInternetConnectivityChange    += OnInternetConnectivityChange;
    ///         NetworkServices.OnHostReachabilityChange        += OnHostReachabilityChange;
    ///     }
    /// 
    ///     private void OnDisable()
    ///     {
    ///         // unregistering event
    ///         NetworkServices.OnInternetConnectivityChange    -= OnInternetConnectivityChange;
    ///         NetworkServices.OnHostReachabilityChange        -= OnHostReachabilityChange;
    ///     }
    /// 
    ///     private void OnInternetConnectivityChange(NetworkServicesInternetConnectivityStatus data)
    ///     {
    ///         if (data.IsConnected)
    ///         {
    ///             // notify user that he/she is online
    ///         }
    ///         else
    ///         {
    ///             // notify user that he/she is offline
    ///         }
    ///     }
    /// 
    ///     private void OnHostReachabilityChange(NetworkServicesHostReachabilityStatus data)
    ///     {
    ///         Debug.Log("Host connectivity status: " + data.IsReachable);
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class NetworkServices
    {
        #region Static fields

        private     static  INativeNetworkServicesInterface     s_nativeInterface    = null;

        #endregion

        #region Static properties

        public static NetworkServicesUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.NetworkServicesSettings;
            }
        }

        /// <summary>
        /// A boolean value that is used to determine internet connectivity status.
        /// </summary>
        /// <value><c>true</c> if connected to network; otherwise, <c>false</c>.</value>
        public static bool IsInternetActive
        {
            get;
            private set;
        }

        /// <summary>
        /// A boolean value that is used to determine whether host is reachable or not.
        /// </summary>
        /// <value><c>true</c> if is host reachable; otherwise, <c>false</c>.</value>
        public static bool IsHostReachable
        {
            get;
            private set;
        }

        /// <summary>
        /// A boolean value that is used to determine whether notifier is running or not.
        /// </summary>
        /// <value><c>true</c> if notifier is active; otherwise, <c>false</c>.</value>
        public static bool IsNotifierActive
        {
            get;
            private set;
        }

        #endregion
        
        #region Static events

        /// <summary>
        /// Event that will be called whenever network state changes.
        /// </summary>
        public static event Callback<NetworkServicesInternetConnectivityStatusChangeResult> OnInternetConnectivityChange;

        /// <summary>
        /// Event that will be called whenever host reachability state changes.
        /// </summary>
        public static event Callback<NetworkServicesHostReachabilityStatusChangeResult> OnHostReachabilityChange;
        
        #endregion

        #region Public methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        internal static void Initialize()
        {
            // create interface object
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeNetworkServicesInterface>(ImplementationBlueprint.NetworkServices, UnitySettings.IsEnabled);

            // set default state
            IsInternetActive        = true;
            IsHostReachable         = true;

            RegisterForEvents();

            // start if specified
            if (UnitySettings.AutoStartNotifier)
            {
                SurrogateCoroutine.WaitUntilAndInvoke(new WaitForFixedUpdate(), StartNotifier);
            }
        }

        /// <summary>
        /// Starts the notifier.
        /// </summary>
        public static void StartNotifier()
        {
            try
            {
                // check current status and stop the existing notifier instance if required
                if (IsNotifierActive)
                {
                    StopNotifier();
                }

                // make request
                IsNotifierActive  = true;
                s_nativeInterface.StartNotifier();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Stops the notifier.
        /// </summary>
        public static void StopNotifier()
        {
            try
            {
                // check whether notifier is running
                if (!IsNotifierActive)
                {
                    return;
                }

                // make request
                IsNotifierActive = false;
                s_nativeInterface.StopNotifier();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Private methods

        private static void RegisterForEvents()
        {
            s_nativeInterface.OnInternetConnectivityChange  += HandleInternetConnectivityChangeInternalCallback;
            s_nativeInterface.OnHostReachabilityChange      += HandleHostReachabilityChangeInternalCallback;
        }

        private static void UnregisterFromEvents()
        {
            s_nativeInterface.OnInternetConnectivityChange  -= HandleInternetConnectivityChangeInternalCallback;
            s_nativeInterface.OnHostReachabilityChange      -= HandleHostReachabilityChangeInternalCallback;
        }

        #endregion

        #region Event callback methods

        private static void HandleInternetConnectivityChangeInternalCallback(bool isConnected)
        {
            // update value
            IsInternetActive    = isConnected;

            // notify listeners
            var    result       = new NetworkServicesInternetConnectivityStatusChangeResult()
            {
                IsConnected     = isConnected,
            };
            CallbackDispatcher.InvokeOnMainThread(OnInternetConnectivityChange, result);
        }

        private static void HandleHostReachabilityChangeInternalCallback(bool isReachable)
        {
            // update value
            IsHostReachable     = isReachable;

            // notify listeners
            var     result      = new NetworkServicesHostReachabilityStatusChangeResult()
            {
                IsReachable     = isReachable,
            };
            CallbackDispatcher.InvokeOnMainThread(OnHostReachabilityChange, result);
        }

        #endregion
    }
}
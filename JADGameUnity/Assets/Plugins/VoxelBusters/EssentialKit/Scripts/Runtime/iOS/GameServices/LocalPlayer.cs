#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal sealed class LocalPlayer : Player, ILocalPlayer
    {
        #region Static fields

        private     static  readonly    LocalPlayer     s_localPlayer       = null;

        #endregion

        #region Static event

        private     static  AuthChangeInternalCallback  s_onAuthChange;

        #endregion

        #region Constructors

        static LocalPlayer()
        {
            // register
            PlayerBinding.NPLocalPlayerRegisterCallbacks(HandleAuthChangeNativeCallback);

            // create local player
            s_localPlayer   = new LocalPlayer();
            s_localPlayer.UpdateNativeReference();
        }

        ~LocalPlayer()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        public static LocalPlayer GetLocalPlayer()
        {
            return s_localPlayer;
        }

        public static void Authenticate()
        {
            PlayerBinding.NPLocalPlayerAuthenticate();
        }

        public static void SetAuthChangeCallback(AuthChangeInternalCallback callback)
        {
            // set value
            s_onAuthChange  = callback;
        }

        #endregion

        #region Private methods

        private void UpdateNativeReference()
        {
            var     localPlayerPtr  = PlayerBinding.NPLocalPlayerGetLocalPlayer();

            // set properties
            NativeObjectRef         = new IosNativeObjectRef(localPlayerPtr);
        }

        #endregion

        #region ILocalPlayer implementation

        public bool IsAuthenticated
        {
            get
            {
                return PlayerBinding.NPLocalPlayerIsAuthenticated();
            }
        }

        public bool IsUnderAge
        {
            get
            {
                return PlayerBinding.NPLocalPlayerIsUnderage();
            }
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(GameServicesAuthStateChangeNativeCallback))]
        private static void HandleAuthChangeNativeCallback(GKLocalPlayerAuthState state, string error)
        {
            s_localPlayer.UpdateNativeReference();

            // send result
            var     authStatus  = GameCenterUtility.ConvertToLocalPlayerAuthStatus(state);
            var     errorObj    = Error.CreateNullableError(description: error);
            s_onAuthChange(authStatus, errorObj);
        }

        #endregion
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    internal sealed class LocalPlayer : Player, ILocalPlayer
    {
        #region Static fields

        private     static  LocalPlayer                 s_localPlayer       = new LocalPlayer();

        private     static  AuthChangeInternalCallback  s_onAuthChange;

        #endregion

        #region Constructors

        private LocalPlayer(string playerId = null, string displayName = null, string alias = null) 
            : base(playerId, displayName, alias)
        { }
            
        #endregion

        #region Create methods

        private static LocalPlayer CreateLocalPlayerFromData(PlayerData data)
        {
            return new LocalPlayer(data.Id, data.Name, data.Name);
        }

        #endregion

        #region Static methods

        public static LocalPlayer GetLocalPlayer()
        {
            return s_localPlayer;
        }

        public static void Authenticate()
        {
            GameServicesSimulator.Instance.Authenticate((status, error) =>
            {
                bool isLoggedIn     = (status == LocalPlayerAuthStatus.Authenticated);

                // update local references
                s_localPlayer       = isLoggedIn ? CreateLocalPlayerFromData(GameServicesSimulator.Instance.GetLocalPlayer()) : new LocalPlayer();

                // notify listeners
                if (s_onAuthChange != null)
                {
                    s_onAuthChange(status, error);
                }
            });
        }

        public static void Signout()
        {
            Diagnostics.LogNotSupported("Signout");
        }

        public static void SetAuthChangeCallback(AuthChangeInternalCallback callback)
        {
            // set value
            s_onAuthChange = callback;
        }

        #endregion

        #region ILocalPlayer implementation

        public bool IsAuthenticated
        {
            get
            {
                return GameServicesSimulator.Instance.IsAuthenticated();
            }
        }

        public bool IsUnderAge
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal class NullPlayer : PlayerBase
    {
        #region Constructors

        public NullPlayer()
        {
            LogNotSupported();
        }

        #endregion

        #region Static methods

        public static void LoadPlayers(string[] playerIds, LoadPlayersInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("Player");
        }

        #endregion

        #region Base class methods

        protected override string GetIdInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override string GetAliasInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override string GetDisplayNameInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
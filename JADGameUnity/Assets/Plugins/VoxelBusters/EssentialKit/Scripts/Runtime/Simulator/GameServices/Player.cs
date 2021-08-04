using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    internal class Player : PlayerBase
    {
        #region Fields

        private     string          m_id;

        private     string          m_name;

        private     string          m_alias;

        #endregion

        #region Constructors

        public Player(string id, string name, string alias)
        {
            // set properties
            m_id        = id;
            m_name      = name;
            m_alias     = alias;
        }

        #endregion

        #region Create methods

        internal static Player CreatePlayerFromData(PlayerData data)
        {
            return new Player(data.Id, data.Name, data.Name);
        }

        #endregion

        #region Static methods

        public static void LoadPlayers(string[] playerIds, LoadPlayersInternalCallback callback)
        {
            // get data
            var     data        = GameServicesSimulator.Instance.FindPlayers(playerIds);
            var     players     = Array.ConvertAll(data, (item) => CreatePlayerFromData(item));

            // send result
            callback(players, null);
        }

        #endregion

        #region Base class methods

        protected override string GetIdInternal()
        {
            return m_id;
        }

        protected override string GetAliasInternal()
        {
            return m_alias;
        }

        protected override string GetDisplayNameInternal()
        {
            return m_name;
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            Error   error;
            var     image   = GameServicesSimulator.Instance.GetPlayerImage(Id, out error);

            // send result
            callback(image.EncodeToPNG(), error);
        }

        #endregion
    }
}
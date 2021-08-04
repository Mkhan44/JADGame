using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="GameServices.LoadPlayers(string[], EventCallback{GameServicesLoadPlayersResult})"/> operation is completed.
    /// </summary>
    public class GameServicesLoadPlayersResult
    {
        #region Properties

        /// <summary>
        /// An array of requested players.
        /// </summary>
        public IPlayer[] Players
        {
            get;
            internal set;
        }

        #endregion
    }
}
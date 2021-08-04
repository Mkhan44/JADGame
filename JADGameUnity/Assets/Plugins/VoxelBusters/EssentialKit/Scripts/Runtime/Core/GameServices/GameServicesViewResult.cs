using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when game view is closed.
    /// </summary>
    public class GameServicesViewResult
    {
        #region Properties

        public GameServicesViewResultCode ResultCode
        {
            get;
            internal set;
        }

        #endregion
    }
}
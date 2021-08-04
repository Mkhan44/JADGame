using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="MediaServices.RequestCameraAccess(bool, EventCallback{MediaServicesRequestCameraAccessResult})"/> operation is completed.
    /// </summary>
    public class MediaServicesRequestCameraAccessResult
    {
        #region Properties

        /// <summary>
        /// The access permission granted by the user.
        /// </summary>
        public CameraAccessStatus AccessStatus
        {
            get;
            internal set;
        }

        #endregion
    }
}

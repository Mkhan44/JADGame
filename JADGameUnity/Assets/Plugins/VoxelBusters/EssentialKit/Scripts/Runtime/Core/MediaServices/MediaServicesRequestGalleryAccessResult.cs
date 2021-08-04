using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="MediaServices.RequestGalleryAccess(GalleryAccessMode, bool, EventCallback{MediaServicesRequestGalleryAccessResult})"/> operation is completed.
    /// </summary>
    public class MediaServicesRequestGalleryAccessResult
    {
        #region Properties

        /// <summary>
        /// The access permission provided by the user.
        /// </summary>
        public GalleryAccessStatus AccessStatus
        {
            get;
            internal set;
        }

        #endregion
    }
}

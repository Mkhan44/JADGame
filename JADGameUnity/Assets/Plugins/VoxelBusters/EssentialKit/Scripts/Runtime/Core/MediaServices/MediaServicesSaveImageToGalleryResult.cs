using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="MediaServices.SaveImageToGallery(Texture2D, EventCallback{MediaServicesSaveImageToGalleryResult})"/> operation is completed.
    /// </summary>
    public class MediaServicesSaveImageToGalleryResult
    {
        #region Properties

        /// <summary>
        /// The status of requested operation.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get;
            internal set;
        }

        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// An access status the user can grant for an app to access the gallery data.
    /// </summary>
    public enum GalleryAccessStatus
    {
        /// <summary> The user has not yet made a choice regarding whether this app can access the gallery data. </summary>
        NotDetermined,

        /// <summary> The application is not authorized to access the gallery data. </summary>
        Restricted,

        /// <summary> The user explicitly denied access to gallery data for this application. </summary>
        Denied,

        /// <summary> The application is authorized to access gallery data. </summary>
        Authorized,
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// An access status the user can grant for an app to access the camera.
    /// </summary>
    public enum CameraAccessStatus
    {
        /// <summary> The user has not yet made a choice regarding whether this app can access the camera. </summary>
        NotDetermined,

        /// <summary> The application is not authorized to access the camera. </summary>
        Restricted,

        /// <summary> The user explicitly denied access to camera for this application. </summary>
        Denied,

        /// <summary> The application is authorized to access camera. </summary>
        Authorized,
    }
}
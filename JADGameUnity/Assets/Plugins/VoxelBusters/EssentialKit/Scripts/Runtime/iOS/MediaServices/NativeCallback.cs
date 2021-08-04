#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit.MediaServicesCore.iOS
{
    internal delegate void RequestPhotoLibraryAccessNativeCallback(PHAuthorizationStatus status, string error, IntPtr tagPtr);

    internal delegate void RequestCameraAccessNativeCallback(AVAuthorizationStatus status, string error, IntPtr tagPtr);

    internal delegate void PickImageNativeCallback(IntPtr imageDataPtr, PickImageFinishReason reasonCode, IntPtr tagPtr);

    internal delegate void SaveImageToAlbumNativeCallback(bool success, string error, IntPtr tagPtr);
}
#endif
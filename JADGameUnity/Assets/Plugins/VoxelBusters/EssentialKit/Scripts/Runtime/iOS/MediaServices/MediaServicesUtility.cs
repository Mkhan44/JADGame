#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.MediaServicesCore.iOS
{
    internal static class MediaServicesUtility
    {
        public static GalleryAccessStatus ConvertToGalleryAccessStatus(PHAuthorizationStatus status)
        {
            switch (status)
            {
                case PHAuthorizationStatus.PHAuthorizationStatusDenied:
                    return GalleryAccessStatus.Denied;

                case PHAuthorizationStatus.PHAuthorizationStatusAuthorized:
                    return GalleryAccessStatus.Authorized;

                case PHAuthorizationStatus.PHAuthorizationStatusRestricted:
                    return GalleryAccessStatus.Restricted;

                case PHAuthorizationStatus.PHAuthorizationStatusNotDetermined:
                    return GalleryAccessStatus.NotDetermined;

                default:
                    throw VBException.SwitchCaseNotImplemented(status);
            }
        }

        public static CameraAccessStatus ConvertToCameraAccessStatus(AVAuthorizationStatus status)
        {
            switch (status)
            {
                case AVAuthorizationStatus.AVAuthorizationStatusDenied:
                    return CameraAccessStatus.Denied;

                case AVAuthorizationStatus.AVAuthorizationStatusAuthorized:
                    return CameraAccessStatus.Authorized;

                case AVAuthorizationStatus.AVAuthorizationStatusRestricted:
                    return CameraAccessStatus.Restricted;

                case AVAuthorizationStatus.AVAuthorizationStatusNotDetermined:
                    return CameraAccessStatus.NotDetermined;

                default:
                    throw VBException.SwitchCaseNotImplemented(status);
            }
        }
    }
}
#endif
#if UNITY_ANDROID
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Android
{
    internal static class Converter
    {
        public static GalleryAccessStatus from(NativeGalleryAccessStatus status)
        {
            switch (status)
            {
                case NativeGalleryAccessStatus.Authorized:
                    return GalleryAccessStatus.Authorized;
                case NativeGalleryAccessStatus.Denied:
                    return GalleryAccessStatus.Denied;
                default:
                    throw VBException.SwitchCaseNotImplemented(status);
            }
        }

        public static CameraAccessStatus from(NativeCameraAccessStatus status)
        {
            switch (status)
            {
                case NativeCameraAccessStatus.Authorized:
                    return CameraAccessStatus.Authorized;
                case NativeCameraAccessStatus.Denied:
                    return CameraAccessStatus.Denied;
                default:
                    throw VBException.SwitchCaseNotImplemented(status);
            }
        }
    }
}
#endif

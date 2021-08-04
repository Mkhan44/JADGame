using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.MediaServicesCore
{
    internal sealed class NullMediaServicesInterface : NativeMediaServicesInterfaceBase
    {
        #region Constructors

        public NullMediaServicesInterface()
            : base(isAvailable: false)
        {
            LogNotSupported();
        }

        #endregion
        
        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("MediaServices");
        }

        #endregion

        #region Base class methods

        public override void RequestGalleryAccess(GalleryAccessMode mode, RequestGalleryAccessInternalCallback callback)
        {
            LogNotSupported();

            callback(GalleryAccessStatus.Restricted, Diagnostics.kFeatureNotSupported);
        }

        public override GalleryAccessStatus GetGalleryAccessStatus(GalleryAccessMode mode)
        {
            LogNotSupported();

            return GalleryAccessStatus.Restricted;
        }

        public override void RequestCameraAccess(RequestCameraAccessInternalCallback callback)
        {
            LogNotSupported();

            callback(CameraAccessStatus.Restricted, Diagnostics.kFeatureNotSupported);
        }
        
        public override CameraAccessStatus GetCameraAccessStatus()
        {
            LogNotSupported();

            return CameraAccessStatus.Restricted;
        }

        public override bool CanSelectImageFromGallery()
        {
            LogNotSupported();

            return false;
        }

        public override void SelectImageFromGallery(bool canEdit, SelectImageInternalCallback callback)
        {
            LogNotSupported();
            
            callback(null, Diagnostics.kFeatureNotSupported); 
        }

        public override bool CanCaptureImageFromCamera()
        {
            LogNotSupported();

            return false;
        }

        public override void CaptureImageFromCamera(bool canEdit, SelectImageInternalCallback callback)
        {
            LogNotSupported();
            
            callback(null, Diagnostics.kFeatureNotSupported); 
        }

        public override bool CanSaveImageToGallery()
        {
            LogNotSupported();

            return false;
        }

        public override void SaveImageToGallery(string albumName, Texture2D image, SaveImageToGalleryInternalCallback callback)
        {
            LogNotSupported();
            
            callback(false, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
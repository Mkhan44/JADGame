using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.MediaServicesCore
{
    public abstract class NativeMediaServicesInterfaceBase : NativeFeatureInterfaceBase, INativeMediaServicesInterface
    {
        #region Constructors

        protected NativeMediaServicesInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeMediaServicesInterface implementation

        public abstract void RequestGalleryAccess(GalleryAccessMode mode, RequestGalleryAccessInternalCallback callback);
        
        public abstract GalleryAccessStatus GetGalleryAccessStatus(GalleryAccessMode mode);

        public abstract void RequestCameraAccess(RequestCameraAccessInternalCallback callback);
        
        public abstract CameraAccessStatus GetCameraAccessStatus();

        public abstract bool CanSelectImageFromGallery();

        public abstract void SelectImageFromGallery(bool canEdit, SelectImageInternalCallback callback);

        public abstract bool CanCaptureImageFromCamera();
        
        public abstract void CaptureImageFromCamera(bool canEdit, SelectImageInternalCallback callback);

        public abstract bool CanSaveImageToGallery();

        public abstract void SaveImageToGallery(string albumName, Texture2D image, SaveImageToGalleryInternalCallback callback);

        #endregion
    }
}
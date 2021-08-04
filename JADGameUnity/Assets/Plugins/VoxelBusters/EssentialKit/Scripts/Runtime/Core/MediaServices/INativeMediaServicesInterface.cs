using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.MediaServicesCore
{
    public interface INativeMediaServicesInterface : INativeFeatureInterface
    {
        #region Methods

        void RequestGalleryAccess(GalleryAccessMode mode, RequestGalleryAccessInternalCallback callback);

        GalleryAccessStatus GetGalleryAccessStatus(GalleryAccessMode mode);

        void RequestCameraAccess(RequestCameraAccessInternalCallback callback);
        
        CameraAccessStatus GetCameraAccessStatus();

        bool CanSelectImageFromGallery();

        void SelectImageFromGallery(bool canEdit, SelectImageInternalCallback callback);

        bool CanCaptureImageFromCamera();
        
        void CaptureImageFromCamera(bool canEdit, SelectImageInternalCallback callback);

        bool CanSaveImageToGallery();

        void SaveImageToGallery(string albumName, Texture2D image, SaveImageToGalleryInternalCallback callback);
        
        #endregion
    }
}
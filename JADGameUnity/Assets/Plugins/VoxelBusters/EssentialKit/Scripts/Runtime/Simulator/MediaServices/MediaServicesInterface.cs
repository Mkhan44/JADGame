using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Simulator
{
    public sealed class MediaServicesInterface : NativeMediaServicesInterfaceBase 
    {
        #region Constructors

        public MediaServicesInterface()
            : base(isAvailable: true)
        { }

        #endregion
        
        #region Base class methods

        public override void RequestGalleryAccess(GalleryAccessMode mode, RequestGalleryAccessInternalCallback callback)
        {
            MediaServicesSimulator.Instance.RequestGalleryAccess((status, error) => callback(status, error));
        }

        public override GalleryAccessStatus GetGalleryAccessStatus(GalleryAccessMode mode)
        {
            return MediaServicesSimulator.Instance.GetGalleryAccessStatus();
        }

        public override void RequestCameraAccess(RequestCameraAccessInternalCallback callback)
        {
            MediaServicesSimulator.Instance.RequestCameraAccess((status, error) => callback(status, error));
        }
        
        public override CameraAccessStatus GetCameraAccessStatus()
        {
            return MediaServicesSimulator.Instance.GetCameraAccessStatus();
        }

        public override bool CanSelectImageFromGallery()
        {
            return MediaServicesSimulator.Instance.CanSelectImageFromGallery();
        }

        public override void SelectImageFromGallery(bool canEdit, SelectImageInternalCallback callback)
        {
            MediaServicesSimulator.Instance.SelectImageFromGallery((imageData, error) => callback(imageData, error));
        }

        public override bool CanCaptureImageFromCamera()
        {
            return MediaServicesSimulator.Instance.CanCaptureImageFromCamera();
        }

        public override void CaptureImageFromCamera(bool canEdit, SelectImageInternalCallback callback)
        {
            MediaServicesSimulator.Instance.CaptureImageFromCamera((texture, error) => callback(texture, error));
        }

        public override bool CanSaveImageToGallery()
        {
            return MediaServicesSimulator.Instance.CanSaveImageToGallery();
        }

        public override void SaveImageToGallery(string albumName, Texture2D image, SaveImageToGalleryInternalCallback callback)
        {
            MediaServicesSimulator.Instance.SaveImageToGallery(image, (status, error) => callback(status, error));
        }

        #endregion
    }
}
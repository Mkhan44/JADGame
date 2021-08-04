using System.Text;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class MediaServicesDemo : DemoActionPanelBase<MediaServicesDemoAction, MediaServicesDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     RectTransform[]     m_galleryPermissionDependentObjects     = null;

        [SerializeField]
        private     RectTransform[]     m_cameraPermissionDependentObjects      = null;

        #endregion

        #region Base class methods

        protected override void Start()
        {
            base.Start();

            // set default state
            OnGalleryAccessStatusChange(MediaServices.GetGalleryAccessStatus(GalleryAccessMode.ReadWrite));
            OnCameraAccessStatusChange(MediaServices.GetCameraAccessStatus());
        }

        protected override void OnActionSelectInternal(MediaServicesDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case MediaServicesDemoActionType.RequestGalleryAccess:
                    MediaServices.RequestGalleryAccess(GalleryAccessMode.ReadWrite, callback: (result, error) =>
                    {
                        Log("Request for gallery access finished.");
                        Log("Gallery access status: " + result.AccessStatus);

                        OnGalleryAccessStatusChange(result.AccessStatus);
                    });
                    break;

                case MediaServicesDemoActionType.GetGalleryAccessStatus:
                    var     accessStatus1   = MediaServices.GetGalleryAccessStatus(GalleryAccessMode.ReadWrite);
                    Log("Gallery access status: " + accessStatus1);
                    break;

                case MediaServicesDemoActionType.RequestCameraAccess:
                    MediaServices.RequestCameraAccess(callback: (result, error) =>
                    {
                        Log("Request for camera access finished.");
                        Log("Camera access status: " + result.AccessStatus);
                    
                        OnCameraAccessStatusChange(result.AccessStatus);
                    });
                    break;

                case MediaServicesDemoActionType.GetCameraAccessStatus:
                    var     accessStatus2   = MediaServices.GetCameraAccessStatus();
                    Log("Camera access status: " + accessStatus2);
                    break;

                case MediaServicesDemoActionType.CanSelectImageFromGallery:
                    bool    status1     = MediaServices.CanSelectImageFromGallery();
                    Log("Can select image from gallery: " + status1);
                    break;

                case MediaServicesDemoActionType.SelectImageFromGallery:
                    MediaServices.SelectImageFromGallery(true, (textureData, error) =>
                    {
                        if (error == null)
                        {
                            Log("Select image from gallery finished successfully.");
                        }
                        else
                        {
                            Log("Select image from gallery failed with error. Error: " + error);
                        }
                    });
                    break;

                case MediaServicesDemoActionType.CanCaptureImageFromCamera:
                    bool    status2     = MediaServices.CanCaptureImageFromCamera();
                    Log("Can capture image from camera: " + status2);
                    break;

                case MediaServicesDemoActionType.CaptureImageFromCamera:
                    MediaServices.CaptureImageFromCamera(true, (textureData, error) =>
                    {
                        if (error == null)
                        {
                            Log("Capture image using camera finished successfully.");
                        }
                        else
                        {
                            Log("Capture image using camera failed with error. Error: " + error);
                        }
                    });
                    break;

                case MediaServicesDemoActionType.CanSaveImageToGallery:
                    bool    status3     = MediaServices.CanSaveImageToGallery();
                    Log("Can save image to gallery: " + status3);
                    break;

                case MediaServicesDemoActionType.SaveImageToGallery:
                    var     image       = DemoResources.GetRandomImage();
                    MediaServices.SaveImageToGallery(image, (result, error) =>
                    {
                        if (error == null)
                        {
                            Log("Save image to gallery finished successfully.");
                        }
                        else
                        {
                            Log("Save image to gallery failed with error. Error: " + error);
                        }
                    });
                    break;

                case MediaServicesDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kMediaServices);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Private methods
        
        private void OnGalleryAccessStatusChange(GalleryAccessStatus newStatus)
        {
            // update UI
            bool    active  = (newStatus == GalleryAccessStatus.Authorized);
            foreach (var rect in m_galleryPermissionDependentObjects)
            {
                rect.gameObject.SetActive(active);
            }
        }

        private void OnCameraAccessStatusChange(CameraAccessStatus newStatus)
        {
            // update UI
            bool    active  = (newStatus == CameraAccessStatus.Authorized);
            foreach (var rect in m_cameraPermissionDependentObjects)
            {
                rect.gameObject.SetActive(active);
            }
        }

        #endregion
    }
}

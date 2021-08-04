#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Android
{
    public sealed class MediaServicesInterface : NativeMediaServicesInterfaceBase 
    {
        #region Fields

        private NativeMediaServices m_instance;

        #endregion
        #region Constructors

        public MediaServicesInterface()
            : base(isAvailable: true)
        {
            m_instance = new NativeMediaServices(NativeUnityPluginUtility.GetContext());
        }

        #endregion

        #region Base class methods

        public override void RequestGalleryAccess(GalleryAccessMode mode, RequestGalleryAccessInternalCallback callback)
        {
            if(mode == GalleryAccessMode.Read)
            {
                m_instance.RequestGalleryReadAccess("Read external storage permission required for accessing gallery resources", new NativeRequestGalleryAccessListener()
                {
                    onCompleteCallback = (NativeGalleryAccessStatus status) =>
                    {
                        callback(Converter.from(status), null);
                    }
                });
            }
            else
            {
                m_instance.RequestGalleryReadWriteAccess("Write external storage permission required for writing to gallery", new NativeRequestGalleryAccessListener()
                {
                    onCompleteCallback = (NativeGalleryAccessStatus status) =>
                    {
                        callback(Converter.from(status), null);
                    }
                });
            }
            
        }

        public override GalleryAccessStatus GetGalleryAccessStatus(GalleryAccessMode mode)
        {
            if(mode == GalleryAccessMode.Read)
            {
                return Converter.from(m_instance.GetGalleryReadAccessStatus());
            }
            else
            {
                return Converter.from(m_instance.GetGalleryReadWriteAccessStatus());
            }
        }

        public override void RequestCameraAccess(RequestCameraAccessInternalCallback callback)
        {
            m_instance.RequestCameraAccess("Camera permission required for taking pictures", new NativeRequestCameraAccessListener()
            {
                onCompleteCallback = (NativeCameraAccessStatus status) =>
                {
                    callback(Converter.from(status), null);
                }
            });
        }
        
        public override CameraAccessStatus GetCameraAccessStatus()
        {
            return Converter.from(m_instance.GetCameraAccessStatus());
        }

        public override bool CanSelectImageFromGallery()
        {
            return GetGalleryAccessStatus(GalleryAccessMode.Read) == GalleryAccessStatus.Authorized;
        }

        public override void SelectImageFromGallery(bool canEdit, SelectImageInternalCallback callback)
        {

            m_instance.PickAssetFromGallery("image/*", "Select image from gallery", new NativeMediaAssetSelectionListener()
            {
                onSuccessCallback = (asset) =>
                {
                    FinishLoadingAsset(asset, callback);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        public override bool CanCaptureImageFromCamera()
        {
            return GetCameraAccessStatus() == CameraAccessStatus.Authorized;
        }

        public override void CaptureImageFromCamera(bool canEdit, SelectImageInternalCallback callback)
        {
            m_instance.CaptureImageFromCamera("Take picture from camera", new NativeMediaAssetSelectionListener()
            {
                onSuccessCallback = (asset) =>
                {
                    FinishLoadingAsset(asset, callback);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        public override bool CanSaveImageToGallery()
        {
            return GetGalleryAccessStatus(GalleryAccessMode.ReadWrite) == GalleryAccessStatus.Authorized; ;
        }

        public override void SaveImageToGallery(string albumName, Texture2D image, SaveImageToGalleryInternalCallback callback)
        {
            string      mimeType;
            byte[]      imageData   = image.Encode(out mimeType);

            m_instance.SaveAssetToGallery(albumName, new NativeByteBuffer(imageData), new NativeSaveAssetToGalleryListener()
            {
                onSuccessCallback = () => callback(true, null),
                onFailureCallback = (error) => callback(false, new Error(error))
            });
        }

        #endregion

        #region Utility methods

        private void FinishLoadingAsset(NativeAsset asset, SelectImageInternalCallback callback)
        {
            Debug.Log("Loading asset...");
            asset.Load(new NativeLoadAssetListener()
            {
                onSuccessCallback = (data) =>
                {
                    Debug.Log("Loading asset successful... : " + data + "Bytes : " + data.GetBytes());
                    callback(data.GetBytes(), null);
                },
                onFailureCallback = (error) =>
                {
                    Debug.Log("Loading asset failed... : " + error);

                    callback(null, new Error(error));
                }
            });
        }

        #endregion
    }
}
#endif
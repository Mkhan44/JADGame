#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.MediaServicesCore.iOS
{
    public sealed class MediaServicesInterface : NativeMediaServicesInterfaceBase 
    {
        #region Constructors

        static MediaServicesInterface()
        {
            // register callbacks
            MediaServicesBinding.NPMediaServicesRegisterCallbacks(HandleRequestPhotoLibraryAccessNativeCallback, HandleRequestCameraAccessNativeCallback, HandlePickImageFromPhotoLibraryNativeCallback, HandleSaveImageToAlbumNativeCallback);
        }

        public MediaServicesInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base class methods

        public override void RequestGalleryAccess(GalleryAccessMode mode, RequestGalleryAccessInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            MediaServicesBinding.NPMediaServicesRequestPhotoLibraryAccess(tagPtr);
        }

        public override GalleryAccessStatus GetGalleryAccessStatus(GalleryAccessMode mode)
        {
            var     status      = MediaServicesBinding.NPMediaServicesGetPhotoLibraryAccessStatus();
            return MediaServicesUtility.ConvertToGalleryAccessStatus(status);
        }

        public override void RequestCameraAccess(RequestCameraAccessInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            MediaServicesBinding.NPMediaServicesRequestCameraAccess(tagPtr);
        }
        
        public override CameraAccessStatus GetCameraAccessStatus()
        {
            var     status      = MediaServicesBinding.NPMediaServicesGetCameraAccessStatus();
            return MediaServicesUtility.ConvertToCameraAccessStatus(status);
        }

        public override bool CanSelectImageFromGallery()
        {
            return MediaServicesBinding.NPMediaServicesCanPickImageFromGallery();
        }

        public override void SelectImageFromGallery(bool canEdit, SelectImageInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            MediaServicesBinding.NPMediaServicesPickImageFromGallery(canEdit, tagPtr);
        }

        public override bool CanCaptureImageFromCamera()
        {
            return MediaServicesBinding.NPMediaServicesCanPickImageFromCamera();
        }

        public override void CaptureImageFromCamera(bool canEdit, SelectImageInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            MediaServicesBinding.NPMediaServicesPickImageFromCamera(canEdit, tagPtr);
        }

        public override bool CanSaveImageToGallery()
        {
            return MediaServicesBinding.NPMediaServicesCanSaveImageToAlbum();
        }

        public override void SaveImageToGallery(string albumName, Texture2D image, SaveImageToGalleryInternalCallback callback)
        {
            var     imageData   = image.Encode();
            var     handle      = GCHandle.Alloc(imageData, GCHandleType.Pinned);

            // make request
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            MediaServicesBinding.NPMediaServicesSaveImageToAlbum(albumName, handle.AddrOfPinnedObject(), imageData.Length, tagPtr);

            // release pinned data object
            handle.Free();
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(RequestPhotoLibraryAccessNativeCallback))]
        private static void HandleRequestPhotoLibraryAccessNativeCallback(PHAuthorizationStatus status, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     callback    = (RequestGalleryAccessInternalCallback)tagHandle.Target;
                var     errorObj    = Error.CreateNullableError(description: error);
                callback.Invoke(MediaServicesUtility.ConvertToGalleryAccessStatus(status), errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(RequestCameraAccessNativeCallback))]
        private static void HandleRequestCameraAccessNativeCallback(AVAuthorizationStatus status, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     callback    = (RequestCameraAccessInternalCallback)tagHandle.Target;
                var     errorObj    = Error.CreateNullableError(description: error);
                callback.Invoke(MediaServicesUtility.ConvertToCameraAccessStatus(status), errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(PickImageNativeCallback))]
        private static void HandlePickImageFromPhotoLibraryNativeCallback(IntPtr imageDataPtr, PickImageFinishReason reasonCode, IntPtr tagPtr)
        {
            // get image
            IosNativePluginsUtility.LoadImage(imageDataPtr, (imageData, error) =>
            {
                var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

                try
                {
                    // send result
                    var     callback    = (SelectImageInternalCallback)tagHandle.Target;
                    callback.Invoke(imageData, error);
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                }
                finally
                {
                    // release handle
                    tagHandle.Free();
                }
            });
        }

        [MonoPInvokeCallback(typeof(SaveImageToAlbumNativeCallback))]
        private static void HandleSaveImageToAlbumNativeCallback(bool success, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     callback    = (SaveImageToGalleryInternalCallback)tagHandle.Target;
                var     errorObj    = Error.CreateNullableError(description: error);
                callback.Invoke(success, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        #endregion
    }
}
#endif
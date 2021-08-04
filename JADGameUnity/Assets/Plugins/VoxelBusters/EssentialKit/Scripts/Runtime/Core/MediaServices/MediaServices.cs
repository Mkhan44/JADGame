using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.MediaServicesCore;
using System;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface to access devices's media gallery and camera for picking images and playing videos.
    /// </summary>
    public static class MediaServices
    {
        #region Static fields

        private     static  INativeMediaServicesInterface   s_nativeInterface    = null;

        #endregion

        #region Static properties

        public static MediaServicesUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.MediaServicesSettings;
            }
        }

        #endregion

        #region Constructors

        static MediaServices()
        {
            // create interface object
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeMediaServicesInterface>(ImplementationBlueprint.MediaServices, UnitySettings.IsEnabled);
        }

        #endregion

        #region Setup methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        #endregion

        #region Photo methods

        /// <summary>
        /// Requests for user permission to access the gallery data.
        /// </summary>
        /// <param name="mode">The access mode your app is requesting.</param>
        /// <param name="showPrepermissionDialog">Indicates whether pre-confirmation is required, before prompting system permission dialog.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void RequestGalleryAccess(GalleryAccessMode mode, bool showPrepermissionDialog = true, EventCallback<MediaServicesRequestGalleryAccessResult> callback = null)
        {
            var     permissionHandler   = NativeFeatureUsagePermissionHandler.Default;
            if (showPrepermissionDialog && (permissionHandler != null))
            {
                permissionHandler.ShowPrepermissionDialog(
                    permissionType: GetGalleryAccessModeStringType(mode),
                    onAllowCallback: () =>
                    {
                        // ask user permission
                        RequestGalleryAccessInternal(mode, callback);
                    },
                    onDenyCallback: () =>
                    {
                        CallbackDispatcher.InvokeOnMainThread(callback, result: null, error: new Error(description: "User cancelled."));
                    });
            }
            else
            {
                RequestGalleryAccessInternal(mode, callback);
            }
        }

        /// <summary>
        /// Returns the current authorization status provided to access the gallery.
        /// </summary>
        /// <description>
        /// To see different authorization status, see <see cref="GalleryAccessStatus"/>.
        /// </description>
        /// <param name="mode">The access mode your app is requesting.</param>
        public static GalleryAccessStatus GetGalleryAccessStatus(GalleryAccessMode mode)
        {
            try
            {
                // make request
                return s_nativeInterface.GetGalleryAccessStatus(mode);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return GalleryAccessStatus.NotDetermined;
            }
        }

        /// <summary>
        /// Requests for user permission to access the camera.
        /// </summary>
        /// <param name="showPrepermissionDialog">Indicates whether pre-confirmation is required, before prompting system permission dialog.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void RequestCameraAccess(bool showPrepermissionDialog = true, EventCallback<MediaServicesRequestCameraAccessResult> callback = null)
        {
            var     permissionHandler   = NativeFeatureUsagePermissionHandler.Default;
            if (showPrepermissionDialog && (permissionHandler != null))
            {
                permissionHandler.ShowPrepermissionDialog(
                    permissionType: NativeFeatureUsagePermissionType.kCamera,
                    onAllowCallback: () =>
                    {
                        // ask user permission
                        RequestCameraAccessInternal(callback);
                    },
                    onDenyCallback: () =>
                    {
                        CallbackDispatcher.InvokeOnMainThread(callback, result: null, error: new Error(description: "User cancelled."));
                    });
            }
            else
            {
                RequestCameraAccessInternal(callback);
            }
        }

        /// <summary>
        /// Returns the current authorization status provided to access the camera.
        /// </summary>
        /// <description>
        /// To see different authorization status, see <see cref="CameraAccessStatus"/>.
        /// </description>
        public static CameraAccessStatus GetCameraAccessStatus()
        {
            try
            {
                // make request
                return s_nativeInterface.GetCameraAccessStatus();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return CameraAccessStatus.NotDetermined;
            }
        }

        /// <summary>
        /// Returns a boolean value indicating whether the device supports picking media from gallery.
        /// </summary>
        /// <returns><c>true</c>, if the device supports picking media from gallery, <c>false</c> otherwise.</returns>
        public static bool CanSelectImageFromGallery()
        {
            try
            {
                // make request
                return s_nativeInterface.CanSelectImageFromGallery();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return false;
            }
        }

        /// <summary>
        /// Opens the image picker window.
        /// </summary>
        /// <param name="canEdit">If set to <c>true</c> default edit options are shown.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void SelectImageFromGallery(bool canEdit, EventCallback<TextureData> callback)
        {
            try
            {
                // make request
                s_nativeInterface.SelectImageFromGallery(canEdit, (imageData, error) => SendSelectImageResult(callback, imageData, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Returns a boolean value indicating whether the device supports capturing photo using camera.
        /// </summary>
        /// <returns><c>true</c>, if device supports capturing photo using camera, <c>false</c> otherwise.</returns>
        public static bool CanCaptureImageFromCamera()
        {
            try
            {
                // make request
                return s_nativeInterface.CanCaptureImageFromCamera();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return false;
            }
        }

        /// <summary>
        /// Captures the photo from camera.
        /// </summary>
        /// <param name="canEdit">If set to <c>true</c> default edit options are shown.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void CaptureImageFromCamera(bool canEdit, EventCallback<TextureData> callback)
        {
            try
            {
                // make request
                s_nativeInterface.CaptureImageFromCamera(canEdit, (imageData, error) => SendSelectImageResult(callback, imageData, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Returns a boolean value indicating whether the device supports saving image to gallery.
        /// </summary>
        /// <returns><c>true</c>, if device supports saving image to gallery, <c>false</c> otherwise.</returns>
        public static bool CanSaveImageToGallery()
        {
            try
            {
                // make request
                return s_nativeInterface.CanSaveImageToGallery();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return false;
            }
        }

        /// <summary>
        /// Saves the specified image to gallery.
        /// </summary>
        /// <param name="image">Image to be saved.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void SaveImageToGallery(Texture2D image, EventCallback<MediaServicesSaveImageToGalleryResult> callback)
        {
            SaveImageToGallery(null, image, callback);
        }

        /// <summary>
        /// Saves the specified image to gallery.
        /// </summary>
        /// <param name="albumName">The album name to which image has to saved.</param>
        /// <param name="image">Image to be saved.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void SaveImageToGallery(string albumName, Texture2D image, EventCallback<MediaServicesSaveImageToGalleryResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.SaveImageToGallery(albumName, image, (success, error) => SendSaveImageToGalleryResult(callback, success, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Extension methods

        /// <summary>
        /// Opens the image picker window after user grants required permissions.
        /// </summary>
        /// <param name="canEdit">If set to <c>true</c> default edit options are shown.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void SelectImageFromGalleryWithUserPermision(bool canEdit, EventCallback<TextureData> callback)
        {
            // get user permission incase if its not provided yet
            var     accessStatus    = GetGalleryAccessStatus(GalleryAccessMode.Read);
            if (accessStatus == GalleryAccessStatus.NotDetermined)
            {
                RequestGalleryAccess(GalleryAccessMode.Read, showPrepermissionDialog: true, callback: (accessResult, accessError) =>
                {
                    SelectImageFromGalleryInSafeMode(accessResult.AccessStatus, canEdit, callback);
                });
            }
            else
            {
                SelectImageFromGalleryInSafeMode(accessStatus, canEdit, callback);
            }
        }

        /// <summary>
        /// Captures the photo from camera after user grants required permissions.
        /// </summary>
        /// <param name="canEdit">If set to <c>true</c> default edit options are shown.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void CaptureImageFromCameraWithUserPermision(bool canEdit, EventCallback<TextureData> callback)
        {
            // get user permission incase if its not requested yet
            var     accessStatus    = GetCameraAccessStatus();
            if (accessStatus == CameraAccessStatus.NotDetermined)
            {
                RequestCameraAccess(showPrepermissionDialog: true, callback: (accessResult, accessError) =>
                {
                    CaptureImageFromCameraInSafeMode(accessResult.AccessStatus, canEdit, callback);
                });
            }
            else
            {
                CaptureImageFromCameraInSafeMode(accessStatus, canEdit, callback);
            }
        }

        /// <summary>
        /// Saves the specified image to gallery after user grants required permissions.
        /// </summary>
        /// <param name="albumName">The album name to which image has to saved.</param>
        /// <param name="image">Image to be saved.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void SaveImageToGalleryWithUserPermision(string albumName, Texture2D image, EventCallback<MediaServicesSaveImageToGalleryResult> callback)
        {
            // get user permission incase if its not requested yet
            var     accessStatus    = GetGalleryAccessStatus(GalleryAccessMode.ReadWrite);
            if (accessStatus == GalleryAccessStatus.NotDetermined)
            {
                RequestGalleryAccess(GalleryAccessMode.ReadWrite, showPrepermissionDialog: true, callback: (accessResult, accessError) =>
                {
                    SaveImageToGalleryInSafeMode(accessResult.AccessStatus, albumName, image, callback);
                });
            }
            else
            {
                SaveImageToGalleryInSafeMode(accessStatus, albumName, image, callback);
            }
        }

        #endregion

        #region Private static methods

        private static string GetGalleryAccessModeStringType(GalleryAccessMode mode)
        {
            switch (mode)
            {
                case GalleryAccessMode.Read:
                    return NativeFeatureUsagePermissionType.kGalleryRead;

                case GalleryAccessMode.ReadWrite:
                    return NativeFeatureUsagePermissionType.kGalleryReadWrite;
            }

            throw VBException.NotSupported();
        }

        private static void RequestGalleryAccessInternal(GalleryAccessMode mode, EventCallback<MediaServicesRequestGalleryAccessResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.RequestGalleryAccess(mode, (status, error) =>
                {
                    // send result to caller object
                    var     result      = new MediaServicesRequestGalleryAccessResult()
                    {
                        AccessStatus    = status,
                    };
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        private static void RequestCameraAccessInternal(EventCallback<MediaServicesRequestCameraAccessResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.RequestCameraAccess((status, error) =>
                {
                    // send result to caller object
                    var     result      = new MediaServicesRequestCameraAccessResult()
                    {
                        AccessStatus    = status,
                    };
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        private static void SelectImageFromGalleryInSafeMode(GalleryAccessStatus accessStatus, bool canEdit, EventCallback<TextureData> callback)
        {
            if (GalleryAccessStatus.Authorized == accessStatus)
            {
                SelectImageFromGallery(canEdit, callback);
            }
            else
            {
                SendSelectImageResult(callback, null, new Error(description: "Not authorized!"));
            }
        }

        private static void CaptureImageFromCameraInSafeMode(CameraAccessStatus accessStatus, bool canEdit, EventCallback<TextureData> callback)
        {
            if (CameraAccessStatus.Authorized == accessStatus)
            {
                CaptureImageFromCamera(canEdit, callback);
            }
            else
            {
                SendSelectImageResult(callback, null, new Error(description: "Not authorized!"));
            }
        }

        private static void SaveImageToGalleryInSafeMode(GalleryAccessStatus accessStatus, string albumName, Texture2D image, EventCallback<MediaServicesSaveImageToGalleryResult> callback)
        {
            if (GalleryAccessStatus.Authorized == accessStatus)
            {
                SaveImageToGallery(albumName, image, callback);
            }
            else
            {
                SendSaveImageToGalleryResult(callback, false, new Error(description: "Not authorized!"));
            }
        }

        private static void SendSelectImageResult(EventCallback<TextureData> callback, byte[] imageData, Error error)
        {
            // send result to caller object
            var     textureData     = (imageData == null) ? null : new TextureData(imageData);
            CallbackDispatcher.InvokeOnMainThread(callback, textureData, error);
        }

        private static void SendSaveImageToGalleryResult(EventCallback<MediaServicesSaveImageToGalleryResult> callback, bool success, Error error)
        {
            // send result to caller object
            var     result  = new MediaServicesSaveImageToGalleryResult()
            {
                Success     = success,
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        #endregion
    }
}
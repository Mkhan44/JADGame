#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Android
{
    public class NativeMediaServices : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Private properties
        private NativeActivity Activity
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        public NativeMediaServices(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
            Activity    = new NativeActivity(context);
        }

        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }
        #endregion
        #region Public methods

        public void RequestGalleryReadAccess(string permissionInfo, NativeRequestGalleryAccessListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeMediaServices][Method(RunOnUiThread) : RequestGalleryReadAccess]");
#endif
                Call(Native.Method.kRequestGalleryReadAccess, permissionInfo, listener);
            });
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMediaServices][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void RequestCameraAccess(string permissionInfo, NativeRequestCameraAccessListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeMediaServices][Method(RunOnUiThread) : RequestCameraAccess]");
#endif
                Call(Native.Method.kRequestCameraAccess, permissionInfo, listener);
            });
        }
        public NativeGalleryAccessStatus GetGalleryReadAccessStatus()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMediaServices][Method : GetGalleryReadAccessStatus]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetGalleryReadAccessStatus);
            NativeGalleryAccessStatus data  = NativeGalleryAccessStatusHelper.ReadFromValue(nativeObj);
            return data;
        }
        public NativeGalleryAccessStatus GetGalleryReadWriteAccessStatus()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMediaServices][Method : GetGalleryReadWriteAccessStatus]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetGalleryReadWriteAccessStatus);
            NativeGalleryAccessStatus data  = NativeGalleryAccessStatusHelper.ReadFromValue(nativeObj);
            return data;
        }
        public NativeGalleryAccessStatus GetGalleryWriteAccessStatus()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMediaServices][Method : GetGalleryWriteAccessStatus]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetGalleryWriteAccessStatus);
            NativeGalleryAccessStatus data  = NativeGalleryAccessStatusHelper.ReadFromValue(nativeObj);
            return data;
        }
        public NativeCameraAccessStatus GetCameraAccessStatus()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMediaServices][Method : GetCameraAccessStatus]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetCameraAccessStatus);
            NativeCameraAccessStatus data  = NativeCameraAccessStatusHelper.ReadFromValue(nativeObj);
            return data;
        }
        public void CaptureImageFromCamera(string title, NativeMediaAssetSelectionListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeMediaServices][Method(RunOnUiThread) : CaptureImageFromCamera]");
#endif
                Call(Native.Method.kCaptureImageFromCamera, title, listener);
            });
        }
        public void SaveAssetToGallery(string folderName, NativeByteBuffer assetData, NativeSaveAssetToGalleryListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeMediaServices][Method(RunOnUiThread) : SaveAssetToGallery]");
#endif
                Call(Native.Method.kSaveAssetToGallery, folderName, assetData.NativeObject, listener);
            });
        }
        public void RequestGalleryReadWriteAccess(string permissionInfo, NativeRequestGalleryAccessListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMediaServices][Method : RequestGalleryReadWriteAccess]");
#endif
            Call(Native.Method.kRequestGalleryReadWriteAccess, permissionInfo, listener);
        }
        public void PickAssetFromGallery(string mimeType, string pickerTitle, NativeMediaAssetSelectionListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeMediaServices][Method(RunOnUiThread) : PickAssetFromGallery]");
#endif
                Call(Native.Method.kPickAssetFromGallery, mimeType, pickerTitle, listener);
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.mediaservices.MediaServices";

            internal class Method
            {
                internal const string kPickAssetFromGallery = "pickAssetFromGallery";
                internal const string kCaptureImageFromCamera = "captureImageFromCamera";
                internal const string kRequestCameraAccess = "requestCameraAccess";
                internal const string kGetGalleryReadWriteAccessStatus = "getGalleryReadWriteAccessStatus";
                internal const string kGetCameraAccessStatus = "getCameraAccessStatus";
                internal const string kSaveAssetToGallery = "saveAssetToGallery";
                internal const string kRequestGalleryReadAccess = "requestGalleryReadAccess";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kGetGalleryReadAccessStatus = "getGalleryReadAccessStatus";
                internal const string kRequestGalleryReadWriteAccess = "requestGalleryReadWriteAccess";
                internal const string kGetGalleryWriteAccessStatus = "getGalleryWriteAccessStatus";
            }

        }
    }
}
#endif
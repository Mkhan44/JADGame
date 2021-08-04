#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.CloudServicesCore.Android
{
    public class NativeSaveCloudDataListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnSaveCloudDataSuccessDelegate();
        public delegate void OnSaveCloudDataFailedDelegate(string error);

        #endregion

        #region Public callbacks

        public OnSaveCloudDataSuccessDelegate  onSaveCloudDataSuccessCallback;
        public OnSaveCloudDataFailedDelegate  onSaveCloudDataFailedCallback;

        #endregion


        #region Constructors

        public NativeSaveCloudDataListener() : base("com.voxelbusters.nativeplugins.essentialkit.features.cloudservices.ISaveCloudDataListener")
        {
        }

        #endregion


        #region Public methods

        public void onSaveCloudDataSuccess()
        {
            if(onSaveCloudDataSuccessCallback != null)
            {
                onSaveCloudDataSuccessCallback();
            }
        }
        public void onSaveCloudDataFailed(string error)
        {
            if(onSaveCloudDataFailedCallback != null)
            {
                onSaveCloudDataFailedCallback(error);
            }
        }

        #endregion
    }
}
#endif
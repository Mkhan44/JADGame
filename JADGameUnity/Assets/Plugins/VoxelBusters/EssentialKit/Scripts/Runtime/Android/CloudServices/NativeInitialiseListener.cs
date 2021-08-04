#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.CloudServicesCore.Android
{
    public class NativeInitialiseListener : AndroidJavaProxy
    {
        #region Delegates

        public delegate void OnInitialiseFailedDelegate(string error);
        public delegate void OnInitialiseSuccessDelegate();

        #endregion

        #region Public callbacks

        public OnInitialiseFailedDelegate  onInitialiseFailedCallback;
        public OnInitialiseSuccessDelegate  onInitialiseSuccessCallback;

        #endregion


        #region Constructors

        public NativeInitialiseListener() : base("com.voxelbusters.nativeplugins.essentialkit.features.cloudservices.IInitialiseListener")
        {
        }

        #endregion


        #region Public methods

        public void onInitialiseFailed(string error)
        {
            if(onInitialiseFailedCallback != null)
            {
                onInitialiseFailedCallback(error);
            }
        }
        public void onInitialiseSuccess()
        {
            if(onInitialiseSuccessCallback != null)
            {
                onInitialiseSuccessCallback();
            }
        }

        #endregion
    }
}
#endif
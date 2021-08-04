#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public class NativeGamePlayer : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeGamePlayer(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeGamePlayer(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeGamePlayer()
        {
            DebugLogger.Log("Disposing NativeGamePlayer");
        }
#endif
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

        public string GetName()
        {
            return Call<string>(Native.Method.kGetName);
        }
        public string GetId()
        {
            return Call<string>(Native.Method.kGetId);
        }
        public string GetDisplayName()
        {
            return Call<string>(Native.Method.kGetDisplayName);
        }
        public NativeAsset GetProfileImage(bool needHighRes)
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetProfileImage, needHighRes);
            NativeAsset data  = new  NativeAsset(nativeObj);
            return data;
        }
        public string GetTitle()
        {
            return Call<string>(Native.Method.kGetTitle);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.gameservices.GamePlayer";

            internal class Method
            {
                internal const string kGetTitle = "getTitle";
                internal const string kGetName = "getName";
                internal const string kGetDisplayName = "getDisplayName";
                internal const string kGetProfileImage = "getProfileImage";
                internal const string kGetId = "getId";
            }

        }
    }
}
#endif
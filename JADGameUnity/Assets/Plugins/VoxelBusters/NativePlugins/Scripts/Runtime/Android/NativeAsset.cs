#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativeAsset : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeAsset(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeAsset(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeAsset()
        {
            DebugLogger.Log("Disposing NativeAsset");
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

        public void Load(NativeLoadAssetListener listener)
        {
            Call(Native.Method.kLoad, listener);
        }
        public void LoadRemote(NativeLoadAssetListener listener)
        {
            Call(Native.Method.kLoadRemote, listener);
        }
        public bool IsValid()
        {
            return Call<bool>(Native.Method.kIsValid);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.common.Asset";

            internal class Method
            {
                internal const string kIsValid = "isValid";
                internal const string kLoadRemote = "loadRemote";
                internal const string kLoad = "load";
            }

        }
    }
}
#endif
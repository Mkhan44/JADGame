#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public class NativeNotificationTrigger : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Public properties

        public bool Repeat
        {
            get
            {
                return Get<bool>("repeat");
            }

            set
            {
                Set<bool>("repeat", value);
            }
        }

        #endregion
        #region Constructor

        // Default constructor
        public NativeNotificationTrigger(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeNotificationTrigger(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeNotificationTrigger() : base(Native.kClassName)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeNotificationTrigger()
        {
            DebugLogger.Log("Disposing NativeNotificationTrigger");
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

        public NativeJSONObject ToJson()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kToJson);
            NativeJSONObject data  = new  NativeJSONObject(nativeObj);
            return data;
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.NotificationTrigger";

            internal class Method
            {
                internal const string kToJson = "toJson";
            }

        }
    }
}
#endif
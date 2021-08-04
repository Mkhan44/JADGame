#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public class NativeTimeIntervalNotificationTrigger : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Public properties

        public long TimeInterval
        {
            get
            {
                return Get<long>("timeInterval");
            }

            set
            {
                Set<long>("timeInterval", value);
            }
        }


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
        public NativeTimeIntervalNotificationTrigger(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeTimeIntervalNotificationTrigger(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeTimeIntervalNotificationTrigger(long timeInterval, bool repeat) : base(Native.kClassName ,timeInterval, repeat)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeTimeIntervalNotificationTrigger()
        {
            DebugLogger.Log("Disposing NativeTimeIntervalNotificationTrigger");
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
        public static NativeTimeIntervalNotificationTrigger FromJson(NativeJSONObject jsonObject)
        {
            AndroidJavaObject nativeObj = GetClass().CallStatic<AndroidJavaObject>(Native.Method.kFromJson, jsonObject.NativeObject);
            NativeTimeIntervalNotificationTrigger data  = new  NativeTimeIntervalNotificationTrigger(nativeObj);
            return data;
        }

        #endregion
        #region Public methods

        public NativeDate GetNextTriggerDate()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetNextTriggerDate);
            NativeDate data  = new  NativeDate(nativeObj);
            return data;
        }
        public NativeJSONObject ToJson()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kToJson);
            NativeJSONObject data  = new  NativeJSONObject(nativeObj);
            return data;
        }
        public void UpdateStartTimestamp(long timestamp)
        {
            Call(Native.Method.kUpdateStartTimestamp, timestamp);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.TimeIntervalNotificationTrigger";

            internal class Method
            {
                internal const string kFromJson = "fromJson";
                internal const string kUpdateStartTimestamp = "updateStartTimestamp";
                internal const string kGetNextTriggerDate = "getNextTriggerDate";
                internal const string kToJson = "toJson";
            }

        }
    }
}
#endif
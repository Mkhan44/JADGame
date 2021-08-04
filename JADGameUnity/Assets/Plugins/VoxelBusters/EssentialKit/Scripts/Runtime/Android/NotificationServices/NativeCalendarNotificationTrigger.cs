#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public class NativeCalendarNotificationTrigger : NativeAndroidJavaObjectWrapper
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
        public NativeCalendarNotificationTrigger(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeCalendarNotificationTrigger(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeCalendarNotificationTrigger(NativeCalendarType calendarType, bool repeat) : base(Native.kClassName ,calendarType, repeat)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeCalendarNotificationTrigger()
        {
            DebugLogger.Log("Disposing NativeCalendarNotificationTrigger");
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
        public static NativeCalendarNotificationTrigger FromJson(NativeJSONObject jsonObject)
        {
            AndroidJavaObject nativeObj = GetClass().CallStatic<AndroidJavaObject>(Native.Method.kFromJson, jsonObject.NativeObject);
            NativeCalendarNotificationTrigger data  = new  NativeCalendarNotificationTrigger(nativeObj);
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
        public void SetYear(int year)
        {
            Call(Native.Method.kSetYear, year);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.CalendarNotificationTrigger";

            internal class Method
            {
                internal const string kFromJson = "fromJson";
                internal const string kUpdateStartTimestamp = "updateStartTimestamp";
                internal const string kSetYear = "setYear";
                internal const string kGetNextTriggerDate = "getNextTriggerDate";
                internal const string kToJson = "toJson";
            }

        }
    }
}
#endif
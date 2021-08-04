#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public class NativeNotificationBuilder : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeNotificationBuilder(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeNotificationBuilder(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeNotificationBuilder(string id) : base(Native.kClassName ,id)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeNotificationBuilder()
        {
            DebugLogger.Log("Disposing NativeNotificationBuilder");
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

        public void SetTag(string tag)
        {
            Call(Native.Method.kSetTag, tag);
        }
        public NativeNotification Build()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kBuild);
            NativeNotification data  = new  NativeNotification(nativeObj);
            return data;
        }
        public void SetTitle(string title)
        {
            Call(Native.Method.kSetTitle, title);
        }
        public void SetBigPicture(string name)
        {
            Call(Native.Method.kSetBigPicture, name);
        }
        public void SetLargeIcon(string name)
        {
            Call(Native.Method.kSetLargeIcon, name);
        }
        public void SetUserInfo(string json)
        {
            Call(Native.Method.kSetUserInfo, json);
        }
        public void SetBody(string body)
        {
            Call(Native.Method.kSetBody, body);
        }
        public void SetBadge(int value)
        {
            Call(Native.Method.kSetBadge, value);
        }
        public void SetSoundFileName(string name)
        {
            Call(Native.Method.kSetSoundFileName, name);
        }
        public void SetTrigger(NativeTimeIntervalNotificationTrigger trigger)
        {
            Call(Native.Method.kSetTrigger, trigger.NativeObject);
        }
        public void SetTrigger(NativeLocationNotificationTrigger trigger)
        {
            Call(Native.Method.kSetTrigger, trigger.NativeObject);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.NotificationBuilder";

            internal class Method
            {
                internal const string kSetTitle = "setTitle";
                internal const string kSetBadge = "setBadge";
                internal const string kSetLargeIcon = "setLargeIcon";
                internal const string kSetBigPicture = "setBigPicture";
                internal const string kSetBody = "setBody";
                internal const string kSetUserInfo = "setUserInfo";
                internal const string kSetTrigger = "setTrigger";
                internal const string kSetTag = "setTag";
                internal const string kSetSoundFileName = "setSoundFileName";
                internal const string kBuild = "build";
            }

        }
    }
}
#endif
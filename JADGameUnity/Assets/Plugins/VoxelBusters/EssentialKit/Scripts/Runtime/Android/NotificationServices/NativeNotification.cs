#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public class NativeNotification : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Public properties

        public string Id
        {
            get
            {
                return Get<string>("id");
            }

            set
            {
                Set<string>("id", value);
            }
        }


        public string ContentTitle
        {
            get
            {
                return Get<string>("contentTitle");
            }

            set
            {
                Set<string>("contentTitle", value);
            }
        }


        public string TickerText
        {
            get
            {
                return Get<string>("tickerText");
            }

            set
            {
                Set<string>("tickerText", value);
            }
        }


        public string ContentText
        {
            get
            {
                return Get<string>("contentText");
            }

            set
            {
                Set<string>("contentText", value);
            }
        }


        public int Badge
        {
            get
            {
                return Get<int>("badge");
            }

            set
            {
                Set<int>("badge", value);
            }
        }


        public string SoundFileName
        {
            get
            {
                return Get<string>("soundFileName");
            }

            set
            {
                Set<string>("soundFileName", value);
            }
        }


        public string BigPicture
        {
            get
            {
                return Get<string>("bigPicture");
            }

            set
            {
                Set<string>("bigPicture", value);
            }
        }


        public string Tag
        {
            get
            {
                return Get<string>("tag");
            }

            set
            {
                Set<string>("tag", value);
            }
        }


        public string LargeIcon
        {
            get
            {
                return Get<string>("largeIcon");
            }

            set
            {
                Set<string>("largeIcon", value);
            }
        }


        public string ChannelId
        {
            get
            {
                return Get<string>("channelId");
            }

            set
            {
                Set<string>("channelId", value);
            }
        }


        public NativeNotificationImportance Priority
        {
            get
            {
                return NativeNotificationImportanceHelper.ReadFromValue(Get<AndroidJavaObject>("priority"));
            }

            set
            {
                Set<AndroidJavaObject>("priority", NativeNotificationImportanceHelper.CreateWithValue(value));
            }
        }


        public bool IsLaunchNotification
        {
            get
            {
                return Get<bool>("isLaunchNotification");
            }

            set
            {
                Set<bool>("isLaunchNotification", value);
            }
        }


        public bool IsRemoteNotification
        {
            get
            {
                return Get<bool>("isRemoteNotification");
            }

            set
            {
                Set<bool>("isRemoteNotification", value);
            }
        }


        public NativeJSONObject UserInfo
        {
            get
            {
                AndroidJavaObject javaObject = Get<AndroidJavaObject>("userInfo");
                if (javaObject != null)
                {
                    return new NativeJSONObject(javaObject);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Set<AndroidJavaObject>("userInfo", value.NativeObject);
            }
        }


        public NativeNotificationTrigger Trigger
        {
            get
            {
                AndroidJavaObject javaObject = Get<AndroidJavaObject>("trigger");
                if (javaObject != null)
                {
                    return new NativeNotificationTrigger(javaObject);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Set<AndroidJavaObject>("trigger", value.NativeObject);
            }
        }

        #endregion
        #region Constructor

        // Default constructor
        public NativeNotification(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeNotification(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeNotification(string notificationId) : base(Native.kClassName ,notificationId)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeNotification()
        {
            DebugLogger.Log("Disposing NativeNotification");
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
        public static NativeJSONObject ToJson(NativeContext context, NativeNotification notification)
        {
            AndroidJavaObject nativeObj = GetClass().CallStatic<AndroidJavaObject>(Native.Method.kToJson, context.NativeObject, notification.NativeObject);
            NativeJSONObject data  = new  NativeJSONObject(nativeObj);
            return data;
        }
        public static NativeNotification FromJson(NativeContext context, NativeJSONObject json)
        {
            AndroidJavaObject nativeObj = GetClass().CallStatic<AndroidJavaObject>(Native.Method.kFromJson, context.NativeObject, json.NativeObject);
            NativeNotification data  = new  NativeNotification(nativeObj);
            return data;
        }

        #endregion
        #region Public methods

        public void Dispatch(NativeContext context)
        {
            Call(Native.Method.kDispatch, context.NativeObject);
        }
        public bool HasDateTimeTrigger()
        {
            return Call<bool>(Native.Method.kHasDateTimeTrigger);
        }
        public bool HasLocationTrigger()
        {
            return Call<bool>(Native.Method.kHasLocationTrigger);
        }
        public string GetPersistenceId()
        {
            return Call<string>(Native.Method.kGetPersistenceId);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.Notification";

            internal class Method
            {
                internal const string kFromJson = "fromJson";
                internal const string kDispatch = "dispatch";
                internal const string kHasDateTimeTrigger = "hasDateTimeTrigger";
                internal const string kHasLocationTrigger = "hasLocationTrigger";
                internal const string kToJson = "toJson";
                internal const string kGetPersistenceId = "getPersistenceId";
            }

        }
    }
}
#endif
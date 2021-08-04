#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public class NativeMessageComposer : NativeAndroidJavaObjectWrapper
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

        public NativeMessageComposer(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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
        public static bool CanSendText(NativeContext context)
        {
            return GetClass().CallStatic<bool>(Native.Method.kCanSendText, context.NativeObject);
        }

        public static bool CanSendAttachments(NativeContext context)
        {
            return GetClass().CallStatic<bool>(Native.Method.kCanSendAttachments, context.NativeObject);
        }

        public static bool CanSendSubject(NativeContext context)
        {
            return GetClass().CallStatic<bool>(Native.Method.kCanSendSubject, context.NativeObject);
        }

        #endregion
        #region Public methods

        public void AddAttachment(NativeByteBuffer data, string mimeType, string fileName)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMessageComposer][Method : AddAttachment]");
#endif
            Call(Native.Method.kAddAttachment, data.NativeObject, mimeType, fileName);
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMessageComposer][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void SetBody(string body, bool isHtml)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMessageComposer][Method : SetBody]");
#endif
            Call(Native.Method.kSetBody, body, isHtml);
        }
        public void SetRecipients(string[] receipients)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMessageComposer][Method : SetRecipients]");
#endif
            Call(Native.Method.kSetRecipients, receipients);
        }
        public void Show(NativeMessageComposerListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeMessageComposer][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow, listener);
            });
        }
        public void SetSubject(string value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeMessageComposer][Method : SetSubject]");
#endif
            Call(Native.Method.kSetSubject, value);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.sharingservices.MessageComposer";

            internal class Method
            {
                internal const string kAddAttachment = "addAttachment";
                internal const string kSetRecipients = "setRecipients";
                internal const string kSetBody = "setBody";
                internal const string kCanSendText = "canSendText";
                internal const string kCanSendAttachments = "canSendAttachments";
                internal const string kSetSubject = "setSubject";
                internal const string kCanSendSubject = "canSendSubject";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kShow = "show";
            }

        }
    }
}
#endif
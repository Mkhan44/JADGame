#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public class NativeSocialShareComposer : NativeAndroidJavaObjectWrapper
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

        public NativeSocialShareComposer(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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
        public static bool IsComposerAvailable(NativeContext context, NativeSocialShareComposerType type)
        {
            return GetClass().CallStatic<bool>(Native.Method.kIsComposerAvailable, context.NativeObject, NativeSocialShareComposerTypeHelper.CreateWithValue(type));
        }

        #endregion
        #region Public methods

        public void SetUrl(string urlString)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : SetUrl]");
#endif
            Call(Native.Method.kSetUrl, urlString);
        }
        public void SetText(string text)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : SetText]");
#endif
            Call(Native.Method.kSetText, text);
        }
        public void SetComposerType(NativeSocialShareComposerType type)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : SetComposerType]");
#endif
            Call(Native.Method.kSetComposerType, NativeSocialShareComposerTypeHelper.CreateWithValue(type));
        }
        public void AddAttachment(NativeByteBuffer data, string mimeType, string fileName)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : AddAttachment]");
#endif
            Call(Native.Method.kAddAttachment, data.NativeObject, mimeType, fileName);
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeSocialShareComposer][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void Show(NativeSocialShareComposerListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeSocialShareComposer][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow, listener);
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.sharingservices.SocialShareComposer";

            internal class Method
            {
                internal const string kAddAttachment = "addAttachment";
                internal const string kIsComposerAvailable = "isComposerAvailable";
                internal const string kSetText = "setText";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kSetComposerType = "setComposerType";
                internal const string kSetUrl = "setUrl";
                internal const string kShow = "show";
            }

        }
    }
}
#endif
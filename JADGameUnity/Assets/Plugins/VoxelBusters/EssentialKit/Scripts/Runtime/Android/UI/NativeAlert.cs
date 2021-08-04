#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.UICore.Android
{
    public class NativeAlert : NativeAndroidJavaObjectWrapper
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

        public NativeAlert(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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
        #endregion
        #region Public methods

        public string GetMessage()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAlert][Method : GetMessage]");
#endif
            return Call<string>(Native.Method.kGetMessage);
        }
        public void Dismiss()
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeAlert][Method(RunOnUiThread) : Dismiss]");
#endif
                Call(Native.Method.kDismiss);
            });
        }
        public void SetMessage(string message)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAlert][Method : SetMessage]");
#endif
            Call(Native.Method.kSetMessage, message);
        }
        public void AddButton(string text, bool isCancelType)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAlert][Method : AddButton]");
#endif
            Call(Native.Method.kAddButton, text, isCancelType);
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAlert][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void SetTitle(string title)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAlert][Method : SetTitle]");
#endif
            Call(Native.Method.kSetTitle, title);
        }
        public string GetTitle()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeAlert][Method : GetTitle]");
#endif
            return Call<string>(Native.Method.kGetTitle);
        }
        public void Show(NativeButtonClickListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeAlert][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow, listener);
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.uiviews.Alert";

            internal class Method
            {
                internal const string kGetTitle = "getTitle";
                internal const string kSetTitle = "setTitle";
                internal const string kDismiss = "dismiss";
                internal const string kSetMessage = "setMessage";
                internal const string kAddButton = "addButton";
                internal const string kGetMessage = "getMessage";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kShow = "show";
            }

        }
    }
}
#endif
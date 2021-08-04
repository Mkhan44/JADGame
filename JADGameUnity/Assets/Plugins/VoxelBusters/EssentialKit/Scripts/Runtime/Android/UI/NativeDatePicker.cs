#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.UICore.Android
{
    public class NativeDatePicker : NativeAndroidJavaObjectWrapper
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

        public NativeDatePicker(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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

        public void SetValue(int year, int month, int dayOfMonth)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeDatePicker][Method(RunOnUiThread) : SetValue]");
#endif
                Call(Native.Method.kSetValue, year, month, dayOfMonth);
            });
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeDatePicker][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void SetListener(NativeDatePickerListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeDatePicker][Method(RunOnUiThread) : SetListener]");
#endif
                Call(Native.Method.kSetListener, listener);
            });
        }
        public void Show()
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeDatePicker][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow);
            });
        }
        public void SetMinValue(NativeDate minValue)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeDatePicker][Method(RunOnUiThread) : SetMinValue]");
#endif
                Call(Native.Method.kSetMinValue, minValue.NativeObject);
            });
        }
        public void SetMaxValue(NativeDate maxValue)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeDatePicker][Method(RunOnUiThread) : SetMaxValue]");
#endif
                Call(Native.Method.kSetMaxValue, maxValue.NativeObject);
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.uiviews.DatePicker";

            internal class Method
            {
                internal const string kSetValue = "setValue";
                internal const string kSetMinValue = "setMinValue";
                internal const string kSetMaxValue = "setMaxValue";
                internal const string kSetListener = "setListener";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kShow = "show";
            }

        }
    }
}
#endif
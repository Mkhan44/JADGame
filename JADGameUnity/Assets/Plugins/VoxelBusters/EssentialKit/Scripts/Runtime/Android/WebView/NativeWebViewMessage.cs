#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.WebViewCore.Android
{
    public class NativeWebViewMessage : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Public properties

        public string Url
        {
            get
            {
                return Get<string>("url");
            }

            set
            {
                Set<string>("url", value);
            }
        }


        public string Host
        {
            get
            {
                return Get<string>("host");
            }

            set
            {
                Set<string>("host", value);
            }
        }


        public string Scheme
        {
            get
            {
                return Get<string>("scheme");
            }

            set
            {
                Set<string>("scheme", value);
            }
        }

        #endregion
        #region Constructor

        // Default constructor
        public NativeWebViewMessage(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeWebViewMessage(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativeWebViewMessage() : base(Native.kClassName)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeWebViewMessage()
        {
            DebugLogger.Log("Disposing NativeWebViewMessage");
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

        public string GetArgumentValue(string key)
        {
            return Call<string>(Native.Method.kGetArgumentValue, key);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.webview.WebViewMessage";

            internal class Method
            {
                internal const string kGetArgumentValue = "getArgumentValue";
            }

        }
    }
}
#endif
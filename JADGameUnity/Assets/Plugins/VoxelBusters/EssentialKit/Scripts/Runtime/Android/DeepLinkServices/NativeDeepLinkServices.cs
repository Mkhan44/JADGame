#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore.Android
{
    public class NativeDeepLinkServices : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion

        #region Constructor

        public NativeDeepLinkServices(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
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

        public bool CanHandleUrlSchemeLink(string url)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeDeepLinkServices][Method : CanHandleUrlSchemeLink]");
#endif
            return Call<bool>(Native.Method.kCanHandleUrlSchemeLink, url);
        }
        public bool CanHandleUniversalLink(string url)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeDeepLinkServices][Method : CanHandleUniversalLink]");
#endif
            return Call<bool>(Native.Method.kCanHandleUniversalLink, url);
        }
        public void Initialise(NativeDeepLinkRequestListener deepLinkListener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeDeepLinkServices][Method : Initialise]");
#endif
            Call(Native.Method.kInitialise, deepLinkListener);
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeDeepLinkServices][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.deeplinkservices.DeepLinkServices";

            internal class Method
            {
                internal const string kCanHandleUniversalLink = "canHandleUniversalLink";
                internal const string kCanHandleUrlSchemeLink = "canHandleUrlSchemeLink";
                internal const string kInitialise = "initialise";
                internal const string kGetFeatureName = "getFeatureName";
            }

        }
    }
}
#endif
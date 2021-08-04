#if UNITY_ANDROID
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public class NativePoint : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativePoint(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativePoint(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }
        public NativePoint(double x, double y) : base(Native.kClassName ,x, y)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativePoint()
        {
            DebugLogger.Log("Disposing NativePoint");
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

        public double GetX()
        {
            return Call<double>(Native.Method.kGetX);
        }
        public double GetY()
        {
            return Call<double>(Native.Method.kGetY);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.common.Point";

            internal class Method
            {
                internal const string kGetX = "getX";
                internal const string kGetY = "getY";
            }

        }
    }
}
#endif
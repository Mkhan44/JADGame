#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    public class NativeBillingProduct : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeBillingProduct(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeBillingProduct(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeBillingProduct()
        {
            DebugLogger.Log("Disposing NativeBillingProduct");
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

        public bool IsRewardable()
        {
            return Call<bool>(Native.Method.kIsRewardable);
        }
        public bool IsSubscription()
        {
            return Call<bool>(Native.Method.kIsSubscription);
        }
        public long GetPriceInMicros()
        {
            return Call<long>(Native.Method.kGetPriceInMicros);
        }
        public long GetOriginalPriceInMicros()
        {
            return Call<long>(Native.Method.kGetOriginalPriceInMicros);
        }
        public long GetIntroductoryPriceInMicros()
        {
            return Call<long>(Native.Method.kGetIntroductoryPriceInMicros);
        }
        public string GetTitle()
        {
            return Call<string>(Native.Method.kGetTitle);
        }
        public string GetPrice()
        {
            return Call<string>(Native.Method.kGetPrice);
        }
        public string GetProductIdentifier()
        {
            return Call<string>(Native.Method.kGetProductIdentifier);
        }
        public string GetIconUrl()
        {
            return Call<string>(Native.Method.kGetIconUrl);
        }
        public string GetOriginalPrice()
        {
            return Call<string>(Native.Method.kGetOriginalPrice);
        }
        public string GetIntroductoryPrice()
        {
            return Call<string>(Native.Method.kGetIntroductoryPrice);
        }
        public string GetDescription()
        {
            return Call<string>(Native.Method.kGetDescription);
        }
        public string GetCurrencyCode()
        {
            return Call<string>(Native.Method.kGetCurrencyCode);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.billingservices.common.BillingProduct";

            internal class Method
            {
                internal const string kGetTitle = "getTitle";
                internal const string kGetPrice = "getPrice";
                internal const string kIsRewardable = "isRewardable";
                internal const string kGetIntroductoryPrice = "getIntroductoryPrice";
                internal const string kGetProductIdentifier = "getProductIdentifier";
                internal const string kGetIconUrl = "getIconUrl";
                internal const string kGetOriginalPriceInMicros = "getOriginalPriceInMicros";
                internal const string kGetDescription = "getDescription";
                internal const string kIsSubscription = "isSubscription";
                internal const string kGetCurrencyCode = "getCurrencyCode";
                internal const string kGetOriginalPrice = "getOriginalPrice";
                internal const string kGetPriceInMicros = "getPriceInMicros";
                internal const string kGetIntroductoryPriceInMicros = "getIntroductoryPriceInMicros";
            }

        }
    }
}
#endif
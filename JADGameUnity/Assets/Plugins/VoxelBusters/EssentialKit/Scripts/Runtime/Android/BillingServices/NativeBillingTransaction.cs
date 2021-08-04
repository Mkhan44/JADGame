#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    public class NativeBillingTransaction : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeBillingTransaction(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeBillingTransaction(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeBillingTransaction()
        {
            DebugLogger.Log("Disposing NativeBillingTransaction");
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

        public string GetId()
        {
            return Call<string>(Native.Method.kGetId);
        }
        public NativeBillingTransactionState GetState()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetState);
            NativeBillingTransactionState data  = NativeBillingTransactionStateHelper.ReadFromValue(nativeObj);
            return data;
        }
        public string GetSignature()
        {
            return Call<string>(Native.Method.kGetSignature);
        }
        public string GetPurchaseData()
        {
            return Call<string>(Native.Method.kGetPurchaseData);
        }
        public NativeDate GetPurchaseDate()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetPurchaseDate);
            NativeDate data  = new  NativeDate(nativeObj);
            return data;
        }
        public string GetReceipt()
        {
            return Call<string>(Native.Method.kGetReceipt);
        }
        public string GetUserTag()
        {
            return Call<string>(Native.Method.kGetUserTag);
        }
        public NativeBillingTransactionVerificationState GetVerificationState()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetVerificationState);
            NativeBillingTransactionVerificationState data  = NativeBillingTransactionVerificationStateHelper.ReadFromValue(nativeObj);
            return data;
        }
        public bool IsAcknowledged()
        {
            return Call<bool>(Native.Method.kIsAcknowledged);
        }
        public string GetProductIdentifier()
        {
            return Call<string>(Native.Method.kGetProductIdentifier);
        }
        public void SetVerificationState(NativeBillingTransactionVerificationState verificationState)
        {
            Call(Native.Method.kSetVerificationState, NativeBillingTransactionVerificationStateHelper.CreateWithValue(verificationState));
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.billingservices.common.BillingTransaction";

            internal class Method
            {
                internal const string kGetState = "getState";
                internal const string kGetSignature = "getSignature";
                internal const string kSetVerificationState = "setVerificationState";
                internal const string kGetProductIdentifier = "getProductIdentifier";
                internal const string kGetVerificationState = "getVerificationState";
                internal const string kGetReceipt = "getReceipt";
                internal const string kGetUserTag = "getUserTag";
                internal const string kIsAcknowledged = "isAcknowledged";
                internal const string kGetPurchaseData = "getPurchaseData";
                internal const string kGetPurchaseDate = "getPurchaseDate";
                internal const string kGetId = "getId";
            }

        }
    }
}
#endif
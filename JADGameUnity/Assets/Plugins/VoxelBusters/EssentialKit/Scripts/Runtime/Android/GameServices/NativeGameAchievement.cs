#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public class NativeGameAchievement : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeGameAchievement(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeGameAchievement(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeGameAchievement()
        {
            DebugLogger.Log("Disposing NativeGameAchievement");
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

        public string GetName()
        {
            return Call<string>(Native.Method.kGetName);
        }
        public string GetId()
        {
            return Call<string>(Native.Method.kGetId);
        }
        public bool IsHidden()
        {
            return Call<bool>(Native.Method.kIsHidden);
        }
        public string GetDescription()
        {
            return Call<string>(Native.Method.kGetDescription);
        }
        public void ReportProgress(int stepsToSet, NativeReportProgressListener listener)
        {
            Call(Native.Method.kReportProgress, stepsToSet, listener);
        }
        public bool IsRevealed()
        {
            return Call<bool>(Native.Method.kIsRevealed);
        }
        public int GetTotalSteps()
        {
            return Call<int>(Native.Method.kGetTotalSteps);
        }
        public bool IsUnlocked()
        {
            return Call<bool>(Native.Method.kIsUnlocked);
        }
        public int GetCurrentSteps()
        {
            return Call<int>(Native.Method.kGetCurrentSteps);
        }
        public NativeDate GetLastReportedDate()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetLastReportedDate);
            NativeDate data  = new  NativeDate(nativeObj);
            return data;
        }
        public NativeAsset GetRevealedImage()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetRevealedImage);
            NativeAsset data  = new  NativeAsset(nativeObj);
            return data;
        }
        public NativeAsset GetUnlockedImage()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetUnlockedImage);
            NativeAsset data  = new  NativeAsset(nativeObj);
            return data;
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.gameservices.GameAchievement";

            internal class Method
            {
                internal const string kIsHidden = "isHidden";
                internal const string kGetTotalSteps = "getTotalSteps";
                internal const string kGetLastReportedDate = "getLastReportedDate";
                internal const string kGetName = "getName";
                internal const string kIsUnlocked = "isUnlocked";
                internal const string kIsRevealed = "isRevealed";
                internal const string kGetDescription = "getDescription";
                internal const string kReportProgress = "reportProgress";
                internal const string kGetCurrentSteps = "getCurrentSteps";
                internal const string kGetUnlockedImage = "getUnlockedImage";
                internal const string kGetRevealedImage = "getRevealedImage";
                internal const string kGetId = "getId";
            }

        }
    }
}
#endif
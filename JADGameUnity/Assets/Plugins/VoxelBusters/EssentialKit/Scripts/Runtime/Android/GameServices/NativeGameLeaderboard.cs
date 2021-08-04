#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public class NativeGameLeaderboard : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Constructor

        // Default constructor
        public NativeGameLeaderboard(AndroidJavaObject androidJavaObject) : base(Native.kClassName, androidJavaObject)
        {
        }
        public NativeGameLeaderboard(NativeAndroidJavaObjectWrapper wrapper) : base(wrapper)
        {
        }

#if NATIVE_PLUGINS_DEBUG_ENABLED
        ~NativeGameLeaderboard()
        {
            DebugLogger.Log("Disposing NativeGameLeaderboard");
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
        public NativeAsset GetImage()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetImage);
            NativeAsset data  = new  NativeAsset(nativeObj);
            return data;
        }
        public NativeGameLeaderboardScore GetLocalPlayerScore()
        {
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetLocalPlayerScore);
            NativeGameLeaderboardScore data  = new  NativeGameLeaderboardScore(nativeObj);
            return data;
        }
        public void LoadTopScores(NativeLeaderboardTimeVariant timeVariant, NativeLeaderboardCollectionVariant collectionVariant, int count, bool forceRefresh, NativeLoadScoresListener listener)
        {
            Call(Native.Method.kLoadTopScores, NativeLeaderboardTimeVariantHelper.CreateWithValue(timeVariant), NativeLeaderboardCollectionVariantHelper.CreateWithValue(collectionVariant), count, forceRefresh, listener);
        }
        public void LoadPlayerCenteredScores(NativeLeaderboardTimeVariant timeVariant, NativeLeaderboardCollectionVariant collectionVariant, int count, bool forceRefresh, NativeLoadScoresListener listener)
        {
            Call(Native.Method.kLoadPlayerCenteredScores, NativeLeaderboardTimeVariantHelper.CreateWithValue(timeVariant), NativeLeaderboardCollectionVariantHelper.CreateWithValue(collectionVariant), count, forceRefresh, listener);
        }
        public void LoadMoreScores(int maxResults, int pageDirection, NativeLoadScoresListener listener)
        {
            Call(Native.Method.kLoadMoreScores, maxResults, pageDirection, listener);
        }
        public void LoadLocalPlayerScore(NativeLeaderboardTimeVariant timeVariant, NativeLeaderboardCollectionVariant collectionVariant, NativeLoadLocalPlayerScoreListener listener)
        {
            Call(Native.Method.kLoadLocalPlayerScore, NativeLeaderboardTimeVariantHelper.CreateWithValue(timeVariant), NativeLeaderboardCollectionVariantHelper.CreateWithValue(collectionVariant), listener);
        }
        public void Show(NativeActivity activity, NativeLeaderboardTimeVariant timeSpan, NativeViewListener listener)
        {
            activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeGameLeaderboard][Method(RunOnUiThread) : Show]");
#endif
                Call(Native.Method.kShow, NativeLeaderboardTimeVariantHelper.CreateWithValue(timeSpan), listener);
            });
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.gameservices.GameLeaderboard";

            internal class Method
            {
                internal const string kGetImage = "getImage";
                internal const string kLoadLocalPlayerScore = "loadLocalPlayerScore";
                internal const string kLoadTopScores = "loadTopScores";
                internal const string kGetLocalPlayerScore = "getLocalPlayerScore";
                internal const string kGetName = "getName";
                internal const string kLoadPlayerCenteredScores = "loadPlayerCenteredScores";
                internal const string kLoadMoreScores = "loadMoreScores";
                internal const string kGetId = "getId";
                internal const string kShow = "show";
            }

        }
    }
}
#endif
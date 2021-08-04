#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public class NativeGameServices : NativeAndroidJavaObjectWrapper
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

        public NativeGameServices(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
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

        public void LoadAchievements(NativeLoadAchievementsListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : LoadAchievements]");
#endif
            Call(Native.Method.kLoadAchievements, listener);
        }
        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public void Signout()
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeGameServices][Method(RunOnUiThread) : Signout]");
#endif
                Call(Native.Method.kSignout);
            });
        }
        public void LoadPlayer(string playerId, NativeLoadPlayersListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : LoadPlayer]");
#endif
            Call(Native.Method.kLoadPlayer, playerId, listener);
        }
        public void LoadPlayers(string[] playerIds, NativeLoadPlayersListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : LoadPlayers]");
#endif
            Call(Native.Method.kLoadPlayers, playerIds, listener);
        }
        public void Authenticate(NativePlayerAuthenticationListener listener, bool silentModeOnly)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeGameServices][Method(RunOnUiThread) : Authenticate]");
#endif
                Call(Native.Method.kAuthenticate, listener, silentModeOnly);
            });
        }
        public NativeGameLeaderboard CreateLeaderboard(string id)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : CreateLeaderboard]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kCreateLeaderboard, id);
            NativeGameLeaderboard data  = new  NativeGameLeaderboard(nativeObj);
            return data;
        }
        public NativeGameAchievement CreateAchievement(string id)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : CreateAchievement]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kCreateAchievement, id);
            NativeGameAchievement data  = new  NativeGameAchievement(nativeObj);
            return data;
        }
        public NativeGameLeaderboardScore CreateScore(string leaderboardId)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : CreateScore]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kCreateScore, leaderboardId);
            NativeGameLeaderboardScore data  = new  NativeGameLeaderboardScore(nativeObj);
            return data;
        }
        public void LoadLeaderboards(NativeLoadLeaderboardsListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : LoadLeaderboards]");
#endif
            Call(Native.Method.kLoadLeaderboards, listener);
        }
        public void ShowLeaderboards(string leaderboardId, NativeLeaderboardTimeVariant timeSpan, NativeViewListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeGameServices][Method(RunOnUiThread) : ShowLeaderboards]");
#endif
                Call(Native.Method.kShowLeaderboards, leaderboardId, NativeLeaderboardTimeVariantHelper.CreateWithValue(timeSpan), listener);
            });
        }
        public void ShowAchievements(NativeViewListener listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeGameServices][Method(RunOnUiThread) : ShowAchievements]");
#endif
                Call(Native.Method.kShowAchievements, listener);
            });
        }
        public void LoadServerCredentials(NativeLoadServerCredentials listener)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeGameServices][Method(RunOnUiThread) : LoadServerCredentials]");
#endif
                Call(Native.Method.kLoadServerCredentials, listener);
            });
        }
        public NativeGameAchievement GetAchievement(string achievementId)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeGameServices][Method : GetAchievement]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetAchievement, achievementId);
            NativeGameAchievement data  = new  NativeGameAchievement(nativeObj);
            return data;
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.gameservices.GameServices";

            internal class Method
            {
                internal const string kAuthenticate = "authenticate";
                internal const string kSignout = "signout";
                internal const string kLoadPlayers = "loadPlayers";
                internal const string kCreateScore = "createScore";
                internal const string kLoadServerCredentials = "loadServerCredentials";
                internal const string kLoadPlayer = "loadPlayer";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kGetAchievement = "getAchievement";
                internal const string kShowLeaderboards = "showLeaderboards";
                internal const string kLoadLeaderboards = "loadLeaderboards";
                internal const string kShowAchievements = "showAchievements";
                internal const string kLoadAchievements = "loadAchievements";
                internal const string kCreateLeaderboard = "createLeaderboard";
                internal const string kCreateAchievement = "createAchievement";
            }

        }
    }
}
#endif
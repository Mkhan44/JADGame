using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.GameServicesCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface to easily integrate popular social gaming functionalities such as achievements, leaderboards on your mobile games.
    /// </summary>
    /// <description>
    /// <para>
    /// Internally, game services feature uses native game servers for handling functionalities. 
    /// So while running your game on iOS devices, Game Center servers will be used. Whereas on Android platform, Play Game Services server will be used. 
    /// </para>
    /// <para>
    /// Goto <a href="https://developer.apple.com/library/ios/documentation/LanguagesUtilities/Conceptual/iTunesConnectGameCenter_Guide/AccessAndEnable/AccessAndEnable.html">iTune's Connect</a> to configure leaderboard, achievemnts for your iOS game. 
    /// And for Android, add records at <a href="https://developers.google.com/games/services/android/quickstart">Google Developer Console</a>.
    /// </para>
    /// </description>
    public static class GameServices
    {
        #region Static fields

        private     static  INativeGameServicesInterface    s_nativeInterface;

        #endregion

        #region Static properties

        public static GameServicesUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.GameServicesSettings;
            }
        }

        public static LeaderboardDefinition[] LeaderboardDefinitions 
        {
            get;
            private set;
        }

        public static AchievementDefinition[] AchievementDefinitions
        { 
            get;
            private set;
        }

        /// <summary>
        /// Returns the local player.
        /// </summary>
        /// <value>The local player.</value>
        public static ILocalPlayer LocalPlayer
        {
            get
            {
                try
                {
                    return (s_nativeInterface != null) ? s_nativeInterface.GetLocalPlayer() : new NullLocalPlayer();
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                    return null;
                }
            }
        }

        /// <summary>
        /// A boolean value indicating whether this local player is authenticated.
        /// </summary>
        /// <value><c>true</c> if is authenticated; otherwise, <c>false</c>.</value>
        public static bool IsAuthenticated
        {
            get
            {
                try
                {
                    return LocalPlayer.IsAuthenticated;
                }
                catch (Exception exception)
                {
                    DebugLogger.LogException(exception);
                    return false;
                }
            }
        }

        /// <summary>
        /// Returns the cached leaderboards array.
        /// </summary>
        /// <remarks>
        /// \note This property is invalid until a call to <see cref="LoadLeaderboards(EventCallback{GameServicesLoadLeaderboardsResult})"/> is completed.
        /// </remarks>
        public static ILeaderboard[] Leaderboards
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the cached achievement description array.
        /// </summary>
        /// <remarks>
        /// \note This property is invalid until a call to <see cref="LoadAchievementDescriptions(EventCallback{GameServicesLoadAchievementDescriptionsResult})"/> is completed.
        /// </remarks>
        public static IAchievementDescription[] AchievementDescriptions
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the cached achievements array.
        /// </summary>
        /// <remarks>
        /// \note This property is invalid until a call to <see cref="LoadAchievements(EventCallback{GameServicesLoadAchievementsResult})"/> is completed.
        /// </remarks>
        public static IAchievement[] Achievements
        {
            get;
            private set;
        }

        #endregion

        #region Static events

        /// <summary>
        /// Event called on local player auth change.
        /// </summary>
        public static event EventCallback<GameServicesAuthStatusChangeResult> OnAuthStatusChange;

        #endregion

        #region Setup methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        /// <summary>
        /// Initializes game services.
        /// </summary>
        public static void Initialize()
        {
            Initialize(UnitySettings.Leaderboards, UnitySettings.Achievements);
        }

        internal static void Initialize(LeaderboardDefinition[] leaderboardDefinitions, AchievementDefinition[] achievementDefinitions)
        {
            // validate arguments
            Assertions.AssertIfArrayIsNull(leaderboardDefinitions, "leaderboardDefinitions");
            Assertions.AssertIfArrayIsNull(achievementDefinitions, "achievementDefinitions");

            // initialise components
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeGameServicesInterface>(ImplementationBlueprint.GameServices, UnitySettings.IsEnabled);

            // set properties
            Leaderboards            = new ILeaderboard[0];
            AchievementDescriptions = new IAchievementDescription[0];
            Achievements            = new IAchievement[0];
            LeaderboardDefinitions  = leaderboardDefinitions;
            AchievementDefinitions  = achievementDefinitions;

            // register for events
            s_nativeInterface.SetCanShowAchievementCompletionBanner(UnitySettings.ShowAchievementCompletionBanner);
            s_nativeInterface.SetAuthChangeCallback(HandleAuthChangeInternalCallback);
        }

        internal static LeaderboardDefinition FindLeaderboardDefinitionWithId(string leaderboardId)
        {
            return Array.Find(LeaderboardDefinitions, (item) => string.Equals(item.Id, leaderboardId));
        }

        internal static LeaderboardDefinition FindLeaderboardDefinitionWithPlatformId(string leaderboardPlatformId)
        {
            return Array.Find(LeaderboardDefinitions, (item) => string.Equals(item.GetPlatformIdForActivePlatform(), leaderboardPlatformId));
        }

        internal static AchievementDefinition FindAchievementDefinitionWithId(string achievementId)
        {
            return Array.Find(AchievementDefinitions, (item) => string.Equals(item.Id, achievementId));
        }

        internal static AchievementDefinition FindAchievementDefinitionWithPlatformId(string achievementPlatformId)
        {
            return Array.Find(AchievementDefinitions, (item) => string.Equals(item.GetPlatformIdForActivePlatform(), achievementPlatformId));
        }

        #endregion

        #region Leaderboard methods

        /// <summary>
        /// Creates a new instance of leaderboard object.
        /// </summary>
        /// <param name="leaderboardId">A string used to uniquely identify the leaderboard.</param>
        public static ILeaderboard CreateLeaderboard(string leaderboardId)
        {
            // find settings information for specified id
            var     settings    = FindLeaderboardDefinitionWithId(leaderboardId);
            Assertions.AssertIfTrue(null == settings, "Could not find settings for specified id: " + leaderboardId);

            try
            {
                // create object
                return s_nativeInterface.CreateLeaderboard(settings.Id, settings.GetPlatformIdForActivePlatform());
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
        }

        /// <summary>
        /// Loads the leaderboards.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void LoadLeaderboards(EventCallback<GameServicesLoadLeaderboardsResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.LoadLeaderboards((leaderboards, error) => SendLoadLeaderboardsResult(callback, leaderboards, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Achievement methods

        /// <summary>
        /// Creates a new instance of achievement object.
        /// </summary>
        /// <param name="achievementId">A string used to uniquely identify the achievement.</param>
        public static IAchievement CreateAchievement(string achievementId)
        {
            // find settings information for specified id
            var     settings    = FindAchievementDefinitionWithId(achievementId);
            Assertions.AssertIfTrue(null == settings, "Could not find settings for specified id: " + achievementId);

            try
            {
                // make request
                return s_nativeInterface.CreateAchievement(settings.Id, settings.GetPlatformIdForActivePlatform());
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
        }

        /// <summary>
        /// Loads the achievement descriptions from game server.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void LoadAchievementDescriptions(EventCallback<GameServicesLoadAchievementDescriptionsResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.LoadAchievementDescriptions((descriptions, error) => SendLoadAchievementDescriptionsResult(callback, descriptions, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Loads previously submitted achievement progress for the current local player.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void LoadAchievements(EventCallback<GameServicesLoadAchievementsResult> callback)
        {
            try
            {
                // make request
                s_nativeInterface.LoadAchievements((achievements, error) => SendLoadAchievementsResult(callback, achievements, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Reports the local user's achievement progress to game server, using platform specific id.
        /// </summary>
        /// <param name="achievementId">A string used to uniquely identify the achievement.</param>
        /// <param name="percentageCompleted">The value indicates how far the player has progressed.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void ReportAchievementProgress(string achievementId, double percentageCompleted, CompletionCallback callback)
        {
            var     achievement     = CreateAchievement(achievementId);
            ReportAchievementProgress(achievement, percentageCompleted, callback);
        }

        /// <summary>
        /// Reports the local user's achievement progress to game server.
        /// </summary>
        /// <param name="achievementDescription">The achievement description object.</param>
        /// <param name="percentageCompleted">The value indicates how far the player has progressed.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void ReportAchievementProgress(IAchievementDescription achievementDescription, double percentageCompleted, CompletionCallback callback)
        {
            var     achievement     = CreateAchievement(achievementDescription.Id);
            ReportAchievementProgress(achievement, percentageCompleted, callback);
        }

        /// <summary>
        /// Reports the local user's achievement progress to game server.
        /// </summary>
        /// <param name="achievement">The achievement object.</param>
        /// <param name="percentageCompleted">The value indicates how far the player has progressed.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void ReportAchievementProgress(IAchievement achievement, double percentageCompleted, CompletionCallback callback)
        {
            Assertions.AssertIfArgIsNull(achievement, "achievement");

            // submit information
            achievement.PercentageCompleted = percentageCompleted;
            achievement.ReportProgress(callback);
        }

        #endregion

        #region Player methods

        /// <summary>
        /// Initiates authentication process for the local player on the device.
        /// </summary>
        public static void Authenticate()
        {
            try
            {
                s_nativeInterface.Authenticate();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Signout the local player on the device.
        /// </summary>
        public static void Signout()
        {
            try
            {
                s_nativeInterface.Signout();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Loads the player details from game server.
        /// </summary>
        /// <param name="playerIds">An array of player id's whose details has to be retrieved from game server.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void LoadPlayers(string[] playerIds, EventCallback<GameServicesLoadPlayersResult> callback)
        {
            // validate arguments
            Assertions.AssertIfArrayIsNullOrEmpty(playerIds, "playerIds");

            try
            {
                // make request
                s_nativeInterface.LoadPlayers(playerIds, (players, error) => SendLoadPlayersResult(callback, players, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Score methods

        /// <summary>
        /// Creates the score for specified leaderboard.
        /// </summary>
        /// <returns>The score object.</returns>
        /// <param name="leaderboardId">A string used to uniquely identify the leaderboard.</param>
        public static IScore CreateScore(string leaderboardId)
        {
            // find settings information for specified id
            var     settings    = FindLeaderboardDefinitionWithId(leaderboardId);
            Assertions.AssertIfTrue(null == settings, "Could not find settings for specified id: " + leaderboardId);

            try
            {
                // create object
                return s_nativeInterface.CreateScore(settings.Id, settings.GetPlatformIdForActivePlatform());
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
        }

        /// <summary>
        /// Creates the score for specified leaderboard.
        /// </summary>
        /// <returns>The score object.</returns>
        /// <param name="leaderboard">The leaderboard object.</param>
        public static IScore CreateScore(ILeaderboard leaderboard)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(leaderboard, "leaderboard");

            try
            {
                // create object
                return s_nativeInterface.CreateScore(leaderboard.Id, leaderboard.PlatformId);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
        }

        /// <summary>
        /// Reports the score to game server.
        /// </summary>
        /// <param name="leaderboardId">A string used to uniquely identify the leaderboard.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void ReportScore(string leaderboardId, long value, CompletionCallback callback)
        {
            // create score object
            var     score   = CreateScore(leaderboardId);
            Assertions.AssertIfPropertyIsNull(score, "score");
            
            score.Value     = value;
            
            // submit score
            score.ReportScore(callback);
        }

        /// <summary>
        /// Reports the score to game server.
        /// </summary>
        /// <param name="leaderboard">The leaderboard object.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void ReportScore(ILeaderboard leaderboard, long value, CompletionCallback callback)
        {
            // create score object
            var     score   = CreateScore(leaderboard);
            Assertions.AssertIfPropertyIsNull(score, "score");

            score.Value     = value;
            
            // submit score
            score.ReportScore(callback);
        }

        #endregion

        #region View methods

        /// <summary>
        /// Opens the standard view to display all the leaderboards.
        /// </summary>
        /// <param name="timescope">A time filter used to restrict which scores are displayed to the player.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void ShowLeaderboards(LeaderboardTimeScope timescope = LeaderboardTimeScope.AllTime, EventCallback<GameServicesViewResult> callback = null)
        {
            try
            {
                // make request
                s_nativeInterface.ShowLeaderboard(null, null, timescope, (error) => SendViewClosedResult(callback, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Opens the standard view to display leaderboard scores corresponding to given id.
        /// </summary>
        /// <param name="leaderboardId">A string used to identify the leaderboard.</param>
        /// <param name="timescope">A time filter used to restrict which scores are displayed to the player.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        /// <remarks>
        /// \note Incase, if you want to list out all the leaderboards that are used in your game, then pass <c>null</c> for leaderboard identifier.
        /// </remarks>
        public static void ShowLeaderboard(string leaderboardId, LeaderboardTimeScope timescope = LeaderboardTimeScope.AllTime, EventCallback<GameServicesViewResult> callback = null)
        {
            // find settings information for specified id
            var     settings    = FindLeaderboardDefinitionWithId(leaderboardId);
            Assertions.AssertIfTrue(null == settings, "Could not find settings for specified id: " + leaderboardId);

            try
            {
                // make request
                s_nativeInterface.ShowLeaderboard(leaderboardId, settings.GetPlatformIdForActivePlatform(), timescope, (error) => SendViewClosedResult(callback, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Opens the standard view to display leaderboard scores corresponding to given leaderboard.
        /// </summary>
        /// <param name="leaderboard">The leaderboard object.</param>
        /// <param name="timescope">A time filter used to restrict which scores are displayed to the player.</param>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        /// <remarks>
        /// \note Incase, if you want to list out all the leaderboards that are used in your game, then pass <c>null</c> for leaderboard identifier.
        /// </remarks>
        public static void ShowLeaderboard(ILeaderboard leaderboard, LeaderboardTimeScope timescope = LeaderboardTimeScope.AllTime, EventCallback<GameServicesViewResult> callback = null)
        {
            // validate arguments
            Assertions.AssertIfArgIsNull(leaderboard, "leaderboard");

            try
            {
                // make request
                s_nativeInterface.ShowLeaderboard(leaderboard.Id, leaderboard.PlatformId, timescope, (error) => SendViewClosedResult(callback, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Opens the standard view to display achievement progress screen for the local player.
        /// </summary>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        public static void ShowAchievements(EventCallback<GameServicesViewResult> callback = null)
        {
            try
            {
                s_nativeInterface.ShowAchievements((error) => SendViewClosedResult(callback, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Misc methods

        
        public static void LoadServerCredentials(EventCallback<GameServicesLoadServerCredentialsResult>  callback)
        {
            try
            {
                s_nativeInterface.LoadServerCredentials((credentials, error) => SendLoadServerCredentialsResult(callback, credentials, error));
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Callback methods

        private static void HandleAuthChangeInternalCallback(LocalPlayerAuthStatus authStatus, Error error)
        {
            // invoke event
            var     result      = new GameServicesAuthStatusChangeResult()
            {
                LocalPlayer     = LocalPlayer,
                AuthStatus      = authStatus,
            };
            CallbackDispatcher.InvokeOnMainThread(OnAuthStatusChange, result, error);
        }

        private static void SendLoadLeaderboardsResult(EventCallback<GameServicesLoadLeaderboardsResult> callback, ILeaderboard[] leaderboards, Error error)
        {
            // check whether fetched value is null
            leaderboards        = leaderboards ?? new ILeaderboard[0];

            // cache data
            Leaderboards        = leaderboards;

            // send result to caller object
            var     result      = new GameServicesLoadLeaderboardsResult()
            {
                Leaderboards    = leaderboards,
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        private static void SendLoadAchievementDescriptionsResult(EventCallback<GameServicesLoadAchievementDescriptionsResult> callback, IAchievementDescription[] descriptions, Error error)
        {
            // check whether fetched value is null
            descriptions            = descriptions ?? new IAchievementDescription[0];

            // cache data
            AchievementDescriptions = descriptions;

            // send result to caller object
            var     result          = new GameServicesLoadAchievementDescriptionsResult()
            {
                AchievementDescriptions     = descriptions,
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        private static void SendLoadAchievementsResult(EventCallback<GameServicesLoadAchievementsResult> callback, IAchievement[] achievements, Error error)
        {
            // check whether fetched value is null
            achievements        = achievements ?? new IAchievement[0];

            // update cached data
            Achievements        = achievements;

            // send result to caller object
            var     result      = new GameServicesLoadAchievementsResult()
            {
                Achievements    = achievements,
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        private static void SendLoadPlayersResult(EventCallback<GameServicesLoadPlayersResult> callback, IPlayer[] players, Error error)
        {
            // send result to caller object
            var     result      = new GameServicesLoadPlayersResult()
            {
                Players         = players ?? new IPlayer[0],
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        private static void SendViewClosedResult(EventCallback<GameServicesViewResult> callback, Error error)
        {
            // send result to caller object
            var     result      = new GameServicesViewResult()
            {
                ResultCode      = (error == null) ? GameServicesViewResultCode.Done : GameServicesViewResultCode.Unknown,
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        private static void SendLoadServerCredentialsResult(EventCallback<GameServicesLoadServerCredentialsResult> callback, ServerCredentials serverCredentials, Error error)
        {
            // send result to caller object
            var result = new GameServicesLoadServerCredentialsResult()
            {
                ServerCredentials = serverCredentials
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        #endregion
    }
}

#if UNITY_ANDROID
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public sealed class GameServicesInterface : NativeGameServicesInterfaceBase 
    {
#region Fields

        private NativeGameServices m_instance;
        private AuthChangeInternalCallback m_authChangeCallback;
        private LocalPlayer m_localPlayer;

#endregion

#region Constructors

        public GameServicesInterface()
            : base(isAvailable: true)
        {
            m_instance = new NativeGameServices(NativeUnityPluginUtility.GetContext());
            m_localPlayer = new LocalPlayer();
        }

#endregion

#region INativeGameServicesInterface implementation

        public override void LoadLeaderboards(LoadLeaderboardsInternalCallback callback)
        {
            m_instance.LoadLeaderboards(new NativeLoadLeaderboardsListener()
            {
                onSuccessCallback = (nativeList) =>
                {
                    //Filter achievements which are listed in the settings only
                    List<NativeGameLeaderboard> nativeLeaderboards = nativeList.Get();
                    List<Leaderboard> filteredLeaderboards = new List<Leaderboard>();

                    foreach (NativeGameLeaderboard each in nativeLeaderboards)
                    {
                        var settings = GameServices.FindLeaderboardDefinitionWithPlatformId(each.GetId());
                        if (settings != null)
                        {
                            filteredLeaderboards.Add(new Leaderboard(settings.Id, each));
                        }
                    }

                    Leaderboard[] leaderboards = filteredLeaderboards.ToArray();
                    callback(leaderboards, null);
                },
                onFailureCallback = (string error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        public override void ShowLeaderboard(string leaderboardId, string leaderboardPlatformId, LeaderboardTimeScope timeScope, ViewClosedInternalCallback callback)
        {
            m_instance.ShowLeaderboards(leaderboardPlatformId, Converter.from(timeScope), new NativeViewListener()
            {
                onCloseCallback = (error) =>
                {
                    callback(new Error(error));
                }
            });
        }

        public override ILeaderboard CreateLeaderboard(string id, string platformId)
        {
            NativeGameLeaderboard nativeGameLeaderboard = m_instance.CreateLeaderboard(platformId);
            return new Leaderboard(id, nativeGameLeaderboard);
        }

        public override void LoadAchievementDescriptions(LoadAchievementDescriptionsInternalCallback callback)
        {
            AchievementDescription.SetAchievedDescriptionFormats(GameServices.UnitySettings.AndroidProperties.AchievedDescriptionFormats);
            m_instance.LoadAchievements(new NativeLoadAchievementsListener()
            {
                onSuccessCallback = (NativeArrayBuffer<NativeGameAchievement> nativeArray)  =>
                {
                    List<AchievementDescription> filteredAchievements = new List<AchievementDescription>();
                    int count = nativeArray.Size();
                    for(int i=0; i<count; i++)
                    {
                        NativeGameAchievement each = nativeArray.Get(i);
                        var settings = GameServices.FindAchievementDefinitionWithPlatformId(each.GetId());
                        if (settings != null)
                        {
                            filteredAchievements.Add(new AchievementDescription(settings.Id, each, settings.NumOfStepsToUnlock));
                        }
                    }
                    callback(filteredAchievements.ToArray(), null);
                },
                onFailureCallback = (string error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        public override void LoadAchievements(LoadAchievementsInternalCallback callback)
        {
            m_instance.LoadAchievements(new NativeLoadAchievementsListener()
            {
                onSuccessCallback = (NativeArrayBuffer<NativeGameAchievement> nativeArray) =>
                {
                    List<Achievement> filteredAchievements = new List<Achievement>();
                    int count = nativeArray.Size();
                    for (int i = 0; i < count; i++)
                    {
                        NativeGameAchievement each = nativeArray.Get(i);
                        var settings = GameServices.FindAchievementDefinitionWithPlatformId(each.GetId());
                        if (settings != null && (!each.GetLastReportedDate().IsNull()))
                        {
                            filteredAchievements.Add(new Achievement(settings.Id, each));
                        }
                    }
                    callback(filteredAchievements.ToArray(), null);
                },
                onFailureCallback = (string error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        public override void ShowAchievements(ViewClosedInternalCallback callback)
        {
            m_instance.ShowAchievements(new NativeViewListener()
            {
                onCloseCallback = (error) =>
                {
                    callback(new Error(error));
                }
            });
        }

        public override void SetCanShowAchievementCompletionBanner(bool value)
        {
            DebugLogger.LogWarning("This operation is not allowed on Android");
        }

        public override IAchievement CreateAchievement(string id, string platformId)
        {
            NativeGameAchievement nativeGameAchievement = m_instance.CreateAchievement(platformId);
            return new Achievement(id, nativeGameAchievement);
        }

        public override void LoadPlayers(string[] playerIds, LoadPlayersInternalCallback callback)
        {
            m_instance.LoadPlayers(playerIds, new NativeLoadPlayersListener()
            {
                onSuccessCallback = (nativePlayers) =>
                {
                    Player[] players = NativeUnityPluginUtility.Map<NativeGamePlayer, Player>(nativePlayers.Get());
                    callback(players, null);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        public override void SetAuthChangeCallback(AuthChangeInternalCallback callback)
        {
            m_authChangeCallback = callback;
        }

        public override void Authenticate()
        {
            m_instance.Authenticate(new NativePlayerAuthenticationListener()
            {
                onSuccessCallback = (player) =>
                {
                    if(m_authChangeCallback != null)
                    {
                        if(player == null || player.NativeObject == null)
                        {
                            m_localPlayer.SetPlayer(null);
                            m_authChangeCallback(LocalPlayerAuthStatus.NotAvailable, null);
                        }
                        else
                        {
                            m_localPlayer.SetPlayer(player);
                            m_authChangeCallback(LocalPlayerAuthStatus.Authenticated, null);
                        }
                    }
                },
                onFailureCallback = (error) =>
                {
                    if(m_authChangeCallback != null)
                    {
                        m_authChangeCallback(LocalPlayerAuthStatus.NotAvailable, new Error(error));
                    }
                }
            }, false);
        }

        public override void Signout()
        {
            m_instance.Signout();
        }

        public override ILocalPlayer GetLocalPlayer()
        {
            return m_localPlayer;
        }

        public override IScore CreateScore(string leaderboardId, string leaderboardPlatformId)
        {
            NativeGameLeaderboardScore nativeScore = m_instance.CreateScore(leaderboardPlatformId);
            return new Score(nativeScore);
        }

        public override void LoadServerCredentials(LoadServerCredentialsInternalCallback callback)
        {
            m_instance.LoadServerCredentials(new NativeLoadServerCredentials()
            {
                onSuccessCallback = (serverAuthCode, idToken, email) =>
                {
                    if (callback != null)
                    {
                        ServerCredentials.AndroidPlatformProperties properties = new ServerCredentials.AndroidPlatformProperties(serverAuthCode, idToken, email);
                        ServerCredentials credentials = new ServerCredentials(androidProperties: properties);

                        callback(credentials, null);
                    }
                },
                onFailureCallback = (error) =>
                {
                    if(callback != null)
                    {
                        callback(null, new Error(error));
                    }
                }
            });
            
        }

#endregion
    }
}
#endif
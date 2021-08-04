#if UNITY_ANDROID
using System;
using System.Runtime.InteropServices;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    internal sealed class Leaderboard : LeaderboardBase
    {
        #region Fields

        private NativeGameLeaderboard   m_instance;
        private LeaderboardTimeScope    m_timeScope;
        private LeaderboardPlayerScope  m_playerScope;
        private Score                   m_localPlayerScore;

        #endregion

        #region Constructors

        public Leaderboard(string id, NativeGameLeaderboard nativeGameLeaderboard)
            :base(id, nativeGameLeaderboard.GetId())
        {
            m_instance = nativeGameLeaderboard;
            LoadScoresQuerySize = 10;
        }

        ~Leaderboard()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            return m_instance.GetName();
        }

        protected override LeaderboardPlayerScope GetPlayerScopeInternal()
        {
            return m_playerScope;
        }

        protected override void SetPlayerScopeInternal(LeaderboardPlayerScope value)
        {
            m_playerScope = value;
        }

        protected override LeaderboardTimeScope GetTimeScopeInternal()
        {
            return m_timeScope;
        }
        
        protected override void SetTimeScopeInternal(LeaderboardTimeScope value)
        {
            m_timeScope = value;
        }

        protected override IScore GetLocalPlayerScoreInternal()
        {
            return m_localPlayerScore;
        }

        protected override void LoadTopScoresInternal(LoadScoresInternalCallback callback)
        {
            NativeLeaderboardTimeVariant timeVariant = Converter.from(m_timeScope);
            NativeLeaderboardCollectionVariant collectionVariant = Converter.from(m_playerScope);

            m_instance.LoadTopScores(timeVariant, collectionVariant, LoadScoresQuerySize, true, new NativeLoadScoresListener()
            {
                onSuccessCallback = (nativeScores) =>
                {
                    Score[] scores = NativeUnityPluginUtility.Map<NativeGameLeaderboardScore, Score>(nativeScores.Get());
                    FinishByFetchingLocalPlayerScore(timeVariant, collectionVariant, scores, callback);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        protected override void LoadPlayerCenteredScoresInternal(LoadScoresInternalCallback callback)
        {
            NativeLeaderboardTimeVariant timeVariant = Converter.from(m_timeScope);
            NativeLeaderboardCollectionVariant collectionVariant = Converter.from(m_playerScope);

            m_instance.LoadPlayerCenteredScores(timeVariant, collectionVariant, LoadScoresQuerySize, true, new NativeLoadScoresListener()
            {
                onSuccessCallback = (nativeScores) =>
                {
                    Score[] scores = NativeUnityPluginUtility.Map<NativeGameLeaderboardScore, Score>(nativeScores.Get());
                    FinishByFetchingLocalPlayerScore(timeVariant, collectionVariant, scores, callback);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        protected override void LoadNextInternal(LoadScoresInternalCallback callback)
        {
            m_instance.LoadMoreScores(LoadScoresQuerySize, 1, new NativeLoadScoresListener()
            {
                onSuccessCallback = (nativeScores) =>
                {
                    Score[] scores = NativeUnityPluginUtility.Map<NativeGameLeaderboardScore, Score>(nativeScores.Get());
                    callback(scores, null);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        protected override void LoadPreviousInternal(LoadScoresInternalCallback callback)
        {
            m_instance.LoadMoreScores(LoadScoresQuerySize, -1, new NativeLoadScoresListener()
            {
                onSuccessCallback = (nativeScores) =>
                {
                    Score[] scores = NativeUnityPluginUtility.Map<NativeGameLeaderboardScore, Score>(nativeScores.Get());
                    callback(scores, null);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            NativeAsset asset = m_instance.GetImage();
            asset.LoadRemote(new NativeLoadAssetListener()
            {
                onSuccessCallback = (data) =>
                {
                    callback(data.GetBytes(), null);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        #endregion

        #region Private methods


        private void FinishByFetchingLocalPlayerScore(NativeLeaderboardTimeVariant timeVariant, NativeLeaderboardCollectionVariant collectionVariant, Score[] scores, LoadScoresInternalCallback callback)
        {
            m_instance.LoadLocalPlayerScore(timeVariant, collectionVariant, new NativeLoadLocalPlayerScoreListener()
            {
                onSuccessCallback = (NativeGameLeaderboardScore nativeLocalPlayerScore) =>
                {
                    if (nativeLocalPlayerScore.IsNull())
                    {
                        m_localPlayerScore = null;
                    }
                    else
                    {
                        m_localPlayerScore = new Score(nativeLocalPlayerScore);
                    }

                    callback(scores, null);
                },
                onFailureCallback = (error) =>
                {
                    DebugLogger.LogWarning("Failed retrieving local player score : " + error);
                    callback(scores, null);
                }

            });
        }

        #endregion
    }
}
#endif
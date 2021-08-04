using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    internal sealed class Leaderboard : LeaderboardBase
    {
        #region Fields

        private         string                      m_title                 = string.Empty;

        private         LeaderboardPlayerScope      m_playerScope           = LeaderboardPlayerScope.Global;

        private         LeaderboardTimeScope        m_timeScope             = LeaderboardTimeScope.AllTime;            

        private         Score                       m_localPlayerScore      = null;

        #endregion

        #region Constructors

        public Leaderboard(string id, string platformId, string title = "") 
            : base(id: id, platformId: platformId)
        {
            // set properties
            m_title     = title;
        }

        #endregion

        #region Static methods

        private static Leaderboard[] ConvertToLeaderboardArray(IEnumerator<LeaderboardData> enumerator)
        {
            return SystemUtility.ConvertEnumeratorItems(
                enumerator: enumerator, 
                converter: (input) =>
                {
                    string  leaderboardId   = input.Id;
                    var     settings        = GameServices.FindLeaderboardDefinitionWithId(leaderboardId);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified id: {0}", leaderboardId);
                        return null;
                    }

                    return new Leaderboard(
                        id: leaderboardId, 
                        platformId: settings.GetPlatformIdForActivePlatform(), 
                        title: input.Title);
                },
                includeNullObjects: false);
        }

        #endregion

        #region Static methods

        public static void LoadLeaderboards(LoadLeaderboardsInternalCallback callback)
        {
            // get data
            Error   error;
            var     enumerator      = GameServicesSimulator.Instance.GetLeaderboards(out error);
        
            // parse data and send callback
            if (error == null)
            {
                var     leaderboards    = ConvertToLeaderboardArray(enumerator);
                callback(leaderboards, null);
            }
            else
            {
                callback(null, error);
            }
        }

        public static void ShowLeaderboardView(string leaderboardId, LeaderboardTimeScope timescope, ViewClosedInternalCallback callback)
        {
            GameServicesSimulator.Instance.ShowLeaderboardView((error) => callback(error));
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            return m_title;
        }

        protected override LeaderboardPlayerScope GetPlayerScopeInternal()
        {
            return m_playerScope;
        }

        protected override void SetPlayerScopeInternal(LeaderboardPlayerScope value)
        {
            // set value
            m_playerScope   = value;
        }

        protected override LeaderboardTimeScope GetTimeScopeInternal()
        {
            return m_timeScope;
        }
        
        protected override void SetTimeScopeInternal(LeaderboardTimeScope value)
        {
            // set value
            m_timeScope     = value;
        }

        protected override IScore GetLocalPlayerScoreInternal()
        {
            return m_localPlayerScore;
        }

        protected override void LoadTopScoresInternal(LoadScoresInternalCallback callback)
        {
            // get data
            Error       error;
            ScoreData   localPlayerScore;
            var         enumerator          = GameServicesSimulator.Instance.GetLeaderboardScores(Id, out localPlayerScore, out error);

            // parse data
            if (error  == null)
            {
                var     scores              = new List<Score>();
                while (enumerator.MoveNext())
                {
                    var     current         = enumerator.Current;
                    scores.Add(Score.CreateScoreFromData(current));
                }

                // update local player score
                m_localPlayerScore          = Score.CreateScoreFromData(localPlayerScore);    

                // send result 
                callback(scores.ToArray(), error);
            }
            else
            {
                // send result 
                callback(null, error);
            }
        }

        protected override void LoadPlayerCenteredScoresInternal(LoadScoresInternalCallback callback)
        {
            LoadTopScoresInternal(callback);
        }

        protected override void LoadNextInternal(LoadScoresInternalCallback callback)
        {
            if (m_localPlayerScore == null)
            {
                LoadTopScoresInternal(callback);
                return;
            }

            callback(null, null);
        }

        protected override void LoadPreviousInternal(LoadScoresInternalCallback callback)
        {
            if (m_localPlayerScore == null)
            {
                LoadTopScoresInternal(callback);
                return;
            }

            callback(null, null);
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            Error   error;
            var     image   = GameServicesSimulator.Instance.GetLeaderboardImage(Id, out error);

            // send result to caller object
            callback(image.EncodeToPNG(), error);
        }

        #endregion
    }
}
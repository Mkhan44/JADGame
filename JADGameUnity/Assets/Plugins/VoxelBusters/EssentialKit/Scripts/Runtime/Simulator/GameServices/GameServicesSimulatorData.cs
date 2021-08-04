using System;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    [Serializable]
    internal sealed class GameServicesSimulatorData
    {
        #region Fields

        [SerializeField]
        private     List<AchievementDescriptionData>    m_achievementDescriptionTable;

        [SerializeField]
        private     List<AchievementData>               m_achievementTable;

        [SerializeField]
        private     List<LeaderboardData>               m_leaderboardTable;

        [SerializeField]
        private     List<ScoreData>                     m_scoreTable;

        [SerializeField]
        private     List<PlayerData>                    m_playerTable;

        [SerializeField]
        private     string                              m_localPlayerId;

        #endregion

        #region Constructors

        public GameServicesSimulatorData(AchievementDescriptionData[] achievementDescriptions, LeaderboardData[] leaderboards)
        {
            // set properties
            m_achievementDescriptionTable   = new List<AchievementDescriptionData>(achievementDescriptions);
            m_achievementTable              = new List<AchievementData>();
            m_leaderboardTable              = new List<LeaderboardData>(leaderboards);
            m_scoreTable                    = new List<ScoreData>();
            m_playerTable                   = new List<PlayerData>();
        }

        #endregion

        #region Public methods

        public IEnumerator<AchievementDescriptionData> GetAchievementDescriptions()
        {
            return m_achievementDescriptionTable.GetEnumerator();
        }

        public IEnumerator<AchievementData> GetAchievements(string playerId)
        {
            return m_achievementTable.FindAll((item) => string.Equals(item.PlayerId, playerId)).GetEnumerator();
        }

        public AchievementData GetAchievement(string id, string playerId)
        {
            int     index   = FindAchievementIndex(id, playerId);
            return (index == -1) ? null : m_achievementTable[index];
        }

        public void AddAchievement(AchievementData achievementData)
        {
            int     index   = FindAchievementIndex(achievementData.Id, achievementData.PlayerId);
            if (index == -1)
            {
                m_achievementTable.Add(achievementData);
            }
            else
            {
                m_achievementTable[index] = achievementData;
            }
        }

        private int FindAchievementIndex(string id, string playerId)
        {
            return m_achievementTable.FindIndex((obj) => string.Equals(obj.Id, id) && string.Equals(obj.PlayerId, playerId));
        }

        #endregion

        #region Leaderboard methods

        public IEnumerator<LeaderboardData> GetLeaderboards()
        {
            return m_leaderboardTable.GetEnumerator();
        }

        public LeaderboardData GetLeaderboardWithId(string id)
        {
            return m_leaderboardTable.Find((obj) => string.Equals(obj.Id, id));
        }

        public IEnumerator<ScoreData> GetLeaderboardScores(string leaderboardId)
        {
            return m_scoreTable.FindAll((item) => string.Equals(item.LeaderboardId, leaderboardId)).GetEnumerator();
        }

        public ScoreData GetLeaderboardScore(string leaderboardId, string playerId)
        {
            return m_scoreTable.Find((item) => string.Equals(item.LeaderboardId, leaderboardId) && string.Equals(item.PlayerId, playerId));
        }

        public void AddScore(ScoreData scoreData)
        {
            int     index   = FindScoreIndex(scoreData.LeaderboardId, scoreData.PlayerId);
            if (index == -1)
            {
                m_scoreTable.Add(scoreData);
            }
            else
            {
                m_scoreTable[index] = scoreData;
            }
        }

        private int FindScoreIndex(string leaderboardId, string playerId)
        {
            return m_scoreTable.FindIndex((obj) => string.Equals(obj.LeaderboardId, leaderboardId) && string.Equals(obj.PlayerId, playerId));
        }

        #endregion

        #region Player methods

        public PlayerData FindPlayer(string id)
        {
            int     index   = FindPlayerIndex(id);
            return index == -1 ? null : m_playerTable[index];
        }

        public void AddPlayer(PlayerData playerData)
        {
            int     index   = FindPlayerIndex(playerData.Id);
            if (index == -1)
            {
                m_playerTable.Add(playerData);
            }
            else
            {
                m_playerTable[index]    = playerData;
            }
        }

        public void SetLocalPlayer(PlayerData player)
        {
            m_localPlayerId   = player.Id;
        }

        public PlayerData GetLocalPlayer()
        {
            return FindPlayer(m_localPlayerId);
        }

        public string GetLocalPlayerId()
        {
            return string.IsNullOrEmpty(m_localPlayerId) ? null : m_localPlayerId; 
        }

        private int FindPlayerIndex(string playerId)
        {
            return m_playerTable.FindIndex((obj) => string.Equals(obj.Id, playerId));
        }

        #endregion
    }
}
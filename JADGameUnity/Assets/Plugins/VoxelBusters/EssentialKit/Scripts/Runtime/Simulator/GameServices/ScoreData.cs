using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    [Serializable]
    public sealed class ScoreData
    {
        #region Fields

        [SerializeField]
        private         string          m_playerId;

        [SerializeField]
        private         string          m_leaderboardId;

        [SerializeField]
        private         long            m_value;           

        #endregion

        #region Properties

        public string PlayerId
        {
            get
            {
                return m_playerId;
            }
            set
            {
                m_playerId    = value;
            }
        }

        public string LeaderboardId
        {
            get
            {
                return m_leaderboardId;
            }
            set
            {
                m_leaderboardId    = value;
            }
        }

        public long Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }

        #endregion
    }
}
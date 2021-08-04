using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    [Serializable]
    public sealed class AchievementData
    {
        #region Fields

        [SerializeField]
        private         string          m_id;

        [SerializeField]
        private         string          m_playerId;

        [SerializeField]
        private         float           m_percentageCompleted;

        [SerializeField]
        private         bool            m_isCompleted;

        #endregion

        #region Properties

        public string Id
        {
            get
            {
                return m_id;
            }
        }

        public string PlayerId
        {
            get
            {
                return m_playerId;
            }
        }

        public float PercentageCompleted
        {
            get
            {
                return m_percentageCompleted;
            }
            set
            {
                m_percentageCompleted    = value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return m_isCompleted;
            }
            set
            {
                m_isCompleted   = value;
            }
        }

        #endregion

        #region Constructors

        public AchievementData(string id, string playerId)
        {
            // set properties
            m_id                    = id;
            m_playerId              = playerId;
            m_percentageCompleted   = 0;
            m_isCompleted           = false;
        }

        #endregion
    }
}
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    [Serializable]
    public sealed class LeaderboardData
    {
        #region Fields

        [SerializeField]
        private         string          m_id;

        [SerializeField]
        private         string          m_title;     

        #endregion

        #region Properties

        public string Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id    = value;
            }
        }

        public string Title
        {
            get
            {
                return m_title;
            }
            set
            {
                m_title    = value;
            }
        }

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

using SPlayer = VoxelBusters.EssentialKit.GameServicesCore.Simulator.Player;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    internal sealed class Score : ScoreBase
    {
        #region Fields

        private     string          m_playerId; 

        private     long            m_rank; 

        private     long            m_value;

        #endregion

        #region Constructors

        public Score(string leaderboardId, string leaderboardPlatformId) 
            : base(leaderboardId, leaderboardPlatformId)
        {
            // set default values
            m_rank          = 0;
            m_value         = 0;
        }

        public Score(string leaderboardPlatformId, string playerId, long rank, long value) 
            : base(leaderboardPlatformId)
        {
            // set properties
            m_playerId      = playerId; 
            m_rank          = rank;
            m_value         = value;
        }

        #endregion

        #region Create methods

        internal static Score CreateScoreFromData(ScoreData data)
        {
            return new Score(data.LeaderboardId, data.PlayerId, 1, data.Value); 
        }

        #endregion

        #region Base class methods

        protected override IPlayer GetPlayerInternal()
        {
            // create player object using data
            var     data    = GameServicesSimulator.Instance.FindPlayerWithId(m_playerId);
            return SPlayer.CreatePlayerFromData(data);
        }

        protected override long GetRankInternal()
        {
            return m_rank;
        }

        protected override long GetValueInternal()
        {
            return m_value;
        }

        protected override void SetValueInternal(long value)
        {
            m_value = value;
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            return default(DateTime);
        }

        protected override void ReportScoreInternal(ReportScoreInternalCallback callback)
        {
            Error   error;
            GameServicesSimulator.Instance.ReportLeaderboardScore(LeaderboardPlatformId, Value, out error);

            // send result
            callback(error);
        }

        #endregion
    }
}
#if UNITY_ANDROID
using System;
using System.Runtime.InteropServices;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    internal sealed class Score : ScoreBase
    {
        #region Fields

        private NativeGameLeaderboardScore m_instance;
        private long m_submissionScore;

        #endregion

        #region Constructors

        public Score(string platformId) : base(platformId)
        {

        }

        public Score(NativeGameLeaderboardScore nativeScore) 
            : base(leaderboardPlatformId: nativeScore.GetLeaderboardId())
        {
            m_instance = nativeScore;
            DebugLogger.Log("Score constructor : " + LeaderboardPlatformId + "  " + nativeScore.GetLeaderboardId());
        }

        ~Score()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        protected override IPlayer GetPlayerInternal()
        {
            return new Player(m_instance.GetPlayer());
        }

        protected override long GetRankInternal()
        {
            return m_instance.GetRank();
        }

        protected override long GetValueInternal()
        {
            return m_instance.GetRawScore();
        }

        protected override void SetValueInternal(long value)
        {
            m_submissionScore = value;
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            return m_instance.GetLastReportedDate().GetDateTime();
        }

        protected override void ReportScoreInternal(ReportScoreInternalCallback callback)
        {
            m_instance.ReportScore(m_submissionScore, new NativeSubmitScoreListener()
            {
                onSuccessCallback = () =>
                {
                    callback(null);
                },
                onFailureCallback = (error) =>
                {
                    callback(new Error(error));
                }
            });          
        }

        #endregion
    }
}
#endif
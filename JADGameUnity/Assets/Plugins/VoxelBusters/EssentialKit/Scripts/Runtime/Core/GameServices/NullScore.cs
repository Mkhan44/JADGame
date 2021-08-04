using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal sealed class NullScore : ScoreBase
    {
        #region Constructors

        public NullScore(string leaderboardId, string leaderboardPlatformId) : base(leaderboardId, leaderboardPlatformId)
        {
            LogNotSupported();
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("Score");
        }

        #endregion

        #region Base class methods

        protected override IPlayer GetPlayerInternal()
        {
            LogNotSupported();

            return new NullPlayer();
        }

        protected override long GetRankInternal()
        {
            LogNotSupported();

            return 0;
        }

        protected override long GetValueInternal()
        {
            LogNotSupported();

            return 0;
        }

        protected override void SetValueInternal(long value)
        {
            LogNotSupported();
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            LogNotSupported();

            return default(DateTime);
        }

        protected override void ReportScoreInternal(ReportScoreInternalCallback callback)
        {
            LogNotSupported();

            callback(Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
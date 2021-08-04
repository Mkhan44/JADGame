using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal sealed class NullAchievement : AchievementBase
    {
        #region Constructors

        public NullAchievement(string id, string platformId) : base(id, platformId)
        {
            LogNotSupported();
        }

        #endregion

        #region Static methods

        public static void SetCanShowBannerOnCompletion(bool value)
        {
            LogNotSupported();
        }

        public static void LoadAchievements(LoadAchievementsInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        public static void ShowAchievementView(ViewClosedInternalCallback callback)
        {
            LogNotSupported();

            callback(Diagnostics.kFeatureNotSupported);
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("Achievement");
        }

        #endregion

        #region Base class methods

        protected override double GetPercentageCompletedInternal()
        {
            LogNotSupported();

            return 0;
        }

        protected override void SetPercentageCompletedInternal(double value)
        {
            LogNotSupported();
        }

        protected override bool GetIsCompletedInternal()
        {
            LogNotSupported();

            return false;
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            LogNotSupported();

            return default(DateTime);
        }

        protected override void ReportProgressInternal(ReportAchievementProgressInternalCallback callback)
        {
            LogNotSupported();

            callback(Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
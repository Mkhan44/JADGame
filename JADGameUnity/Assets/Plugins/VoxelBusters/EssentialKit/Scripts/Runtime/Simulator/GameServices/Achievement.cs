using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    internal sealed class Achievement : AchievementBase
    {
        #region Fields

        private         float           m_percentageCompleted;

        private         bool            m_isCompleted;

        #endregion

        #region Constructors

        public Achievement(string id, string platformId, 
            float percentageCompleted = 0f, bool isCompleted = false) 
            : base(id: id, platformId: platformId)
        {
            // set properties
            m_percentageCompleted   = percentageCompleted;
            m_isCompleted           = isCompleted;
        }

        #endregion

        #region Static methods

        private static Achievement[] ConvertToAchievementArray(IEnumerator<AchievementData> enumerator)
        {
            return SystemUtility.ConvertEnumeratorItems(
                enumerator: enumerator, 
                converter: (input) =>
                {
                    string  achievementId   = input.Id;
                    var     settings        = GameServices.FindAchievementDefinitionWithId(input.Id);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified id: {0}", achievementId);
                        return null;
                    }

                    return new Achievement(
                        id: achievementId, 
                        platformId: settings.GetPlatformIdForActivePlatform(), 
                        percentageCompleted: input.PercentageCompleted, 
                        isCompleted: input.IsCompleted);
                },
                includeNullObjects: false);
        }

        #endregion

        #region Static methods

        public static void LoadAchievements(LoadAchievementsInternalCallback callback)
        {
            // get data
            Error   error;
            var     enumerator      = GameServicesSimulator.Instance.GetAchievements(out error);

            // parse data and send callback
            if (error == null)
            {
                var     achievements    = ConvertToAchievementArray(enumerator);
                callback(achievements, null);
            }
            else
            {
                callback(null, error);
            }
        }

        public static void ShowAchievementView(ViewClosedInternalCallback callback)
        {
            GameServicesSimulator.Instance.ShowAchievementView((error) => callback(error));
        }

        #endregion

        #region Base class methods

        protected override double GetPercentageCompletedInternal()
        {
            return m_percentageCompleted;
        }

        protected override void SetPercentageCompletedInternal(double value)
        {
            m_percentageCompleted   = Mathf.Clamp((float)value, 0, 100f);
        }

        protected override bool GetIsCompletedInternal()
        {
            return m_isCompleted;
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            return default(DateTime);
        }

        protected override void ReportProgressInternal(ReportAchievementProgressInternalCallback callback)
        {
            // report data
            Error   error;
            GameServicesSimulator.Instance.ReportAchievementProgress(Id, (float)PercentageCompleted, out m_isCompleted, out error);

            // send result
            callback(error);
        }

        #endregion
    }
}
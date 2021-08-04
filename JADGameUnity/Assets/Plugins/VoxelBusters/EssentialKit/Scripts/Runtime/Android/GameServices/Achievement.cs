#if UNITY_ANDROID
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    internal sealed class Achievement : AchievementBase
    {
        #region Fields

        private NativeGameAchievement m_instance;
        private double m_reportedProgress;

        #endregion

        #region Constructors

        public Achievement(string id, NativeGameAchievement nativeAchievement) 
            : base(id, nativeAchievement.GetId())
        {
            m_instance = nativeAchievement;
        }

        ~Achievement()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        protected override double GetPercentageCompletedInternal()
        {
            return (m_instance.GetCurrentSteps() / (m_instance.GetTotalSteps()*1.0)) * 100.0;
        }

        protected override void SetPercentageCompletedInternal(double value)
        {
            m_reportedProgress = Mathf.Clamp((float)value, 0, 100f);
        }

        protected override bool GetIsCompletedInternal()
        {
            return m_instance.GetCurrentSteps() == m_instance.GetTotalSteps();
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            return m_instance.GetLastReportedDate().GetDateTime();
        }

        protected override void ReportProgressInternal(ReportAchievementProgressInternalCallback callback)
        {
            int steps = (int)((m_reportedProgress * m_instance.GetTotalSteps())/100.0);
            m_instance.ReportProgress(steps, new NativeReportProgressListener()
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
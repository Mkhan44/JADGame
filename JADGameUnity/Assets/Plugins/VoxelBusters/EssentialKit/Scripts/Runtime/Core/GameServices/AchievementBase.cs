using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    public abstract class AchievementBase : NativeObjectBase, IAchievement
    {
        #region Constructors

        protected AchievementBase(string id, string platformId)
        {
            // set properties
            Id              = id;
            PlatformId      = platformId;
        }

        #endregion

        #region Abstract methods

        protected abstract double GetPercentageCompletedInternal();

        protected abstract void SetPercentageCompletedInternal(double value);
        
        protected abstract bool GetIsCompletedInternal();

        protected abstract DateTime GetLastReportedDateInternal();

        protected abstract void ReportProgressInternal(ReportAchievementProgressInternalCallback callback);

        #endregion

        #region Base class methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("Achievement {");
            sb.Append("Id: ").Append(Id).Append(" ");
            sb.Append("PercentageCompleted: ").Append(PercentageCompleted).Append(" ");
            sb.Append("IsCompleted: ").Append(IsCompleted).Append(" ");
            sb.Append("LastReportedDate: ").Append(LastReportedDate).Append(" ");
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IGameServicesAchievement implementation

        public string Id 
        { 
            get; 
            internal set; 
        }

        public string PlatformId 
        { 
            get; 
            private set; 
        }

        public double PercentageCompleted
        {
            get
            {
                return GetPercentageCompletedInternal();
            }
            set
            {
                SetPercentageCompletedInternal(value);
            }
        }

        public bool IsCompleted
        {
            get
            {
                return GetIsCompletedInternal();
            }
        }

        public DateTime LastReportedDate
        {
            get
            {
                return GetLastReportedDateInternal();
            }
        }

        public void ReportProgress(CompletionCallback callback)
        {
            // retain object to avoid unintentional release
            ManagedObjectReferencePool.Retain(this);

            // make call
            ReportProgressInternal((error) =>
            {
                // send result to caller object
                CallbackDispatcher.InvokeOnMainThread(callback, error);

                // remove object from cache
                ManagedObjectReferencePool.Release(this);
            });
        }

        #endregion
    }
}
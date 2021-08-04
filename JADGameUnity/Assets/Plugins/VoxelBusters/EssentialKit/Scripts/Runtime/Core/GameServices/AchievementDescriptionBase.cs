using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    public abstract class AchievementDescriptionBase : NativeObjectBase, IAchievementDescription
    {
        #region Constructors

        protected AchievementDescriptionBase(string id, string platformId, int numOfStepsToUnlock)
        {
            // set properties
            Id                  = id;
            PlatformId          = platformId;
            NumberOfStepsRequiredToUnlockAchievement    = numOfStepsToUnlock;
        }

        #endregion

        #region Abstract methods

        protected abstract string GetTitleInternal();

        protected abstract string GetUnachievedDescriptionInternal();

        protected abstract string GetAchievedDescriptionInternal();

        protected abstract long GetMaximumPointsInternal();
        
        protected abstract bool GetIsHiddenInternal();

        protected abstract bool GetIsReplayableInternal();

        protected abstract void LoadIncompleteAchievementImageInternal(LoadImageInternalCallback callback);

        protected abstract void LoadImageInternal(LoadImageInternalCallback callback);

        #endregion

        #region Base class methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("AchievementDescription { ");
            sb.Append("Id: ").Append(Id).Append(" ");
            sb.Append("Title: ").Append(Title).Append(" ");
            sb.Append("NumberOfStepsRequiredToUnlockAchievement: ").Append(NumberOfStepsRequiredToUnlockAchievement);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IGameServicesAchievementDescription implementation

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

        public string Title
        {
            get
            {
                return GetTitleInternal();
            }
        }

        public string UnachievedDescription
        {
            get
            {
                return GetUnachievedDescriptionInternal();
            }
        }

        public string AchievedDescription
        {
            get
            {
                return GetAchievedDescriptionInternal();
            }
        }

        public long MaximumPoints
        {
            get
            {
                return GetMaximumPointsInternal();
            }
        }

        public int NumberOfStepsRequiredToUnlockAchievement
        {
            get;
            private set;
        }

        public bool IsHidden
        {
            get
            {
                return GetIsHiddenInternal();
            }
        }

        public bool IsReplayable
        {
            get
            {
                return GetIsReplayableInternal();
            }
        }

        public void LoadIncompleteAchievementImage(EventCallback<TextureData> callback)
        {
            // make request
            LoadIncompleteAchievementImageInternal((imageData, error) =>
            {
                // send result to caller object
                var     data        = (imageData == null) ? null : new TextureData(imageData);
                CallbackDispatcher.InvokeOnMainThread(callback, data, error);
            });
        }

        public void LoadImage(EventCallback<TextureData> callback)
        {
            // make request
            LoadImageInternal((imageData, error) =>
            {
                // send result to caller object
                var     data        = (imageData == null) ? null : new TextureData(imageData);
                CallbackDispatcher.InvokeOnMainThread(callback, data, error);
            });
        }

        #endregion
    }
}
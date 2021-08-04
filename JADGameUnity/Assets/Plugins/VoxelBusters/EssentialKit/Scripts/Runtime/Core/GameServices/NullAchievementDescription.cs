using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    internal sealed class NullAchievementDescription : AchievementDescriptionBase
    {
        #region Constructors

        public NullAchievementDescription(string id, string platformId, int numOfStepsToUnlock)
            : base(id, platformId, numOfStepsToUnlock)
        {
            LogNotSupported();
        }

        #endregion

        #region Static methods

        public static void LoadAchievementDescriptions(LoadAchievementDescriptionsInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("AchievementDescription");
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override string GetUnachievedDescriptionInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override string GetAchievedDescriptionInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override long GetMaximumPointsInternal()
        {
            LogNotSupported();

            return 0;
        }
        
        protected override bool GetIsHiddenInternal()
        {
            LogNotSupported();

            return false;
        }

        protected override bool GetIsReplayableInternal()
        {
            LogNotSupported();

            return false;
        }

        protected override void LoadIncompleteAchievementImageInternal(LoadImageInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            LogNotSupported();

            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
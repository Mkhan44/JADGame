using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    internal sealed class AchievementDescription : AchievementDescriptionBase
    {
        #region Fields

        private         string          m_title;

        #endregion

        #region Constructors

        public AchievementDescription(string id, string platformId, int numOfStepsToUnlock, string title) 
            : base(id: id, platformId: platformId, numOfStepsToUnlock: numOfStepsToUnlock)
        {
            // set properties
            m_title         = title;
        }

        #endregion

        #region Static methods

        private static AchievementDescription[] ConvertToAchievementDescriptionArray(IEnumerator<AchievementDescriptionData> enumerator)
        {
            return SystemUtility.ConvertEnumeratorItems(
                enumerator: enumerator, 
                converter: (input) =>
                {
                    string  achievementId   = input.Id;
                    var     settings        = GameServices.FindAchievementDefinitionWithId(achievementId);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified id: {0}", achievementId);
                        return null;
                    }

                    return new AchievementDescription(
                        id: achievementId, 
                        platformId: settings.GetPlatformIdForActivePlatform(), 
                        numOfStepsToUnlock: settings.NumOfStepsToUnlock, 
                        title: input.Title);
                },
                includeNullObjects: false);
        }

        #endregion

        #region Public static methods

        public static void LoadAchievementDescriptions(LoadAchievementDescriptionsInternalCallback callback)
        {
            // get data
            Error   error;
            var     enumerator      = GameServicesSimulator.Instance.GetAchievementDescriptions(out error);

            // parse data and send callback
            if (error == null)
            {
                var     descriptions    = ConvertToAchievementDescriptionArray(enumerator);
                callback(descriptions, null);
            }
            else
            {
                callback(null, error);
            }
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            return m_title;
        }

        protected override string GetUnachievedDescriptionInternal()
        {
            return string.Empty;
        }

        protected override string GetAchievedDescriptionInternal()
        {
            return string.Empty;
        }

        protected override long GetMaximumPointsInternal()
        {
            return 100;
        }
        
        protected override bool GetIsHiddenInternal()
        {
            return false;
        }

        protected override bool GetIsReplayableInternal()
        {
            return false;
        }

        protected override void LoadIncompleteAchievementImageInternal(LoadImageInternalCallback callback)
        {
            Error   error;
            var     image   = GameServicesSimulator.Instance.GetAchievementImage(Id, out error);

            // send result
            callback(image.EncodeToPNG(), error);
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            // send result
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        #endregion
    }
}
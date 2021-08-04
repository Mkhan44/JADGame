#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    internal sealed class AchievementDescription : AchievementDescriptionBase
    {
        #region Fields

        private NativeGameAchievement   m_instance;
        private static string[]         m_achievedDescriptionFormats;
    
        #endregion
        
        #region Constructors

        public AchievementDescription(string id, NativeGameAchievement nativeGameAchievement, int numOfStepsToUnlock) 
            : base(id: id, platformId: nativeGameAchievement.GetId(), numOfStepsToUnlock: numOfStepsToUnlock)
        {
            m_instance = nativeGameAchievement;
            if (NumberOfStepsRequiredToUnlockAchievement != GetMaximumPointsInternal())
            {
                DebugLogger.LogWarning("Number of steps mentioned in play console is different from steps set in plugin's settings!");
            }
        }

        ~AchievementDescription()
        {
            DebugLogger.Log("~AchievementDescriptor");
            Dispose(false);
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            return m_instance.GetName();
        }

        protected override string GetUnachievedDescriptionInternal()
        {
            return m_instance.GetDescription();
        }

        protected override string GetAchievedDescriptionInternal()
        {
            string achievedDescriptionFormat = GetRandomAchievedDescriptionFormat();

            if(string.IsNullOrEmpty(achievedDescriptionFormat))
            {
                return m_instance.GetDescription();
            }
            else
            {
                return achievedDescriptionFormat.Replace("#", GetTitleInternal());
            }
        }

        protected override long GetMaximumPointsInternal()
        {
            return m_instance.GetTotalSteps();
        }
        
        protected override bool GetIsHiddenInternal()
        {
            return m_instance.IsHidden();
        }

        protected override bool GetIsReplayableInternal()
        {
            return false;
        }

        protected override void LoadIncompleteAchievementImageInternal(LoadImageInternalCallback callback)
        {
            NativeAsset asset = m_instance.GetRevealedImage();
            LoadAsset(asset, callback);
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            NativeAsset asset = m_instance.GetUnlockedImage();
            LoadAsset(asset, callback);
        }

        #endregion

        #region Utility methods

        private void LoadAsset(NativeAsset asset, LoadImageInternalCallback callback)
        {
            asset.LoadRemote(new NativeLoadAssetListener()
            {
                onSuccessCallback = (data) =>
                {
                    callback(data.GetBytes(), null);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        #endregion

        #region  Static methods
        public static void SetAchievedDescriptionFormats(string[] achievedDescriptionFormats)
        {
            m_achievedDescriptionFormats = achievedDescriptionFormats;
        }

        private static string GetRandomAchievedDescriptionFormat()
        {
            if(m_achievedDescriptionFormats != null && m_achievedDescriptionFormats.Length > 0)
            {
                int randomIndex = (int)(DateTime.Now.Ticks % m_achievedDescriptionFormats.Length);
                return m_achievedDescriptionFormats[randomIndex];
            }
            else
            {
                return null;
            }
        }
    #endregion
}
}
#endif
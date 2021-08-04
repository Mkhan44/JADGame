#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;
using VoxelBusters.EssentialKit;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal sealed class AchievementDescription : AchievementDescriptionBase
    {
        #region Fields

        private     string                  m_title;

        private     string                  m_unachievedDescription;

        private     string                  m_achievedDescription;

        private     long                    m_maximumPoints;

        private     bool                    m_isHidden;

        private     bool                    m_isReplayable;
    
        #endregion

        #region Constructors

        static AchievementDescription()
        {
            // register callbacks
            AchievementDescriptionBinding.NPAchievementDescriptionRegisterCallbacks(HandleLoadAchievementDescriptionsNativeCallback, NativeCallbackResponder.HandleLoadImageNativeCallback);
        }

        public AchievementDescription(IntPtr nativePtr, string id, string platformId, int numOfStepsToUnlock)
            : base(id: id, platformId: platformId, numOfStepsToUnlock: numOfStepsToUnlock)
        {
            // set properties
            NativeObjectRef         = new IosNativeObjectRef(nativePtr);
            m_title                 = AchievementDescriptionBinding.NPAchievementDescriptionGetTitle(nativePtr);
            m_unachievedDescription = AchievementDescriptionBinding.NPAchievementDescriptionGetUnachievedDescription(nativePtr);
            m_achievedDescription   = AchievementDescriptionBinding.NPAchievementDescriptionGetAchievedDescription(nativePtr);
            m_maximumPoints         = AchievementDescriptionBinding.NPAchievementDescriptionGetMaximumPoints(nativePtr);
            m_isHidden              = AchievementDescriptionBinding.NPAchievementDescriptionGetHidden(nativePtr);
            m_isReplayable          = AchievementDescriptionBinding.NPAchievementDescriptionGetReplayable(nativePtr);
        }

        ~AchievementDescription()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        private static AchievementDescription[] CreateAchievementDescriptionArray(ref NativeArray nativeArray)
        {
            return MarshalUtility.ConvertNativeArrayItems(
                arrayPtr: nativeArray.Pointer,
                length: nativeArray.Length, 
                converter: (input) =>
                {
                    string  platformId  = AchievementDescriptionBinding.NPAchievementDescriptionGetId(input);
                    var     settings    = GameServices.FindAchievementDefinitionWithPlatformId(platformId);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified platform id: {0}", platformId);
                        return null;
                    }

                    return new AchievementDescription(nativePtr: input, id: settings.Id, platformId: platformId, numOfStepsToUnlock: settings.NumOfStepsToUnlock);
                }, 
                includeNullObjects: false);
        }

        public static void LoadAchievementDescriptions(LoadAchievementDescriptionsInternalCallback callback)
        {
            IntPtr  tagPtr  = MarshalUtility.GetIntPtr(callback);
            AchievementDescriptionBinding.NPAchievementDescriptionLoadDescriptions(tagPtr);
        }

        #endregion

        #region Base class methods

        protected override string GetTitleInternal()
        {
            return m_title;
        }

        protected override string GetUnachievedDescriptionInternal()
        {
            return m_unachievedDescription;
        }

        protected override string GetAchievedDescriptionInternal()
        {
            return m_achievedDescription;
        }

        protected override long GetMaximumPointsInternal()
        {
            return m_maximumPoints;
        }
        
        protected override bool GetIsHiddenInternal()
        {
            return m_isHidden;
        }

        protected override bool GetIsReplayableInternal()
        {
            return m_isReplayable;
        }

        protected override void LoadIncompleteAchievementImageInternal(LoadImageInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            AchievementDescriptionBinding.NPAchievementDescriptionLoadIncompleteAchievementImage(AddrOfNativeObject(), tagPtr);
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            AchievementDescriptionBinding.NPAchievementDescriptionLoadImage(AddrOfNativeObject(), tagPtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(GameServicesLoadArrayNativeCallback))]
        private static void HandleLoadAchievementDescriptionsNativeCallback(ref NativeArray nativeArray, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     descriptions    = CreateAchievementDescriptionArray(ref nativeArray);
                var     errorObj        = Error.CreateNullableError(description: error);
                ((LoadAchievementDescriptionsInternalCallback)tagHandle.Target)(descriptions, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        #endregion
    }
}
#endif
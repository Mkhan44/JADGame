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

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal sealed class Achievement : AchievementBase
    {
        #region Constructors

        static Achievement()
        {
            // register callbacks
            AchievementBinding.NPAchievementRegisterCallbacks(HandleLoadAchievementsCallbackInternal, HandleReportProgressCallbackInternal);
        }

        public Achievement(IntPtr nativePtr, string id, string platformId)
            : base(id, platformId)
        {
            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr);
        }

        public Achievement(string id, string platformId) 
            : base(id, platformId)
        {
            var     nativePtr   = AchievementBinding.NPAchievementCreate(platformId);

            // set properties
            NativeObjectRef     = new IosNativeObjectRef(nativePtr, retain: false);
        }

        ~Achievement()
        {
            Dispose(false);
        }

        #endregion

        #region Static methods

        private static Achievement[] CreateAchievementArray(ref NativeArray nativeArray)
        {
            return MarshalUtility.ConvertNativeArrayItems(
                arrayPtr: nativeArray.Pointer,
                length: nativeArray.Length, 
                converter: (input) =>
                {
                    string  platformId  = AchievementBinding.NPAchievementGetId(input);
                    var     settings    = GameServices.FindAchievementDefinitionWithPlatformId(platformId);
                    if (null == settings)
                    {
                        DebugLogger.LogWarningFormat("Could not find settings for specified platform id: {0}", platformId);
                        return null;
                    }

                    return new Achievement(nativePtr: input, id: settings.Id, platformId: platformId);
                }, 
                includeNullObjects: false);
        }

        public static void SetCanShowBannerOnCompletion(bool value)
        {
            AchievementBinding.NPAchievementSetCanShowBannerOnCompletion(value);
        }

        public static void LoadAchievements(LoadAchievementsInternalCallback callback)
        {
            IntPtr  tagPtr  = MarshalUtility.GetIntPtr(callback);
            AchievementBinding.NPAchievementLoadAchievements(tagPtr);
        }

        public static void ShowAchievementView(ViewClosedInternalCallback callback)
        {
            AchievementBinding.NPAchievementShowView(MarshalUtility.GetIntPtr(callback));
        }

        #endregion

        #region Base class methods

        protected override double GetPercentageCompletedInternal()
        {
            return AchievementBinding.NPAchievementGetPercentageCompleted(AddrOfNativeObject());
        }

        protected override void SetPercentageCompletedInternal(double value)
        {
            AchievementBinding.NPAchievementSetPercentageCompleted(AddrOfNativeObject(), value);
        }

        protected override bool GetIsCompletedInternal()
        {
            return AchievementBinding.NPAchievementGetIsCompleted(AddrOfNativeObject());
        }

        protected override DateTime GetLastReportedDateInternal()
        {
            string  dateStr     = AchievementBinding.NPAchievementGetLastReportedDate(AddrOfNativeObject());
            return IosNativePluginsUtility.ParseDateTimeStringInUTCFormat(dateStr);
        }

        protected override void ReportProgressInternal(ReportAchievementProgressInternalCallback callback)
        {
            IntPtr  tagPtr      = MarshalUtility.GetIntPtr(callback);
            AchievementBinding.NPAchievementReportProgress(AddrOfNativeObject(), tagPtr);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(GameServicesLoadArrayNativeCallback))]
        private static void HandleLoadAchievementsCallbackInternal(ref NativeArray nativeArray, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     achievements    = CreateAchievementArray(ref nativeArray);
                var     errorObj        = Error.CreateNullableError(description: error);
                ((LoadAchievementsInternalCallback)tagHandle.Target)(achievements, errorObj);
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

        [MonoPInvokeCallback(typeof(GameServicesReportNativeCallback))]
        private static void HandleReportProgressCallbackInternal(string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     errorObj    = Error.CreateNullableError(description: error);
                ((ReportAchievementProgressInternalCallback)tagHandle.Target)(errorObj);
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
#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.CloudServicesCore.iOS
{
    internal static class CloudServicesUtility
    {
        #region Converter methods

        public static CloudUserAccountStatus ConvertToCloudUserAccountStatus(CKAccountStatus accountStatus)
        {
            switch (accountStatus)
            {
                case CKAccountStatus.CKAccountStatusAvailable:
                    return CloudUserAccountStatus.Available;

                case CKAccountStatus.CKAccountStatusNoAccount:
                    return CloudUserAccountStatus.NoAccount;

                case CKAccountStatus.CKAccountStatusRestricted:
                    return CloudUserAccountStatus.Restricted;

                case CKAccountStatus.CKAccountStatusCouldNotDetermine:
                    return CloudUserAccountStatus.CouldNotDetermine;

                default:
                    throw VBException.SwitchCaseNotImplemented(accountStatus);
            }
        }

        public static CloudSavedDataChangeReasonCode ConvertToCloudSavedDataChangeReasonCode(NSUbiquitousKeyValueStoreChange changeCode)
        {
            switch (changeCode)
            {
                case NSUbiquitousKeyValueStoreChange.NSUbiquitousKeyValueStoreAccountChange:
                    return CloudSavedDataChangeReasonCode.AccountChange;

                case NSUbiquitousKeyValueStoreChange.NSUbiquitousKeyValueStoreInitialSyncChange:
                    return CloudSavedDataChangeReasonCode.InitialSyncChange;

                case NSUbiquitousKeyValueStoreChange.NSUbiquitousKeyValueStoreQuotaViolationChange:
                    return CloudSavedDataChangeReasonCode.QuotaViolationChange;

                case NSUbiquitousKeyValueStoreChange.NSUbiquitousKeyValueStoreServerChange:
                    return CloudSavedDataChangeReasonCode.ServerChange;

                default:
                    throw VBException.SwitchCaseNotImplemented(changeCode);
            }
        }

        #endregion
    }
}
#endif
#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal static class GameCenterUtility
    {
        public static LeaderboardPlayerScope ConvertToLeaderboardPlayerScope(GKLeaderboardPlayerScope value)
        {
            switch (value)
            {
                case GKLeaderboardPlayerScope.GKLeaderboardPlayerScopeFriendsOnly:
                    return LeaderboardPlayerScope.FriendsOnly;

                case GKLeaderboardPlayerScope.GKLeaderboardPlayerScopeGlobal:
                    return LeaderboardPlayerScope.Global;

                default:
                    throw VBException.SwitchCaseNotImplemented(value);
            }
        }

        public static GKLeaderboardPlayerScope ConvertToGKLeaderboardPlayerScope(LeaderboardPlayerScope value)
        {
            switch (value)
            {
                case LeaderboardPlayerScope.FriendsOnly:
                    return GKLeaderboardPlayerScope.GKLeaderboardPlayerScopeFriendsOnly;

                case LeaderboardPlayerScope.Global:
                    return GKLeaderboardPlayerScope.GKLeaderboardPlayerScopeGlobal;

                default:
                    throw VBException.SwitchCaseNotImplemented(value);
            }
        }

        public static LeaderboardTimeScope ConvertToLeaderboardTimeScope(GKLeaderboardTimeScope value)
        {
            switch (value)
            {
                case GKLeaderboardTimeScope.GKLeaderboardTimeScopeToday:
                    return LeaderboardTimeScope.Today;

                case GKLeaderboardTimeScope.GKLeaderboardTimeScopeWeek:
                    return LeaderboardTimeScope.Week;

                case GKLeaderboardTimeScope.GKLeaderboardTimeScopeAllTime:
                    return LeaderboardTimeScope.AllTime;

                default:
                    throw VBException.SwitchCaseNotImplemented(value);
            }
        }

        public static GKLeaderboardTimeScope ConvertToGKLeaderboardTimeScope(LeaderboardTimeScope value)
        {
            switch (value)
            {
                case LeaderboardTimeScope.Today:
                    return GKLeaderboardTimeScope.GKLeaderboardTimeScopeToday;

                case LeaderboardTimeScope.Week:
                    return GKLeaderboardTimeScope.GKLeaderboardTimeScopeWeek;

                case LeaderboardTimeScope.AllTime:
                    return GKLeaderboardTimeScope.GKLeaderboardTimeScopeAllTime;

                default:
                    throw VBException.SwitchCaseNotImplemented(value);
            }
        }

        public static LocalPlayerAuthStatus ConvertToLocalPlayerAuthStatus(GKLocalPlayerAuthState value)
        {
            switch (value)
            {
                case GKLocalPlayerAuthState.GKLocalPlayerAuthStateNotFound:
                    return LocalPlayerAuthStatus.NotAvailable;

                case GKLocalPlayerAuthState.GKLocalPlayerAuthStateAuthenticating:
                    return LocalPlayerAuthStatus.Authenticating;

                case GKLocalPlayerAuthState.GKLocalPlayerAuthStateAvailable:
                    return LocalPlayerAuthStatus.Authenticated;

                default:
                    throw VBException.SwitchCaseNotImplemented(value);
            }
        }
    }
}
#endif
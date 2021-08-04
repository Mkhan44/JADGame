#if UNITY_ANDROID
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    internal static class Converter
    {
        public static LeaderboardTimeScope from(NativeLeaderboardTimeVariant timeSpan)
        {
            switch(timeSpan)
            {
                case NativeLeaderboardTimeVariant.Daily:
                    return LeaderboardTimeScope.Today;
                case NativeLeaderboardTimeVariant.Weekly:
                    return LeaderboardTimeScope.Week;
                case NativeLeaderboardTimeVariant.AllTime:
                    return LeaderboardTimeScope.AllTime;
                default:
                    throw VBException.SwitchCaseNotImplemented(timeSpan);
            }
        }

        public static NativeLeaderboardTimeVariant from(LeaderboardTimeScope timeScope)
        {
            switch (timeScope)
            {
                case LeaderboardTimeScope.Today:
                    return NativeLeaderboardTimeVariant.Daily;
                case LeaderboardTimeScope.Week:
                    return NativeLeaderboardTimeVariant.Weekly;
                case LeaderboardTimeScope.AllTime:
                    return NativeLeaderboardTimeVariant.AllTime;
                default:
                    throw VBException.SwitchCaseNotImplemented(timeScope);
            }
        }


        public static LeaderboardPlayerScope from(NativeLeaderboardCollectionVariant collectionVariant)
        {
            switch (collectionVariant)
            {
                case NativeLeaderboardCollectionVariant.Public:
                    return LeaderboardPlayerScope.Global;
                default:
                    DebugLogger.LogWarning("Only Global player scope is possible on Android");
                    return LeaderboardPlayerScope.Global;
            }
        }

        public static NativeLeaderboardCollectionVariant from(LeaderboardPlayerScope playerScope)
        {
            switch (playerScope)
            {
                case LeaderboardPlayerScope.Global:
                    return NativeLeaderboardCollectionVariant.Public;
                case LeaderboardPlayerScope.FriendsOnly:
                    return NativeLeaderboardCollectionVariant.Friends;
                default:
                    DebugLogger.LogWarning("Only Global player scope is possible on Android. Defaults to Global scope.");
                    return NativeLeaderboardCollectionVariant.Public;
            }
        }
    }
}
#endif

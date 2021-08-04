//
//  NPGameKitScoreBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <GameKit/GameKit.h>
#import "NPGameServicesDataTypes.h"
#import "NPKit.h"

// static fields
static GameServicesReportNativeCallback             _reportScoreCallback                    = nil;

#pragma mark - Native binding calls

NPBINDING DONTSTRIP void NPScoreRegisterCallbacks(GameServicesReportNativeCallback reportScoreCallback)
{
    // save reference
    _reportScoreCallback    = reportScoreCallback;
}

NPBINDING DONTSTRIP void* NPScoreCreate(const char* leaderboardId)
{
    GKScore*    score       = [[GKScore alloc] initWithLeaderboardIdentifier:NPCreateNSStringFromCString(leaderboardId)];
    return NPRetainWithOwnershipTransfer(score);
}

NPBINDING DONTSTRIP const char* NPScoreGetLeaderboardId(void* scorePtr)
{
    GKScore*    score       = (__bridge GKScore*)scorePtr;
    return NPCreateCStringCopyFromNSString(score.leaderboardIdentifier);
}

NPBINDING DONTSTRIP long NPScoreGetRank(void* scorePtr)
{
    GKScore*    score       = (__bridge GKScore*)scorePtr;
    return score.rank;
}

NPBINDING DONTSTRIP long NPScoreGetValue(void* scorePtr)
{
    GKScore*    score       = (__bridge GKScore*)scorePtr;
    return score.value;
}

NPBINDING DONTSTRIP void NPScoreSetValue(void* scorePtr, long value)
{
    GKScore*    score       = (__bridge GKScore*)scorePtr;
    score.value             = value;
}

NPBINDING DONTSTRIP const char* NPScoreGetLastReportedDate(void* scorePtr)
{
    GKScore*    score       = (__bridge GKScore*)scorePtr;
    NSString*   date        = NPCreateNSStringFromNSDate(score.date);
    return NPCreateCStringCopyFromNSString(date);
}

NPBINDING DONTSTRIP void* NPScoreGetPlayer(void* scorePtr)
{
    GKScore*    score       = (__bridge GKScore*)scorePtr;
    return (__bridge void*)score.player;
}

NPBINDING DONTSTRIP void NPScoreReportScore(void* scorePtr, void* tagPtr)
{
    GKScore*    score       = (__bridge GKScore*)scorePtr;
    [GKScore reportScores:@[score] withCompletionHandler:^(NSError* _Nullable error) {
        // send data
        _reportScoreCallback(NPCreateCStringFromNSError(error), tagPtr);
    }];
}

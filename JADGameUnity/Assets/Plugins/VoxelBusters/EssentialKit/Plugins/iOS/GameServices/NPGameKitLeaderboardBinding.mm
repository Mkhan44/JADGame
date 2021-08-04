//
//  NPGameKitLeaderboardBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <GameKit/GameKit.h>
#import "NPGameKitManager.h"
#import "NPGameServicesDataTypes.h"
#import "NPKit.h"
#import "NPManagedPointerCache.h"

// static fields
static GameServicesLoadArrayNativeCallback          _loadLeaderboardsCallback               = nil;
static GameServicesLoadArrayNativeCallback          _loadLeaderboardScoresCallback          = nil;
static GameServicesLoadImageNativeCallback          _loadImageCallback                      = nil;

#pragma mark - Native binding calls

NPBINDING DONTSTRIP void NPLeaderboardRegisterCallbacks(GameServicesLoadArrayNativeCallback loadLeaderboardsCallback, GameServicesLoadArrayNativeCallback loadScoresCallback, GameServicesLoadImageNativeCallback loadImageCallback)
{
    // set properties
    _loadLeaderboardsCallback       = loadLeaderboardsCallback;
    _loadLeaderboardScoresCallback  = loadScoresCallback;
    _loadImageCallback              = loadImageCallback;
}

NPBINDING DONTSTRIP void NPLeaderboardLoadLeaderboards(void* tagPtr)
{
    [GKLeaderboard loadLeaderboardsWithCompletionHandler:^(NSArray<GKLeaderboard*>* _Nullable leaderboards, NSError* _Nullable error) {
        // send data
        NPArray*    nativeArray     = NPCreateNativeArrayFromNSArray(leaderboards);
        _loadLeaderboardsCallback(nativeArray, NPCreateCStringFromNSError(error), tagPtr);
        
        // release properties
        delete(nativeArray);
    }];
}

NPBINDING DONTSTRIP void* NPLeaderboardCreate(const char* id)
{
    GKLeaderboard*  leaderboard     = [[GKLeaderboard alloc] init];
    leaderboard.identifier          = NPCreateNSStringFromCString(id);
    return NPRetainWithOwnershipTransfer(leaderboard);
}

NPBINDING DONTSTRIP const char* NPLeaderboardGetId(void* leaderboardPtr)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    return NPCreateCStringCopyFromNSString(leaderboard.identifier);
}

NPBINDING DONTSTRIP const char* NPLeaderboardGetTitle(void* leaderboardPtr)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    return NPCreateCStringCopyFromNSString(leaderboard.title);
}

NPBINDING DONTSTRIP GKLeaderboardPlayerScope NPLeaderboardGetPlayerScope(void* leaderboardPtr)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    return leaderboard.playerScope;
}

NPBINDING DONTSTRIP void NPLeaderboardSetPlayerScope(void* leaderboardPtr, GKLeaderboardPlayerScope playerScope)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    leaderboard.playerScope         = playerScope;
}

NPBINDING DONTSTRIP GKLeaderboardTimeScope NPLeaderboardGetTimeScope(void* leaderboardPtr)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    return leaderboard.timeScope;
}

NPBINDING DONTSTRIP void NPLeaderboardSetTimeScope(void* leaderboardPtr, GKLeaderboardTimeScope timeScope)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    leaderboard.timeScope                   = timeScope;
}

NPBINDING DONTSTRIP void* NPLeaderboardGetLocalPlayerScore(void* leaderboardPtr)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    return (__bridge void*)leaderboard.localPlayerScore;
}

NPBINDING DONTSTRIP void NPLeaderboardLoadScores(void* leaderboardPtr, long startIndex, int count, void* tagPtr)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    leaderboard.range               = NSMakeRange(startIndex, count);
    [leaderboard loadScoresWithCompletionHandler:^(NSArray<GKScore*>* _Nullable scores, NSError* _Nullable error) {
        // send data
        NPArray*    nativeArray     = NPCreateNativeArrayFromNSArray(scores);
        _loadLeaderboardScoresCallback(nativeArray, NPCreateCStringFromNSError(error), tagPtr);
        
        // release properties
        delete(nativeArray);
    }];
}

NPBINDING DONTSTRIP void NPLeaderboardLoadImage(void* leaderboardPtr, void* tagPtr)
{
    GKLeaderboard*  leaderboard     = (__bridge GKLeaderboard*)leaderboardPtr;
    [leaderboard loadImageWithCompletionHandler:^(UIImage* _Nullable image, NSError* _Nullable error) {
        // send data
        if (error)
        {
            _loadImageCallback(nil, -1, NPCreateCStringFromNSError(error), tagPtr);
        }
        else
        {
            NSData* imageData       = NPEncodeImageAsData(image, UIImageEncodeTypePNG);
            _loadImageCallback((void*)[imageData bytes], (int)[imageData length], nil, tagPtr);
        }
    }];
}

NPBINDING DONTSTRIP void NPLeaderboardShowView(const char* leaderboardID, int timeScope, void* tagPtr)
{
    // create view controller
    GKGameCenterViewController* gameCenterVC    = [[GKGameCenterViewController alloc] init];
    gameCenterVC.viewState                      = GKGameCenterViewControllerStateLeaderboards;
    gameCenterVC.leaderboardIdentifier          = NPCreateNSStringFromCString(leaderboardID);
    gameCenterVC.leaderboardTimeScope           = (GKLeaderboardTimeScope)timeScope;
    [[NPGameKitManager sharedManager] showGameCenterViewController:gameCenterVC withTag:tagPtr];
}

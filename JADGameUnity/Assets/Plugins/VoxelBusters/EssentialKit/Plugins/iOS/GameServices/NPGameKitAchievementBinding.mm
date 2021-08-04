//
//  NPGameKitAchievementBinding.mm
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
static GameServicesLoadArrayNativeCallback          _loadAchievementsCallback               = nil;
static GameServicesReportNativeCallback             _reportAchievementCallback              = nil;
static bool                                         _showsBannerOnCompletion                = false;

#pragma mark - Native binding calls

NPBINDING DONTSTRIP void NPAchievementRegisterCallbacks(GameServicesLoadArrayNativeCallback loadAchievementsCallback, GameServicesReportNativeCallback reportAchievementCallback)
{
    _loadAchievementsCallback   = loadAchievementsCallback;
    _reportAchievementCallback  = reportAchievementCallback;
}

NPBINDING DONTSTRIP void NPAchievementSetCanShowBannerOnCompletion(bool showsBannerOnCompletion)
{
    // set properties
    _showsBannerOnCompletion    = showsBannerOnCompletion;
}

NPBINDING DONTSTRIP void NPAchievementLoadAchievements(void* tagPtr)
{
    [GKAchievement loadAchievementsWithCompletionHandler:^(NSArray<GKAchievement*>* _Nullable achievements, NSError* _Nullable error) {
        // send data
        NPArray*    nativeArray = NPCreateNativeArrayFromNSArray(achievements);
        _loadAchievementsCallback(nativeArray, NPCreateCStringFromNSError(error), tagPtr);
        
        // release properties
        delete(nativeArray);
    }];
}

NPBINDING DONTSTRIP void* NPAchievementCreate(const char* id)
{
    GKAchievement*  achievement         = [[GKAchievement alloc] initWithIdentifier:NPCreateNSStringFromCString(id)];
    achievement.showsCompletionBanner   = _showsBannerOnCompletion;
    return NPRetainWithOwnershipTransfer(achievement);
}

NPBINDING DONTSTRIP const char* NPAchievementGetId(void* achievementPtr)
{
    GKAchievement*  achievement         = (__bridge GKAchievement*)achievementPtr;
    return NPCreateCStringCopyFromNSString(achievement.identifier);
}

NPBINDING DONTSTRIP double NPAchievementGetPercentageCompleted(void* achievementPtr)
{
    GKAchievement*  achievement         = (__bridge GKAchievement*)achievementPtr;
    return achievement.percentComplete;
}

NPBINDING DONTSTRIP void NPAchievementSetPercentageCompleted(void* achievementPtr, double percentComplete)
{
    GKAchievement*  achievement         = (__bridge GKAchievement*)achievementPtr;
    achievement.percentComplete         = percentComplete;
}

NPBINDING DONTSTRIP bool NPAchievementGetIsCompleted(void* achievementPtr)
{
    GKAchievement*  achievement         = (__bridge GKAchievement*)achievementPtr;
    return achievement.isCompleted;
}

NPBINDING DONTSTRIP const char* NPAchievementGetLastReportedDate(void* achievementPtr)
{
    GKAchievement*  achievement         = (__bridge GKAchievement*)achievementPtr;
    NSString*       lastReportedDate    = NPCreateNSStringFromNSDate(achievement.lastReportedDate);
    return NPCreateCStringCopyFromNSString(lastReportedDate);
}

NPBINDING DONTSTRIP void NPAchievementReportProgress(void* achievementPtr, void* tagPtr)
{
    GKAchievement*  achievement         = (__bridge GKAchievement*)achievementPtr;
    [GKAchievement reportAchievements:@[achievement] withCompletionHandler:^(NSError * _Nullable error) {
        // send data
        _reportAchievementCallback(NPCreateCStringFromNSError(error), tagPtr);
    }];
}

NPBINDING DONTSTRIP void NPAchievementShowView(void* tagPtr)
{
    GKGameCenterViewController* gameCenterVC    = [[GKGameCenterViewController alloc] init];
    gameCenterVC.viewState                      = GKGameCenterViewControllerStateAchievements;
    [[NPGameKitManager sharedManager] showGameCenterViewController:gameCenterVC withTag:tagPtr];
}

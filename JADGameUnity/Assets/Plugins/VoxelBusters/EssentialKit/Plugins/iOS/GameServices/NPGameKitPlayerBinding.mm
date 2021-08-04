//
//  NPGameKitPlayerBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <GameKit/GameKit.h>
#import "NPGameKitManager.h"
#import "NPGameServicesDataTypes.h"
#import "NPKit.h"

// static fields
static GameServicesLoadArrayNativeCallback          _loadPlayersCallback                    = nil;
static GameServicesLoadImageNativeCallback          _loadImageCallback                      = nil;

#pragma mark - Native binding calls

NPBINDING DONTSTRIP void NPPlayerRegisterCallbacks(GameServicesLoadArrayNativeCallback loadPlayersCallback, GameServicesLoadImageNativeCallback loadPlayerImageCallback)
{
    // save reference to callback
    _loadPlayersCallback    = loadPlayersCallback;
    _loadImageCallback      = loadPlayerImageCallback;
}

NPBINDING DONTSTRIP void NPPlayerLoadPlayers(const char** playerIds, int count, void* tagPtr)
{
    [GKPlayer loadPlayersForIdentifiers:NPCreateArrayOfNSString(playerIds, count) withCompletionHandler:^(NSArray<GKPlayer*>* _Nullable players, NSError* _Nullable error) {
        // send data
        NPArray*    nativeArray     = NPCreateNativeArrayFromNSArray(players);
        _loadPlayersCallback(nativeArray, NPCreateCStringFromNSError(error), tagPtr);
        
        // release properties
        delete(nativeArray);
    }];
}

NPBINDING DONTSTRIP const char* NPPlayerGetId(void* playerPtr)
{
    GKPlayer*   player      = (__bridge GKPlayer*)playerPtr;
    return NPCreateCStringCopyFromNSString(player.playerID);
}

NPBINDING DONTSTRIP const char* NPPlayerGetAlias(void* playerPtr)
{
    GKPlayer*   player      = (__bridge GKPlayer*)playerPtr;
    return NPCreateCStringCopyFromNSString(player.alias);
}

NPBINDING DONTSTRIP const char* NPPlayerGetDisplayName(void* playerPtr)
{
    GKPlayer*   player      = (__bridge GKPlayer*)playerPtr;
    return NPCreateCStringCopyFromNSString(player.displayName);
}

NPBINDING DONTSTRIP void NPPlayerLoadImage(void* playerPtr, void* tagPtr)
{
    GKPlayer*   player      = (__bridge GKPlayer*)playerPtr;
    [player loadPhotoForSize:GKPhotoSizeNormal withCompletionHandler:^(UIImage* _Nullable photo, NSError* _Nullable error) {
        // send data
        if (error)
        {
            _loadImageCallback(nil, -1, NPCreateCStringFromNSError(error), tagPtr);
        }
        else
        {
            NSData* imageData   = NPEncodeImageAsData(photo, UIImageEncodeTypePNG);
            _loadImageCallback((void*)[imageData bytes], (int)[imageData length], nil, tagPtr);
        }
    }];
}

NPBINDING DONTSTRIP void NPLocalPlayerRegisterCallbacks(GameServicesAuthStateChangeNativeCallback authChangeCallback)
{
    // set value
    [NPGameKitManager setAuthChangeCallback:authChangeCallback];
}

NPBINDING DONTSTRIP void* NPLocalPlayerGetLocalPlayer()
{
    return (__bridge void*)[GKLocalPlayer localPlayer];
}

NPBINDING DONTSTRIP void NPLocalPlayerAuthenticate()
{
    [[NPGameKitManager sharedManager] authenticate];
}

NPBINDING DONTSTRIP bool NPLocalPlayerIsAuthenticated()
{
    return [[GKLocalPlayer localPlayer] isAuthenticated];
}

NPBINDING DONTSTRIP bool NPLocalPlayerIsUnderage()
{
    return [[GKLocalPlayer localPlayer] isUnderage];
}

//
//  NPGameKitObserver.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <GameKit/GameKit.h>
#import "NPGameServicesDataTypes.h"

@interface NPGameKitManager : NSObject<GKGameCenterControllerDelegate, UIPopoverPresentationControllerDelegate>

+ (NPGameKitManager*)sharedManager;
+ (void)setGameCenterViewClosedCallback:(GameServicesViewClosedNativeCallback)callback;
+ (void)setAuthChangeCallback:(GameServicesAuthStateChangeNativeCallback)callback;
+ (void)setGameCenterLoadServerCredentialsCompleteCallback:(GameServicesLoadServerCredentialsNativeCallback)callback;

- (void)authenticate;
- (bool)isShowingGameCenterView;
- (void)showGameCenterViewController:(GKGameCenterViewController*)viewController withTag:(void*)tagPtr;
- (void)loadServerCredentials: (void*) tagPtr;

@end


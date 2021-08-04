//
//  NPGameServicesDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <GameKit/GameKit.h>
#import "NPDefines.h"
#import "NPUnityDataTypes.h"

#pragma mark - Enums

typedef enum : NSInteger
{
    GKLocalPlayerAuthStateNotFound,
    GKLocalPlayerAuthStateAuthenticating,
    GKLocalPlayerAuthStateAvailable,
} GKLocalPlayerAuthState;

#pragma mark - Callback definitions

// callback signatures
typedef void (*GameServicesLoadArrayNativeCallback)(NPArray* nativeArray, const char* error, void* tagPtr);

typedef void (*GameServicesReportNativeCallback)(const char* error, void* tagPtr);

typedef void (*GameServicesAuthStateChangeNativeCallback)(GKLocalPlayerAuthState state, const char* error);

typedef void (*GameServicesLoadImageNativeCallback)(void* dataPtr, int dataLength, const char* error, void* tagPtr);

typedef void (*GameServicesViewClosedNativeCallback)(const char* error, void* tagPtr);

typedef void (*GameServicesLoadServerCredentialsNativeCallback)(const char* publicKeyUrl, void* signaturePtr, int signatureDataLength, void* saltPtr, int saltDataLength, long timestamp, const char* error, void* tagPtr);

//
//  NPMediaServicesDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Photos/Photos.h>

// enums
typedef enum : NSInteger
{
    PickImageFinishReasonCancelled,
    PickImageFinishReasonFailed,
    PickImageFinishReasonSuccess,
} PickImageFinishReason;

// callback signatures
typedef void (*RequestPhotoLibraryAccessNativeCallback)(PHAuthorizationStatus status, const char* error, void* tagPtr);

typedef void (*RequestCameraAccessNativeCallback)(AVAuthorizationStatus status, const char* error, void* tagPtr);

typedef void (*PickImageNativeCallback)(void* imageDataPtr, PickImageFinishReason reasonCode, void* tagPtr);

typedef void (*SaveImageToAlbumNativeCallback)(bool success, const char* error, void* tagPtr);

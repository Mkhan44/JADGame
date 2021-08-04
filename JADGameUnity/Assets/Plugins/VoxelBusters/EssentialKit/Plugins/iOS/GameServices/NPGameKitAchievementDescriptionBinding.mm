//
//  NPGameKitAchievementDescriptionBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <GameKit/GameKit.h>
#import "NPGameServicesDataTypes.h"
#import "NPKit.h"

// static fields
static GameServicesLoadArrayNativeCallback          _loadAchievementDescriptionsCallback    = nil;
static GameServicesLoadImageNativeCallback          _loadImageCallback                      = nil;

#pragma mark - Native binding calls

NPBINDING DONTSTRIP void NPAchievementDescriptionRegisterCallbacks(GameServicesLoadArrayNativeCallback loadDescriptionsCallback, GameServicesLoadImageNativeCallback loadImageCallback)
{
    // save references
    _loadAchievementDescriptionsCallback        = loadDescriptionsCallback;
    _loadImageCallback                          = loadImageCallback;
}

NPBINDING DONTSTRIP void NPAchievementDescriptionLoadDescriptions(void* tagPtr)
{
    [GKAchievementDescription loadAchievementDescriptionsWithCompletionHandler:^(NSArray<GKAchievementDescription*>* _Nullable descriptions, NSError* _Nullable error) {
        // send data
        NPArray*    nativeArray = NPCreateNativeArrayFromNSArray(descriptions);
        _loadAchievementDescriptionsCallback(nativeArray, NPCreateCStringFromNSError(error), tagPtr);
        
        // release properties
        delete(nativeArray);
    }];
}

NPBINDING DONTSTRIP const char* NPAchievementDescriptionGetId(void* descriptionPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    return NPCreateCStringCopyFromNSString(description.identifier);
}

NPBINDING DONTSTRIP const char* NPAchievementDescriptionGetTitle(void* descriptionPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    return NPCreateCStringCopyFromNSString(description.title);
}

NPBINDING DONTSTRIP const char* NPAchievementDescriptionGetAchievedDescription(void* descriptionPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    return NPCreateCStringCopyFromNSString(description.achievedDescription);
}

NPBINDING DONTSTRIP const char* NPAchievementDescriptionGetUnachievedDescription(void* descriptionPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    return NPCreateCStringCopyFromNSString(description.unachievedDescription);
}

NPBINDING DONTSTRIP long NPAchievementDescriptionGetMaximumPoints(void* descriptionPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    return (long)description.maximumPoints;
}

NPBINDING DONTSTRIP bool NPAchievementDescriptionGetHidden(void* descriptionPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    return description.hidden;
}

NPBINDING DONTSTRIP bool NPAchievementDescriptionGetReplayable(void* descriptionPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    return description.replayable;
}

NPBINDING DONTSTRIP void NPAchievementDescriptionLoadIncompleteAchievementImage(void* descriptionPtr, void* tagPtr)
{
    UIImage*    image       = [GKAchievementDescription incompleteAchievementImage];
    if (image)
    {
        NSData* imageData   = NPEncodeImageAsData(image, UIImageEncodeTypePNG);
        _loadImageCallback((void*)[imageData bytes], (int)[imageData length], nil, tagPtr);
    }
    else
    {
        _loadImageCallback(nil, -1, "No image", tagPtr);
    }
}

NPBINDING DONTSTRIP void NPAchievementDescriptionLoadImage(void* descriptionPtr, void* tagPtr)
{
    GKAchievementDescription*   description = (__bridge GKAchievementDescription*)descriptionPtr;
    [description loadImageWithCompletionHandler:^(UIImage* _Nullable image, NSError* _Nullable error) {
        // send data
        if (error)
        {
            _loadImageCallback(nil, -1, NPCreateCStringFromNSError(error), tagPtr);
        }
        else
        {
            NSData*     imageData   = NPEncodeImageAsData(image, UIImageEncodeTypePNG);
            _loadImageCallback((void*)[imageData bytes], (int)[imageData length], nil, tagPtr);
        }
    }];
}

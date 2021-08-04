//
//  NPMediaServicesBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPPhotoLibrary.h"
#import "NPMediaServicesDataTypes.h"
#import "NPKit.h"

#pragma mark - Native binding calls

NPBINDING DONTSTRIP void NPMediaServicesRegisterCallbacks(RequestPhotoLibraryAccessNativeCallback requestPhotoLibraryAccessCallback,
                                                          RequestCameraAccessNativeCallback requestCameraAccessCallback,
                                                          PickImageNativeCallback pickImageCallback,
                                                          SaveImageToAlbumNativeCallback saveImageToAlbumCallback)
{
    [NPPhotoLibrary setRequestPhotoLibraryAccessCallback:requestPhotoLibraryAccessCallback];
    [NPPhotoLibrary setRequestCameraAccessCallback:requestCameraAccessCallback];
    [NPPhotoLibrary setPickImageCallback:pickImageCallback];
    [NPPhotoLibrary setSaveImageToAlbumCallback:saveImageToAlbumCallback];
}

NPBINDING DONTSTRIP void NPMediaServicesRequestPhotoLibraryAccess(void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] requestPhotoLibraryAccess:tagPtr];
}

NPBINDING DONTSTRIP PHAuthorizationStatus NPMediaServicesGetPhotoLibraryAccessStatus()
{
    return [[NPPhotoLibrary sharedInstance] getPhotoLibraryAccessStatus];
}

NPBINDING DONTSTRIP void NPMediaServicesRequestCameraAccess(void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] requestCameraAccess:tagPtr];
}

NPBINDING DONTSTRIP AVAuthorizationStatus NPMediaServicesGetCameraAccessStatus()
{
    return [[NPPhotoLibrary sharedInstance] getCameraAccessStatus];
}

NPBINDING DONTSTRIP bool NPMediaServicesCanPickImageFromGallery()
{
    return [[NPPhotoLibrary sharedInstance] canPickImageFromGallery];
}

NPBINDING DONTSTRIP void NPMediaServicesPickImageFromGallery(bool canEdit, void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] pickImageFromGallery:canEdit withTag:tagPtr];
}

NPBINDING DONTSTRIP bool NPMediaServicesCanPickImageFromCamera()
{
    return [[NPPhotoLibrary sharedInstance] canPickImageFromCamera];
}

NPBINDING DONTSTRIP void NPMediaServicesPickImageFromCamera(bool canEdit, void* tagPtr)
{
    [[NPPhotoLibrary sharedInstance] pickImageFromCamera:canEdit withTag:tagPtr];
}

NPBINDING DONTSTRIP bool NPMediaServicesCanSaveImageToAlbum()
{
    return [[NPPhotoLibrary sharedInstance] canSaveImageToAlbum];
}

NPBINDING DONTSTRIP void NPMediaServicesSaveImageToAlbum(const char* albumName, void* imageDataPtr, int imageDataLength, void* tagPtr)
{
    UIImage*    image   = NPCreateImage(imageDataPtr, imageDataLength);
    [[NPPhotoLibrary sharedInstance] saveImageToAlbum:image withTag:tagPtr];
}

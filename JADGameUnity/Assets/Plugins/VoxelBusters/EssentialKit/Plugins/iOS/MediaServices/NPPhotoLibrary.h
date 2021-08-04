//
//  NPPhotoLibrary.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Photos/Photos.h>
#import "NPMediaServicesDataTypes.h"

@interface NPPhotoLibrary : NSObject<UINavigationControllerDelegate, UIImagePickerControllerDelegate, UIPopoverPresentationControllerDelegate>

+ (void)setRequestPhotoLibraryAccessCallback:(RequestPhotoLibraryAccessNativeCallback)callback;
+ (void)setRequestCameraAccessCallback:(RequestCameraAccessNativeCallback)callback;
+ (void)setPickImageCallback:(PickImageNativeCallback)callback;
+ (void)setSaveImageToAlbumCallback:(SaveImageToAlbumNativeCallback)callback;
+ (NPPhotoLibrary*)sharedInstance;

// photo library methods
- (void)requestPhotoLibraryAccess:(void*)tagPtr;
- (PHAuthorizationStatus)getPhotoLibraryAccessStatus;
- (void)requestCameraAccess:(void*)tagPtr;
- (AVAuthorizationStatus)getCameraAccessStatus;

- (bool)canPickImageFromGallery;
- (void)pickImageFromGallery:(bool)canEdit withTag:(void*)tagPtr;
- (bool)canPickImageFromCamera;
- (void)pickImageFromCamera:(bool)canEdit withTag:(void*)tagPtr;

- (bool)canSaveImageToAlbum;
- (void)saveImageToAlbum:(UIImage*)image withTag:(void*)tagPtr;

@end

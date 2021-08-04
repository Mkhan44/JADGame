//
//  NPPhotoLibrary.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPPhotoLibrary.h"
#import "NPKit.h"
#import "UIViewController+Presentation.h"

static RequestPhotoLibraryAccessNativeCallback      _requestPhotoLibraryAccessCallback;
static RequestCameraAccessNativeCallback            _requestCameraAccessCallback;
static PickImageNativeCallback                      _pickImageCallback;
static SaveImageToAlbumNativeCallback               _saveImageToAlbumCallback;
static NPPhotoLibrary*                              _sharedInstance;
static void*                                        _pickImageRequestTag;

@interface NPPhotoLibrary ()

@property(nonatomic, strong) UIImagePickerController* pickerController;

@end

@implementation NPPhotoLibrary

@synthesize pickerController = _pickerController;

+ (void)setRequestPhotoLibraryAccessCallback:(RequestPhotoLibraryAccessNativeCallback)callback
{
    _requestPhotoLibraryAccessCallback      = callback;
}

+ (void)setRequestCameraAccessCallback:(RequestCameraAccessNativeCallback)callback
{
    _requestCameraAccessCallback            = callback;
}

+ (void)setPickImageCallback:(PickImageNativeCallback)callback
{
    _pickImageCallback                      = callback;
}

+ (void)setSaveImageToAlbumCallback:(SaveImageToAlbumNativeCallback)callback
{
    _saveImageToAlbumCallback               = callback;
}

+ (NPPhotoLibrary*)sharedInstance
{
    if (!_sharedInstance)
    {
        _sharedInstance = [[NPPhotoLibrary alloc] init];
    }
    return _sharedInstance;
}

- (void)requestPhotoLibraryAccess:(void*)tagPtr
{
    [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
        // send callback
        _requestPhotoLibraryAccessCallback(status, nil, tagPtr);
    }];
}

- (PHAuthorizationStatus)getPhotoLibraryAccessStatus
{
    return [PHPhotoLibrary authorizationStatus];
}

- (void)requestCameraAccess:(void*)tagPtr
{
    [AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^(BOOL granted) {
        // send callback
        AVAuthorizationStatus   status  = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
        _requestCameraAccessCallback(status, nil, tagPtr);
    }];
}

- (AVAuthorizationStatus)getCameraAccessStatus
{
    return [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
}

- (bool)canPickImageFromGallery
{
    return [UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypePhotoLibrary];
}

- (void)pickImageFromGallery:(bool)canEdit withTag:(void*)tagPtr
{
    if (self.pickerController)
    {
        return;
    }
    
    // cache tag
    _pickImageRequestTag    = tagPtr;
    
    // open image picker in gallery view mode
    self.pickerController   = [self createImagePickerForSourceType:UIImagePickerControllerSourceTypePhotoLibrary withEditOption:canEdit];
    [UnityGetGLViewController() presentViewControllerInPopoverStyleIfRequired:_pickerController
                                                                 withDelegate:self
                                                                 fromPosition:GetLastTouchPosition()
                                                                     animated:YES
                                                                   completion:nil];
}

- (bool)canPickImageFromCamera
{
    return [UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera];
}

- (void)pickImageFromCamera:(bool)canEdit withTag:(void*)tagPtr
{
    if (self.pickerController)
    {
        return;
    }
    
    // cache tag
    _pickImageRequestTag    = tagPtr;
    
    // open image picker in camera view mode
    self.pickerController   = [self createImagePickerForSourceType:UIImagePickerControllerSourceTypeCamera withEditOption:canEdit];
    [UnityGetGLViewController() presentViewController:self.pickerController animated:YES completion:nil];
}

- (bool)canSaveImageToAlbum
{
    // auth status check is handled on Unity end
    return true;
}

- (void)saveImageToAlbum:(UIImage*)image withTag:(void*)tagPtr
{
    UIImageWriteToSavedPhotosAlbum(image, self, @selector(image:didFinishSavingWithError:contextInfo:), tagPtr);
}

- (void)image:(UIImage*)image didFinishSavingWithError:(NSError*)error contextInfo:(void*)contextInfo
{
    _saveImageToAlbumCallback((error == nil), NPCreateCStringFromNSError(error), contextInfo);
}

#pragma mark - Private methods

- (UIImagePickerController*)createImagePickerForSourceType:(UIImagePickerControllerSourceType)pickerSource
                                            withEditOption:(BOOL)canEdit
{
    UIImagePickerController*    picker  = [[UIImagePickerController alloc] init];
    picker.delegate                     = self;
    picker.allowsEditing                = canEdit;
    picker.sourceType                   = pickerSource;
    picker.mediaTypes                   = @[(NSString*)kUTTypeImage,
                                            (NSString*)kUTTypeJPEG,
                                            (NSString*)kUTTypeJPEG2000,
                                            (NSString*)kUTTypePNG];
    return picker;
}

- (bool)isVideoTypeMediaArray:(NSArray *)mediaTypes
{
    NSString *mediaType = [mediaTypes objectAtIndex:0];
    return (CFStringCompare((CFStringRef) mediaType, kUTTypeMovie, 0) == kCFCompareEqualTo);
}

- (void)dismissImagePickerController
{
    // dismiss controller
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
    
    // release properties
    self.pickerController.delegate  = nil;
    self.pickerController           = nil;
    _pickImageRequestTag            = nil;
}

#pragma mark - UIImagePickerControllerDelegate methods

- (void)imagePickerController:(UIImagePickerController*)picker didFinishPickingMediaWithInfo:(NSDictionary<UIImagePickerControllerInfoKey, id>*)info
{
    // process response
    if ([self isVideoTypeMediaArray:picker.mediaTypes])
    {
        NSLog(@"[NativePlugins] Finished picking video.");
    }
    else
    {
        NSLog(@"[NativePlugins] Finished picking image.");
        UIImage*    editedImage     = [info objectForKey:UIImagePickerControllerEditedImage];
        UIImage*    originalImage   = [info objectForKey:UIImagePickerControllerOriginalImage];
        
        // we will use edited image if it is available
        UIImage*    selectedImage   = editedImage ? editedImage : originalImage;
        NSData*     imageData       = NPEncodeImageAsData(selectedImage, UIImageEncodeTypePNG);
        _pickImageCallback((__bridge void*)imageData, PickImageFinishReasonSuccess, _pickImageRequestTag);
    }
    
    // reset state
    [self dismissImagePickerController];
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController*)picker
{
    if ([self isVideoTypeMediaArray:picker.mediaTypes])
    {
        NSLog(@"[NativePlugins] Cancelled picking video.");
    }
    else
    {
        NSLog(@"[NativePlugins] Cancelled picking image.");

        _pickImageCallback(nil, PickImageFinishReasonCancelled, _pickImageRequestTag);
    }
    
    // reset properties
    [self dismissImagePickerController];
}

#pragma mark - UIPopoverPresentationControllerDelegate implementation

- (void)presentationControllerDidDismiss:(UIPresentationController*)presentationController
{
    NSLog(@"[NativePlugins] Image picker closed.");
}

#pragma mark - Unused code

/*
- (void)saveImageToAlbum:(UIImage*)image withTag:(void*)tagPtr
{
    // create blocks
    void (^saveBlock)(PHAssetCollection* assetCollection) = ^void(PHAssetCollection* assetCollection) {
        [[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
            PHAssetChangeRequest*           assetChangeRequest              = [PHAssetChangeRequest creationRequestForAssetFromImage:image];
            PHAssetCollectionChangeRequest* assetCollectionChangeRequest    = [PHAssetCollectionChangeRequest changeRequestForAssetCollection:assetCollection];
            [assetCollectionChangeRequest addAssets:@[[assetChangeRequest placeholderForCreatedAsset]]];
        }
                                          completionHandler:^(BOOL success, NSError *error) {
                                              // send callback
                                              _saveImageToAlbumCallback(success, NPCreateCStringFromNSError(error), tagPtr);
                                          }];
    };
    void (^saveFailedBlock)(NSError* error)   = ^void(NSError* error) {
        // send fail information
        _saveImageToAlbumCallback(false, NPCreateCStringFromNSError(error), tagPtr);
    };
    
    PHFetchOptions* fetchOptions    = [[PHFetchOptions alloc] init];
    fetchOptions.predicate          = [NSPredicate predicateWithFormat:@"localizedTitle = %@", albumName];
    PHFetchResult*  fetchResult     = [PHAssetCollection fetchAssetCollectionsWithType:PHAssetCollectionTypeAlbum subtype:PHAssetCollectionSubtypeAny options:fetchOptions];
    if (fetchResult.count > 0)
    {
        saveBlock(fetchResult.firstObject);
    }
    else
    {
        __block PHObjectPlaceholder*    albumPlaceholder;
        [[PHPhotoLibrary sharedPhotoLibrary] performChanges:^{
            PHAssetCollectionChangeRequest* changeRequest = [PHAssetCollectionChangeRequest creationRequestForAssetCollectionWithTitle:albumName];
            albumPlaceholder        = changeRequest.placeholderForCreatedAssetCollection;
            
        } completionHandler:^(BOOL success, NSError *error) {
            if (success)
            {
                PHFetchResult*  fetchResult = [PHAssetCollection fetchAssetCollectionsWithLocalIdentifiers:@[albumPlaceholder.localIdentifier] options:nil];
                if (fetchResult.count > 0)
                {
                    saveBlock(fetchResult.firstObject);
                }
            }
            else
            {
                NSLog(@"[NativePlugins] Failed to create album: %@", error);
                saveFailedBlock(error);
            }
        }];
    }
}
*/

@end

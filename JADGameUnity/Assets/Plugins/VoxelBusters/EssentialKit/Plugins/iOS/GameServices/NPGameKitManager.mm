//
//  NPGameKitObserver.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPGameKitManager.h"
#import "NPKit.h"
#import "NPManagedPointerCache.h"
#import "UIViewController+Presentation.h"

// static fields
static NPGameKitManager*                                _sharedObserver                             = nil;

static GameServicesViewClosedNativeCallback             _gameCenterViewClosedCallback               = nil;
static GameServicesAuthStateChangeNativeCallback        _authChangeCallback                         = nil;
static GameServicesLoadServerCredentialsNativeCallback  _gameCenterLoadServerCredentialsCallback    = nil;


@interface NPGameKitManager ()

@property(nonatomic, strong) GKGameCenterViewController* gameCenterViewController;

@end

@implementation NPGameKitManager

@synthesize gameCenterViewController = _gameCenterViewController;

#pragma mark - Static methods

+ (NPGameKitManager*)sharedManager
{
    if (nil == _sharedObserver)
    {
        _sharedObserver = [[NPGameKitManager alloc] init];
    }
    
    return _sharedObserver;
}

+ (void)setGameCenterViewClosedCallback:(GameServicesViewClosedNativeCallback)callback
{
    _gameCenterViewClosedCallback  = callback;
}

+ (void)setAuthChangeCallback:(GameServicesAuthStateChangeNativeCallback)callback
{
    _authChangeCallback             = callback;
}

+ (void)setGameCenterLoadServerCredentialsCompleteCallback:(GameServicesLoadServerCredentialsNativeCallback)callback
{
    _gameCenterLoadServerCredentialsCallback    = callback;
}



+ (GKLocalPlayerAuthState)convertAuthFlagToEnumValue:(bool)isAuthenticated
{
    return isAuthenticated ? GKLocalPlayerAuthStateAvailable : GKLocalPlayerAuthStateNotFound;
}

#pragma mark - Instance methods

- (id)init
{
    self    = [super init];
    if (self)
    {
        // register for system notification
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(authenticationDidChangeNotification:) name:GKPlayerAuthenticationDidChangeNotificationName
                                                   object:nil];
    }
    
    return self;
}

- (void)dealloc
{
    // unregister from notification
    [[NSNotificationCenter defaultCenter] removeObserver:self
                                                    name:GKPlayerAuthenticationDidChangeNotificationName
                                                  object:nil];
}

- (void)authenticate
{
    __weak GKLocalPlayer*  localPlayer = [GKLocalPlayer localPlayer];
    
    // check current status
    if (!localPlayer.isAuthenticated)
    {
        // set auth handler
        localPlayer.authenticateHandler = ^(UIViewController* viewController, NSError* error) {
            if (viewController != nil)
            {
                // notify observer that we are authenticating the user
                _authChangeCallback(GKLocalPlayerAuthStateAuthenticating, nil);
                
                // show corresponding view
                [UnityGetGLViewController() presentViewController:viewController animated:YES completion:^{
                    NSLog(@"Showing auth view.");
                }];
            }
            else
            {
                GKLocalPlayerAuthState  state   = [NPGameKitManager convertAuthFlagToEnumValue: localPlayer.isAuthenticated];
                _authChangeCallback(state, NPCreateCStringFromNSError(error));
            }
        };;
    }
    else
    {
        _authChangeCallback(GKLocalPlayerAuthStateAvailable, nil);
    }
}

- (bool)isShowingGameCenterView
{
    return (self.gameCenterViewController != nil);
}

- (void)showGameCenterViewController:(GKGameCenterViewController*)viewController withTag:(void*)tagPtr
{
    // cache reference
    self.gameCenterViewController   = viewController;
    
    // configure contoller
    _gameCenterViewController.gameCenterDelegate    = self;

    // add instance to tracker
    [[NPManagedPointerCache sharedInstance] addPointer:tagPtr forKey:_gameCenterViewController];
    
    // present view  contoller
    CGRect  viewFrame   = [UnityGetGLView() frame];
    CGPoint spawnPoint  = CGPointMake(CGRectGetMidX(viewFrame), CGRectGetMidY(viewFrame));
    [UnityGetGLViewController() presentViewControllerInPopoverStyleIfRequired:_gameCenterViewController
                                                                 withDelegate:self
                                                                 fromPosition:spawnPoint
                                                                     animated:YES
                                                                   completion:nil];
}

- (void)loadServerCredentials: (void*) tagPtr
{
/*if !defined(__IPHONE_13_5)
    [[GKLocalPlayer localPlayer] generateIdentityVerificationSignatureWithCompletionHandler:^(NSURL *publicKeyUrl, NSData *signature, NSData *salt, uint64_t timestamp, NSError *error)
#else
    [[GKLocalPlayer localPlayer] fetchItemsForIdentityVerificationSignature:^(NSURL *publicKeyUrl, NSData *signature, NSData *salt, uint64_t timestamp, NSError *error)
#endif*/
     [[GKLocalPlayer localPlayer] generateIdentityVerificationSignatureWithCompletionHandler:^(NSURL *publicKeyUrl, NSData *signature, NSData *salt, uint64_t timestamp, NSError *error)
     {
         if(error == nil)
         {
             NSString *publicKeyUrlString       = [publicKeyUrl absoluteString];
                          
             _gameCenterLoadServerCredentialsCallback(NPCreateCStringCopyFromNSString(publicKeyUrlString), (void*)[signature bytes], (int)[signature length], (void*)[salt bytes], (int)[salt length], timestamp, nil, tagPtr);
         }
         else
         {
             _gameCenterLoadServerCredentialsCallback(nil, nil, 0, nil, 0, 0, NPCreateCStringFromNSError(error), tagPtr);
         }
     }];
}

#pragma mark - Notification callback

- (void)authenticationDidChangeNotification:(NSNotification *)notification
{
    // notify listener
    __weak GKLocalPlayer*   localPlayer = [GKLocalPlayer localPlayer];
    GKLocalPlayerAuthState  state       = [NPGameKitManager convertAuthFlagToEnumValue: localPlayer.isAuthenticated];
    _authChangeCallback(state, nil);
}

#pragma mark - GKGameCenterControllerDelegate implementation

- (void)gameCenterViewControllerDidFinish:(nonnull GKGameCenterViewController*)gameCenterViewController
{
    NSLog(@"[NativePlugins] Game center view closed.");
    
    // dimiss view controller
    [UnityGetGLViewController() dismissViewControllerAnimated:YES completion:nil];
    
    // invoke associated event
    id      key     = gameCenterViewController;
    void*   pointer = [[NPManagedPointerCache sharedInstance] pointerForKey:key];
    if (pointer != nil)
    {
        _gameCenterViewClosedCallback(nil, pointer);
    }

    // reset properties
    [[NPManagedPointerCache sharedInstance] removePointerForKey:key];
    self.gameCenterViewController   = nil;
}

#pragma mark - UIPopoverPresentationControllerDelegate implementation

- (void)presentationControllerDidDismiss:(UIPresentationController*)presentationController
{
    GKGameCenterViewController* gcViewController    = (GKGameCenterViewController*)presentationController.presentingViewController;
    [self gameCenterViewControllerDidFinish:gcViewController];
}

@end

#pragma mark - Native binding calls

NPBINDING DONTSTRIP void NPGameServicesSetViewClosedCallback(GameServicesViewClosedNativeCallback callback)
{
    _gameCenterViewClosedCallback   = callback;
}

NPBINDING DONTSTRIP void NPGameServicesLoadServerCredentials(void* tagPtr)
{
    [[NPGameKitManager sharedManager] loadServerCredentials: tagPtr];
}

NPBINDING DONTSTRIP void NPGameServicesLoadServerCredentialsCompleteCallback(GameServicesLoadServerCredentialsNativeCallback callback)
{
    _gameCenterLoadServerCredentialsCallback = callback;
}

//
//  NPStoreReceiptVerificationManager.m
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPStoreReceiptVerificationManager.h"
#import "RMStore.h"
#import "RMStoreAppReceiptVerificator.h"
#import "NPStoreReceiptServerVerificator.h"

// constants
const NSString* bundleIdentifier	= nil;
const NSString* bundleVersion		= nil;

// static properties
static NPStoreReceiptVerificationManager* _sharedInstance = nil;

@interface NPStoreReceiptVerificationManager ()

@property(nonatomic, strong) RMStoreAppReceiptVerificator*      localVerificator;
@property(nonatomic, strong) NPStoreReceiptServerVerificator*   serverVerificator;

@end

@implementation NPStoreReceiptVerificationManager

@synthesize localVerificator    = _localVerificator;
@synthesize serverVerificator   = _serverVerificator;

#pragma mark - Init Methods

- (id)init
{
	self	= [super init];
	if (self)
	{
		// set properties
		self.localVerificator   = [self createAppReceiptVerificator];
        self.serverVerificator  = [[NPStoreReceiptServerVerificator alloc] init];
	}
	
	return self;
}

- (void)dealloc
{ }

#pragma mark - Class methods

+ (NPStoreReceiptVerificationManager*)sharedManager
{
    if (_sharedInstance == nil)
    {
        _sharedInstance = [[NPStoreReceiptVerificationManager alloc] init];
    }
    
    return _sharedInstance;
}

#pragma mark - Public methods

- (void)setServerValidationURL:(NSString*)URLString
{
	[self.serverVerificator setCustomServerURLString:URLString];
}

- (void)setSharedSecretKey:(NSString*)key
{
	[self.serverVerificator setSharedSecretKey:key];
}

- (void)verifyTransaction:(SKPaymentTransaction*)transaction :(void (^)(bool success))completionBlock
{
    // validate arguments
    if (completionBlock == nil)
    {
        NSLog(@"[NativePlugins] Callback not found.");
        return;
    }
    
	// failed transactions are not considered for validation, by default they are considered as success
	if ([transaction transactionState] == SKPaymentTransactionStateFailed)
	{
        completionBlock(true);
		return;
	}
	
	// for successful transactions, execute receipt verification
	[self.localVerificator verifyTransaction:transaction success:^{

        // proceed with server check
        [self.serverVerificator verifyReceiptData:[self.localVerificator getReceiptData]
                                          success:^{
                   completionBlock(true);
               } failure:^(NSError *error) {
                   completionBlock(false);
               }];
    } failure:^(NSError *error) {
        completionBlock(false);
    }];
}

#pragma mark - Private methods

- (RMStoreAppReceiptVerificator*)createAppReceiptVerificator
{
    RMStoreAppReceiptVerificator*   appReceiptVerificator   = [[RMStoreAppReceiptVerificator alloc] init];
    [appReceiptVerificator setBundleIdentifier:[bundleIdentifier copy]];
    [appReceiptVerificator setBundleVersion:[bundleVersion copy]];
    
    return appReceiptVerificator;
}

@end

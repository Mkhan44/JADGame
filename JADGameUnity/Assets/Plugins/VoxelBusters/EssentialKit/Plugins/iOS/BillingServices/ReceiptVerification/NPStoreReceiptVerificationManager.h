//
//  NPStoreReceiptVerificationManager.h
//  Cross Platform Native Plugins
//
//  Created by Ashwin kumar on 17/09/15.
//  Copyright (c) 2015 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>

@interface NPStoreReceiptVerificationManager : NSObject

+ (NPStoreReceiptVerificationManager*)sharedManager;

// set properties
- (void)setServerValidationURL:(NSString*)URLString;
- (void)setSharedSecretKey:(NSString*)newKey;

// verify methods
- (void)verifyTransaction:(SKPaymentTransaction*)transaction :(void (^)(bool success))completionBlock;

@end

//
//  NPStoreKitObserver.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <StoreKit/StoreKit.h>
#import "NPBillingServicesDataTypes.h"

@interface NPStoreKitObserver : NSObject<SKProductsRequestDelegate, SKPaymentTransactionObserver>

// properties
@property(nonatomic) bool               usesReceiptVerification;

// class methods
+ (NPStoreKitObserver*)sharedObserver;

// callbacks
+ (void)setRequestForProductsCallback:(RequestForProductsNativeCallback)callback;
+ (void)setTransactionStateChangeCallback:(TransactionStateChangeNativeCallback)callback;
+ (void)setRestorePurchasesCallback:(RestorePurchasesNativeCallback)callback;

// setup
- (void)requestForBillingProducts:(NSArray<NSString*>*)productIds;
- (SKProduct*)getProductWithId:(NSString*)productId;
- (bool)buyProductWithId:(NSString*)productId quantity:(int)quantity andUsername:(NSString*)username;
- (NPStoreReceiptVerificationState)getReceiptVerificationStateForTransaction:(SKPaymentTransaction*)transaction;
- (bool)finishTransaction:(SKPaymentTransaction*)transaction;
- (NSArray<SKPaymentTransaction*>*)getPendingTransactions;
- (bool)tryClearingUnfinishedTransactions;

- (void)restorePurchasesWithUsername:(NSString*)username;

@end

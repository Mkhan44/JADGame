//
//  NPBillingServicesBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "NPBillingServicesDataTypes.h"
#import "NPDefines.h"
#import "NPStoreKitObserver.h"
#import "NPStoreReceiptVerificationManager.h"

#pragma mark - Native binding methods

NPBINDING DONTSTRIP bool NPBillingServicesCanMakePayments()
{
    return [SKPaymentQueue canMakePayments];
}

NPBINDING DONTSTRIP void NPBillingServicesRegisterCallbacks(RequestForProductsNativeCallback requestForProductsCallback,
                                                            TransactionStateChangeNativeCallback transactionStateChangeCallback,
                                                            RestorePurchasesNativeCallback restorePurchasesCallback)
{
    // set properties
    [NPStoreKitObserver setRequestForProductsCallback:requestForProductsCallback];
    [NPStoreKitObserver setTransactionStateChangeCallback:transactionStateChangeCallback];
    [NPStoreKitObserver setRestorePurchasesCallback:restorePurchasesCallback];
}

NPBINDING DONTSTRIP void NPBillingServicesInit(bool usesReceiptVerification, const char* customReceiptVerificationServerURL)
{
    // update status
    NPStoreKitObserver* sharedObserver  = [NPStoreKitObserver sharedObserver];
    [sharedObserver setUsesReceiptVerification:usesReceiptVerification];
    
    // configure components
    NPStoreReceiptVerificationManager*  verificationManager = [NPStoreReceiptVerificationManager sharedManager];
    [verificationManager setServerValidationURL:NPCreateNSStringFromCString(customReceiptVerificationServerURL)];
    [verificationManager setSharedSecretKey:nil];
}

NPBINDING DONTSTRIP void NPBillingServicesRequestForBillingProducts(const char** productIds, int length)
{
    NSArray<NSString*>* productIdArray  = NPCreateArrayOfNSString(productIds, length);
    [[NPStoreKitObserver sharedObserver] requestForBillingProducts:productIdArray];
}

NPBINDING DONTSTRIP bool NPBillingServicesBuyProduct(const char* productId, int quantity, const char* username)
{
    return [[NPStoreKitObserver sharedObserver] buyProductWithId:NPCreateNSStringFromCString(productId)
                                                        quantity:quantity
                                                     andUsername:NPCreateNSStringFromCString(username)];
}

NPBINDING DONTSTRIP void* NPBillingServicesGetTransactions(int* length)
{
    NSArray<SKPaymentTransaction*>* transactions    = [[NPStoreKitObserver sharedObserver] getPendingTransactions];
    
    // convert native object to blittable type
    return NPCreateTransactionDataArray(transactions, length);
}

NPBINDING DONTSTRIP NPStoreReceiptVerificationState NPBillingServicesGetReceiptVerificationState(void* transactionPtr)
{
    SKPaymentTransaction*   transaction = (__bridge SKPaymentTransaction*)transactionPtr;
    return [[NPStoreKitObserver sharedObserver] getReceiptVerificationStateForTransaction:transaction];
}

NPBINDING DONTSTRIP void NPBillingServicesRestorePurchases(const char* username)
{
    [[NPStoreKitObserver sharedObserver] restorePurchasesWithUsername:NPCreateNSStringFromCString(username)];
}

NPBINDING DONTSTRIP void NPBillingServicesFinishTransactions(void** transactionsPtr, int length)
{
    // create native transaction object array
    for (int iter = 0; iter < length; iter++)
    {
        SKPaymentTransaction*   transaction = (__bridge SKPaymentTransaction*)transactionsPtr[iter];
        [[NPStoreKitObserver sharedObserver] finishTransaction:transaction];
    }
}

NPBINDING DONTSTRIP void NPBillingServicesGetOriginalTransaction(void* transactionPtr, SKPaymentTransactionData* originalTransactionData)
{
    SKPaymentTransaction*       transaction = (__bridge SKPaymentTransaction*)transactionPtr;
    if (transaction && transaction.originalTransaction)
    {
        originalTransactionData->CopyProperties(transaction.originalTransaction);
    }
}

NPBINDING DONTSTRIP bool NPBillingServicesTryClearingUnfinishedTransactions()
{
    return [[NPStoreKitObserver sharedObserver] tryClearingUnfinishedTransactions];
}

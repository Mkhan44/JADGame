//
//  NPBillingServicesDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <StoreKit/StoreKit.h>
#import "NPKit.h"

// callback signatures
typedef void (*RequestForProductsNativeCallback)(void* productsPtr, int length, const char* error, NPArray* invalidProductIds);
typedef void (*TransactionStateChangeNativeCallback)(void* transactionsPtr, int length);
typedef void (*RestorePurchasesNativeCallback)(void* transactionsPtr, int length, const char* error);

// utility methods
void* NPCreateProductsDataArray(NSArray<SKProduct*>* array, int* length);
void* NPCreateTransactionDataArray(NSArray<SKPaymentTransaction*>* array, int* length);
void ReleaseProductsDataArray(void* array, int length);
void ReleaseTransactionsDataArray(void* array, int length);

typedef enum : NSInteger
{
    NPStoreReceiptVerificationStateNotChecked,
    NPStoreReceiptVerificationStateSuccess,
    NPStoreReceiptVerificationStateFailed
} NPStoreReceiptVerificationState;

// blittable product object
struct SKProductData
{
    // properties
    void*               nativeObjectPtr;
    void*               identifierPtr;
    void*               localizedTitlePtr;
    void*               localizedDescriptionPtr;
    double              price;
    void*               localizedPricePtr;
    void*               currencyCodePtr;
    void*               currencySymbolPtr;
    
    // constructors
    ~SKProductData();
    
    // methods
    void CopyProperties(SKProduct* product);
};
typedef struct SKProductData SKProductData;

// blittable payment object
struct SKPaymentData
{
    // properties
    void*               nativeObjectPtr;
    void*               productIdentifierPtr;
    int                 quantity;
    void*               applicationUsernamePtr;

    // constructors
    ~SKPaymentData();
    
    // methods
    void CopyProperties(SKPayment* payment);
};
typedef struct SKPaymentData SKPaymentData;

// blittable transaction object
struct SKPaymentTransactionData
{
    void*                           nativeObjectPtr;
    void*                           identifierPtr;
    SKPaymentData                   paymentData;
    void*                           datePtr;
    int                             transactionState;
    void*                           receiptDataPtr;
    void*                           errorPtr;

    // constructors
    ~SKPaymentTransactionData();

    // methods
    void CopyProperties(SKPaymentTransaction* transaction);
};
typedef struct SKPaymentTransactionData SKPaymentTransactionData;

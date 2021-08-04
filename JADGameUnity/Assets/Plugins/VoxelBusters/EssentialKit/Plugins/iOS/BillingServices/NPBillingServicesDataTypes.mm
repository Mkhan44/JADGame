//
//  NPBillingServicesDataTypes.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPBillingServicesDataTypes.h"
#import "NSData+Base64.h"

void SKProductData::CopyProperties(SKProduct* product)
{
    this->nativeObjectPtr               = (__bridge void*)product;
    this->identifierPtr                 = NPCreateCStringFromNSString(product.productIdentifier);
    this->localizedDescriptionPtr       = NPCreateCStringFromNSString(product.localizedDescription);
    this->localizedTitlePtr             = NPCreateCStringFromNSString(product.localizedTitle);
    this->price                         = [product.price doubleValue];
    
    // get localized price information
    NSNumberFormatter* priceFormatter   = NPGetBillingPriceFormatter(product.priceLocale);
    this->localizedPricePtr             = NPCreateCStringFromNSString([priceFormatter stringFromNumber:product.price]);
    this->currencyCodePtr               = NPCreateCStringFromNSString(priceFormatter.currencyCode);
    this->currencySymbolPtr             = NPCreateCStringFromNSString(priceFormatter.currencySymbol);
}

SKProductData::~SKProductData()
{
    // release c allocations
}

void SKPaymentData::CopyProperties(SKPayment* payment)
{
    this->nativeObjectPtr               = (__bridge void*)payment;
    this->productIdentifierPtr          = NPCreateCStringFromNSString(payment.productIdentifier);
    this->quantity                      = (int)payment.quantity;
    this->applicationUsernamePtr        = NPCreateCStringFromNSString(payment.applicationUsername);
}

SKPaymentData::~SKPaymentData()
{
    // release c allocations
}

void SKPaymentTransactionData::CopyProperties(SKPaymentTransaction* transaction)
{
    // create payment data object
    SKPaymentData*  newPaymentData          = (SKPaymentData*)malloc(sizeof(SKPaymentData));
    newPaymentData->CopyProperties(transaction.payment);
    
    NSData*         receiptData             = [NSData dataWithContentsOfURL:[[NSBundle mainBundle] appStoreReceiptURL]];
    NSString*       receiptDataStr          = nil;
    if (receiptData != nil)
    {
        receiptDataStr                      = [receiptData stringByBase64Encoding];
    }
    
    // copy values
    this->nativeObjectPtr                   = (__bridge void*)transaction;
    this->identifierPtr                     = NPCreateCStringFromNSString(transaction.transactionIdentifier);
    this->paymentData                       = *newPaymentData;
    this->datePtr                           =  NPCreateCStringFromNSString(NPCreateNSStringFromNSDate(transaction.transactionDate));
    this->transactionState                  = (int)transaction.transactionState;
    this->receiptDataPtr                    = NPCreateCStringFromNSString(receiptDataStr);
    this->errorPtr                          = NPCreateCStringFromNSError(transaction.error);
}

SKPaymentTransactionData::~SKPaymentTransactionData()
{
    // release c allocations
    free(&paymentData);
}

#pragma mark - Utility methods

void* NPCreateProductsDataArray(NSArray<SKProduct*>* array, int* length)
{
    if (array)
    {
        // set length
        *length     = (int)[array count];
        
        // create data array
        SKProductData*      newDataArray    = (SKProductData*)calloc(*length, sizeof(SKProductData));
        for (int iter = 0; iter < *length; iter++)
        {
            SKProduct*      product         = [array objectAtIndex:iter];
            SKProductData*  newDataObject   = &newDataArray[iter];
            newDataObject->CopyProperties(product);
        }
        
        return newDataArray;
    }
    else
    {
        *length     = -1;
        
        return nil;
    }
}

void* NPCreateTransactionDataArray(NSArray<SKPaymentTransaction*>* array, int* length)
{
    if (array)
    {
        // set length
        *length     = (int)[array count];
        
        // create data array
        SKPaymentTransactionData*       newDataArray    = (SKPaymentTransactionData*)calloc(*length, sizeof(SKPaymentTransactionData));
        for (int iter = 0; iter < *length ; iter++)
        {
            SKPaymentTransaction*       transaction     = [array objectAtIndex:iter];
            SKPaymentTransactionData*   newDataObject   = &newDataArray[iter];
            newDataObject->CopyProperties(transaction);
        }
        
        return newDataArray;
    }
    else
    {
        *length     = -1;
        
        return nil;
    }
}

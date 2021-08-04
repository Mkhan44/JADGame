//
//  NPStoreKitObserver.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPStoreKitObserver.h"
#import "NPDefines.h"
#import "NPStoreReceiptVerificationManager.h"

typedef enum
{
    TransactionTypePurchase,
    TransactionTypeRestore
} TransactionType;

// static fields
static NPStoreKitObserver*                      _sharedObserver                 = nil;

static RequestForProductsNativeCallback         _requestForProductsCallback     = nil;
static TransactionStateChangeNativeCallback     _transactionStateChangeCallback = nil;
static RestorePurchasesNativeCallback           _restorePurchasesCallback       = nil;

@interface NPStoreKitObserver ()

@property(nonatomic, strong) SKProductsRequest*     activeRequest;
@property(nonatomic, copy) NSArray<SKProduct*>*     activeProducts;
@property(nonatomic) bool                           isRestoringCompletedTransactions;
@property(nonatomic, strong) NSMutableArray<SKPaymentTransaction*>* restoredTransactions;
@property(nonatomic, strong) NSMutableDictionary*   cachedTransactionVerificationResults;
@end

@implementation NPStoreKitObserver

@synthesize usesReceiptVerification                 = _usesReceiptVerification;
@synthesize activeRequest                           = _activeRequest;
@synthesize activeProducts                          = _activeProducts;
@synthesize isRestoringCompletedTransactions        = _isRestoringCompletedTransactions;
@synthesize restoredTransactions                    = _restoredTransactions;
@synthesize cachedTransactionVerificationResults    = _cachedTransactionVerificationResults;

+ (NPStoreKitObserver*)sharedObserver
{
    if (nil == _sharedObserver)
    {
        _sharedObserver         = [[NPStoreKitObserver alloc] init];
    }
    return _sharedObserver;
}

+ (void)setRequestForProductsCallback:(RequestForProductsNativeCallback)callback
{
    _requestForProductsCallback = callback;
}

+ (void)setTransactionStateChangeCallback:(TransactionStateChangeNativeCallback)callback
{
    _transactionStateChangeCallback = callback;
}

+ (void)setRestorePurchasesCallback:(RestorePurchasesNativeCallback)callback
{
    _restorePurchasesCallback   = callback;
}

- (id)init
{
    self = [super init];
    if (self)
    {
        // register as observer
        [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
        
        // create objects
        self.isRestoringCompletedTransactions       = false;
        self.restoredTransactions                   = [NSMutableArray array];
        self.cachedTransactionVerificationResults   = [NSMutableDictionary dictionary];
    }
    
    return self;
}

- (void)dealloc
{
    // deregister as observer
    [[SKPaymentQueue defaultQueue] removeTransactionObserver:self];
}

#pragma mark - Public methods

- (void)requestForBillingProducts:(NSArray<NSString*>*)productIds
{
    // check whether we have any open requests
    if (self.activeRequest)
    {
        NSLog(@"[NativePlugins] Found an open products request, so ignoring this call.");
        return;
    }
    
    // check whether we have product info
    if (!productIds || productIds.count == 0)
    {
        NSLog(@"[NativePlugins] Could not find product id information.");
        return;
    }
    
    // create request
    NSSet<NSString*>*   productIdSet    = [NSSet setWithArray:productIds];
    SKProductsRequest*  newRequest      = [[SKProductsRequest alloc] initWithProductIdentifiers:productIdSet];
    [newRequest setDelegate:self];
    
    // save reference
    self.activeRequest  = newRequest;
    
    // start request
    [self.activeRequest start];
}

- (SKProduct*)getProductWithId:(NSString*)productId
{
    for (SKProduct *product in self.activeProducts)
    {
        if ([product.productIdentifier isEqualToString:productId])
        {
            return product;
        }
    }
    
    return nil;
}

- (bool)buyProductWithId:(NSString*)productId quantity:(int)quantity andUsername:(NSString*)username
{
    // find product with given id
    SKProduct*  product     = [self getProductWithId:productId];
    if (product)
    {
        // create payment object
        SKMutablePayment*   payment     = [SKMutablePayment paymentWithProduct:product];
        payment.quantity                = quantity;
        payment.applicationUsername     = username;
        
        // initiate request
        [[SKPaymentQueue defaultQueue] addPayment:payment];
        return true;
    }
    
    return false;
}

- (NPStoreReceiptVerificationState)getReceiptVerificationStateForTransaction:(SKPaymentTransaction*)transaction
{
    NSNumber*   value   = [self.cachedTransactionVerificationResults objectForKey:[NSValue valueWithNonretainedObject:transaction]];
    return (value) ? (NPStoreReceiptVerificationState)[value intValue] : NPStoreReceiptVerificationStateNotChecked;
}

- (bool)finishTransaction:(SKPaymentTransaction*)transaction
{    
    // check whether transaction state is valid for requested action
    SKPaymentTransactionState   transactionState    = [transaction transactionState];
    if (transactionState == SKPaymentTransactionStatePurchasing ||
        transactionState == SKPaymentTransactionStateDeferred)
    {
        return false;
    }
    
    // reset state associated with specified transaction
    [self.cachedTransactionVerificationResults removeObjectForKey:[NSValue valueWithNonretainedObject:transaction]];
    
    // mark that transaction is processed
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
    
    return true;
}

- (NSArray<SKPaymentTransaction*>*)getPendingTransactions
{
    return [[SKPaymentQueue defaultQueue] transactions];
}

- (bool)tryClearingUnfinishedTransactions
{
    NSArray<SKPaymentTransaction*>*     transactions    = [self getPendingTransactions];
    if ([transactions count] > 0)
    {
        [[NPStoreKitObserver sharedObserver] paymentQueue:[SKPaymentQueue defaultQueue] updatedTransactions:transactions];
        return true;
    }
    
    return false;
}

- (void)restorePurchasesWithUsername:(NSString*)username
{
    // update flag indicator
    self.isRestoringCompletedTransactions   = true;
    
    // initiate restore request
    if (username != nil)
    {
        [[SKPaymentQueue defaultQueue] restoreCompletedTransactionsWithApplicationUsername:username];
    }
    else
    {
        [[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
    }
}

#pragma mark - Private methods

- (void)processCompletedTransactions:(NSArray*)transactions ofType:(TransactionType)type
{
    if ([self usesReceiptVerification] && [transactions count] > 0)
    {
        __block NSArray* array = transactions;
        [self verifyTransactions:array :^{
            [self sendTransactionUpdateEvent:type updatedTransactions:transactions];
        }];
    }
    else
    {
        [self sendTransactionUpdateEvent:type updatedTransactions:transactions];
    }
}

- (SKPaymentTransaction*)findPendingTransactionWithId:(NSString*)transactionId
{
    NSArray<SKPaymentTransaction*>* pendingTransactions = [self getPendingTransactions];
    for (SKPaymentTransaction* transaction in pendingTransactions)
    {
        if ([transaction.transactionIdentifier isEqualToString:transactionId])
        {
            return transaction;
        }
    }
    
    return nil;
}

- (bool)isLoadingProductsInfo
{
    return (self.activeRequest != nil) || (self.activeProducts == nil);
}

- (void)resetProductRequestProperties
{
    [self.activeRequest setDelegate:nil];
    self.activeRequest  = nil;
}

- (void)clearTransactionCache
{
    // reset all the transactions validation state
    [self.cachedTransactionVerificationResults removeAllObjects];
}

#pragma mark - Verification methods

- (void)verifyTransactions:(NSArray*)transactions :(void (^)())completionBlock
{
    // check whether we have any items to process
    __block long    numOfTransactions   = [transactions count];
    if (numOfTransactions == 0)
    {
        if (completionBlock != NULL)
            completionBlock();
        
        return;
    }
    
    // start verifying each transaction in the array
    __block int numOfCompletedTransactions  = 0;
    for (SKPaymentTransaction* transaction in transactions)
    {
        [[NPStoreReceiptVerificationManager sharedManager] verifyTransaction:transaction :^(bool success) {
            NSLog(@"[NativePlugins] Completed receipt verification for product with id %@ status is %d", transaction.payment.productIdentifier, success);
            
            // set state
            NPStoreReceiptVerificationState state = success ? NPStoreReceiptVerificationStateSuccess : NPStoreReceiptVerificationStateFailed;
            [self.cachedTransactionVerificationResults setObject:[NSNumber numberWithLong:state] forKey:[NSValue valueWithNonretainedObject:transaction]];
            
            // check whether all the transactions are processed
            numOfCompletedTransactions++;
            if (numOfCompletedTransactions == numOfTransactions)
            {
                if (completionBlock != nil)
                {
                    completionBlock();
                }
            }
        }];
    }
}

- (void)sendTransactionUpdateEvent:(TransactionType)type updatedTransactions:(NSArray*)transactions
{
    // convert native object to blittable type
    int     cArrayLength;
    void*   cArray          = NPCreateTransactionDataArray(transactions, &cArrayLength);
    
    // send event data
    if (type == TransactionTypePurchase)
    {
        _transactionStateChangeCallback(cArray, cArrayLength);
    }
    else if (type == TransactionTypeRestore)
    {
        _restorePurchasesCallback(cArray, cArrayLength, nil);
    }
    
    // release c array
    free(cArray);
}

#pragma mark - SKProductsRequestDelegate implementation

- (void)productsRequest:(SKProductsRequest*)request didReceiveResponse:(SKProductsResponse*)response
{
    NSLog(@"[NativePlugins] Load store products finished successfully.");
    
    // update info
    self.activeProducts         = response.products;
    [self resetProductRequestProperties];

    // send data to observer
    int         dataArrayLength = 0;
    void*       dataArray       = NPCreateProductsDataArray(response.products, &dataArrayLength);
    NPArray*    invalidIdArray  = NPCreateArrayOfCString(response.invalidProductIdentifiers);
    _requestForProductsCallback(dataArray, dataArrayLength, nil, invalidIdArray);
    
    // release c objects
    free(dataArray);
    delete(invalidIdArray);
}

- (void)request:(SKRequest*)request didFailWithError:(NSError*)error
{
    NSLog(@"[NativePlugins] Load store products failed with error %@.", [error description]);
    
    // update info copy
    self.activeProducts     = nil;
    [self resetProductRequestProperties];

    // send info
    const char* cError      = NPCreateCStringFromNSError(error);
    _requestForProductsCallback(nil, -1, cError, new NPArray(-1));
}

#pragma mark - SKPaymentTransactionObserver implementation

- (void)paymentQueue:(SKPaymentQueue*)queue updatedTransactions:(NSArray<SKPaymentTransaction*>*)transactions
{
    // check whether we are good to receive transaction information
    if ([self isLoadingProductsInfo])
    {
        NSLog(@"[NativePlugins] Ignoring transaction state change callback.");
        return;
    }
    
    // reset properties
    [self.restoredTransactions removeAllObjects];
    
    // filter transactions based on state
    NSLog(@"[NativePlugins] Processing transaction change callback.");
    
    bool                                    hasPurchasingTransactions   = false;
    NSMutableArray<SKPaymentTransaction*>*  incompleteTransactions      = [NSMutableArray arrayWithCapacity:4];
    NSMutableArray<SKPaymentTransaction*>*  finishedTransactions        = [NSMutableArray arrayWithCapacity:4];
    for (SKPaymentTransaction* transaction in queue.transactions)
    {
        switch (transaction.transactionState)
        {
            case SKPaymentTransactionStatePurchasing:
                hasPurchasingTransactions   = true;
                [incompleteTransactions addObject:transaction];
                break;
                
            case SKPaymentTransactionStateDeferred:
                [incompleteTransactions addObject:transaction];
                break;
                
            case SKPaymentTransactionStatePurchased:
            case SKPaymentTransactionStateFailed:
                [finishedTransactions addObject:transaction];
                break;
                
            case SKPaymentTransactionStateRestored:
                [self.restoredTransactions addObject:transaction];
                break;
                
            default:
                NSLog(@"Unhandled transaction detected. Transaction: %@", transaction);
                break;
        }
    }
    
    // note that transaction queue can contain zombie objects and our system will try to process them along with finished transactions
    if ([incompleteTransactions count] > 0)
    {
        [self sendTransactionUpdateEvent:TransactionTypePurchase updatedTransactions:incompleteTransactions];
    }
    if (!hasPurchasingTransactions)
    {
        if ([finishedTransactions count] > 0)
        {
            [self processCompletedTransactions:finishedTransactions ofType:TransactionTypePurchase];
        }
        if (!self.isRestoringCompletedTransactions && [self.restoredTransactions count] > 0)
        {
            [self handleRestoredTransactions];
        }
    }
}

- (void)paymentQueue:(SKPaymentQueue*)queue removedTransactions:(NSArray<SKPaymentTransaction*>*)transactions
{
    for (SKPaymentTransaction* transaction in transactions)
    {
        NSLog(@"[NativePlugins] Removing transactions with id %@.", transaction.transactionIdentifier);
    }
}

- (void)paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue*)queue
{
    NSLog(@"[NativePlugins] Restore purchases finished");
    
    [self handleRestoredTransactions];
}

- (void)paymentQueue:(SKPaymentQueue*)queue restoreCompletedTransactionsFailedWithError:(NSError*)error
{
    NSLog(@"[NativePlugins] Restore purchases failed with error %@.", [error description]);
    
    // reset state
    self.isRestoringCompletedTransactions   = false;
    
    // invoke event
    const char* cError      = NPCreateCStringFromNSError(error);
    _restorePurchasesCallback(nil, -1, cError);
}

- (BOOL)paymentQueue:(SKPaymentQueue*)queue shouldAddStorePayment:(SKPayment*)payment forProduct:(SKProduct*)product
{
    return YES;
}

- (void)handleRestoredTransactions
{
    NSArray* restoreTransactionsCopy    = [self.restoredTransactions copy];
        
    // reset state related to restored transactions
    self.isRestoringCompletedTransactions   = false;
    [self.restoredTransactions removeAllObjects];
    
    // process transactions
    [self processCompletedTransactions:restoreTransactionsCopy ofType:TransactionTypeRestore];
}

@end

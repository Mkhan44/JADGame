//
//  NPCloudServicesDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <CloudKit/CloudKit.h>
#import "NPKit.h"

#pragma mark - Custom definitions

// custom datatypes
struct CKAccountData
{
    // variables
    void*   accountIdentifierPtr;
    
    // destructors
    CKAccountData(NSString* identityToken);
    ~CKAccountData();
    
    CKAccountData(CKAccountData const& other);
};
typedef CKAccountData CKAccountData;

#pragma mark - Callback definitions

// callback signatures
typedef void (*UserChangeNativeCallback)(CKAccountData* accountData, const char* error);
typedef void (*SavedDataChangeNativeCallback)(int changeReason, NPArray* changedKeys);

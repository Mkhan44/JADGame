//
//  NPCloudServicesDataTypes.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPCloudServicesDataTypes.h"

CKAccountData::CKAccountData(NSString* identityToken)
{
    this->accountIdentifierPtr   = NPCreateCStringFromNSString(identityToken);
}

CKAccountData::~CKAccountData()
{
    // release c objects
    NSLog(@"Destructor call.");
}

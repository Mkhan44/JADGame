//
//  NPDeepLinkServicesBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "NPUnityAppController.h"
#import "NPKit.h"

#pragma mark - Native binding methods

NPBINDING DONTSTRIP void NPDeepLinkServicesRegisterCallbacks(HandleUrlSchemeCallback handleUrlSchemeCallback, HandleUniversalLinkCallback handleUniversalLinkCallback)
{
    [NPUnityAppController registerUrlSchemeHandler:handleUrlSchemeCallback];
    [NPUnityAppController registerUniversalLinkHandler:handleUniversalLinkCallback];
}

NPBINDING DONTSTRIP void NPDeepLinkServicesInit()
{
    NPUnityAppController*   appController   = (NPUnityAppController*)GetAppController();
    [appController initDeepLinkServices];
}

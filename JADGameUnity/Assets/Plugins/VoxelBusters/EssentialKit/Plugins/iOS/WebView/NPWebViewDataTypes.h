//
//  NPWebViewDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPDefines.h"

// callback signatures
typedef void (*WebViewNativeCallback)(IntPtr nativePtr, const char* error);

typedef void (*WebViewRunJavaScriptNativeCallback)(IntPtr nativePtr, const char* result, const char* error, IntPtr tagPtr);

typedef void (*WebViewURLSchemeMatchFoundNativeCallback)(IntPtr nativePtr, const char* url);

//
//  NPWebViewBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "NPWebView.h"
#import "NPKit.h"

NPBINDING DONTSTRIP void NPWebViewRegisterCallbacks(WebViewNativeCallback onShowCallback,
                                                    WebViewNativeCallback onHideCallback,
                                                    WebViewNativeCallback loadStartCallback,
                                                    WebViewNativeCallback loadFinishCallback,
                                                    WebViewRunJavaScriptNativeCallback runJavaScriptFinishCallback,
                                                    WebViewURLSchemeMatchFoundNativeCallback urlMatchFoundCallback)
{
    // set callbacks
    [NPWebView setOnShowCallback:onShowCallback];
    [NPWebView setOnHideCallback:onHideCallback];
    [NPWebView setLoadStartCallback:loadStartCallback];
    [NPWebView setLoadFinishCallback:loadFinishCallback];
    [NPWebView setRunJavaScriptFinishCallback:runJavaScriptFinishCallback];
    [NPWebView setURLSchemeMatchFoundCallback:urlMatchFoundCallback];
}

NPBINDING DONTSTRIP IntPtr NPWebViewCreate()
{
    NPWebView*  webView     = [[NPWebView alloc] initWithFrame:CGRectMake(0, 0, 100, 100)];
    [webView setHidden:YES];
    return NPRetainWithOwnershipTransfer(webView);
}

NPBINDING DONTSTRIP const char* NPWebViewGetURL(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    NSString*   urlString   = [webView urlString];
    return NPCreateCStringCopyFromNSString(urlString);
}

NPBINDING DONTSTRIP const char* NPWebViewGetTitle(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return NPCreateCStringCopyFromNSString([webView title]);
}

NPBINDING DONTSTRIP NPUnityRect NPWebViewGetNormalizedRect(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    CGRect      cgRect      = [webView normalisedRect];
    
    // create new rect
    NPUnityRect rect        = NPRectMake(cgRect);
    return rect;
}

NPBINDING DONTSTRIP void NPWebViewSetNormalizedRect(IntPtr nativePtr, NPUnityRect rect)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView setNormalisedRect:CGRectMake(rect.x, rect.y, rect.width, rect.height)];
}

NPBINDING DONTSTRIP NPWebViewStyle NPWebViewGetStyle(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView style];
}

NPBINDING DONTSTRIP void NPWebViewSetStyle(IntPtr nativePtr, NPWebViewStyle style)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView setStyle:style];
}

NPBINDING DONTSTRIP bool NPWebViewGetScalesPageToFit(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView scalesPageToFit];
}

NPBINDING DONTSTRIP void NPWebViewSetScalesPageToFit(IntPtr nativePtr, bool value)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView setScalesPageToFit:value];
}

NPBINDING DONTSTRIP bool NPWebViewGetCanBounce(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView canBounce];
}

NPBINDING DONTSTRIP void NPWebViewSetCanBounce(IntPtr nativePtr, bool value)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView setCanBounce:value];
}

NPBINDING DONTSTRIP NPUnityColor NPWebViewGetBackgroundColor(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return NPColorCreateFromUIColor([webView color]);
}

NPBINDING DONTSTRIP void NPWebViewSetBackgroundColor(IntPtr nativePtr, NPUnityColor color)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView setColor:[UIColor colorWithRed:color.r green:color.g blue:color.b alpha:color.a]];
}

NPBINDING DONTSTRIP double NPWebViewGetProgress(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView progress];
}

NPBINDING DONTSTRIP bool NPWebViewGetIsLoading(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView isLoading];
}

NPBINDING DONTSTRIP bool NPWebViewGetJavaScriptEnabled(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView javaScriptEnabled];
}

NPBINDING DONTSTRIP void NPWebViewSetJavaScriptEnabled(IntPtr nativePtr, bool enabled)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    return [webView setJavaScriptEnabled:enabled];
}

NPBINDING DONTSTRIP void NPWebViewShow(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView showInView:UnityGetGLView()];
}

NPBINDING DONTSTRIP void NPWebViewHide(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView dismiss];
}

NPBINDING DONTSTRIP void NPWebViewLoadURL(IntPtr nativePtr, const char* urlStr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView loadURL:NPCreateNSURLFromCString(urlStr)];
}

NPBINDING DONTSTRIP void NPWebViewLoadHtmlString(IntPtr nativePtr, const char* htmlStr, const char* baseUrl)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView loadHTMLString:NPCreateNSStringFromCString(htmlStr) baseURL:NPCreateNSURLFromCString(baseUrl)];
}

NPBINDING DONTSTRIP void NPWebViewLoadData(IntPtr nativePtr, NPUnityAttachment attachmentData, const char* encodingName, const char* baseUrl)
{
    NSData*     data        = [NSData dataWithBytes:attachmentData.dataArrayPtr length:attachmentData.dataArrayLength];
    NSString*   mimeType    = NPCreateNSStringFromCString((const char*)attachmentData.mimeTypePtr);
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView loadData:data
             MIMEType:mimeType
characterEncodingName:NPGetTextEncodingName(NPCreateNSStringFromCString(encodingName))
              baseURL:NPCreateNSURLFromCString(baseUrl)];
}

NPBINDING DONTSTRIP void NPWebViewReload(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView reload];
}

NPBINDING DONTSTRIP void NPWebViewStopLoading(IntPtr nativePtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView stopLoading];
}

NPBINDING DONTSTRIP void NPWebViewRunJavaScript(IntPtr nativePtr, const char* script, IntPtr tagPtr)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView runJavaScript:NPCreateNSStringFromCString(script) withRequestTag:tagPtr];
}

NPBINDING DONTSTRIP void NPWebViewAddURLScheme(IntPtr nativePtr, const char* scheme)
{
    NPWebView*  webView     = (__bridge NPWebView*)nativePtr;
    [webView addURLScheme:NPCreateNSStringFromCString(scheme)];
}

// reference: https://stackoverflow.com/questions/27105094/how-to-remove-cache-in-wkwebview
NPBINDING DONTSTRIP void NPWebViewClearCache()
{
    NSSet*  websiteDataTypes    = [NSSet setWithArray:@[WKWebsiteDataTypeDiskCache,
                                                        WKWebsiteDataTypeMemoryCache]];
    NSDate* dateFrom            = [NSDate dateWithTimeIntervalSince1970:0];
    [[WKWebsiteDataStore defaultDataStore] removeDataOfTypes:websiteDataTypes modifiedSince:dateFrom completionHandler:^{
        // done
    }];
}

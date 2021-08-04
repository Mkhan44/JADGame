//
//  NPWebView.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>
#import "NPWebViewTabBar.h"
#import "NPWebViewDataTypes.h"

typedef enum : int
{
    WebViewStyleNone,
    WebViewStylePopup,
    WebViewStyleBrowser,
} NPWebViewStyle;

@interface NPWebView : UIView<NPWebViewTabBarDelegate, WKUIDelegate, WKNavigationDelegate, UIScrollViewDelegate>

// Properties
@property(nonatomic) bool               canDismiss;
@property(nonatomic) NPWebViewStyle     style;
@property(nonatomic) bool               showSpinnerOnLoad;
@property(nonatomic) CGRect             normalisedRect;

// static callback methods
+ (void)setOnShowCallback:(nonnull WebViewNativeCallback)callback;
+ (void)setOnHideCallback:(nonnull WebViewNativeCallback)callback;
+ (void)setLoadStartCallback:(nonnull WebViewNativeCallback)callback;
+ (void)setLoadFinishCallback:(nonnull WebViewNativeCallback)callback;
+ (void)setRunJavaScriptFinishCallback:(nonnull WebViewRunJavaScriptNativeCallback)callback;
+ (void)setURLSchemeMatchFoundCallback:(nonnull WebViewURLSchemeMatchFoundNativeCallback)callback;

// init
- (id)initWithFrame:(CGRect)frame;

// additional properties
- (bool)isLoading;
- (double)progress;
- (NSString*)urlString;
- (NSString*)title;
- (void)setCanBounce:(bool)canBounce;
- (bool)canBounce;
- (void)setColor:(UIColor*)color;
- (UIColor*)color;
- (void)setScalesPageToFit:(bool)scalesPageToFit;
- (bool)scalesPageToFit;
- (void)setAllowsInlineMediaPlayback:(bool)allowsInlineMediaPlayback;
- (bool)allowsInlineMediaPlayback;
- (void)setJavaScriptEnabled:(bool)javaScriptEnabled;
- (bool)javaScriptEnabled;

// presentation
- (void)showInView:(nonnull UIView*)view;
- (void)dismiss;
- (BOOL)isShowing;

// load requests
- (void)loadURL:(nonnull NSURL*)url;
- (void)loadHTMLString:(nonnull NSString*)htmlString baseURL:(nullable NSURL*)baseURL;
- (void)loadData:(nonnull NSData*)data MIMEType:(nonnull NSString*)MIMEType characterEncodingName:(nonnull NSString*)characterEncodingName baseURL:(nonnull NSURL*)baseURL;
- (void)runJavaScript:(nonnull NSString*)script withRequestTag:(nonnull void*)tagPtr;
- (void)reload;
- (void)stopLoading;

// url scheme
- (void)addURLScheme:(nonnull NSString*)scheme;
- (BOOL)shouldStartLoadRequestWithURLScheme:(nonnull NSString*)URLScheme;

@end

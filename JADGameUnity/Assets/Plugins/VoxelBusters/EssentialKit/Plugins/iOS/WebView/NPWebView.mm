//
//  NPWebView.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPWebView.h"
#import "NPKit.h"
#import "UIView+LayoutConstraints.h"

// constants
// reference: https://github.com/mopub/mopub-ios-sdk/blob/master/MoPubSDK/Internal/HTML/MPWebView.m
static NSString *const kMoPubScalesPageToFitScript = @"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=device-width, initial-scale=1.0, user-scalable=no'); document.getElementsByTagName('head')[0].appendChild(meta);";

#define kTabBarHeight  44

// native callbacks
static  WebViewNativeCallback                       _onShowCallback                 = nil;
static  WebViewNativeCallback                       _onHideCallback                 = nil;
static  WebViewNativeCallback                       _loadStartCallback              = nil;
static  WebViewNativeCallback                       _loadFinishCallback             = nil;
static  WebViewRunJavaScriptNativeCallback          _runJavaScriptCallback          = nil;
static  WebViewURLSchemeMatchFoundNativeCallback    _urlSchemeMatchFoundCallback    = nil;

@interface NPWebView ()

@property(nonatomic, strong) WKWebView*                     webView;
@property(nonatomic, strong) NPWebViewTabBar*               tabBar;
@property(nonatomic, strong) UIButton*                      closeButton;
@property(nonatomic, strong) NSMutableArray<NSString*>*     urlSchemes;
          
@end

@implementation NPWebView

@synthesize canDismiss              = _canDismiss;
@synthesize style                   = _style;
@synthesize showSpinnerOnLoad       = _showSpinnerOnLoad;
@synthesize normalisedRect          = _normalisedRect;

@synthesize webView;
@synthesize tabBar;
@synthesize closeButton;
@synthesize urlSchemes;

#pragma mark - Static methods

+(void)setOnShowCallback:(WebViewNativeCallback)callback
{
    _onShowCallback     = callback;
}

+(void)setOnHideCallback:(WebViewNativeCallback)callback
{
    _onHideCallback     = callback;
}

+(void)setLoadStartCallback:(WebViewNativeCallback)callback
{
    _loadStartCallback  = callback;
}

+(void)setLoadFinishCallback:(WebViewNativeCallback)callback
{
    _loadFinishCallback = callback;
}

+(void)setRunJavaScriptFinishCallback:(WebViewRunJavaScriptNativeCallback)callback
{
    _runJavaScriptCallback      = callback;
}

+(void)setURLSchemeMatchFoundCallback:(WebViewURLSchemeMatchFoundNativeCallback)callback
{
    _urlSchemeMatchFoundCallback = callback;
}

#pragma mark - Instance methods

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self)
    {
        // set default properties
        self.urlSchemes     = [NSMutableArray array];
        self.translatesAutoresizingMaskIntoConstraints  = NO;

        // create subview components
        self.webView        = [self createWebView];
        self.tabBar         = [self createToolbar];
        self.closeButton    = [self createCloseButton];
        
        // add subviews
        [self addSubview:self.webView];
        [self addSubview:self.tabBar];
        [self addSubview:self.closeButton];
        [self addConstraintsToSubViews];
        
        [self setBackgroundColor:[UIColor blackColor]];
    }
    
    return self;
}

- (void)dealloc
{
    // reset webview properties
    [self.webView setUIDelegate:nil];
    [self.webView setNavigationDelegate:nil];
    [self.webView stopLoading];
}

- (WKWebView*)createWebView
{
    // create configuration object
    WKWebViewConfiguration* webConfig   = [[WKWebViewConfiguration alloc] init];
    [webConfig setMediaTypesRequiringUserActionForPlayback:WKAudiovisualMediaTypeAll];
    [webConfig setAllowsInlineMediaPlayback:YES];

    // setup webview
    WKWebView*  webView         = [[WKWebView alloc] initWithFrame:CGRectZero configuration:webConfig];
    webView.UIDelegate          = self;
    webView.navigationDelegate  = self;
    webView.opaque              = false;
    [webView setTranslatesAutoresizingMaskIntoConstraints: NO];
    
    return webView;
}

- (NPWebViewTabBar*)createToolbar
{
    // setup toolbar
    NPWebViewTabBar*    tabBar  = [[NPWebViewTabBar alloc] init];
    [tabBar setDelegate:self];
    [tabBar setTranslatesAutoresizingMaskIntoConstraints: NO];

    return tabBar;
}

- (UIButton*)createCloseButton
{
    UIImage*    closeBtnImage   = LoadImageFromResources(@"webview_close_button_custom", @"png");
    if (!closeBtnImage)
    {
        closeBtnImage           = LoadImageFromResources(@"webview_close_button", @"png");
    }
    
    // create button
    UIButton*   closeButton     = [UIButton buttonWithType:UIButtonTypeCustom];
    [closeButton setFrame:CGRectMake(0, 0, closeBtnImage.size.width, closeBtnImage.size.height)];
    [closeButton setImage:closeBtnImage forState:UIControlStateNormal];
    [closeButton addTarget:self action:@selector(onCloseButtonPress:) forControlEvents:UIControlEventTouchUpInside];
    [closeButton setTranslatesAutoresizingMaskIntoConstraints: NO];

    return closeButton;
}

- (void)addConstraintsToSubViews
{
    // close button section
    __weak UIButton*        closeButton = self.closeButton;
    [closeButton removeExistingConstraints];
    
    [closeButton.topAnchor constraintEqualToAnchor:self.topAnchor constant:closeButton.frame.size.height/2].active = YES;
    
    if (@available(iOS 11.0, *)) {
        [closeButton.trailingAnchor constraintEqualToAnchor:self.safeAreaLayoutGuide.trailingAnchor constant: -closeButton.frame.size.width/2].active   = YES;
    } else {
        // Fallback on earlier versions
        [closeButton.trailingAnchor constraintEqualToAnchor:self.trailingAnchor].active   = YES;
    }

    // tabbar section
    __weak NPWebViewTabBar* tabBar      = self.tabBar;
    [webView removeExistingConstraints];
    [tabBar.widthAnchor constraintEqualToAnchor:self.widthAnchor].active        = YES;
    [tabBar.bottomAnchor constraintEqualToAnchor:self.bottomAnchor].active = YES;
    [tabBar.centerXAnchor constraintEqualToAnchor:self.centerXAnchor].active = YES;
    
    if (@available(iOS 11.0, *)) {
        [tabBar.topAnchor constraintEqualToAnchor:self.safeAreaLayoutGuide.bottomAnchor constant:-kTabBarHeight].active = YES;
    }
    else
    {
        [tabBar.topAnchor constraintEqualToAnchor:self.bottomAnchor].active = YES;
    }
    
    [self updateWebViewConstraints];
}

- (void)updateWebViewConstraints
{
    // tabbar section
    __weak WKWebView*   webView      = self.webView;
    [webView removeExistingConstraints];

    float   bottomPadding   = (self.style == WebViewStyleBrowser) ? -kTabBarHeight : 0;
    [webView.widthAnchor constraintEqualToAnchor:self.widthAnchor].active  = YES;
    [webView.centerXAnchor constraintEqualToAnchor:self.centerXAnchor].active = YES;
    [webView.topAnchor constraintEqualToAnchor:self.topAnchor].active = YES;
    if(self.style == WebViewStyleBrowser)
    {
        NSLayoutYAxisAnchor* referenceAnchor = nil;
        
        if (@available(iOS 11.0, *))
        {
            referenceAnchor = self.safeAreaLayoutGuide.bottomAnchor;
        }
        else
        {
            // Fallback on earlier versions
            referenceAnchor = self.bottomAnchor;
            
        }
        
        [webView.bottomAnchor constraintEqualToAnchor:referenceAnchor constant:bottomPadding].active   = YES;
    }
    else
    {
        [webView.heightAnchor constraintEqualToAnchor:self.heightAnchor].active   = YES;
    }
}

- (void)updateViewConstraints:(UIView*)parentView
{
    // prepare to set constraints
    __weak NPWebView*   view= self;
    [view removeExistingConstraints];
    
    // set new constraints
    [NSLayoutConstraint constraintWithItem: view attribute: NSLayoutAttributeTop
    relatedBy: NSLayoutRelationEqual toItem: parentView
    attribute: NSLayoutAttributeTop multiplier: 1 constant: CGRectGetMinY(_normalisedRect) * parentView.bounds.size.height].active = YES;
    
    [NSLayoutConstraint constraintWithItem: view attribute: NSLayoutAttributeLeft
    relatedBy: NSLayoutRelationEqual toItem: parentView
    attribute: NSLayoutAttributeLeft multiplier: 1 constant: CGRectGetMinX(_normalisedRect) * parentView.bounds.size.width].active = YES;
    
    
    [view.widthAnchor constraintEqualToAnchor:parentView.widthAnchor multiplier:CGRectGetWidth(_normalisedRect)].active     = YES;
    [view.heightAnchor constraintEqualToAnchor:parentView.heightAnchor multiplier:CGRectGetHeight(_normalisedRect)].active  = YES;
}

#pragma mark - Properties

- (void)setCanDismiss:(BOOL)canDismiss
{
    _canDismiss     = canDismiss;
    
    // update button state
    [[self closeButton] setEnabled:canDismiss];
    [[self tabBar] setCanStop:canDismiss];
}

- (void)setStyle:(NPWebViewStyle)style
{
    _style          = style;
    
    // by default toolbar and close button are hidden
    NPWebViewTabBar*    tabBar      = self.tabBar;
    tabBar.hidden                   = (style != WebViewStyleBrowser);
    
    UIButton*   closeButton         = self.closeButton;
    closeButton.hidden              = (style != WebViewStylePopup);;

    // update constraints
    [self updateWebViewConstraints];
}

- (void)setShowSpinnerOnLoad:(BOOL)showSpinnerOnLoad
{
    _showSpinnerOnLoad  = showSpinnerOnLoad;
    
    // show activity indicator if webview is loading
    if ([[self webView] isLoading] && showSpinnerOnLoad)
    {
        [self setLoadingSpinnerVisible:YES];
    }
    else
    {
        [self setLoadingSpinnerVisible:NO];
    }
}

- (void)setNormalisedRect:(CGRect)normalisedRect
{
    NSLog(@"Rect %@", NSStringFromCGRect(normalisedRect));
    _normalisedRect = normalisedRect;
    
    // check whether view is visible
    if (!self.hidden)
    {
        [self updateViewConstraints:self.superview];
    }
}

- (bool)isLoading
{
    return [[self webView] isLoading];
}

- (double)progress
{
    return [[self webView] estimatedProgress];
}

- (NSString*)urlString
{
    NSURL*  url = [[self webView] URL];
    return url ? [url absoluteString] : nil;
}

- (NSString*)title
{
    return [[self webView] title];
}

- (void)setCanBounce:(bool)canBounce
{
    // update webview property
    [[[self webView] scrollView] setBounces:canBounce];
}

- (bool)canBounce
{
    return [[[self webView] scrollView] bounces];
}

- (void)setColor:(UIColor*)color
{
    [super setBackgroundColor:[UIColor blackColor]];
    
    // use same value for webview
    [[self webView] setBackgroundColor:color];
}

- (UIColor*)color
{
    return [[self webView] backgroundColor];
}

- (bool)scalesPageToFit
{
    return ([[[self webView] scrollView] delegate] != NULL);
}

- (void)setScalesPageToFit:(bool)scalesPageToFit
{
    // reference: https://github.com/mopub/mopub-ios-sdk/blob/master/MoPubSDK/Internal/HTML/MPWebView.m
    WKWebView*  webView  = [self webView];
    if (!scalesPageToFit)
    {
        [[webView scrollView] setDelegate: nil];
        [[[webView configuration] userContentController] removeAllUserScripts];
    }
    else
    {
        // make sure the scroll view can't scroll (prevent double tap to zoom)
        [[webView scrollView] setDelegate: self];
        
        // inject the user script to scale the page if needed
        if (webView.configuration.userContentController.userScripts.count == 0)
        {
            WKUserScript*   viewportScript = [[WKUserScript alloc] initWithSource:kMoPubScalesPageToFitScript
                                                                    injectionTime:WKUserScriptInjectionTimeAtDocumentEnd
                                                                 forMainFrameOnly:YES];
            [[[webView configuration] userContentController] addUserScript:viewportScript];
        }
    }
}

- (void)setAllowsInlineMediaPlayback:(bool)allowsInlineMediaPlayback
{
    [[[self webView] configuration] setAllowsInlineMediaPlayback:allowsInlineMediaPlayback];
}

- (bool)allowsInlineMediaPlayback
{
    return  [[[self webView] configuration] allowsInlineMediaPlayback];
}

- (void)setJavaScriptEnabled:(bool)javaScriptEnabled
{
    WKWebViewConfiguration* webConfig   = [[self webView] configuration];
    [[webConfig preferences] setJavaScriptEnabled:javaScriptEnabled];
}

- (bool)javaScriptEnabled
{
    WKWebViewConfiguration* webConfig   = [[self webView] configuration];
    return [[webConfig preferences] javaScriptEnabled];
}

#pragma mark - Frame size

- (void)showInView:(UIView*)view
{
    // check whether view is hidden
    if (!self.hidden)
    {
        return;
    }
    
    // update visiblity
    [self setHidden:NO];
    
    // attach it to view
    [view addSubview:self];
    [view bringSubviewToFront:self];
    [self updateViewConstraints:view];
    
    // send callback
    _onShowCallback((__bridge void*)self, nil);
}

- (void)dismiss
{
    // update property value
    [self setHidden:YES];
    [self removeFromSuperview];

    // stop request
    [self stopLoading];
    
    // send callback
    _onHideCallback((__bridge void*)self, nil);
}

- (BOOL)isShowing
{
    return ![self isHidden];
}

#pragma mark - Load methods

- (void)loadURL:(NSURL*)url
{
    NSURLRequest*   request = [NSURLRequest requestWithURL:url];
    [[self webView] loadRequest:request];
}

- (void)loadHTMLString:(NSString*)htmlString baseURL:(NSURL*)baseURL
{
    [[self webView] loadHTMLString:htmlString baseURL:baseURL];
}

- (void)loadData:(NSData*)data MIMEType:(NSString*)MIMEType characterEncodingName:(NSString*)characterEncodingName baseURL:(NSURL*)baseURL
{
    [[self webView] loadData:data MIMEType:MIMEType characterEncodingName:characterEncodingName baseURL:baseURL];
}

- (void)runJavaScript:(NSString*)script withRequestTag:(void*)tagPtr
{
    [[self webView] evaluateJavaScript:script completionHandler:^(id _Nullable result, NSError* _Nullable error) {
        
        // send callback
        if (!error)
        {
            NSString*   resultJSONStr   = [NSString stringWithFormat:@"%@", result];
            _runJavaScriptCallback((__bridge void*)self, NPCreateCStringFromNSString(resultJSONStr), nil, tagPtr);
        }
        else
        {
            _runJavaScriptCallback((__bridge void*)self, nil, NPCreateCStringFromNSError(error), tagPtr);
        }
    }];
}

- (void)reload
{
    [[self webView] reload];
}

- (void)stopLoading
{
    // stops loading webview
    [[self webView] stopLoading];
    
    // hide activity spinner views
    [self setLoadingSpinnerVisible:NO];
}

#pragma mark - URL Scheme

- (void)addURLScheme:(NSString*)scheme
{
    [self.urlSchemes addObject:scheme];
}

- (BOOL)shouldStartLoadRequestWithURLScheme:(NSString*)URLScheme
{
    return NO;
}

#pragma mark - Callbacks

- (void)didFindMatchingURLSchemeForURL:(NSURL*)requestURL
{
    NSLog(@"[NativePlugins] Found matching URL scheme: %@.", [requestURL scheme]);
    _urlSchemeMatchFoundCallback((__bridge void*)self, NPCreateCStringFromNSString([requestURL absoluteString]));
}

#pragma mark - NPWebViewTabBarDelegate implementation

- (void)didSelectGoBackOnTabBar:(NPWebViewTabBar*)toolbar
{
    NSLog(@"[NativePlugins] User pressed go back.");
    [[self webView] goBack];
}

- (void)didSelectGoForwardOnTabBar:(NPWebViewTabBar*)toolbar
{
    NSLog(@"[NativePlugins] User pressed go forward.");
    [[self webView] goForward];
}

- (void)didSelectStopOnTabBar:(NPWebViewTabBar*)toolbar
{
    NSLog(@"[NativePlugins] User pressed stop.");
    [self dismiss];
}

- (void)didSelectReloadOnTabBar:(NPWebViewTabBar*)toolbar
{
    NSLog(@"[NativePlugins] User pressed reload.");
    [self reload];
}

#pragma mark - Button callback methods

- (void)onCloseButtonPress:(id)sender
{
    [self dismiss];
}

#pragma mark - Misc methods

- (void)setLoadingSpinnerVisible:(BOOL)visible
{
    [UIApplication sharedApplication].networkActivityIndicatorVisible   = visible;
}

- (void)refreshScreenBasedOnWebViewState
{
    WKWebView*          webView = [self webView];
    
    // update toolbar state
    NPWebViewTabBar*    tabBar  = [self tabBar];
    [tabBar setCanGoBack:webView.canGoBack];
    [tabBar setCanGoForward:webView.canGoForward];
    
    // update indictator state
    [self setLoadingSpinnerVisible:([webView isLoading] && self.showSpinnerOnLoad)];
}

#pragma mark - WKNavigationDelegate Methods

- (void)webView:(WKWebView*)webView didStartProvisionalNavigation:(null_unspecified WKNavigation*)navigation
{
    NSLog(@"[NativePlugins] Webview started load request.");
    [self refreshScreenBasedOnWebViewState];
    
    // send callback
    _loadStartCallback((__bridge void*)self, nil);
}

- (void)webView:(WKWebView*)webView didFinishNavigation:(null_unspecified WKNavigation*)navigation
{
    NSLog(@"[NativePlugins] Webview finished loading.");
    [self refreshScreenBasedOnWebViewState];
    
    // send callback
    _loadFinishCallback((__bridge void*)self, nil);
}

- (void)webView:(WKWebView*)webView didFailNavigation:(null_unspecified WKNavigation*)navigation withError:(NSError*)error
{
    NSLog(@"[NativePlugins] Webview failed to load.");
    [self refreshScreenBasedOnWebViewState];
    
    // send callback
    _loadFinishCallback((__bridge void*)self, NPCreateCStringFromNSError(error));
}

- (void)webView:(WKWebView*)webView decidePolicyForNavigationAction:(WKNavigationAction*)navigationAction decisionHandler:(void (^)(WKNavigationActionPolicy))decisionHandler
{
    NSURL       *requestURL         = [[navigationAction request] URL];
    NSString    *currentURLScheme   = [requestURL scheme];
    
    if ([self.urlSchemes indexOfObject:currentURLScheme] != NSNotFound)
    {
        // notify observers about matching url scheme
        [self didFindMatchingURLSchemeForURL:requestURL];
        
        // check if we need to continue with load request
        if (![self shouldStartLoadRequestWithURLScheme:currentURLScheme])
        {
            decisionHandler(WKNavigationActionPolicyCancel);
            return;
        }
    }
    
    decisionHandler(WKNavigationActionPolicyAllow);
}

#pragma mark - UIScrollViewDelegate Methods

// reference: https://github.com/mopub/mopub-ios-sdk/blob/master/MoPubSDK/Internal/HTML/MPWebView.m
// avoid double tap to zoom in WKWebView
// delegate is only set when scalesPagesToFit is set to NO
- (UIView*)viewForZoomingInScrollView:(UIScrollView*)scrollView
{
    return nil;
}

@end

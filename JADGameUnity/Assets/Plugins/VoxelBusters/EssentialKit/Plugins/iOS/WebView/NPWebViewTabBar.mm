//
//  NPWebViewTabBar.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPWebViewTabBar.h"
#import "UIView+LayoutConstraints.h"

@interface NPWebViewTabBar ()

// properties
@property(nonatomic, strong) UIToolbar*         toolBar;
@property(nonatomic, strong) UIBarButtonItem*   backButton;
@property(nonatomic, strong) UIBarButtonItem*   stopButton;
@property(nonatomic, strong) UIBarButtonItem*   reloadButton;
@property(nonatomic, strong) UIBarButtonItem*   forwardButton;

@end

@implementation NPWebViewTabBar

@synthesize delegate;

@synthesize toolBar;
@synthesize backButton;
@synthesize stopButton;
@synthesize reloadButton;
@synthesize forwardButton;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    
    if (self)
    {
        // create bar items
        UIBarButtonItem*    backButton      = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemRewind
                                                                                            target:self
                                                                                            action:@selector(onGoBackButtonPress)];
        UIBarButtonItem*    stopButton      = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemStop
                                                                                            target:self
                                                                                            action:@selector(onStopButtonPress)];
        UIBarButtonItem*    reloadButton    = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemRefresh
                                                                                            target:self
                                                                                            action:@selector(onReloadButtonPress)];
        UIBarButtonItem*    forwardButton   = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFastForward
                                                                                            target:self
                                                                                            action:@selector(onGoForwardButtonPress)];
        UIBarButtonItem*    flexispace      = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace
                                                                                            target:nil
                                                                                            action:nil];
        
        // create tab bar
        NSArray*            toolBarItems    = @[backButton,
                                                flexispace,
                                                forwardButton,
                                                flexispace,
                                                reloadButton,
                                                flexispace,
                                                stopButton];
        UIToolbar*          toolBar         = [self createToolBarWithFrame:CGRectMake(0, 0, frame.size.width, frame.size.height)
                                                                  andItems:toolBarItems];
        
        // save reference
        self.backButton     = backButton;
        self.stopButton     = stopButton;
        self.reloadButton   = reloadButton;
        self.forwardButton  = forwardButton;
        self.toolBar        = toolBar;
        
        // add to view
        [self addSubview:self.toolBar];
        
        // add constraints
        [toolBar removeExistingConstraints];
        [toolBar.widthAnchor constraintEqualToAnchor:self.widthAnchor].active   = true;
        [toolBar.topAnchor constraintEqualToAnchor:self.topAnchor].active = true;
        [toolBar.centerXAnchor constraintEqualToAnchor:self.centerXAnchor].active = true;
        [toolBar.heightAnchor constraintLessThanOrEqualToAnchor:self.heightAnchor].active = true;
        
        [self setBackgroundColor:[UIColor blackColor]];
    }
    return self;
}

-(UIToolbar*)createToolBarWithFrame:(CGRect)frame andItems:(NSArray*)items
{
    UIToolbar*  toolBar = [[UIToolbar alloc] initWithFrame:frame];
    [toolBar setTranslatesAutoresizingMaskIntoConstraints:false];
    [toolBar setItems:items];
    [toolBar setBarStyle:UIBarStyleBlack];
    
    return toolBar;
}

#pragma mark - button states

- (void)setCanGoBack:(BOOL)canGoBack
{
    self.backButton.enabled     = canGoBack;
}

- (void)setCanStop:(BOOL)canStop
{
    self.stopButton.enabled     = canStop;
}

- (void)setCanRefresh:(BOOL)canRefresh
{
    self.reloadButton.enabled   = canRefresh;
}

- (void)setCanGoForward:(BOOL)canGoForward
{
    self.forwardButton.enabled  = canGoForward;
}

#pragma mark - Button callbacks

- (void)onGoBackButtonPress
{
    if (delegate)
    {
        [delegate didSelectGoBackOnTabBar:self];
    }
}

- (void)onGoForwardButtonPress
{
    if (delegate)
    {
        [delegate didSelectGoForwardOnTabBar:self];
    }
}

- (void)onStopButtonPress
{
    if (delegate)
    {
        [delegate didSelectStopOnTabBar:self];
    }
}

- (void)onReloadButtonPress
{
    if (delegate)
    {
        [delegate didSelectReloadOnTabBar:self];
    }
}

@end

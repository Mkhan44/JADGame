//
//  NPWebViewTabBar.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>

@class NPWebViewTabBar;

@protocol NPWebViewTabBarDelegate <NSObject>

- (void)didSelectGoBackOnTabBar:(NPWebViewTabBar*)toolbar;
- (void)didSelectGoForwardOnTabBar:(NPWebViewTabBar*)toolbar;
- (void)didSelectStopOnTabBar:(NPWebViewTabBar*)toolbar;
- (void)didSelectReloadOnTabBar:(NPWebViewTabBar*)toolbar;

@end

@interface NPWebViewTabBar : UIView

@property(nonatomic, assign) id<NPWebViewTabBarDelegate> delegate;

// button states
- (void)setCanGoBack:(BOOL)canGoBack;
- (void)setCanStop:(BOOL)canStop;
- (void)setCanRefresh:(BOOL)canRefresh;
- (void)setCanGoForward:(BOOL)canGoForward;

@end

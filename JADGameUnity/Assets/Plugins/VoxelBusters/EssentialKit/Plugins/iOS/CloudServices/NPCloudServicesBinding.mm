//
//  NPCloudServicesBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "NPCloudServicesDataTypes.h"
#import "NPKit.h"

// static fields
static UserChangeNativeCallback        _userChangeCallback          = nil;
static SavedDataChangeNativeCallback   _savedDataChangeCallback     = nil;

#pragma mark - Wrappers

@interface NativePluginsCloudServicesObserver : NSObject

@end

@implementation NativePluginsCloudServicesObserver

- (id)init
{
    self = [super init];
    if (self)
    {
        // register for events
        NSUbiquitousKeyValueStore*  defaultStore = [NSUbiquitousKeyValueStore defaultStore];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(onDataStoreChanged:)
                                                     name:NSUbiquitousKeyValueStoreDidChangeExternallyNotification
                                                   object:defaultStore];
        
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(onAccountChanged:)
                                                     name:NSUbiquityIdentityDidChangeNotification
                                                   object:nil];
                
        
        
        // get user info
        [self performSelector:@selector(sendUserAccountInfo) withObject:nil  afterDelay:1];
    }
    
    return self;
}

- (void)dealloc
{
    // unregister from events
    [[NSNotificationCenter defaultCenter] removeObserver:self
                                                    name:NSUbiquitousKeyValueStoreDidChangeExternallyNotification
                                                  object:[NSUbiquitousKeyValueStore defaultStore]];
    
    [[NSNotificationCenter defaultCenter] removeObserver:self
                                                    name:NSUbiquityIdentityDidChangeNotification
                                                  object:[NSUbiquitousKeyValueStore defaultStore]];
}

#pragma mark - User methods

- (void)sendUserAccountInfo
{
    id                  identityToken   = [[NSFileManager defaultManager] ubiquityIdentityToken];
    NSString*           idTokenStr      = (identityToken) ? NPExtractTokenFromNSData(identityToken) : nil;
    
    // create c format data
    CKAccountData*      accountData     = new CKAccountData(idTokenStr);
    
    // send callback
    _userChangeCallback(accountData, nil);
    
    // release c allocations
    delete(accountData);
}

#pragma mark - Callback methods

- (void)onDataStoreChanged:(NSNotification *)notification
{
    // get required data
    NSDictionary*       userInfo            = [notification userInfo];
    NSNumber*           changeReason        = [userInfo objectForKey:NSUbiquitousKeyValueStoreChangeReasonKey];
    NSArray<NSString*>* changedKeys         = [userInfo objectForKey:NSUbiquitousKeyValueStoreChangedKeysKey];
    
    // send an event
    NPArray*            changedKeysCArray   = NPCreateArrayOfCString(changedKeys);
    _savedDataChangeCallback([changeReason intValue], changedKeysCArray);
    
    // check whether user information changed
    if ([changeReason intValue] == NSUbiquitousKeyValueStoreAccountChange)
    {
        [self sendUserAccountInfo];
    }
    
    // release c data
    delete(changedKeysCArray);
}

- (void)onAccountChanged:(NSNotification *)notification
{
    [self sendUserAccountInfo];
}

@end

#pragma mark - Native binding methods

NPBINDING DONTSTRIP void NPCloudServicesRegisterCallbacks(UserChangeNativeCallback userChangeCallback, SavedDataChangeNativeCallback savedDataChangeCallback)
{
    // save values
    _userChangeCallback         = userChangeCallback;
    _savedDataChangeCallback    = savedDataChangeCallback;
}

NPBINDING DONTSTRIP void NPCloudServicesInit()
{
    // create observer
    static  NativePluginsCloudServicesObserver*   sharedObserver      = nil;
    if (nil == sharedObserver)
    {
        sharedObserver  = [[NativePluginsCloudServicesObserver alloc] init];
    }
}

NPBINDING DONTSTRIP bool NPCloudServicesGetBool(const char* key)
{
    return [[NSUbiquitousKeyValueStore defaultStore] boolForKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP long NPCloudServicesGetLong(const char* key)
{
    return [[NSUbiquitousKeyValueStore defaultStore] longLongForKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP double NPCloudServicesGetDouble(const char* key)
{
    return [[NSUbiquitousKeyValueStore defaultStore] doubleForKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP char* NPCloudServicesGetString(const char* key)
{
    NSString*   savedValue  = [[NSUbiquitousKeyValueStore defaultStore] stringForKey:NPCreateNSStringFromCString(key)];
    return NPCreateCStringCopyFromNSString(savedValue);
}

NPBINDING DONTSTRIP const void* NPCloudServicesGetByteArray(const char* key, int &length)
{
    NSData*     savedValue  = [[NSUbiquitousKeyValueStore defaultStore] dataForKey:NPCreateNSStringFromCString(key)];
    if (savedValue)
    {
        length  = (int)[savedValue length];
        return [savedValue bytes];
    }
    else
    {
        length  = 0;
        return nil;
    }
}

NPBINDING DONTSTRIP char* NPCloudServicesGetArray(const char* key)
{
    NSArray*        savedValue  = [[NSUbiquitousKeyValueStore defaultStore] arrayForKey:NPCreateNSStringFromCString(key)];
    if (savedValue)
    {
        NSError*    error;
        NSString*   jsonStr     = NPToJson(savedValue, &error);
        
        if (error)
        {
            NSLog(@"[NativePlugins] Failed to convert to json string.");
            return nil;
        }
        
        return NPCreateCStringCopyFromNSString(jsonStr);
    }
    
    return nil;
}

NPBINDING DONTSTRIP char* NPCloudServicesGetDictionary(const char* key)
{
    NSDictionary*   savedValue  = [[NSUbiquitousKeyValueStore defaultStore] dictionaryForKey:NPCreateNSStringFromCString(key)];
    if (savedValue)
    {
        NSError*    error;
        NSString*   jsonStr     = NPToJson(savedValue, &error);
        
        if (error)
        {
            NSLog(@"[NativePlugins] Failed to convert to json string.");
            return nil;
        }
        
        return NPCreateCStringCopyFromNSString(jsonStr);
    }
    
    return nil;
}

NPBINDING DONTSTRIP void NPCloudServicesSetBool(const char* key, bool value)
{
    [[NSUbiquitousKeyValueStore defaultStore] setBool:value forKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP void NPCloudServicesSetLong(const char* key, long value)
{
    [[NSUbiquitousKeyValueStore defaultStore] setLongLong:value forKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP void NPCloudServicesSetDouble(const char* key, double value)
{
    [[NSUbiquitousKeyValueStore defaultStore] setDouble:value forKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP void NPCloudServicesSetString(const char* key, const char* value)
{
    [[NSUbiquitousKeyValueStore defaultStore] setString:NPCreateNSStringFromCString(value) forKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP void NPCloudServicesSetByteArray(const char* key, const void* value, int length)
{
    NSData* data = (value) ? [NSData dataWithBytes:value length:length] : nil;
    [[NSUbiquitousKeyValueStore defaultStore] setData:data forKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP void NPCloudServicesSetArray(const char* key, const char* jsonCStr)
{
    id  object  = nil;
    if (jsonCStr)
    {
        // create native object
        NSError*    error;
        object                  = NPFromJson(NPCreateNSStringFromCString(jsonCStr), &error);
        if (error)
        {
            NSLog(@"[NativePlugins] Failed to create NSArray object from given json string.");
            return;
        }
    }
    
    // add data to cloud
    [[NSUbiquitousKeyValueStore defaultStore] setArray:(NSArray*)object forKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP void NPCloudServicesSetDictionary(const char* key, const char* jsonCStr)
{
    id  object  = nil;
    if (jsonCStr)
    {
        // create native object
        NSError*    error;
        object                  = NPFromJson(NPCreateNSStringFromCString(jsonCStr), &error);
        if (error)
        {
            NSLog(@"[NativePlugins] Failed to create NSDictionary object from given json string.");
            return;
        }
    }
    
    // add data to cloud
    [[NSUbiquitousKeyValueStore defaultStore] setDictionary:(NSDictionary*)object forKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP void NPCloudServicesRemoveKey(const char* key)
{
    [[NSUbiquitousKeyValueStore defaultStore] removeObjectForKey:NPCreateNSStringFromCString(key)];
}

NPBINDING DONTSTRIP bool NPCloudServicesSynchronize()
{
	return [[NSUbiquitousKeyValueStore defaultStore] synchronize];
}

NPBINDING DONTSTRIP char* NPCloudServicesSnapshot()
{
    NSDictionary*   snapshot    = [[NSUbiquitousKeyValueStore defaultStore] dictionaryRepresentation];
    if (snapshot)
    {
        // convert to json representation
        NSError*    error;
        NSString*   jsonStr     = NPToJson(snapshot, &error);
        if (error)
        {
            NSLog(@"[NativePlugins] Failed to convert to json representation.");
            return nil;
        }
        
        return NPCreateCStringCopyFromNSString(jsonStr);
    }
    
    return nil;
}

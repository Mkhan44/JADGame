using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    internal static class ImplementationBlueprint
    {
        #region Constants

        private     const   string      kMainAssembly                   = "VoxelBusters.EssentialKit";
        
        private     const   string      kIOSAssembly                    = "VoxelBusters.EssentialKit.iOSModule";

        private     const   string      kAndroidAssembly                = "VoxelBusters.EssentialKit.AndroidModule";

        private     const   string      kSimulatorAssembly              = "VoxelBusters.EssentialKit.SimulatorModule";

        private     const   string      kRootNamespace                  = "VoxelBusters.EssentialKit";

        private     const   string      kAddressBookNamespace           = kRootNamespace + ".AddressBookCore";

        private     const   string      kBillingServicesNamespace       = kRootNamespace + ".BillingServicesCore";

        private     const   string      kCloudServicesNamespace         = kRootNamespace + ".CloudServicesCore";

        private     const   string      kGameServicesNamespace          = kRootNamespace + ".GameServicesCore";

        private     const   string      kMediaServicesNamespace         = kRootNamespace + ".MediaServicesCore";

        private     const   string      kNativeUINamespace              = kRootNamespace + ".NativeUICore";

        private     const   string      kNetworkServicesNamespace       = kRootNamespace + ".NetworkServicesCore";

        private     const   string      kNotificationServicesNamespace  = kRootNamespace + ".NotificationServicesCore";

        private     const   string      kSharingServicesNamespace       = kRootNamespace + ".SharingServicesCore";

        private     const   string      kWebViewNamespace               = kRootNamespace + ".WebViewCore";

        private     const   string      kExtrasNamespace                = kRootNamespace + ".ExtrasCore";

        private     const   string      kDeepLinkServicesNamespace      = kRootNamespace + ".DeepLinkServicesCore";

        #endregion

        #region Static properties

        public static NativeFeaturePackageConfiguration AddressBook 
        { 
            get { return GetAddressBookConfig(); } 
        }

        public static NativeFeaturePackageConfiguration BillingServices 
        { 
            get { return GetBillingServicesConfig(); } 
        }

        public static NativeFeaturePackageConfiguration CloudServices 
        { 
            get { return GetCloudServicesConfig(); } 
        }
        
        public static NativeFeaturePackageConfiguration GameServices
        {
            get { return GetGameServicesConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration MediaServices
        {
            get { return GetMediaServicesConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration NativeUI
        {
            get { return GetNativeUIConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration NetworkServices
        {
            get { return GetNetworkServicesConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration NotificationServices
        {
            get { return GetNotificationServicesConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration SharingServices
        {
            get { return GetSharingServicesConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration WebView
        {
            get { return GetWebViewConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration Extras
        {
            get { return GetExtrasConfig(); }
        }
        
        public static NativeFeaturePackageConfiguration DeepLinkServices
        {
            get { return GetDeepLinkServicesConfig(); }
        }
        
        #endregion

        #region Constructors

        private static NativeFeaturePackageConfiguration GetAddressBookConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kAddressBookNamespace + ".iOS", nativeInterfaceType: "AddressBookInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kAddressBookNamespace + ".iOS", nativeInterfaceType: "AddressBookInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kAddressBookNamespace + ".Android", nativeInterfaceType: "AddressBookInterface"),
                },
                simulatorPackage: new NativeFeaturePackageDefinition(assembly: kSimulatorAssembly, ns: kAddressBookNamespace + ".Simulator", nativeInterfaceType: "AddressBookInterface"),
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kAddressBookNamespace, nativeInterfaceType: "NullAddressBookInterface"));
        }
            
        private static NativeFeaturePackageConfiguration GetBillingServicesConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kBillingServicesNamespace + ".iOS", nativeInterfaceType: "BillingServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kBillingServicesNamespace + ".iOS", nativeInterfaceType: "BillingServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kBillingServicesNamespace + ".Android", nativeInterfaceType: "BillingServicesInterface"),
                },
                simulatorPackage: new NativeFeaturePackageDefinition(assembly: kSimulatorAssembly, ns: kBillingServicesNamespace + ".Simulator", nativeInterfaceType: "BillingServicesInterface"),
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kBillingServicesNamespace, nativeInterfaceType: "NullBillingServicesInterface"));
        }
            
        private static NativeFeaturePackageConfiguration GetCloudServicesConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kCloudServicesNamespace + ".iOS", nativeInterfaceType: "CloudServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kCloudServicesNamespace + ".iOS", nativeInterfaceType: "CloudServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kCloudServicesNamespace + ".Android", nativeInterfaceType: "CloudServicesInterface"),
                },
                simulatorPackage: new NativeFeaturePackageDefinition(assembly: kSimulatorAssembly, ns: kCloudServicesNamespace + ".Simulator", nativeInterfaceType: "CloudServicesInterface"),
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kCloudServicesNamespace, nativeInterfaceType: "NullCloudServicesInterface"));
        }

        private static NativeFeaturePackageConfiguration GetGameServicesConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kGameServicesNamespace + ".iOS", nativeInterfaceType: "GameCenterInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kGameServicesNamespace + ".iOS", nativeInterfaceType: "GameCenterInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kGameServicesNamespace + ".Android", nativeInterfaceType: "GameServicesInterface"),
                },
                simulatorPackage: new NativeFeaturePackageDefinition(assembly: kSimulatorAssembly, ns: kGameServicesNamespace + ".Simulator", nativeInterfaceType: "GameServicesInterface"),
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kGameServicesNamespace, nativeInterfaceType: "NullGameServicesInterface"));
        }

        private static NativeFeaturePackageConfiguration GetMediaServicesConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kMediaServicesNamespace + ".iOS", nativeInterfaceType: "MediaServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kMediaServicesNamespace + ".iOS", nativeInterfaceType: "MediaServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kMediaServicesNamespace + ".Android", nativeInterfaceType: "MediaServicesInterface"),
                },
                simulatorPackage: new NativeFeaturePackageDefinition(assembly: kSimulatorAssembly, ns: kMediaServicesNamespace + ".Simulator", nativeInterfaceType: "MediaServicesInterface"),
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kMediaServicesNamespace, nativeInterfaceType: "NullMediaServicesInterface"));
        }
            
        private static NativeFeaturePackageConfiguration GetNativeUIConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kNativeUINamespace + ".iOS", nativeInterfaceType: "NativeUIInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kNativeUINamespace + ".iOS", nativeInterfaceType: "NativeUIInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly,ns: kNativeUINamespace + ".Android", nativeInterfaceType: "UIInterface"),
                },
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kNativeUINamespace, nativeInterfaceType: "UnityUIInterface"));
        }
            
        private static NativeFeaturePackageConfiguration GetNetworkServicesConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kNetworkServicesNamespace + ".iOS", nativeInterfaceType: "NetworkServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kNetworkServicesNamespace + ".iOS", nativeInterfaceType: "NetworkServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kNetworkServicesNamespace + ".Android", nativeInterfaceType: "NetworkServicesInterface"),
                },
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kNetworkServicesNamespace, nativeInterfaceType: "UnityNetworkServicesInterface"));
        }
            
        private static NativeFeaturePackageConfiguration GetNotificationServicesConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kNotificationServicesNamespace + ".iOS", nativeInterfaceType: "NotificationCenterInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kNotificationServicesNamespace + ".iOS", nativeInterfaceType: "NotificationCenterInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kNotificationServicesNamespace + ".Android", nativeInterfaceType: "NotificationCenterInterface"),
                },
                simulatorPackage: new NativeFeaturePackageDefinition(assembly: kSimulatorAssembly, ns: kNotificationServicesNamespace + ".Simulator", nativeInterfaceType: "NotificationCenterInterface"),
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kNotificationServicesNamespace, nativeInterfaceType: "NullNotificationCenterInterface"));
        }

        private static NativeFeaturePackageConfiguration GetSharingServicesConfig()
        {    
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kSharingServicesNamespace + ".iOS", nativeInterfaceType: "NativeSharingInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kSharingServicesNamespace + ".iOS", nativeInterfaceType: "NativeSharingInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kSharingServicesNamespace + ".Android", nativeInterfaceType: "SharingServicesInterface"),
                },
                simulatorPackage: new NativeFeaturePackageDefinition(assembly: kSimulatorAssembly, ns: kSharingServicesNamespace + ".Simulator", nativeInterfaceType: "NativeSharingInterface"),
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kSharingServicesNamespace, nativeInterfaceType: "NullSharingInterface"));
        }

        private static NativeFeaturePackageConfiguration GetWebViewConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kWebViewNamespace + ".iOS", nativeInterfaceType: "NativeWebView"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kWebViewNamespace + ".iOS", nativeInterfaceType: "NativeWebView"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kWebViewNamespace + ".Android", nativeInterfaceType: "WebView"),
                },
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kWebViewNamespace, nativeInterfaceType: "NullNativeWebView"));
        }
            
        private static NativeFeaturePackageConfiguration GetExtrasConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kExtrasNamespace + ".iOS", nativeInterfaceType: "NativeUtilityInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kExtrasNamespace + ".iOS", nativeInterfaceType: "NativeUtilityInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kExtrasNamespace + ".Android", nativeInterfaceType: "UtilityInterface"),
                },
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kExtrasNamespace, nativeInterfaceType: "NullNativeUtilityInterface"));
        }
            
        private static NativeFeaturePackageConfiguration GetDeepLinkServicesConfig()
        {
            return new NativeFeaturePackageConfiguration(
                packages: new NativeFeaturePackageDefinition[]
                {
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.IPhonePlayer, assembly: kIOSAssembly, ns: kDeepLinkServicesNamespace + ".iOS", nativeInterfaceType: "DeepLinkServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.tvOS, assembly: kIOSAssembly, ns: kDeepLinkServicesNamespace + ".iOS", nativeInterfaceType: "DeepLinkServicesInterface"),
                    new NativeFeaturePackageDefinition(platform: RuntimePlatform.Android, assembly: kAndroidAssembly, ns: kDeepLinkServicesNamespace + ".Android", nativeInterfaceType: "DeepLinkServicesInterface"),
                },
                fallbackPackage: new NativeFeaturePackageDefinition(assembly: kMainAssembly, ns: kDeepLinkServicesNamespace, nativeInterfaceType: "NullDeepLinkServicesInterface"));
        }
            
        #endregion

        #region Public static methods

        public static NativeFeaturePackageConfiguration GetFeatureConfiguration(string featureName)
        {
            switch (featureName)
            {
                case NativeFeatureType.kAddressBook:
                    return AddressBook;

                case NativeFeatureType.kBillingServices:
                    return BillingServices;

                case NativeFeatureType.kCloudServices:
                    return CloudServices;

                case NativeFeatureType.kExtras:
                    return Extras;

                case NativeFeatureType.kGameServices:
                    return GameServices;

                case NativeFeatureType.kMediaServices:
                    return MediaServices;

                case NativeFeatureType.kNativeUI:
                    return NativeUI;

                case NativeFeatureType.kNetworkServices:
                    return NetworkServices;

                case NativeFeatureType.kNotificationServices:
                    return NotificationServices;

                case NativeFeatureType.KSharingServices:
                    return SharingServices;

                case NativeFeatureType.kWebView:
                    return WebView;

                case NativeFeatureType.kDeepLinkServices:
                    return DeepLinkServices;

                default:
                    return null;
            }
        }

        #endregion
    }
}
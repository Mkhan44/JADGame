using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.Editor
{
    public class UpgradeUtility
    {
        #region Static methods

        public static EssentialKitSettings ImportSettings()
        {
            // open file
            string  filePath    = EditorUtility.OpenFilePanel("Open NPSettings", "", "json");
            if (string.IsNullOrEmpty(filePath))
            {
                DebugLogger.LogError("Failed to locate json file");
                return null;
            }
            string  jsonStr     = IOServices.ReadFile(filePath);
            var     jsonDict    = (IDictionary)ExternalServiceProvider.JsonServiceProvider.FromJson(jsonStr);   

            // load properties
            var     applicationSettings     = ImportApplicationSettingsFromJson(jsonDict);
            var     addressBookSettings     = ImportAddressBookSettingsFromJson(jsonDict);
            var     billingSettings         = ImportBillingSettingsFromJson(jsonDict);
            var     cloudSettings           = ImportCloudSettingsFromJson(jsonDict);
            var     gameServicesSettings    = ImportGameServicesSettingsFromJson(jsonDict);
            var     mediaServicesSettings   = ImportMediaServicesSettingsFromJson(jsonDict);
            var     networkSettings         = ImportNetworkServicesSettingsFromJson(jsonDict);
            var     notificationSettings    = ImportNotificationServicesSettingsFromJson(jsonDict);
            var     sharingServicesSettings = ImportSharingServicesSettingsFromJson(jsonDict);
            var     webViewSettings         = ImportWebViewSettingsFromJson(jsonDict);
            
            // set properties
            var     essentialKitSettings    = EssentialKitSettingsEditorUtility.DefaultSettings;
            essentialKitSettings.ApplicationSettings                = applicationSettings;
            essentialKitSettings.AddressBookSettings                = addressBookSettings;
            essentialKitSettings.BillingServicesSettings            = billingSettings;
            essentialKitSettings.CloudServicesSettings              = cloudSettings;
            essentialKitSettings.GameServicesSettings               = gameServicesSettings;
            essentialKitSettings.MediaServicesSettings              = mediaServicesSettings;
            essentialKitSettings.NetworkServicesSettings            = networkSettings;
            essentialKitSettings.NotificationServicesSettings       = notificationSettings;
            essentialKitSettings.SharingServicesSettings            = sharingServicesSettings;
            essentialKitSettings.WebViewSettings                    = webViewSettings;

            return essentialKitSettings;
        }

        #endregion

        #region Import methods

        private static ApplicationSettings ImportApplicationSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            string  iosId               = ReadJsonValueAtPath<string>(jsonDict, "m_applicationSettings.m_iOS.m_storeIdentifier");
            string  androidId           = ReadJsonValueAtPath<string>(jsonDict, "m_applicationSettings.m_android.m_storeIdentifier");
            var     rateMyAppSettings   = ImportRateMyAppSettingsFromJson(jsonDict);
            
            // create object
            return new ApplicationSettings(
                appStoreIds: new NativePlatformConstantSet(ios: iosId, tvos: iosId, android: androidId),
                rateMyAppSettings: rateMyAppSettings);
        }

        private static AddressBookUnitySettings ImportAddressBookSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled   = GetUsesFeatureValueFromJson(jsonDict, "m_usesAddressBook");

            // create object
            return new AddressBookUnitySettings(isEnabled);
        }

        private static BillingServicesUnitySettings ImportBillingSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled               = GetUsesFeatureValueFromJson(jsonDict, "m_usesBilling");
            string  publicKey               = ReadJsonValueAtPath<string>(jsonDict, "m_billingSettings.m_android.m_publicKey");
            var     productJsonMetaArray    = ReadJsonValueAtPath<IList>(jsonDict,  "m_billingSettings.m_products");
            var     productDefinitionList   = new List<BillingProductDefinition>();
            foreach (IDictionary metaDict in productJsonMetaArray)
            {
                string  title               = ReadJsonValueAtPath<string>(metaDict, "m_name");             
                bool    isConsumable        = ReadJsonValueAtPath<bool>(metaDict,   "m_isConsumable");             
                var     productIdJsonArray  = ReadJsonValueAtPath<IList>(metaDict,  "m_productIdentifiers");

                // create product id set
                var     platformValues      = ConvertToPlatformConstants(productIdJsonArray);
                var     iOSProductId        = PlatformConstantUtility.GetPlatformConstantValue(platformValues, NativePlatform.iOS);
                var     androidProductId    = PlatformConstantUtility.GetPlatformConstantValue(platformValues, NativePlatform.Android);


                // create product and add it to list
                var     productType         = isConsumable ? BillingProductType.Consumable : BillingProductType.NonConsumable;
                productDefinitionList.Add(new BillingProductDefinition(
                    id: title, 
                    platformIdOverrides: new NativePlatformConstantSet(ios: iOSProductId, tvos: iOSProductId, android: androidProductId),
                    productType: productType, 
                    title: title, 
                    description: title, 
                    tag: null));
            }

            // create object
            var     androidSettings = new BillingServicesUnitySettings.AndroidPlatformProperties(publicKey);
            return new BillingServicesUnitySettings(
                enabled: isEnabled, 
                products: productDefinitionList.ToArray(),
                maintainPurchaseHistory: true, 
                autoFinishTransactions: true, 
                verifyTransactionReceipts: true, 
                androidProperties: androidSettings);
        }

        private static CloudServicesUnitySettings ImportCloudSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled       = GetUsesFeatureValueFromJson(jsonDict, "m_usesCloudServices");
            float   syncInterval    = ReadJsonValueAtPath<float>(jsonDict,  "m_cloudServicesSettings.m_android.m_syncInterval");
                                
            // create object
            return new CloudServicesUnitySettings(enabled: isEnabled, syncInterval: (int)syncInterval);
        }

        private static GameServicesUnitySettings ImportGameServicesSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled                   = GetUsesFeatureValueFromJson(jsonDict, "m_usesGameServices");
            bool    showBanner                  = ReadJsonValueAtPath<bool>(jsonDict,   "m_gameServicesSettings.m_iOS.m_showDefaultAchievementCompletionBanner");
            var     leaderboardMetaJsonArray    = ReadJsonValueAtPath<IList>(jsonDict,  "m_gameServicesSettings.m_leaderboardMetadataCollection");
            var     leaderboardList             = new List<LeaderboardDefinition>();
            foreach (IDictionary metaDict in leaderboardMetaJsonArray)
            {
                string  id                      = ReadJsonValueAtPath<string>(metaDict, "m_globalID");
                var     platformIdJsonArray     = ReadJsonValueAtPath<IList>(metaDict,  "m_platformIDs"); 
                var     platformIds             = ConvertToPlatformConstants(platformIdJsonArray);
                var     iOSLeaderboardId        = PlatformConstantUtility.GetPlatformConstantValue(platformIds, NativePlatform.iOS);
                var     androidLeaderboardId    = PlatformConstantUtility.GetPlatformConstantValue(platformIds, NativePlatform.Android);
                leaderboardList.Add(new LeaderboardDefinition(
                    id: id, 
                    platformIdOverrides: new NativePlatformConstantSet(ios: iOSLeaderboardId, tvos: iOSLeaderboardId, android: androidLeaderboardId),
                    title: id));
            }

            var     achievementMetaJsonArray    = ReadJsonValueAtPath<IList>(jsonDict,  "m_gameServicesSettings.m_achievementMetadataCollection");
            var     achievementList             = new List<AchievementDefinition>();
            foreach (IDictionary metaDict in achievementMetaJsonArray)
            {
                string  id                      = ReadJsonValueAtPath<string>(metaDict, "m_globalID");
                var     platformIdJsonArray     = ReadJsonValueAtPath<IList>(metaDict,  "m_platformIDs"); 
                int     noOfSteps               = ReadJsonValueAtPath<int>(metaDict,    "m_noOfSteps");
                var     platformIds             = ConvertToPlatformConstants(platformIdJsonArray);
                var     iOSAchievementId        = PlatformConstantUtility.GetPlatformConstantValue(platformIds, NativePlatform.iOS);
                var     androidAchievementId    = PlatformConstantUtility.GetPlatformConstantValue(platformIds, NativePlatform.Android);
                achievementList.Add(new AchievementDefinition(
                    id: id, 
                    platformIdOverrides: new NativePlatformConstantSet(ios: iOSAchievementId, tvos: iOSAchievementId, android: androidAchievementId),
                    title: id, 
                    numOfStepsToUnlock: noOfSteps));
            }

            var     androidDict                 = ReadJsonValueAtPath<IDictionary>(jsonDict,    "m_gameServicesSettings.m_android");
            string  psApplicationID             = ReadJsonValueAtPath<string>(androidDict,      "m_playServicesApplicationID");
            string  serverClientID              = ReadJsonValueAtPath<string>(androidDict,      "m_serverClientID");
            bool    showErrorDialogs            = ReadJsonValueAtPath<bool>(androidDict,        "m_showDefaultErrorDialogs");

            // create object
            var     androidSettings = new GameServicesUnitySettings.AndroidPlatformProperties(psApplicationID, serverClientID, null, showErrorDialogs);
            return new GameServicesUnitySettings(
                enabled: isEnabled, 
                initializeOnStart: true,
                leaderboards: leaderboardList.ToArray(), 
                achievements: achievementList.ToArray(), 
                showAchievementCompletionBanner: showBanner,
                androidProperties: androidSettings);
        }

        private static MediaServicesUnitySettings ImportMediaServicesSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled       = GetUsesFeatureValueFromJson(jsonDict, "m_mediaLibrary.value");
            bool    usesCamera      = GetUsesFeatureValueFromJson(jsonDict, "m_mediaLibrary.usesCamera");
            bool    usesGallery     = GetUsesFeatureValueFromJson(jsonDict, "m_mediaLibrary.usesPhotoAlbum");

            // create object
            return new MediaServicesUnitySettings(isEnabled, usesCamera, usesGallery);
        }

        private static NetworkServicesUnitySettings ImportNetworkServicesSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled       = GetUsesFeatureValueFromJson(jsonDict,         "m_usesNetworkConnectivity");

            var     settingsDict    = ReadJsonValueAtPath<IDictionary>(jsonDict,    "m_networkConnectivitySettings");
            string  ipv4Address     = ReadJsonValueAtPath<string>(settingsDict,     "m_hostAddressIPV4");
            string  ipv6Address     = ReadJsonValueAtPath<string>(settingsDict,     "m_hostAddressIPV6");
            float   timeOutPeriod   = ReadJsonValueAtPath<float>(settingsDict,      "m_timeOutPeriod");
            int     maxRetry        = ReadJsonValueAtPath<int>(settingsDict,        "m_maxRetryCount");
            float   timeGap         = ReadJsonValueAtPath<float>(settingsDict,      "m_timeGapBetweenPolling");
            int     port            = ReadJsonValueAtPath<int>(settingsDict,        "m_android.m_port");

            // create object
            var     hostAddress     = new NetworkServicesUnitySettings.Address(ipv4Address, ipv6Address);
            var     pingSettings    = new NetworkServicesUnitySettings.PingTestSettings(maxRetry, timeGap, timeOutPeriod, port);
            return new NetworkServicesUnitySettings(isEnabled, hostAddress, true, pingSettings);
        }

        private static NotificationServicesUnitySettings ImportNotificationServicesSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled                   = GetUsesFeatureValueFromJson(jsonDict,             "m_notificationService.value");
            bool    usesPushService             = GetUsesFeatureValueFromJson(jsonDict,             "m_notificationService.usesRemoteNotification");
            bool    usesOneSignal               = ReadJsonValueAtPath<bool>(jsonDict,               "m_applicationSettings.m_supportedAddonServices.m_usesOneSignal");

            var     androidDict                 = ReadJsonValueAtPath<IDictionary>(jsonDict,        "m_notificationSettings.m_android");
            bool    allowVibration              = ReadJsonValueAtPath<bool>(androidDict,            "m_allowVibration");
            string  tickerTextKey               = ReadJsonValueAtPath<string>(androidDict,          "m_tickerTextKey");
            string  contentTextKey              = ReadJsonValueAtPath<string>(androidDict,          "m_contentTextKey");
            string  contentTitleKey             = ReadJsonValueAtPath<string>(androidDict,          "m_contentTitleKey");
            string  userInfoKey                 = ReadJsonValueAtPath<string>(androidDict,          "m_userInfoKey");
            string  tagKey                      = ReadJsonValueAtPath<string>(androidDict,          "m_tagKey");

            // create object
            var     pushServiceType     = usesOneSignal ? PushNotificationServiceType.OneSignal : usesPushService ? PushNotificationServiceType.Custom : PushNotificationServiceType.None;
            var     payloadKeys         = new NotificationServicesUnitySettings.AndroidPlatformProperties.Keys(tickerTextKey, contentTitleKey, contentTextKey, userInfoKey, tagKey);
            var     androidSettings     = new NotificationServicesUnitySettings.AndroidPlatformProperties(false, allowVibration, null, null, false, "#FFFFFF",payloadKeys);
            return new NotificationServicesUnitySettings(isEnabled, usesLocationBasedNotification: false, pushNotificationServiceType: pushServiceType, androidProperties: androidSettings);
        }

        private static RateMyAppSettings ImportRateMyAppSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            var     rateMyAppDict                   = ReadJsonValueAtPath<IDictionary>(jsonDict,        "m_utilitySettings.m_rateMyApp");
            bool    isEnabled                       = ReadJsonValueAtPath<bool>(rateMyAppDict,          "m_isEnabled");
            string  title                           = ReadJsonValueAtPath<string>(rateMyAppDict,        "m_title");
            string  message                         = ReadJsonValueAtPath<string>(rateMyAppDict,        "m_message");
            int     showFirstPromptAfterHours       = ReadJsonValueAtPath<int>(rateMyAppDict,           "m_showFirstPromptAfterHours");
            int     successivePromptAfterHours      = ReadJsonValueAtPath<int>(rateMyAppDict,           "m_successivePromptAfterHours");
            int     successivePromptAfterLaunches   = ReadJsonValueAtPath<int>(rateMyAppDict,           "m_successivePromptAfterLaunches");
            string  okLabel                         = ReadJsonValueAtPath<string>(rateMyAppDict,        "m_rateItButtonText");
            string  cancelLabel                     = ReadJsonValueAtPath<string>(rateMyAppDict,        "m_remindMeLaterButtonText");
            string  remindLaterLabel                = ReadJsonValueAtPath<string>(rateMyAppDict,        "m_dontAskButtonText");

            // create object    
            var     dialogSettings                  = new RateMyAppConfirmationDialogSettings(true, title, message, okLabel, cancelLabel, remindLaterLabel, true);
            var     defaultValidatorSettings        = new RateMyAppDefaultControllerSettings(new RateMyAppDefaultControllerSettings.PromptConstraints(showFirstPromptAfterHours, 0), new RateMyAppDefaultControllerSettings.PromptConstraints(successivePromptAfterHours, successivePromptAfterLaunches));
            return new RateMyAppSettings(isEnabled, dialogSettings, defaultValidatorSettings);
        }

        private static SharingServicesUnitySettings ImportSharingServicesSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled       = GetUsesFeatureValueFromJson(jsonDict, "m_usesSharing");

            // create object
            return new SharingServicesUnitySettings(isEnabled);
        }

        private static WebViewUnitySettings ImportWebViewSettingsFromJson(IDictionary jsonDict)
        {
            // read properties
            bool    isEnabled       = GetUsesFeatureValueFromJson(jsonDict, "m_usesWebView");

            // create object
            return new WebViewUnitySettings(isEnabled);
        }

        #endregion

        #region Read methods

        private static T ReadJsonValueAtPath<T>(IDictionary jsonDict, string keyPath)
        {
            var     pathComponents  = keyPath.Split('.');
            var     dictionary      = jsonDict;

            // traverse through path
            int     iter            = 0;
            while (iter < (pathComponents.Length - 1))
            {
                string  currentKey  = pathComponents[iter++];
                dictionary          = (IDictionary)dictionary[currentKey];
            }

            // read last key value
            string  lastKey         = pathComponents[pathComponents.Length - 1];
            object  value           = dictionary[lastKey];

            // typecast return value
            if (value == null)
            {
                return default(T);
            }

            var     targetType      = typeof(T);
            if (targetType.IsInstanceOfType(value))
            {
                return (T)value;
            }

#if !NETFX_CORE
            if (targetType.IsEnum)
#else
            if (targetType.GetTypeInfo().IsEnum)
#endif
            {
                return (T)Enum.ToObject(targetType, value);
            }
            else
            {
                return (T)Convert.ChangeType(value, targetType);
            }
        }

        private static bool GetUsesFeatureValueFromJson(IDictionary jsonDict, string flagName)
        {
            return ReadJsonValueAtPath<bool>(jsonDict, "m_applicationSettings.m_supportedFeatures." + flagName);
        }

        private static PlatformConstant[] ConvertToPlatformConstants(IList jsonArray)
        {
            var     constantList    = new List<PlatformConstant>();
            foreach (IDictionary idDict in jsonArray)
            {
                int     platform    = ReadJsonValueAtPath<int>(idDict,      "m_platform");
                string  value       = ReadJsonValueAtPath<string>(idDict,   "m_value");

                // add to list
                switch (platform)
                {
                    case 0:
                        constantList.Add(PlatformConstant.iOS(value));
                        break;

                    case 1:
                        constantList.Add(PlatformConstant.Android(value));
                        break;
                }
            }

            return constantList.ToArray();
        }

        #endregion
    }
}
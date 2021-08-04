#if UNITY_EDITOR && UNITY_ANDROID
using UnityEditor;
using VoxelBusters.CoreLibrary.Editor.NativePlugins.Build;
using VoxelBusters.CoreLibrary;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.Editor.Android
{
    [InitializeOnLoad]
    public class AndroidBuildProcess 
    {
        #region Static fields

        private static EssentialKitSettings s_settings;

        #endregion

        #region Constructors

        static AndroidBuildProcess()
        {
            // unregister from events
            BuildProcessObserver.OnBuildTargetChange        -= OnBuildTargetChange;
            BuildProcessObserver.OnPreprocessUnityBuild     -= OnPreprocessBuild;
            BuildProcessObserver.OnPostprocessUnityBuild    -= OnPostprocessBuild;

            // register for events
            BuildProcessObserver.OnBuildTargetChange        += OnBuildTargetChange;
            BuildProcessObserver.OnPreprocessUnityBuild     += OnPreprocessBuild;
            BuildProcessObserver.OnPostprocessUnityBuild    += OnPostprocessBuild;
        }

        #endregion

        #region Static methods

        public static void OnBuildTargetChange(BuildTarget previousTarget, BuildTarget newTarget)
        {
            BuildForTarget(newTarget);
        }
    

        public static void OnPreprocessBuild(BuildInfo buildInfo)
        {
            BuildForTarget(buildInfo.Target);
        }

        public static void OnPostprocessBuild(BuildInfo buildInfo)
        {
            // check whether target platform is android
            if (buildInfo.Target != BuildTarget.Android)
            {
                return;
            }
        }

        #endregion

        #region Private methods

        private static void BuildForTarget(BuildTarget target)
        {
            // check whether plugin is configured
            if (!EssentialKitSettingsEditorUtility.SettingsExists)
            {
                EssentialKitSettingsEditorUtility.ShowSettingsNotFoundErrorDialog();
                return;
            }

            if (target != BuildTarget.Android)
            {
                return;
            }

            // update properties
            s_settings      = EssentialKitSettingsEditorUtility.DefaultSettings;

            // perform tasks
            PrepareForBuild();

            EssentialKitBuildUtility.CreateStrippingFile(target);

            // warn missing details and abort if not met
        }

        private static void PrepareForBuild()
        {
            // check if all required data is available for each feature
            CheckDataAvailabilityForFeatures();

            // enable required libraries
            EnabledRequiredLibraries();

            // generate config files
            //1. Copy required settings and configurable text to android xml files
            UpdateAndroidResourceFiles();
        }

        private static void CheckDataAvailabilityForFeatures()
        {
            StringBuilder builder = new StringBuilder();

            if(s_settings.BillingServicesSettings.IsEnabled)
            {
                if(string.IsNullOrEmpty(s_settings.BillingServicesSettings.AndroidProperties.PublicKey))
                {
                    builder.Append("Billing feature needs Public Key to be set in EssentialKit Settings inspector \n");
                }
            }

            if(s_settings.DeepLinkServicesSettings.IsEnabled)
            {
                if(s_settings.DeepLinkServicesSettings.AndroidProperties.CustomSchemeUrls.Length == 0 && s_settings.DeepLinkServicesSettings.AndroidProperties.UniversalLinks.Length == 0)
                {
                    builder.Append("Deep Link Services feature needs atleast one entry of Url schemes or Universal links in EssentialKit Settings inspector \n");
                    builder.Append("In-case if you don't want to use Deep Link services, disable them in EssentialKit settings inspector \n\n");
                }
            }

            if (s_settings.CloudServicesSettings.IsEnabled || s_settings.GameServicesSettings.IsEnabled)
            {
                if(string.IsNullOrEmpty(s_settings.GameServicesSettings.AndroidProperties.PlayServicesApplicationId))
                {
                   builder.Append("Game Services or Cloud Services need PlayServicesApplicationId field to be specified on Android\n");
                }
            }

            if (s_settings.MediaServicesSettings.IsEnabled)
            {
                if (!(s_settings.MediaServicesSettings.SavesFilesToGallery || s_settings.MediaServicesSettings.UsesCamera || s_settings.MediaServicesSettings.UsesGallery))
                {
                    builder.Append("Media Services needs atleast one of the listed flags enabled in EssentialKit Settings inspector \n");
                }
            }

            if (s_settings.NotificationServicesSettings.IsEnabled && s_settings.NotificationServicesSettings.PushNotificationServiceType == PushNotificationServiceType.Custom)
            {
                if (!IOServices.FileExists(FirebaseSettingsGenerator.kGoogleServicesJsonPath))
                {
                    builder.Append("Please add google-services.json under Assets folder for using Firebase Cloud Messaging(Push Notifications). You can fetch the file from Firebase console under your project -> Project Settings -> General : https://console.firebase.google.com.\n");
                    builder.Append("In-case if you don't want to use Push notifications/Remote notifications, set Push notification service type to \"None\" in EssentialKit settings inspector \n");
                }

            }

            if (builder.Length != 0)
            {
                string error = string.Format("[VoxelBusters : {0}] \n{1}", Constants.kProductDisplayName, builder.ToString());
                if (NativeFeatureUnitySettingsBase.CanToggleFeatureUsageState())
                {
                    Debug.LogError(error, s_settings);
                }
                else
                {
                    Debug.LogWarning(error, s_settings);
                }
                
            }
        }

        private static void EnabledRequiredLibraries()
        {

            Dictionary<string, bool> config = new Dictionary<string, bool>();

            config.Add("feature.addressbook",           s_settings.AddressBookSettings.IsEnabled);
            config.Add("feature.billingservices",       s_settings.BillingServicesSettings.IsEnabled);
            config.Add("feature.cloudservices",         s_settings.CloudServicesSettings.IsEnabled);
            config.Add("feature.deeplinkservices",      s_settings.DeepLinkServicesSettings.IsEnabled);
            config.Add("feature.gameservices",          s_settings.GameServicesSettings.IsEnabled);
            config.Add("feature.mediaservices",         s_settings.MediaServicesSettings.IsEnabled);
            config.Add("feature.networkservices",       s_settings.NetworkServicesSettings.IsEnabled);
            config.Add("feature.notificationservices",  s_settings.NotificationServicesSettings.IsEnabled);
            config.Add("feature.sharingservices",       s_settings.SharingServicesSettings.IsEnabled);
            config.Add("feature.socialauth",            s_settings.CloudServicesSettings.IsEnabled || s_settings.GameServicesSettings.IsEnabled);
            config.Add("feature.webview",               s_settings.WebViewSettings.IsEnabled);
            config.Add("feature.uiviews",               true);
            config.Add("feature.extras",                true);
            config.Add("essentialkit.core",             true);


            foreach (var each in config.Keys)
            {
                bool    isEnabled       = config[each];
                string  fileName        = each + ".jar";

                if(isEnabled)
                {
                    IOServices.CopyFile(Constants.kPluginAndroidProjectAllLibsPath + fileName, Constants.kPluginAndroidProjectLibsPath + fileName);
                }
                else
                {
                    IOServices.DeleteFile(Constants.kPluginAndroidProjectLibsPath + fileName);
                }
            }
        }

        private static void UpdateAndroidResourceFiles()
        {

            // generate files
            GenerateRequiredFiles();

            // copy files
            CopyRequiredFiles();

            // update string resources
            UpdateStringsXml();
        }

        private static void GenerateRequiredFiles()
        {
            // generate manifest
            AndroidManifestGenerator.GenerateManifest(s_settings);

            if (s_settings.NotificationServicesSettings.IsEnabled && s_settings.NotificationServicesSettings.PushNotificationServiceType == PushNotificationServiceType.Custom)
            {
                // generate google-services xml
                FirebaseSettingsGenerator.Execute();
            }

            // generate dependencies
            AndroidLibraryDependenciesGenerator.CreateLibraryDependencies();
        }

        private static void CopyRequiredFiles()
        {
            if (s_settings.NotificationServicesSettings.IsEnabled)
            {
                // copying white and colored notification icons if exist
                string assetPath = AssetDatabase.GetAssetPath(s_settings.NotificationServicesSettings.AndroidProperties.WhiteSmallIcon);
                if (!string.IsNullOrEmpty(assetPath))
                    IOServices.CopyFile(assetPath, Constants.kPluginAndroidProjectResDrawablePath + "app_icon_custom_white.png");

                assetPath = AssetDatabase.GetAssetPath(s_settings.NotificationServicesSettings.AndroidProperties.ColouredSmallIcon);
                if (!string.IsNullOrEmpty(assetPath))
                    IOServices.CopyFile(assetPath, Constants.kPluginAndroidProjectResDrawablePath + "app_icon_custom_coloured.png");
            }
        }

        private static void UpdateStringsXml()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();

            if (s_settings.AddressBookSettings.IsEnabled)
            {
                config.Add("ADDRESS_BOOK_PERMISSON_REASON", s_settings.ApplicationSettings.UsagePermissionSettings.AddressBookUsagePermission.GetDescriptionForActivePlatform());
            }
            if (s_settings.NotificationServicesSettings.IsEnabled)
            {
                config.Add("NOTIFICATION_SERVICES_CONTENT_TITLE_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.ContentTitleKey);
                config.Add("NOTIFICATION_SERVICES_CONTENT_TEXT_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.ContentTextKey);
                config.Add("NOTIFICATION_SERVICES_TICKER_TEXT_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.TickerTextKey);
                config.Add("NOTIFICATION_SERVICES_BADGE_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.BadgeKey);
                config.Add("NOTIFICATION_SERVICES_PRIORITY_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.PriorityKey);
                config.Add("NOTIFICATION_SERVICES_SOUND_FILE_NAME_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.SoundFileNameKey);
                config.Add("NOTIFICATION_SERVICES_BIG_PICTURE_FILE_NAME_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.BigPictureKey);
                config.Add("NOTIFICATION_SERVICES_LARGE_ICON_FILE_NAME_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.LargeIconKey);
                config.Add("NOTIFICATION_SERVICES_USER_INFO_KEY", s_settings.NotificationServicesSettings.AndroidProperties.PayloadKeys.UserInfoKey);

                
                config.Add("NOTIFICATION_SERVICES_USES_PUSH_NOTIFICATION_SERVICE", (s_settings.NotificationServicesSettings.PushNotificationServiceType == PushNotificationServiceType.Custom) ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_USES_EXTERNAL_SERVICE", (s_settings.NotificationServicesSettings.PushNotificationServiceType != PushNotificationServiceType.Custom) ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_NEEDS_VIBRATION", s_settings.NotificationServicesSettings.AndroidProperties.AllowVibration ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_NEEDS_CUSTOM_ICON", s_settings.NotificationServicesSettings.AndroidProperties.WhiteSmallIcon ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_ALLOW_NOTIFICATION_DISPLAY_WHEN_APP_IS_FOREGROUND", s_settings.NotificationServicesSettings.AndroidProperties.AllowNotificationDisplayWhenForeground ? "true" : "false");
                config.Add("NOTIFICATION_SERVICES_ACCENT_COLOR", s_settings.NotificationServicesSettings.AndroidProperties.AccentColor);
                
            }

            if (s_settings.GameServicesSettings.IsEnabled)
            {
                config.Add("GAME_SERVICES_SERVER_CLIENT_ID", s_settings.GameServicesSettings.AndroidProperties.ServerClientId.Trim());
                config.Add("GAME_SERVICES_SHOW_ERROR_DIALOGS", s_settings.GameServicesSettings.AndroidProperties.ShowErrorDialogs ? "true" : "false");
                config.Add("GAME_SERVICES_NEEDS_POPUPS_AT_TOP", s_settings.GameServicesSettings.AndroidProperties.DisplayPopupsAtTop ? "true" : "false");
                config.Add("GAME_SERVICES_NEEDS_PROFILE_SCOPE", s_settings.GameServicesSettings.AndroidProperties.NeedsProfileScope ? "true" : "false");
                config.Add("GAME_SERVICES_NEEDS_EMAIL_SCOPE", s_settings.GameServicesSettings.AndroidProperties.NeedsEmailScope ? "true" : "false");
            }

            if (s_settings.WebViewSettings.IsEnabled)
            {
                config.Add("WEB_VIEW_ALLOW_BACK_NAVIGATION_KEY", s_settings.WebViewSettings.AndroidProperties.AllowBackNavigationKey ? "true" : "false");
            }

            config.Add("USES_CLOUD_SERVICES", s_settings.CloudServicesSettings.IsEnabled ? "true" : "false");

            XmlDocument xml = new XmlDocument();
            xml.Load(Constants.kPluginAndroidProjectResValuesStringsPath);
            XmlNodeList nodes = xml.SelectNodes("/resources/string");

            foreach (XmlNode node in nodes)
            {
                XmlAttribute xmlAttribute = node.Attributes["name"];
                string key = xmlAttribute.Value;
                
                if(config.ContainsKey(key))
                {
                    node.InnerText = config[key];
                }
            }

            xml.Save(Constants.kPluginAndroidProjectResValuesStringsPath);
        }

        #endregion
    }
}
#endif
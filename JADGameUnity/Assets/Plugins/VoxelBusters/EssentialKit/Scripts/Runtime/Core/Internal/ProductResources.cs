using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public static class ProductResources
    {
        #region Constants

        private     const   string      kLiteVersionProductPage                 = "http://bit.ly/essentialkitliteunity";
        
        private     const   string      kFullVersionProductPage                 = "http://bit.ly/essentialkitunity";

        private     const   string      kPublisherPage                          = "http://bit.ly/ustoreVB";

        private     const   string      kDocumentationPage                      = "http://bit.ly/essentialkitdocumentation";

        private     const   string      kForumPage                              = "http://bit.ly/essentialkitforum";

        private     const   string      kTutorialPage                           = "http://bit.ly/essentialkittutorials";

        private     const   string      kDiscordPage                            = "https://discord.gg/y4kQAefbJ8";


        private     const   string      kAddressBookPage                        = "https://assetstore.essentialkit.voxelbusters.com/address-book";

        private     const   string      kBillingServicesPage                    = "https://assetstore.essentialkit.voxelbusters.com/billing-services";

        private     const   string      kCloudServicesPage                      = "https://assetstore.essentialkit.voxelbusters.com/cloud-services";

        private     const   string      kExtrasPage                             = "https://assetstore.essentialkit.voxelbusters.com/extras";
        
        private     const   string      kGameServicesPage                       = "https://assetstore.essentialkit.voxelbusters.com/game-services";

        private     const   string      kMediaServicesPage                      = "https://assetstore.essentialkit.voxelbusters.com/media-services";

        private     const   string      kNetworkServicesPage                    = "https://assetstore.essentialkit.voxelbusters.com/network-services";

        private     const   string      kNotificationServicesPage               = "https://assetstore.essentialkit.voxelbusters.com/notification-services";

        private     const   string      kRateMyAppPage                          = "https://assetstore.essentialkit.voxelbusters.com/rate-my-app";

        private     const   string      kSharingPage                            = "https://assetstore.essentialkit.voxelbusters.com/sharing";

        private     const   string      kUIPage                                 = "https://assetstore.essentialkit.voxelbusters.com/features/native-ui";
        
        private     const   string      kWebViewPage                            = "https://assetstore.essentialkit.voxelbusters.com/features/web-view";

        private     const   string      kDeepLinkServicesPage                   = "https://assetstore.essentialkit.voxelbusters.com/features/deep-link-services";
        
        #endregion

        #region Public static methods

        public static void OpenAssetStorePage(bool fullVersion)
        {
            Application.OpenURL(fullVersion ? kFullVersionProductPage : kLiteVersionProductPage);
        }

        public static void OpenPublisherPage()
        {
            Application.OpenURL(kPublisherPage);
        }

        public static void OpenDocumentation()
        {
            Application.OpenURL(kDocumentationPage);
        }

        public static void OpenForum()
        {
            Application.OpenURL(kForumPage);
        }

        public static void OpenTutorials()
        {
            Application.OpenURL(kTutorialPage);
        }

        public static void OpenSupport()
        {
            Application.OpenURL(kDiscordPage);
        }

        public static void OpenResourcePage(string feature)
        {
            string resourcePage = null;
            switch (feature)
            {
                case NativeFeatureType.kAddressBook:
                    resourcePage    = kAddressBookPage;
                    break;

                case NativeFeatureType.kBillingServices:
                    resourcePage    = kBillingServicesPage;
                    break;

                case NativeFeatureType.kCloudServices:
                    resourcePage    = kCloudServicesPage;
                    break;

                case NativeFeatureType.kDeepLinkServices:
                    resourcePage    = kDeepLinkServicesPage;
                    break;

                case NativeFeatureType.kExtras:
                    resourcePage    = kExtrasPage;
                    break;

                case NativeFeatureType.kGameServices:
                    resourcePage    = kGameServicesPage;
                    break;

                case NativeFeatureType.kMediaServices:
                    resourcePage    = kMediaServicesPage;
                    break;

                case NativeFeatureType.kNetworkServices:
                    resourcePage    = kNetworkServicesPage;
                    break;

                case NativeFeatureType.kNotificationServices:
                    resourcePage    = kNotificationServicesPage;
                    break;

                case NativeFeatureType.kRateMyApp:
                    resourcePage    = kRateMyAppPage;
                    break;

                case NativeFeatureType.KSharingServices:
                    resourcePage    = kSharingPage;
                    break;

                case NativeFeatureType.kNativeUI:
                    resourcePage    = kUIPage;
                    break;

                case NativeFeatureType.kWebView:
                    resourcePage    = kWebViewPage;
                    break;
            }

            // open link
            if (resourcePage != null)
            {
                Application.OpenURL(resourcePage);
            }
        }

        #endregion
    }
}
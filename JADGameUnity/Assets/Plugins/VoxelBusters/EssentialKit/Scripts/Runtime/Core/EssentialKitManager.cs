using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    public class EssentialKitManager : PrivateSingletonBehaviour<EssentialKitManager>
    {
        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnLoad()
        {
#pragma warning disable 
            var     singleton   = GetSingleton();
#pragma warning restore 
        }

        #endregion

        #region Unity methods

        protected override void OnSingletonAwake()
        {
            base.OnSingletonAwake();

            CallbackDispatcher.Initialize();

            // set environment variables
            var     settings    = EssentialKitSettings.Instance;
            DebugLogger.SetLogLevel(settings.ApplicationSettings.LogLevel);

            // initialize features
            if (settings.BillingServicesSettings.IsEnabled)
            {
                BillingServices.Initialize();
            }
            if (settings.CloudServicesSettings.IsEnabled)
            {
                CloudServices.Initialize();
            }
            if (settings.DeepLinkServicesSettings.IsEnabled)
            {
                DeepLinkServices.Initialize();
            }
            if (settings.GameServicesSettings.IsEnabled)
            {
                GameServices.Initialize();
            }
            if (settings.NetworkServicesSettings.IsEnabled)
            {
                NetworkServices.Initialize();
            }
            if (settings.NotificationServicesSettings.IsEnabled)
            {
                NotificationServices.Initialize();
            }
            if (settings.ApplicationSettings.RateMyAppSettings.IsEnabled)
            {
                if (null == FindObjectOfType<RateMyApp>())
                {
                    ActivateRateMyAppService();
                }
            }
        }

        #endregion

        #region Private methods

        private static void ActivateRateMyAppService()
        {
            var     prefab      = Resources.Load<GameObject>(Constants.kPluginResourcesPath + "RateMyApp");
            Assertions.AssertIfPropertyIsNull(prefab, "prefab");

            Instantiate(prefab, Vector3.zero, Quaternion.identity);
        }

        #endregion
    }
}
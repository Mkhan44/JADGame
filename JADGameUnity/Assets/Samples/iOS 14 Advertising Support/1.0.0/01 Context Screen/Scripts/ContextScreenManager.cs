﻿using Unity.Advertisement.IosSupport.Components;
using UnityEngine;

namespace Unity.Advertisement.IosSupport.Samples
{
    /// <summary>
    /// This component will trigger the context screen to appear when the scene starts,
    /// if the user hasn't already responded to the iOS tracking dialog.
    /// </summary>
    public class ContextScreenManager : MonoBehaviour
    {
        /// <summary>
        /// The prefab that will be instantiated by this component.
        /// The prefab has to have an ContextScreenView component on its root GameObject.
        /// </summary>
        public ContextScreenView contextScreenPrefab;

        void Start()
        {
#if UNITY_IOS
            // check with iOS to see if the user has accepted or declined tracking

            int mainVersion = 0;

            string[] versionPart = UnityEngine.iOS.Device.systemVersion.Split('.');

            int.TryParse(versionPart[0], out mainVersion);

            /// only run on iOS 14 devices

            if (mainVersion >= 14)
            {
                var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

                if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
                {
                    var contextScreen = Instantiate(contextScreenPrefab).GetComponent<ContextScreenView>();

                    // after the Continue button is pressed, and the tracking request
                    // has been sent, automatically destroy the popup to conserve memory
                    contextScreen.sentTrackingAuthorizationRequest += () => Destroy(contextScreen.gameObject);
                }
            }
            else
            {
                Debug.Log("Not IOS 14 or greater, don't show popup.");
            }
               

           
#else
            Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif
        }
    }   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="NotificationServices.GetSettings(Callback{NotificationServicesGetSettingsResult})"/> is completed.
    /// </summary>
    public class NotificationServicesGetSettingsResult
    {
        #region Properties

        /// <summary>
        /// The runtime settings.
        /// </summary>
        public NotificationSettings Settings
        {
            get;
            internal set;
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.CloudServicesCore
{
    public class CloudUser : ICloudUser
    {
        #region Properties

        public string UserId
        {
            get;
            private set;
        }

        public CloudUserAccountStatus AccountStatus
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public CloudUser(string userId, CloudUserAccountStatus accountStatus)
        {
            // set properties
            UserId          = userId;
            AccountStatus   = accountStatus;
        }

        #endregion
    }
}
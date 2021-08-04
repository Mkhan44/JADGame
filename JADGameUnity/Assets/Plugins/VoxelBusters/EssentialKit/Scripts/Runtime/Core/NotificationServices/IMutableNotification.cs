using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore
{
    public interface IMutableNotification : INotification
    { 
        #region Setter methods

        void SetTitle(string value);

        void SetSubtitle(string value);

        void SetBody(string value);

        void SetBadge(int value);

        void SetUserInfo(IDictionary value);
        
        void SetSoundFileName(string value);
        
        void SetIosProperties(NotificationIosProperties value);
        
        void SetAndroidProperties(NotificationAndroidProperties value);
        
        void SetTrigger(INotificationTrigger trigger);

        #endregion
    }
}
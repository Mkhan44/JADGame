using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore
{
    internal sealed class NullMutableNotification : NotificationBase, IMutableNotification
    {
        #region Constructors

        public NullMutableNotification(string notificationId)
            : base(notificationId)
        {
            //
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("NotificationServices");
        }

        #endregion

        #region Base class implementation

        protected override string GetTitleInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override string GetSubtitleInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override string GetBodyInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override int GetBadgeInternal()
        {
            LogNotSupported();

            return 0;
        }

        protected override IDictionary GetUserInfoInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override string GetSoundFileNameInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override INotificationTrigger GetTriggerInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override NotificationIosProperties GetIosPropertiesInternal()
        {
            LogNotSupported();

            return null;
        }

        protected override NotificationAndroidProperties GetAndroidPropertiesInternal()
        {
            LogNotSupported();

            return null;
        }

        #endregion

        #region IMutableNotification implementation

        public void SetTitle(string value)
        {
            LogNotSupported();
        }

        public void SetSubtitle(string value)
        {
            LogNotSupported();
        }

        public void SetBody(string value)
        {
            LogNotSupported();
        }

        public void SetBadge(int value)
        {
            LogNotSupported();
        }

        public void SetUserInfo(IDictionary value)
        {
            LogNotSupported();
        }

        public void SetSoundFileName(string value)
        {
            LogNotSupported();
        }
        
        public void SetIosProperties(NotificationIosProperties value)
        {
            LogNotSupported();
        }
        
        public void SetAndroidProperties(NotificationAndroidProperties value)
        {
            LogNotSupported();
        }

        public void SetTrigger(INotificationTrigger trigger)
        {
            LogNotSupported();
        }

        #endregion
    }
}
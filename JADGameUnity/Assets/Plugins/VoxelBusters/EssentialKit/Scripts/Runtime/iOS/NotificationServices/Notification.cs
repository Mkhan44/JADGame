#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal class Notification : NotificationBase
    {
        #region Fields

        private     INotificationTrigger            m_trigger;

        private     IosNativeObjectRef              m_nativeContent;

        private     IosNativeObjectRef              m_nativeTrigger;

        protected   NotificationIosProperties       m_iosProperties;
        
        #endregion

        #region Constructors

        protected Notification(string notificationId)
            : base(notificationId)
        {
            // set properties
            m_nativeContent     = null;
            m_nativeTrigger     = null;
            m_iosProperties     = null;
        }

        public Notification(IntPtr requestPtr)
            : base(NotificationBinding.NPNotificationRequestGetId(requestPtr))
        {
            var     contentPtr  = NotificationBinding.NPNotificationRequestGetContent(requestPtr);
            var     triggerPtr  = NotificationBinding.NPNotificationRequestGetTrigger(requestPtr);

            // set native properties
            NativeObjectRef     = new IosNativeObjectRef(requestPtr);
            if (contentPtr != IntPtr.Zero)
            {
                m_nativeContent = new IosNativeObjectRef(contentPtr, retain: false);
            }
            if (triggerPtr != IntPtr.Zero)
            {
                m_nativeTrigger = new IosNativeObjectRef(triggerPtr, retain: false);
            }

            // create ios specific properties object
            string  launchImage = NotificationBinding.NPNotificationContentGetLaunchImageName(m_nativeContent.Pointer);
            m_iosProperties     = new NotificationIosProperties(launchImageFileName: launchImage);
        }

        ~Notification()
        {
            Dispose(false);
        }

        #endregion

        #region Base class implementation

        protected override string GetTitleInternal()
        {
            return NotificationBinding.NPNotificationContentGetTitle(m_nativeContent.Pointer);
        }

        protected override string GetSubtitleInternal()
        {
            return NotificationBinding.NPNotificationContentGetSubtitle(m_nativeContent.Pointer);
        }

        protected override string GetBodyInternal()
        {
            return NotificationBinding.NPNotificationContentGetBody(m_nativeContent.Pointer);
        }

        protected override int GetBadgeInternal()
        {
            return NotificationBinding.NPNotificationContentGetBadge(m_nativeContent.Pointer);
        }

        protected override IDictionary GetUserInfoInternal()
        {
            string  jsonStr     = NotificationBinding.NPNotificationContentGetUserInfo(m_nativeContent.Pointer);
            return (IDictionary)ExternalServiceProvider.JsonServiceProvider.FromJson(jsonStr);
        }

        protected override string GetSoundFileNameInternal()
        {
            return null;
        }

        protected override INotificationTrigger GetTriggerInternal()
        {
            // reading property is allowed for existing notifications only
            if (null == m_trigger)
            {
                if (NativeObjectRef != null && m_nativeTrigger != null)
                {
                    m_trigger   = NotificationTrigger.CreateNotificationTrigger(AddrOfNativeObject(), m_nativeTrigger.Pointer);
                }
            }

            return m_trigger;
        }

        protected override NotificationIosProperties GetIosPropertiesInternal()
        {
            return m_iosProperties;
        }

        protected override NotificationAndroidProperties GetAndroidPropertiesInternal()
        {
            return null;
        }

        #endregion

        #region Private methods

        protected override void Dispose(bool disposing)
        {
            // check whether object is released
            if (IsDisposed)
            {
                return;
            }

            // release all unmanaged type objects
            if (m_nativeContent != null)
            {
                m_nativeContent.Dispose();
            }
            if (m_nativeTrigger != null)
            {
                m_nativeTrigger.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Internal methods

        internal IosNativeObjectRef GetNativeContentInternal()
        {
            return m_nativeContent;
        }

        internal void SetNativeContentInternal(IosNativeObjectRef obj)
        {
            m_nativeContent    = obj;
        }

        internal IosNativeObjectRef GetNativeTriggerInternal()
        {
            return m_nativeTrigger;
        }

        internal void SetTriggerInternal(INotificationTrigger trigger, IosNativeObjectRef nativeTrigger)
        {
            m_trigger           = trigger;
            m_nativeTrigger     = nativeTrigger;
        }

        internal bool CanCreateTrigger()
        {
            return (m_trigger == null);
        }

        #endregion
    }
}
#endif
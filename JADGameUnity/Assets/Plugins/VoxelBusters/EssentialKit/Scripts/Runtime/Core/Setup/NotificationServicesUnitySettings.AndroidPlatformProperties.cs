using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public partial class NotificationServicesUnitySettings 
    {
        [Serializable]
        public class AndroidPlatformProperties 
        {
            #region Fields

            [HideInInspector]
            [SerializeField]
            [Tooltip("If enabled, app will use big style notification.")]
            private     bool            m_needsBigStyle; 

            [SerializeField]
            [Tooltip("If enabled, device vibrates on receiving a notification.")]
            private     bool            m_allowVibration; 
            
            [SerializeField]
            [Tooltip("The texture used as small icon in post Android L Devices.")]
            private     Texture2D       m_whiteSmallIcon;

            [SerializeField]
            [Tooltip("The texture used as small icon in pre Android L Devices.")]
            private     Texture2D       m_colouredSmallIcon;

            [SerializeField]
            [Tooltip("If enabled, notifications are displayed even when app is foreground.")]
            private bool            m_allowNotificationDisplayWhenForeground;

            [SerializeField]
            [Tooltip("If set, the value will be used as accent color for notification.")]
            private string          m_accentColor;


            [SerializeField]
            [Tooltip("Array of payload keys.")]
            private     Keys            m_payloadKeys;
            
            #endregion
            
            #region Properties
            
            public bool NeedsBigStyle 
            {
                get
                {
                    return m_needsBigStyle;
                }
            }
            
            public bool AllowVibration
            {
                get
                {
                    return m_allowVibration;
                }
            }
            
            public Texture2D WhiteSmallIcon
            {
                get 
                { 
                    return m_whiteSmallIcon; 
                }
            }
            
            public Texture2D ColouredSmallIcon
            {
                get 
                { 
                    return m_colouredSmallIcon; 
                }
            }

            public Keys PayloadKeys
            {
                get
                {
                    return m_payloadKeys;
                }
            }

            public bool AllowNotificationDisplayWhenForeground
            {
                get
                {
                    return m_allowNotificationDisplayWhenForeground;
                }
            }


            public string AccentColor
            {
                get
                {
                    return m_accentColor;
                }
            }

            #endregion

            #region Constructors

            public AndroidPlatformProperties(bool needsBigStyle = false, 
                                            bool allowVibration = true, Texture2D whiteSmallIcon = null, Texture2D colouredSmallIcon = null,
                                            bool allowNotificationDisplayWhenForeground = false, string accentColor = "#FFFFFF",
                                            Keys payloadKeys = null)
            {
                // set properties
                m_needsBigStyle                             = needsBigStyle;
                m_allowVibration                            = allowVibration;
                m_whiteSmallIcon                            = whiteSmallIcon;
                m_colouredSmallIcon                         = colouredSmallIcon;
                m_allowNotificationDisplayWhenForeground    = allowNotificationDisplayWhenForeground;
                m_accentColor                               = accentColor;
                m_payloadKeys                               = payloadKeys ?? new Keys();
            }

            #endregion

            #region Nested types

            [Serializable]
            public class Keys
            {
                #region Fields

                [SerializeField]
                [Tooltip("The key used to capture content title property from the payload.")]
                private string m_contentTitle;

                [SerializeField]
                [Tooltip("The key used to capture content text property from the payload.")]
                private string m_contentText;

                [SerializeField]
                [Tooltip("The key used to capture ticker text property from the payload.")]
                private     string          m_tickerText;

                [SerializeField]
                [Tooltip("The key used to capture user info dictionary from the payload.")]
                private     string          m_userInfo;

                [SerializeField]
                [Tooltip("The key used to capture tag property from the payload.")]
                private     string          m_tag;

                [SerializeField]
                [Tooltip("The key used to capture badge property from the payload.")]
                private     string          m_badge;

                [SerializeField]
                [Tooltip("The key used to capture priority property from the payload.")]
                private     string          m_priority;

                [SerializeField]
                [Tooltip("The key used to capture sound property from the payload.")]
                private string              m_sound;

                [SerializeField]
                [Tooltip("The key used to capture big picture property from the payload.")]
                private string              m_bigPicture;

                [SerializeField]
                [Tooltip("The key used to capture large icon property from the payload.")]
                private string              m_largeIcon;


                #endregion

                #region Properties

                public string TickerTextKey
                {
                    get 
                    { 
                        return m_tickerText; 
                    }
                }
                
                public string ContentTitleKey
                {
                    get 
                    { 
                        return m_contentTitle; 
                    }
                }
                
                public string ContentTextKey
                {
                    get 
                    { 
                        return m_contentText; 
                    }
                }
                
                public string UserInfoKey
                {
                    get 
                    { 
                        return m_userInfo; 
                    }
                }
                
                public string TagKey
                {
                    get 
                    { 
                        return m_tag; 
                    }
                }

                public string BadgeKey
                {
                    get
                    {
                        return m_badge;
                    }
                }

                public string PriorityKey
                {
                    get
                    {
                        return m_priority;
                    }
                }

                public string SoundFileNameKey
                {
                    get
                    {
                        return m_sound;
                    }
                }

                public string BigPictureKey
                {
                    get
                    {
                        return m_bigPicture;
                    }
                }

                public string LargeIconKey
                {
                    get
                    {
                        return m_largeIcon;
                    }
                }

                #endregion

                #region Constructors

                public Keys(string tickerText = "ticker_text", string contentTitle = "content_title", 
                            string contentText = "content_text", string userInfo = "user_info",
                            string tag = "tag", string badge = "badge", string priority = "priority", string sound = "sound",
                            string bigPicture = "big_picture", string largeIcon = "large_icon")
                {
                    // set properties
                    m_tickerText        = tickerText;
                    m_contentTitle      = contentTitle;
                    m_contentText       = contentText;
                    m_userInfo          = userInfo;
                    m_tag               = tag;
                    m_priority          = priority;
                    m_badge             = badge;
                    m_sound             = sound;
                    m_bigPicture        = bigPicture;
                    m_largeIcon         = largeIcon;
                }

                #endregion
            }

            #endregion
        }
    }
}
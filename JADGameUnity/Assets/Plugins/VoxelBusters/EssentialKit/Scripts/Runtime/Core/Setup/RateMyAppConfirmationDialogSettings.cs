using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class RateMyAppConfirmationDialogSettings 
    {
        #region Fields

        [SerializeField]
        [Tooltip("If enabled, confirmation dialog is shown prior to prompting rating window.")]
        private     bool                m_canShow               = true;

        [SerializeField]
        [Tooltip("Title.")]
        private     string              m_promptTitle;

        [SerializeField, TextArea]
        [Tooltip("Description.")]
        private     string              m_promptDescription;

        [SerializeField]
        [Tooltip("Positive action button label.")]
        private     string              m_okButtonLabel;

        [SerializeField]
        [Tooltip("Negative action button label.")]
        private     string              m_cancelButtonLabel;

        [SerializeField]
        [Tooltip("Neutral action button label.")]
        private     string              m_remindLaterButtonLabel;

        [SerializeField]
        [Tooltip("Determines whether neutral action button is required.")]
        private     bool                m_canShowRemindMeLaterButton;

        #endregion

        #region Properties

        public bool CanShow
        {
            get
            {
                return m_canShow;
            }
        }

        public string PromptTitle
        {
            get
            {
                return m_promptTitle;
            }
        }

        public string PromptDescription
        {
            get
            {
                return m_promptDescription;
            }
        }

        public string OkButtonLabel
        {
            get
            {
                return m_okButtonLabel;
            }
        }

        public string CancelButtonLabel
        {
            get
            {
                return m_cancelButtonLabel;
            }
        }

        public string RemindLaterButtonLabel
        {
            get
            {
                return m_remindLaterButtonLabel;
            }
        }

        public bool CanShowRemindMeLaterButton
        {
            get
            {
                return m_canShowRemindMeLaterButton;
            }
        }

        #endregion

        #region Constructors

        public RateMyAppConfirmationDialogSettings(bool canShow = true, string title = null, 
                                                   string description = null, string okButtonLabel = null, 
                                                   string cancelButtonLabel = null, string remindLaterButtonLabel = null, 
                                                   bool canShowRemindMeLaterButton = true)
        {
            // set properties
            m_canShow                       = canShow;
            m_promptTitle                   = title ?? "Rate My App";
            m_promptDescription             = description ?? "If you enjoy using Native Plugins would you mind taking a moment to rate it? It wont take more than a minute. Thanks for your support.";
            m_okButtonLabel                 = okButtonLabel ?? "Ok";
            m_cancelButtonLabel             = cancelButtonLabel ?? "Cancel";
            m_remindLaterButtonLabel        = remindLaterButtonLabel ?? "Remind Me Later";
            m_canShowRemindMeLaterButton    = canShowRemindMeLaterButton;
        }

        #endregion
    }
}
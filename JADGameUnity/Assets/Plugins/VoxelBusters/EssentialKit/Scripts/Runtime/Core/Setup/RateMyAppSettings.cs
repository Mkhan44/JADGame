using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public class RateMyAppSettings
    {
        #region Fields

        [SerializeField]
        [Tooltip("If enabled, this feature is marked as required.")]
        private     bool                                m_isEnabled                     = false;

        [SerializeField]
        [Tooltip("Confirmation dialog settings.")]
        private     RateMyAppConfirmationDialogSettings m_confirmationDialogSettings    = new RateMyAppConfirmationDialogSettings();

        [SerializeField]
        [Tooltip("Default controller attributes.")]
        private     RateMyAppDefaultControllerSettings  m_defaultControllerSettings     = new RateMyAppDefaultControllerSettings();

        #endregion

        #region Properties

        public bool IsEnabled
        {
            get
            {
                return m_isEnabled;
            }
        }

        public RateMyAppConfirmationDialogSettings ConfirmationDialogSettings
        {
            get
            {
                return m_confirmationDialogSettings;
            }
        }

        public RateMyAppDefaultControllerSettings DefaultValidatorSettings
        {
            get
            {
                return m_defaultControllerSettings;
            }
        }

        #endregion

        #region Constructors

        public RateMyAppSettings(bool isEnabled = false, RateMyAppConfirmationDialogSettings dialogSettings = null, 
                                 RateMyAppDefaultControllerSettings defaultValidatorSettings = null)
        {
            // set properties
            m_isEnabled                     = isEnabled;
            m_confirmationDialogSettings    = dialogSettings ?? new RateMyAppConfirmationDialogSettings();
            m_defaultControllerSettings     = defaultValidatorSettings ?? new RateMyAppDefaultControllerSettings();
        }

        #endregion
    }
}
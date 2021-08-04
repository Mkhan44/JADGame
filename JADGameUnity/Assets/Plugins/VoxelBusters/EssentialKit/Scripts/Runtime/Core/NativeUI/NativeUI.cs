using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.NativeUICore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access native UI components.
    /// </summary>
    public static class NativeUI
    {
        #region Static fields

        private     static  INativeUIInterface      s_nativeInterface   = null;
            
        #endregion

        #region Static properties

        public static NativeUIUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.NativeUISettings;
            }
        }

        public static INativeUIInterface NativeInterface
        {
            get
            {
                if (s_nativeInterface == null)
                {
                    s_nativeInterface   = CreateNativeInterface();
                }

                return s_nativeInterface;
            }
            set
            {
                s_nativeInterface   = value;
            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Creates a new alert dialog with specified values.
        /// </summary>
        /// <param name="title">The title of the alert.</param>
        /// <param name="message">The descriptive text that provides more details.</param>
        /// <param name="preferredActionLabel">The title of the button.</param>
        /// <param name="preferredActionCallback">The method to execute when the user selects preferred action button.</param>
        /// <param name="cancelActionLabel">The title of the cancel button.</param>
        /// <param name="cancelActionCallback">The method to execute when the user selects cancel button.</param>
        public static void ShowAlertDialog(string title, string message, string preferredActionLabel, Callback preferredActionCallback = null, string cancelActionLabel = null, Callback cancelActionCallback = null)
        {
            var     newInstance     = AlertDialog.CreateInstance();
            newInstance.Title       = title;
            newInstance.Message     = message;
            if (preferredActionLabel != null)
            {
                newInstance.AddButton(preferredActionLabel, preferredActionCallback);
            }
            if (cancelActionLabel != null)
            {
                newInstance.AddCancelButton(cancelActionLabel, cancelActionCallback);
            }
            newInstance.Show();
        }

        #endregion

        #region Private static methods

        private static INativeUIInterface CreateNativeInterface()
        {
            // update interface object based on settings
            return NativeFeatureActivator.CreateInterface<INativeUIInterface>(ImplementationBlueprint.NativeUI, UnitySettings.IsEnabled);
        }

        #endregion
    }
}
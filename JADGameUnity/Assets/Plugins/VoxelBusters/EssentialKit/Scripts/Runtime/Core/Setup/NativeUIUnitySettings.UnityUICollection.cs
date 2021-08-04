using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.UnityUI;
using VoxelBusters.EssentialKit.NativeUICore;

namespace VoxelBusters.EssentialKit
{
    public partial class NativeUIUnitySettings
    {
        [Serializable]
        public class UnityUICollection
        {
            #region Properties

            [SerializeField]
            [Tooltip("Canvas used to render native plugins components (primarily simulator window).")]
            private     UnityUIRenderer         m_rendererPrefab;

            [SerializeField]
            [Tooltip("Custom alert dialog prefab. Object should implement IUnityUIAlertDialog interface.")]
            private     UnityUIAlertDialog      m_alertDialogPrefab;

            [SerializeField]
            private     UnityUIDatePicker       m_datePickerPrefab;

            #endregion

            #region Properties

            public UnityUIRenderer RendererPrefab
            {
                get
                {
                    return m_rendererPrefab;
                }
                set
                {
                    m_rendererPrefab    = value;
                }
            }

            public UnityUIAlertDialog AlertDialogPrefab
            {
                get
                {
                    return m_alertDialogPrefab;
                }
                set
                {
                    m_alertDialogPrefab   = value;
                }
            }

            public UnityUIDatePicker DatePickerPrefab
            {
                get
                {
                    return m_datePickerPrefab;
                }
                set
                {
                    m_datePickerPrefab    = value;
                }
            }

            #endregion
        }
    }
}
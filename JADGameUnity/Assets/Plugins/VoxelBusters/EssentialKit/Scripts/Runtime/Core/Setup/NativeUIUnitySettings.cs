using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    [Serializable]
    public partial class NativeUIUnitySettings : NativeFeatureUnitySettingsBase
    {
        #region Properties

        [SerializeField]
        [Tooltip("Custom assets references.")]
        private     UnityUICollection       m_customUICollection        = new UnityUICollection();

        #endregion

        #region Properties

        public UnityUICollection CustomUICollection
        {
            get
            {
                return m_customUICollection;
            }
            set
            {
                m_customUICollection      = value;
            }
        }

        #endregion

        #region Constructors

        public NativeUIUnitySettings(bool enabled = true)
            : base(enabled)
        { }

        #endregion

        #region Base class methods

        protected override string GetFeatureName()
        {
            return NativeFeatureType.kNativeUI;
        }

        #endregion
    }
}
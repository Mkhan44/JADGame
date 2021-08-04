using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Represents an object containing additional information related to billing product.
    /// </summary>
    [Serializable]
    public partial class BillingProductDefinition
    {
        #region Fields

        [SerializeField]
        private     string                      m_id;

        [SerializeField]
        private     string                      m_platformId;

        [SerializeField]
        private     NativePlatformConstantSet   m_platformIdOverrides;

        [SerializeField]
        private     BillingProductType          m_productType;

        [SerializeField]
        private     string                      m_title;

        [SerializeField]
        private     string                      m_description;

        [SerializeField, HideInInspector]
        private     IosPlatformProperties       m_iosProperties;

        #endregion

        #region Properties

        /// <summary>
        /// The string that identifies the product within Unity environment. (read-only)
        /// </summary>
        public string Id
        {
            get
            {
                return m_id;
            }
        }

        /// <summary>
        /// The type of the product. (read-only)
        /// </summary>
        public BillingProductType ProductType
        {
            get
            {
                return m_productType;
            }
        }

        /// <summary>
        /// The name of the product. (read-only)
        /// </summary>
        public string Title
        {
            get
            {
                return m_title;
            }
        }

        /// <summary>
        /// The description of the product. (read-only)
        /// </summary>
        public string Description
        {
            get
            {
                return m_description;
            }
        }

        public IosPlatformProperties IosProperties
        {
            get
            {
                return m_iosProperties;
            }
        }

        /// <summary>
        /// Additional information associated with this product. This information is provided by the developer.
        /// </summary>
        public object Tag
        {
            get;
            set;
        }

        #endregion

        #region Create methods

        /// <summary>
        /// Creates the product settings object.
        /// </summary>
        /// <returns>The settings object.</returns>
        /// <param name="id">The string that identifies the product within Unity environment.</param>
        /// <param name="productType">The type of the product.</param>
        /// <param name="title">The name of the product.</param>
        /// <param name="description">The description of the product.</param>
        /// <param name="iosProperties">iOS platform specific settings.</param>
        /// <param name="androidProperties">Android platform specific settings.</param>
        /// <param name="tag">Additional information associated with this product.</param>
        public BillingProductDefinition(string id = null, string platformId = null, NativePlatformConstantSet platformIdOverrides = null, BillingProductType productType = BillingProductType.Consumable, string title = null, string description = null, IosPlatformProperties iosProperties = null, object tag = default(object))
        {
            // set properties
            m_id                    = id;
            m_platformId            = platformId;
            m_platformIdOverrides   = platformIdOverrides ?? new NativePlatformConstantSet();
            m_productType           = productType;
            m_title                 = title;
            m_description           = description;
            m_iosProperties         = iosProperties ?? new IosPlatformProperties(); 
            Tag                     = tag;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the store identifier for active platform.
        /// </summary>
        public string GetPlatformIdForActivePlatform()
        {
            return m_platformIdOverrides.GetConstantForActivePlatform(m_platformId);
        }

        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Simulator
{
    [Serializable]
    public sealed class BillingProductData
    {
        #region Fields

        [SerializeField]
        private     string                  m_id;

        [SerializeField]
        private     BillingProductType      m_productType;

        [SerializeField]
        private     string                  m_localizedTitle;

        [SerializeField]
        private     string                  m_localizedPrice;

        [SerializeField]
        private     string                  m_priceCurrencyCode;

        #endregion

        #region Properties

        public string Id
        {
            get
            {
                return m_id;
            }
        }

        public BillingProductType ProductType
        {
            get
            {
                return m_productType;
            }
        }

        public string LocalizedTitle
        {
            get
            {
                return m_localizedTitle;
            }
        }

        public string LocalizedPrice
        {
            get
            {
                return m_localizedPrice;
            }
        }

        public string PriceCurrencyCode
        {
            get
            {
                return m_priceCurrencyCode;
            }
        }

        #endregion

        #region Constructors

        public BillingProductData(string id, string title, string price, string currencyCode, BillingProductType productType)
        {
            // set properties
            m_id                = id;
            m_productType       = productType;
            m_localizedTitle    = title;
            m_localizedPrice    = price;
            m_priceCurrencyCode = currencyCode;
        }

        #endregion
    }
}
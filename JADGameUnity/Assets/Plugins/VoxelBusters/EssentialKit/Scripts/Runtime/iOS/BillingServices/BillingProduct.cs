#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.BillingServicesCore.iOS
{
    internal sealed class BillingProduct : BillingProductBase
    {
        #region Fields
        
        private     string      m_localizedTitle;

        private     string      m_localizedDescription;

        private     string      m_price;

        private     string      m_localizedPrice;

        private     string      m_priceCurrencyCode;

        #endregion

        #region Constructors

        public BillingProduct(IntPtr nativeObjectPtr, string id, 
            string platformId, string localizedTitle, 
            string localizedDescription, string price, 
            string localizedPrice, string priceCurrencyCode, object tag)
            : base(id: id, platformId: platformId, tag: tag)
        {
            // set properties
            NativeObjectRef         = new IosNativeObjectRef(nativeObjectPtr);
            m_localizedTitle        = localizedTitle;
            m_localizedDescription  = localizedDescription;
            m_price                 = price;
            m_localizedPrice        = localizedPrice;
            m_priceCurrencyCode     = priceCurrencyCode;
        }

        ~BillingProduct()
        {
            Dispose(false);
        }

        #endregion

        #region Base methods

        protected override string GetLocalizedTitleInternal()
        {
            return m_localizedTitle;
        }

        protected override string GetLocalizedDescriptionInternal()
        {
            return m_localizedDescription;
        }

        protected override string GetPriceInternal()
        {
            return m_price;
        }

        protected override string GetLocalizedPriceInternal()
        {
            return m_localizedPrice;
        }

        protected override string GetPriceCurrencyCodeInternal()
        {
            return m_priceCurrencyCode;
        }

        #endregion
    }
}
#endif
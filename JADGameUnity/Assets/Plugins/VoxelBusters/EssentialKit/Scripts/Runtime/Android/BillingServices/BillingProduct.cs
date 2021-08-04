#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Android
{
    internal sealed class BillingProduct : BillingProductBase
    {
#region Fields
        
        private     NativeBillingProduct      m_instance;

#endregion

#region Constructors

        public BillingProduct(string id, NativeBillingProduct nativeBillingProduct, object tag)
            : base(id: id, platformId: nativeBillingProduct.GetProductIdentifier(), tag: tag)
        {
            m_instance = nativeBillingProduct;
        }

        ~BillingProduct()
        {
            Dispose(false);
        }

#endregion

#region Base methods

        protected override string GetLocalizedTitleInternal()
        {
            return m_instance.GetTitle();
        }

        protected override string GetLocalizedDescriptionInternal()
        {
            return m_instance.GetDescription();
        }

        protected override string GetPriceInternal()
        {
            return (m_instance.GetPriceInMicros()/1000000.0f).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        protected override string GetLocalizedPriceInternal()
        {
            return m_instance.GetPrice();
        }

        protected override string GetPriceCurrencyCodeInternal()
        {
            return m_instance.GetCurrencyCode();
        }

#endregion
    }
}
#endif
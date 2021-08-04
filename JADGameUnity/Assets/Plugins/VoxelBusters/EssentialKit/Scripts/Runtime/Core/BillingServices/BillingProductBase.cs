using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.BillingServicesCore
{
    public abstract class BillingProductBase : NativeObjectBase, IBillingProduct
    {
        #region Constructors

        protected BillingProductBase(string id, string platformId, object tag)
        {
            // set properties
            Id          = id;
            PlatformId  = platformId;
            Tag         = tag;
        }

        ~BillingProductBase()
        {
            Dispose(false);
        }

        #endregion

        #region Abstract methods

        protected abstract string GetLocalizedTitleInternal();

        protected abstract string GetLocalizedDescriptionInternal();

        protected abstract string GetPriceInternal();

        protected abstract string GetLocalizedPriceInternal();

        protected abstract string GetPriceCurrencyCodeInternal();

        #endregion

        #region Base methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("BillingProduct { ");
            sb.Append("Id: ").Append(Id).Append(" ");
            sb.Append("Title: ").Append(LocalizedTitle).Append(" ");
            sb.Append("Price: ").Append(Price).Append(" ");
            sb.Append("Localized Price: ").Append(LocalizedPrice).Append(" ");
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IBillingProduct implementation

        public string Id 
        { 
            get; 
            private set; 
        }

        public string PlatformId 
        { 
            get; 
            private set; 
        }

        public string LocalizedTitle
        {
            get
            {
                return GetLocalizedTitleInternal();
            }
        }

        public string LocalizedDescription
        {
            get
            {
                return GetLocalizedDescriptionInternal();
            }
        }

        public string Price
        {
            get
            {
                return GetPriceInternal();
            }
        }

        public string LocalizedPrice
        {
            get
            {
                return GetLocalizedPriceInternal();
            }
        }

        public string PriceCurrencyCode
        {
            get
            {
                return GetPriceCurrencyCodeInternal();
            }
        }

        public object Tag 
        { 
            get; 
            private set; 
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore
{
    public sealed class BillingPayment : IBillingPayment
    {
        #region Constructors

        public BillingPayment(string productPlatformId, int quantity, string tag)
            : this(BillingServices.FindProductDefinitionWithPlatformId(productPlatformId).Id, productPlatformId, quantity, tag) 
        { }

        public BillingPayment(string productId, string productPlatformId, int quantity, string tag)
        {
            // set properties
            ProductId           = productId;
            ProductPlatformId   = productPlatformId;
            Quantity            = quantity;
            Tag                 = tag;
        }

        #endregion

        #region IBillingPayment implementation

        public string ProductId
        { 
            get; 
            private set; 
        }

        public string ProductPlatformId
        { 
            get; 
            private set; 
        }

        public int Quantity
        { 
            get; 
            private set; 
        }

        public string Tag
        { 
            get; 
            private set; 
        }

        #endregion
    }
}
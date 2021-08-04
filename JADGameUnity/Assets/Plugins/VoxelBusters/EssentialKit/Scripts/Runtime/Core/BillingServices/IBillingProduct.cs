using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access information about a product registered in Store.
    /// </summary>
    public interface IBillingProduct
    {
        #region Properties

        /// <summary>
        /// The string that identifies the product within Unity environment. (read-only)
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// The string that identifies the product registered in the Store (platform specific). (read-only)
        /// </summary>
        string PlatformId
        {
            get;
        }

        /// <summary>
        /// The name of the product. (read-only)
        /// </summary>
        string LocalizedTitle
        {
            get;
        }

        /// <summary>
        /// A description of the product. (read-only)
        /// </summary>
        string LocalizedDescription
        {
            get;
        }

        /// <summary>
        /// The cost of the product. (read-only)
        /// </summary>
        string Price
        {
            get;
        }

        /// <summary>
        /// The cost of the product prefixed with local currency symbol. (read-only)
        /// </summary>
        string LocalizedPrice
        {
            get;
        }

        /// <summary>
        /// The currency code of the price. (read-only)
        /// </summary>
        string PriceCurrencyCode
        {
            get;
        }

        /// <summary>
        /// Additional information associated with this product. This information is provided by the developer using <see cref="BillingProductDefinition.Tag"/> property.
        /// </summary>
        /// <value>The tag.</value>
        object Tag
        {
            get;
        }

        #endregion
    }
}
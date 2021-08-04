using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides an interface to access purchase request information.
    /// </summary>
    public interface IBillingPayment
    {
        #region Properties

        /// <summary>
        /// The string that identifies the product within Unity environment. (read-only)
        /// </summary>
        string ProductId
        {
            get;
        }

        /// <summary>
        /// The string that identifies the product registered in the Store (platform specific). (read-only)
        /// </summary>
        string ProductPlatformId
        {
            get;
        }

        /// <summary>
        /// The number of units to be purchased. This should be a non-zero number.
        /// </summary>
        int Quantity
        {
            get;
        }

        /// <summary>
        /// An optional user information provided by the developer at the time of initiating purchase.
        /// </summary>
        string Tag
        {
            get;
        }

        #endregion
    }
}
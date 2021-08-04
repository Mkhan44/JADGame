using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.BillingServicesCore.Simulator
{
    [Serializable]
    internal sealed class BillingStoreSimulatorData
    {
        #region Fields

        [SerializeField]
        private    List<string>     m_purchasedProducts     = new List<string>();

        #endregion

        #region Public methods

        public void AddProductToPurchaseHistory(string pid)
        {
            if (!IsProductPurchased(pid))
            {
                m_purchasedProducts.Add(pid);
            }
        }

        public bool IsProductPurchased(string pid)
        {
            return m_purchasedProducts.Contains(pid);
        }

        public string[] GetPurchasedProductIds()
        {
            return m_purchasedProducts.ToArray();
        }

        #endregion
    }
}
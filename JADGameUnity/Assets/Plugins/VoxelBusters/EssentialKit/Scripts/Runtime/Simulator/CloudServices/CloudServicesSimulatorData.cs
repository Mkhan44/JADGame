using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.CloudServicesCore.Simulator
{
    [Serializable]
    internal sealed class CloudServicesSimulatorData
    {
        #region Fields

        [SerializeField]
        private     List<StringKeyValuePair>    m_keyValues     = new List<StringKeyValuePair>();

        #endregion

        #region Public methods

        public void AddData(string key, string value)
        {
            // create item
            StringKeyValuePair  item    = new StringKeyValuePair() { Key = key, Value = value };

            // insert item
            int                 index   = FindIndexWithKey(key);
            if (index == -1)
            {
                m_keyValues.Add(item);
            }
            else
            {
                m_keyValues[index]      = item;
            }
        }

        public string GetData(string key)
        {
            int     index   = FindIndexWithKey(key);
            if (index != -1)
            {
                return m_keyValues[index].Value;
            }

            return null;
        }

        public bool RemoveData(string key)
        {
            int     index   = FindIndexWithKey(key);
            if (index != -1)
            {
                m_keyValues.RemoveAt(index);
                return true;
            }

            return false;
        }

        #endregion

        #region Private methods

        private int FindIndexWithKey(string key)
        {
            return m_keyValues.FindIndex((item) => string.Equals(key, item.Key));
        }

        #endregion
    }
}
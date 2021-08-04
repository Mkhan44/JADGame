using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        #region Fields

        [SerializeField]
        private     TKey        m_key;

        [SerializeField]
        private     TValue      m_value;

        #endregion

        #region Properties

        public TKey Key
        {
            get
            {
                return m_key;
            }
            set
            {
                m_key   = value;
            }
        }

        public TValue Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }

        #endregion
    }
}
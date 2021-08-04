using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.Simulator
{
    [Serializable]
    public class PlayerData
    {
        #region Fields

        [SerializeField]
        private         string          m_id;

        [SerializeField]
        private         string          m_name;

        #endregion

        #region Properties

        public string Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id    = value;
            }
        }

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name    = value;
            }
        }

        #endregion
    }
}
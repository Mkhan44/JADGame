using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    [Serializable]
    public struct LocationCoordinate 
    {
        #region Fields

        [SerializeField]
        private         double              m_latitude;

        [SerializeField]
        private         double              m_longitude;

        #endregion

        #region Properties

        /// <summary>
        /// The latitude in degrees.
        /// </summary>
        public double Latitude
        {
            get
            {
                return m_latitude;
            }
            set
            {
                m_latitude  = value;
            }
        }

        /// <summary>
        /// The longitude in degrees.
        /// </summary>
        public double Longitude
        {
            get
            {
                return m_longitude;
            }
            set
            {
                m_longitude = value;
            }
        }

        #endregion
    }
}
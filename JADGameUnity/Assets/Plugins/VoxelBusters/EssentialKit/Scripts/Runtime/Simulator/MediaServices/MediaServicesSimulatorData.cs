using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Simulator
{
    [Serializable]
    internal sealed class MediaServicesSimulatorData
    {
        #region Fields

        [SerializeField]
        private     GalleryAccessStatus  m_galleryAccessStatus;

        [SerializeField]
        public      CameraAccessStatus   m_cameraAccessStatus;

        #endregion

        #region Properties

        public GalleryAccessStatus GalleryAccessStatus
        {
            get
            {
                return m_galleryAccessStatus;
            }
            set
            {
                m_galleryAccessStatus   = value;
            }
        }

        public CameraAccessStatus CameraAccessStatus
        {
            get
            {
                return m_cameraAccessStatus;
            }
            set
            {
                m_cameraAccessStatus    = value;
            }
        }

        #endregion

        #region Constructors

        public MediaServicesSimulatorData()
        {
            // set properties
            m_galleryAccessStatus = GalleryAccessStatus.NotDetermined;
            m_cameraAccessStatus  = CameraAccessStatus.NotDetermined;
        }

        #endregion
    }
}
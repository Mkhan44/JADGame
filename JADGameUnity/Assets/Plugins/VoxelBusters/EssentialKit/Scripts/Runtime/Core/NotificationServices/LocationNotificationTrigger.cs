using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// A trigger condition that causes a notification to be delivered when the user's device enters or exits the specified geographic region.
    /// </summary>
    [Serializable]
    public sealed class LocationNotificationTrigger : INotificationTrigger
    {
        #region Fields

        [SerializeField]
        private     CircularRegion      m_region;

        [SerializeField]
        private     bool                m_notifyOnEntry     = false;

        [SerializeField]
        private     bool                m_notifyOnExit      = false;

        [SerializeField]
        private     bool                m_repeats           = false;

        #endregion

        #region Properties

        /// <summary>
        /// The region used to determine when the notification is sent.
        /// </summary>
        public CircularRegion Region
        {
            get
            {
                return m_region;
            }
        }

        /// <summary>
        /// A Boolean indicating that notifications are generated upon entry into the region.
        /// </summary>
        public bool NotifyOnEntry
        {
            get
            {
                return m_notifyOnEntry;
            }
        }

        /// <summary>
        /// A Boolean indicating that notifications are generated upon exit from the region.
        /// </summary>
        public bool NotifyOnExit
        {
            get
            {
                return m_notifyOnExit;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="LocationNotificationTrigger"/> class.
        /// </summary>
        /// <param name="region">Region.</param>
        /// <param name="notifyOnEntry">If set to <c>true</c> notify on entry.</param>
        /// <param name="notifyOnExit">If set to <c>true</c> notify on exit.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        public LocationNotificationTrigger(CircularRegion region, bool notifyOnEntry, bool notifyOnExit, bool repeats)
        { 
            // set properties
            m_region        = region;
            m_notifyOnEntry = notifyOnEntry;
            m_notifyOnExit  = notifyOnExit;
            m_repeats       = repeats;
        }

        #endregion

        #region INotificationTrigger implementation

        public bool Repeats
        {
            get
            {
                return m_repeats;
            }
        }

        #endregion
    }
}
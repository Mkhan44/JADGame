using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// A trigger condition that causes a notification to be delivered at a specific date and time.
    /// </summary>
    [Serializable]
    public sealed class CalendarNotificationTrigger : INotificationTrigger
    {
        #region Fields

        [SerializeField]
        private     DateComponents      m_dateComponent;

        [SerializeField]
        private     bool                m_repeats           = false;

        private     DateTime?           m_nextTriggerDate;

        #endregion

        #region Properties

        /// <summary>
        /// The temporal information to use when constructing the trigger. Provide only the date components that are relevant for your trigger.
        /// </summary>
        public DateComponents DateComponent
        {
            get
            {
                return m_dateComponent;
            }
        }

        /// <summary>
        /// The next date at which the trigger conditions will be met.
        /// </summary>
        public DateTime? NextTriggerDate
        {
            get
            {
                return m_nextTriggerDate;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="CalendarNotificationTrigger"/> class.
        /// </summary>
        /// <param name="dateComponent">Date component.</param>
        /// <param name="nextTriggerDate">Next trigger date.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        public CalendarNotificationTrigger(DateComponents dateComponent, bool repeats, DateTime? nextTriggerDate = null)
        {
            // set properties
            m_dateComponent     = dateComponent;
            m_repeats           = repeats;
            m_nextTriggerDate   = nextTriggerDate;
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
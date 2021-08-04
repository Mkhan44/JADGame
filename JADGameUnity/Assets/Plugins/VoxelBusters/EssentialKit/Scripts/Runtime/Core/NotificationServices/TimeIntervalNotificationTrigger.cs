using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// A trigger condition that causes a notification to be delivered after the specified amount of time elapses.
    /// </summary>
    [Serializable]
    public sealed class TimeIntervalNotificationTrigger : INotificationTrigger
    {
        #region Fields

        [SerializeField]
        private     double      m_timeInterval      = 0;

        [SerializeField]
        private     bool        m_repeats           = false;

        private     DateTime?   m_nextTriggerDate;

        #endregion

        #region Properties

        /// <summary>
        /// The time (in seconds) that must elapse from the current time before the trigger fires. This value must be greater than zero.
        /// </summary>
        public double TimeInterval
        {
            get
            {
                return m_timeInterval;
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
        /// Initializes a new instance of the <see cref="TimeIntervalNotificationTrigger"/> class.
        /// </summary>
        /// <param name="timeInterval">Time interval.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        /// <param name="nextTriggerDate">Next trigger date.</param>
        public TimeIntervalNotificationTrigger(double timeInterval, bool repeats, DateTime? nextTriggerDate = null)
        {
            // set properties
            m_timeInterval      = timeInterval;
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
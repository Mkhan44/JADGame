using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public interface IStringPopupOptionsProvider
    {
        string[] GetValues();
    }

    public abstract class StringPopupAttribute : PropertyAttribute 
    {
        #region Fields

        private     string[]        m_constantOptions;

        #endregion

        #region Properties

        public string[] Options
        {
            get
            {
                return (OptionsProvider != null) ? OptionsProvider.GetValues() : m_constantOptions;
            }
        }

        public IStringPopupOptionsProvider OptionsProvider { get; protected set; }

        #endregion

        #region Constructors

        protected StringPopupAttribute()
        { }

        protected StringPopupAttribute(params string[] options)
        {
            // set properties
            m_constantOptions   = options;
            OptionsProvider     = null; 
        }

        #endregion
    }
}
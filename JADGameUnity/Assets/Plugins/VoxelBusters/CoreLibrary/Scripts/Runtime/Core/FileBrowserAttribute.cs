using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class FileBrowserAttribute : PropertyAttribute
    {
        #region Properties

        public string Extension { get; private set; }

        #endregion

        #region Constructors

        public FileBrowserAttribute(string extension = null)
        {
            // set properties
            Extension = extension;
        }

        #endregion
    }
}
using UnityEngine;
using System.Collections;
using System;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Enumeration values indicating the mode in which application wants to access user's gallery.
    /// </summary>
    [Flags]
    public enum GalleryAccessMode
    {
        /// <summary> The ability to access gallery files. </summary>
        Read        = 1 << 0,

        /// <summary> The ability to access and write files to gallery. </summary>
        ReadWrite   = 1 << 1,
    }
}
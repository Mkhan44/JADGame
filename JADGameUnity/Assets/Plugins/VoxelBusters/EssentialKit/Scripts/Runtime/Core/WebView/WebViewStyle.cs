using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Enumeration for supported webview control styles.
    /// </summary>
    public enum WebViewStyle
    {
        /// <summary> 
        /// No controls are shown for web view. This appearence is ideal for banner ads like requirement. 
        /// </summary>
        Default,

        /// <summary>
        /// This option creates a close button at top-right corner of the web view. On clicking this, web view gets dismissed. 
        /// </summary>
        /// <remarks>
        /// \note Incase if you want to permanetly remove web view instance, use <see cref="WebView.Destory"/>.
        /// </remarks>
        Popup,

        /// <summary>
        /// This option provides browser like appearence with 4 buttons for easy access to web view features.
        /// </summary>
        /// <description>
        /// It has Back and Forward buttons for navigating through the history.
        /// Reload button for reloading the current webpage contents.
        /// And finally, Done button for dismissing the web view.
        /// </description>
        Browser
    }
}
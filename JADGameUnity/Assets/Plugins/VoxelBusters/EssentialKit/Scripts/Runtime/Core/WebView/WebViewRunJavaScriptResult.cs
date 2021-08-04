using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="WebView.RunJavaScript(string, EventCallback{WebViewRunJavaScriptResult})"/> operation is completed.
    /// </summary>
    public class WebViewRunJavaScriptResult
    {
        #region Properties

        /// <summary>
        /// The result returned on completing js code.
        /// </summary>
        public string Result
        {
            get;
            internal set;
        }

        #endregion
    }
}
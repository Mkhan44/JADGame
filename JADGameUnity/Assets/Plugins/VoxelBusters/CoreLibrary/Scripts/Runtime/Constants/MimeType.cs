using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// The MimeType class is a collection of most commonly used MIME types. 
    /// </summary>
    /// <description>
    /// MIME types enable apps to recognize the filetype of a file. 
    /// </description>
    public static class MimeType
    {
        #region Constants

        /// <summary> The MIME value used to determine plain text file. (Readonly)</summary>
        public  const   string      kPlainText          = "text/plain";

        /// <summary> The MIME value used to determine normal web pages. (Readonly)</summary>
        public  const   string      kHtmlText           = "text/html";

        /// <summary> The MIME value used to determine javascript content. (Readonly)</summary>
        public  const   string      kJavaScriptText     = "text/javascript";

        /// <summary> The MIME value used to determine jpg image file. (Readonly)</summary>
        public  const   string      kJPGImage           = "image/jpeg";

        /// <summary> The MIME value used to determine png image file. (Readonly)</summary>
        public  const   string      kPNGImage           = "image/png";

        /// <summary> The MIME value used to determine Adobe® PDF documents. (Readonly)</summary>
        public  const   string      kPDF                = "application/pdf";

        #endregion
    }
}
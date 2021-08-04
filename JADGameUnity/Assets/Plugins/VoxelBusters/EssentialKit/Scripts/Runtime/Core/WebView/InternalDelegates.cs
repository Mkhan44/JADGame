using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.WebViewCore
{
    public delegate void WebViewInternalCallback(Error error);

    public delegate void RunJavaScriptInternalCallback(string result, Error error);

    public delegate void URLSchemeMatchFoundInternalCallback(string url);
}
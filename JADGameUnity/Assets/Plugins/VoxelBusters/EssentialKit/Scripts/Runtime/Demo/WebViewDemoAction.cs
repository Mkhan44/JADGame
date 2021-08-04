using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum WebViewDemoActionType
	{
        Create,
        SetRect,
        SetControls,
        SetAutoShowOnLoadFinish,
        SetScalesPageToFit,
        SetCanBounce,
        SetJavaScriptEnabled,
        GetURL,
        GetTitle,
        GetProgress,
        GetIsLoading,
        Show,
        Hide,
        LoadURL,
        LoadHtmlString,
        LoadData,
        Reload,
        StopLoading,
        RunJavaScript,
        ClearCache,
        SetFullScreenRect,
        LoadTexture,
        ResourcePage
	}

	public class WebViewDemoAction : DemoActionBehaviour<WebViewDemoActionType> 
	{}
}
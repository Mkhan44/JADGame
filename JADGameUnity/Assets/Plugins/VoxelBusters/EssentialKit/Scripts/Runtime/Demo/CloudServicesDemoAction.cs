using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum CloudServicesDemoActionType
	{
		GetBool,
		SetBool,
        GetLong,
        SetLong,
        GetDouble,
        SetDouble,
        GetString,
        SetString,
        Synchronize,
        RemoveKey,
		ResourcePage,
	}

	public class CloudServicesDemoAction : DemoActionBehaviour<CloudServicesDemoActionType> 
	{}
}
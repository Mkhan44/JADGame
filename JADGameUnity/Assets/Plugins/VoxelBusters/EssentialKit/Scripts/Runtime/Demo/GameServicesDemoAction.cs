using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum GameServicesDemoActionType
	{
        IsAuthenticated,
        Authenticate,
        LocalPlayer,
        Signout,
		LoadLeaderboards,
        ReportScore,
        LoadAchievementDescriptions,
        LoadAchievements,
        ReportAchievementProgress,
        GetNumOfStepsRequiredToUnlockAchievement,
        ShowLeaderboards,
        ShowAchievements,
        LoadServerCredentials,
		ResourcePage,
	}

	public class GameServicesDemoAction : DemoActionBehaviour<GameServicesDemoActionType> 
	{}
}
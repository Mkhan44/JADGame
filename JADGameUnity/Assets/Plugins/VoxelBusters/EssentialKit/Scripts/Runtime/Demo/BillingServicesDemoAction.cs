using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum BillingServicesDemoActionType
	{
		InitializeStore,
		CanMakePayments,
        IsProductPurchased,
        BuyProduct,
        GetTransactions,
        FinishTransactions,
        RestorePurchases,
		ResourcePage,
	}

	public class BillingServicesDemoAction : DemoActionBehaviour<BillingServicesDemoActionType> 
	{}
}
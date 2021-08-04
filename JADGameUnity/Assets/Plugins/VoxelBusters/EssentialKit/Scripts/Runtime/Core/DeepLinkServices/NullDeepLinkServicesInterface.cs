using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore
{
    public class NullDeepLinkServicesInterface : NativeDeepLinkServicesInterfaceBase, INativeDeepLinkServicesInterface
    {
        #region Constructors

        public NullDeepLinkServicesInterface()
            : base(isAvailable: false)
        { }
            
        #endregion

        #region Base methods

        public override void Init()
        {
            Diagnostics.LogNotSupported("DeepLinkServices");
        }

        #endregion
    }
}
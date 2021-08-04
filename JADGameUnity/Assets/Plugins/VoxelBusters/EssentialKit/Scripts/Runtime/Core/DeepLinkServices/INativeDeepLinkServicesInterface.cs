using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore
{
    public interface INativeDeepLinkServicesInterface : INativeFeatureInterface
    {
        #region Events

        event DynamicLinkOpenInternalCallback OnCustomSchemeUrlOpen;

        event DynamicLinkOpenInternalCallback OnUniversalLinkOpen;

        #endregion

        #region Methods

        void Init();

        void SetCanHandleCustomSchemeUrl(CanHandleDynamicLinkInternal handler);

        void SetCanHandleUniversalLink(CanHandleDynamicLinkInternal handler);

        #endregion
    }
}
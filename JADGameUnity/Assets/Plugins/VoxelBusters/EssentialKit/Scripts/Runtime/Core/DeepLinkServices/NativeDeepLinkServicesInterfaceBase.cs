using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore
{
    public abstract class NativeDeepLinkServicesInterfaceBase : NativeFeatureInterfaceBase, INativeDeepLinkServicesInterface
    {
        #region Fields

        private     CanHandleDynamicLinkInternal    m_canHandleCustomSchemeUrl;

        private     CanHandleDynamicLinkInternal    m_canHandleUniversalLink;

        #endregion

        #region Constructors

        protected NativeDeepLinkServicesInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeDeepLinkServicesInterface implementation
            
        public event DynamicLinkOpenInternalCallback OnCustomSchemeUrlOpen;

        public event DynamicLinkOpenInternalCallback OnUniversalLinkOpen;
        
        public void SetCanHandleCustomSchemeUrl(CanHandleDynamicLinkInternal handler)
        {
            m_canHandleCustomSchemeUrl  = handler;
        }

        public void SetCanHandleUniversalLink(CanHandleDynamicLinkInternal handler)
        {
            m_canHandleUniversalLink    = handler;
        }

        public abstract void Init();

        #endregion

        #region Private methods

        protected bool CanHandleCustomSchemeUrl(string url)
        {
            return m_canHandleCustomSchemeUrl(url);
        }

        protected bool CanHandleUniversalLink(string url)
        {
            return m_canHandleUniversalLink(url);
        }

        protected void SendCustomSchemeUrlOpenEvent(string url)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnCustomSchemeUrlOpen(url));
        }

        protected void SendUniversalLinkOpenEvent(string url)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnUniversalLinkOpen(url));
        }

        #endregion
    }
}
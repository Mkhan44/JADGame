#if UNITY_IOS || UNITY_TVOS
using AOT;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore.iOS
{
    public sealed class DeepLinkServicesInterface : NativeDeepLinkServicesInterfaceBase, INativeDeepLinkServicesInterface
    {
        #region Static fields

        private static  bool                        s_initialised   = false;

        private static  DeepLinkServicesInterface   s_instance      = null;

        #endregion

        #region Constructors

        public DeepLinkServicesInterface() 
            : base(isAvailable: true)
        {
            if (!s_initialised)
            {
                s_initialised       = true;
                DeepLinkServicesBinding.NPDeepLinkServicesRegisterCallbacks(HandleCustomSchemeUrl, HandleUniversalLink);
            }

            // save reference
            s_instance  = this;
        }

        #endregion

        #region Base methods

        public override void Init()
        {
            DeepLinkServicesBinding.NPDeepLinkServicesInit();
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(HandleCustomSchemeUrlNativeCallback))]
        private static bool HandleCustomSchemeUrl(string url)
        {
            if (s_instance.CanHandleCustomSchemeUrl(url))
            {
                s_instance.SendCustomSchemeUrlOpenEvent(url);
                return true;
            }

            return false;
        }

        [MonoPInvokeCallback(typeof(HandleUniversalLinkNativeCallback))]
        private static bool HandleUniversalLink(string url)
        {
            if (s_instance.CanHandleUniversalLink(url))
            {
                s_instance.SendUniversalLinkOpenEvent(url);
                return true;
            }

            return false;
        }

        #endregion
    }
}
#endif
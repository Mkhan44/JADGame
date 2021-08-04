#if UNITY_IOS || UNITY_TVOS
using System.Runtime.InteropServices;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore.iOS
{
    internal static class DeepLinkServicesBinding
    {
        [DllImport("__Internal")]
        public static extern void NPDeepLinkServicesRegisterCallbacks(HandleCustomSchemeUrlNativeCallback handleCustomSchemeUrlCallback, HandleUniversalLinkNativeCallback handleUniversalLinkCallback);

        [DllImport("__Internal")]
        public static extern void NPDeepLinkServicesInit();
    }
}
#endif
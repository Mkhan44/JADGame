#if UNITY_ANDROID
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    internal sealed class LocalPlayer : Player, ILocalPlayer
    {
        #region Constructors

        public LocalPlayer() : base(null)
        {

        }

        #endregion

        #region ILocalPlayer implementation

        public bool IsAuthenticated
        {
            get
            {
                return m_instance != null;
            }
        }

        public bool IsUnderAge
        {
            get
            {
                DebugLogger.LogWarning("This always returns false on Android");
                return false;
            }
        }

        #endregion

        #region Internal methods

        internal void SetPlayer(NativeGamePlayer player)
        {
            m_instance = player;
        }

        #endregion
    }
}
#endif
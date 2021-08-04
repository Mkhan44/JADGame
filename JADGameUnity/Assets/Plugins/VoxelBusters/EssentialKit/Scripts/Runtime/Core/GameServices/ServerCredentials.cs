namespace VoxelBusters.EssentialKit
{
    public partial class ServerCredentials
    {
        #region Fields

        private IosPlatformProperties m_iosProperties;
        private AndroidPlatformProperties m_androidProperties;

        #endregion

        #region Properties

        public IosPlatformProperties IosProperties
        {
            get
            {
                return m_iosProperties;
            }
        }

        public AndroidPlatformProperties AndroidProperties
        {
            get
            {
                return m_androidProperties;
            }
        }

        #endregion

        #region Constructors

        public ServerCredentials(IosPlatformProperties iosProperties = null, AndroidPlatformProperties androidProperties = null)
        {
            m_iosProperties = iosProperties;
            m_androidProperties = androidProperties;
        }

        #endregion
    }
}

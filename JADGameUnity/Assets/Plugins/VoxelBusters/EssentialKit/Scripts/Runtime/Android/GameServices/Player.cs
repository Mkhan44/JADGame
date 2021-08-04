#if UNITY_ANDROID
using System;

using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    internal class Player : PlayerBase
    {
        # region Fields

        protected NativeGamePlayer m_instance;

        #endregion
        #region Constructors

        public Player(NativeGamePlayer nativePlayer)
        {
            m_instance = nativePlayer;
        }

        ~Player()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        protected override string GetIdInternal()
        {
            return m_instance.GetId();
        }

        protected override string GetAliasInternal()
        {
            return m_instance.GetName();
        }

        protected override string GetDisplayNameInternal()
        {
            return m_instance.GetDisplayName();
        }

        protected override void LoadImageInternal(LoadImageInternalCallback callback)
        {
            //#warning What needs to be done if no image exists? How its handled on iOS. Any option for setting default image in settings? Current one just passes null if no image exists without error
            NativeAsset asset = m_instance.GetProfileImage(true);
            asset.LoadRemote(new NativeLoadAssetListener()
            {
                onSuccessCallback = (NativeByteBuffer data) =>
                {
                    callback(data.GetBytes(), null);
                },
                onFailureCallback = (error) =>
                {
                    callback(null, new Error(error));
                }
            });
        }

        #endregion
    }
}
#endif
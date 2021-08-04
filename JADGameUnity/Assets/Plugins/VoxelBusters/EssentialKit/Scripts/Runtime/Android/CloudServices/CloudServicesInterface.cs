#if UNITY_ANDROID
using System.Collections;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;
using System;

namespace VoxelBusters.EssentialKit.CloudServicesCore.Android
{
    public sealed class CloudServicesInterface : NativeCloudServicesInterfaceBase
    {
        #region Static fields

        private NativeCloudServices m_instance;

        #endregion

        #region Constructors

        public CloudServicesInterface()
            : base(isAvailable: true)
        {
            m_instance = new NativeCloudServices(NativeUnityPluginUtility.GetContext());
            m_instance.SetExternalDataChangedListener(new NativeExternalDataChangedListener()
            {
                onChangeCallback = (NativeExternalChangeReason reason, string[] keys, NativeJSONObject localCopy) =>
                {
                    SendSavedDataChangeEvent(Converter.from(reason), keys);
                }
            });

            m_instance.SetUserChangedListener(new NativeOnUserChangedListener()
            {
                onChangeCallback = (string userId) =>
                {
                    SendUserChangeEvent(new CloudUser(userId, CloudUserAccountStatus.Available), null);
                }
            });

            m_instance.SetSyncInterval(CloudServices.UnitySettings.SyncInterval);
        }

        ~CloudServicesInterface()
        {
            Dispose(false);
        }

        #endregion

        #region Base class methods

        public override bool GetBool(string key)
        {
            return m_instance.GetBool(key);
        }

        public override long GetLong(string key)
        {
            return m_instance.GetLong(key);
        }

        public override double GetDouble(string key)
        {
            return m_instance.GetDouble(key);
        }

        public override string GetString(string key)
        {
            return m_instance.GetString(key);
        }

        public override byte[] GetByteArray(string key)
        {
            NativeByteBuffer nativeByteBuffer = m_instance.GetByteArray(key);
            return nativeByteBuffer.GetBytes();
        }

        public override void SetBool(string key, bool value)
        {
            m_instance.SetBool(key, value);
        }

        public override void SetLong(string key, long value)
        {
            m_instance.SetLong(key, value);
        }

        public override void SetDouble(string key, double value)
        {
            m_instance.SetDouble(key, value);
        }

        public override void SetString(string key, string value)
        {
            m_instance.SetString(key, value);
        }

        public override void SetByteArray(string key, byte[] value)
        {
            m_instance.SetByteArray(key, new NativeByteBuffer(value));
        }

        public override void RemoveKey(string key)
        {
            m_instance.RemoveKey(key);
        }

        public override void Synchronize(SynchronizeInternalCallback callback)
        {
            m_instance.Syncronize(new NativeSyncronizeListener()
            {
                onSuccessCallback = () =>
                {
                    callback(true, null);
                },
                onFailureCallback = (String error) =>
                {
                    callback(false, new Error(error));
                }

            });
        }

        public override IDictionary GetSnapshot()
        {
            string jsonStr = m_instance.GetSnapshot();
            if (jsonStr != null)
            {
                return (IDictionary)ExternalServiceProvider.JsonServiceProvider.FromJson(jsonStr);
            }

            return null;
        }

        #endregion
    }
}
#endif
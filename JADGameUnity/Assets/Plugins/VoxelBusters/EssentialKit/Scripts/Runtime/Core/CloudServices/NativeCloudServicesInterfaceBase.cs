using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.CloudServicesCore
{
    public abstract class NativeCloudServicesInterfaceBase : NativeFeatureInterfaceBase, INativeCloudServicesInterface
    {
        #region Constructors

        protected NativeCloudServicesInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeCloudServicesInterface implementation
            
        public event UserChangeInternalCallback OnUserChange;

        public event SavedDataChangeInternalCallback OnSavedDataChange;

        public abstract bool GetBool(string key);

        public abstract long GetLong(string key);

        public abstract double GetDouble(string key);

        public abstract string GetString(string key);

        public abstract byte[] GetByteArray(string key);

        public abstract void SetBool(string key, bool value);

        public abstract void SetLong(string key, long value);

        public abstract void SetDouble(string key, double value);

        public abstract void SetString(string key, string value);

        public abstract void SetByteArray(string key, byte[] value);

        public abstract void RemoveKey(string key);

        public abstract void Synchronize(SynchronizeInternalCallback callback);

        public abstract IDictionary GetSnapshot();

        #endregion

        #region Private methods

        protected void SendUserChangeEvent(CloudUser user, Error error)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnUserChange(user, error));
        }

        protected void SendSavedDataChangeEvent(CloudSavedDataChangeReasonCode changeReason, string[] changedKeys)
        {
            CallbackDispatcher.InvokeOnMainThread(() => OnSavedDataChange(changeReason, changedKeys));
        }

        #endregion
    }
}
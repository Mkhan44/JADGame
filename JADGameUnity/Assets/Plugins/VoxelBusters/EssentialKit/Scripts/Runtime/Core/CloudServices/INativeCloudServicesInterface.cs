using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.CloudServicesCore
{
    public interface INativeCloudServicesInterface : INativeFeatureInterface
    {
        #region Events

        event UserChangeInternalCallback OnUserChange;

        event SavedDataChangeInternalCallback OnSavedDataChange;

        #endregion

        #region Getting values methods

        bool GetBool(string key);

        long GetLong(string key);

        double GetDouble(string key);

        string GetString(string key);

        byte[] GetByteArray(string key);

        #endregion

        #region Setting values methods

        void SetBool(string key, bool value);

        void SetLong(string key, long value);

        void SetDouble(string key, double value);

        void SetString(string key, string value);

        void SetByteArray(string key, byte[] value);

        #endregion

        #region Removing keys methods

        void RemoveKey(string key);

        #endregion

        #region Other methods

        void Synchronize(SynchronizeInternalCallback callback);

        IDictionary GetSnapshot();

        #endregion
    }
}
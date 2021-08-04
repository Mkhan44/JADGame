using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.CloudServicesCore
{
    public class NullCloudServicesInterface : NativeCloudServicesInterfaceBase, INativeCloudServicesInterface
    {
        #region Constructors

        public NullCloudServicesInterface()
            : base(isAvailable: false)
        {
            LogNotSupported();
        }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("CloudServices");
        }

        #endregion

        #region Base methods

        public override bool GetBool(string key)
        {
            LogNotSupported();

            return false;
        }

        public override long GetLong(string key)
        { 
            LogNotSupported();

            return 0L;
        }

        public override double GetDouble(string key)
        { 
            LogNotSupported();

            return 0d;
        }

        public override string GetString(string key)
        { 
            LogNotSupported();

            return null;
        }

        public override byte[] GetByteArray(string key)
        {
            LogNotSupported();

            return null;
        }

        public override void SetBool(string key, bool value)
        { 
            LogNotSupported();
        }

        public override void SetLong(string key, long value)
        { 
            LogNotSupported();
        }

        public override void SetDouble(string key, double value)
        { 
            LogNotSupported();
        }

        public override void SetString(string key, string value)
        { 
            LogNotSupported();
        }

        public override void SetByteArray(string key, byte[] value)
        {
            LogNotSupported();
        }

        public override void RemoveKey(string key)
        { 
            LogNotSupported();
        }

        public override void Synchronize(SynchronizeInternalCallback callback)
        {
            LogNotSupported();

            callback(false, null);
        }

        public override IDictionary GetSnapshot()
        {
            LogNotSupported();

            return null;
        }

        #endregion
    }
}
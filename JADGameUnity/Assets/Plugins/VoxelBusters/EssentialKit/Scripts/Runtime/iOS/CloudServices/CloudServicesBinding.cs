#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.CloudServicesCore.iOS
{
    internal static class CloudServicesBinding
    {
        [DllImport("__Internal")]
        public static extern void NPCloudServicesRegisterCallbacks(UserChangeNativeCallback userChangeCallback, SavedDataChangeNativeCallback savedDataChangeCallback);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesInit();

        [DllImport("__Internal")]
        public static extern bool NPCloudServicesGetBool(string key);

        [DllImport("__Internal")]
        public static extern long NPCloudServicesGetLong(string key);

        [DllImport("__Internal")]
        public static extern double NPCloudServicesGetDouble(string key);

        [DllImport("__Internal")]
        public static extern string NPCloudServicesGetString(string key);

        [DllImport("__Internal")]
        public static extern byte[] NPCloudServicesGetByteArray(string key, out int length);

        [DllImport("__Internal")]
        public static extern string NPCloudServicesGetArray(string key);

        [DllImport("__Internal")]
        public static extern string NPCloudServicesGetDictionary(string key);
        
        [DllImport("__Internal")]
        public static extern void NPCloudServicesSetBool(string key, bool value);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesSetLong(string key, long value);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesSetDouble(string key, double value);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesSetString(string key, string value);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesSetByteArray(string key, byte[] value, int length);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesSetArray(string key, string valueAsJSONStr);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesSetDictionary(string key, string valueAsJSONStr);

        [DllImport("__Internal")]
        public static extern void NPCloudServicesRemoveKey(string key);
        
        [DllImport("__Internal")]
        public static extern bool NPCloudServicesSynchronize();

        [DllImport("__Internal")]
        public static extern string NPCloudServicesSnapshot();
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;

namespace VoxelBusters.EssentialKit.CloudServicesCore.Simulator
{
    public sealed class CloudServicesSimulator : SingletonObject<CloudServicesSimulator>
    {
        #region Fields

        private         CloudServicesSimulatorData      m_simulatorData     = null;

        #endregion

        #region Constructors

        private CloudServicesSimulator()
        {
            // set properties
            m_simulatorData     = null;

            DebugLogger.Log("Call syncronise first to get latest copy of cloud data");
        }

        #endregion

        #region Database methods

        private CloudServicesSimulatorData LoadFromDisk()
        {
            return SimulatorDatabase.Instance.GetObject<CloudServicesSimulatorData>(NativeFeatureType.kCloudServices);
        }

        private void SaveData()
        {
            SimulatorDatabase.Instance.SetObject(NativeFeatureType.kCloudServices, m_simulatorData);
        }

        public static void Reset() 
        {
            SimulatorDatabase.Instance.RemoveObject(NativeFeatureType.kCloudServices);
        }

        #endregion

        #region Public static methods

        public void AddData(string key, string value)
        {
            if (m_simulatorData == null)
            {
                m_simulatorData = new CloudServicesSimulatorData();
            }
            m_simulatorData.AddData(key, value);
        }

        public string GetData(string key)
        {
            return m_simulatorData.GetData(key);
        }

        public bool RemoveData(string key)
        {
            return m_simulatorData.RemoveData(key);
        }

        public string GetSnapshot()
        {
            return JsonUtility.ToJson(m_simulatorData);
        }

        public void Synchronize()
        {
            if (m_simulatorData == null)
            {
                m_simulatorData = LoadFromDisk() ?? new CloudServicesSimulatorData();
                return;
            }

            SaveData();
        }

        #endregion
    }
}
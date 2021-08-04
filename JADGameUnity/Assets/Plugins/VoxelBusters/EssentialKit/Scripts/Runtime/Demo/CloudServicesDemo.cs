using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class CloudServicesDemo : DemoActionPanelBase<CloudServicesDemoAction, CloudServicesDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     InputField      m_keyField          = null;

        [SerializeField]
        private     InputField      m_valueField        = null;

        #endregion

        #region Base class methods

        protected override void OnEnable()
        {
            base.OnEnable();

            // register for events
            CloudServices.OnUserChange              += OnUserChange;
            CloudServices.OnSavedDataChange         += OnSavedDataChange;
            CloudServices.OnSynchronizeComplete     += OnSynchronizeComplete;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // unregister from events
            CloudServices.OnUserChange              -= OnUserChange;
            CloudServices.OnSavedDataChange         -= OnSavedDataChange;
            CloudServices.OnSynchronizeComplete     -= OnSynchronizeComplete;
        }

        protected override void OnActionSelectInternal(CloudServicesDemoAction selectedAction)
        {
            string key  = GetKey();

            switch (selectedAction.ActionType)
            {
                case CloudServicesDemoActionType.GetBool:
                    bool    result1     = CloudServices.GetBool(key);
                    Log("Saved bool value: " + result1);
                    break;

                case CloudServicesDemoActionType.SetBool:
                    CloudServices.SetBool(key, GetInputValueAsBool());
                    Log("Bool value added.");
                    break;

                case CloudServicesDemoActionType.GetLong:
                    long    result2     = CloudServices.GetLong(key);
                    Log("Saved long value: " + result2);
                    break;

                case CloudServicesDemoActionType.SetLong:
                    CloudServices.SetLong(key, GetInputValueAsLong());
                    Log("Long value added.");
                    break;

                case CloudServicesDemoActionType.GetDouble:
                    double  result3     = CloudServices.GetDouble(key);
                    Log("Saved double value: " + result3);
                    break;

                case CloudServicesDemoActionType.SetDouble:
                    CloudServices.SetDouble(key, GetInputValueAsDouble());
                    Log("Double value added.");
                    break;

                case CloudServicesDemoActionType.GetString:
                    string  result4     = CloudServices.GetString(key);
                    Log("Saved string value: " + result4);
                    break;

                case CloudServicesDemoActionType.SetString:
                    CloudServices.SetString(key, GetInputValueAsString());
                    Log("String value added.");
                    break;

                case CloudServicesDemoActionType.Synchronize:
                    CloudServices.Synchronize();
                    break;

                case CloudServicesDemoActionType.RemoveKey:
                    CloudServices.RemoveKey(key);
                    Log("Removed key: " + key);
                    break;

                case CloudServicesDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kCloudServices);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Plugin event methods

        private void OnUserChange(CloudServicesUserChangeResult result, Error error)
        {
            var     user    = result.User;
            Log("Received user change callback.");
            Log("User id: " + user.UserId);
            Log("User status: " + user.AccountStatus);
        }

        private void OnSavedDataChange(CloudServicesSavedDataChangeResult result)
        {
            var     changedKeys = result.ChangedKeys;
            Log("Received saved data change callback.");
            Log("Reason: " + result.ChangeReason);
            Log("Total changed keys: " + changedKeys.Length);
            Log("Here are the changed keys:");
            for (int iter = 0; iter < changedKeys.Length; iter++)
            {
                Log(string.Format("[{0}]: {1}", iter, changedKeys[iter]));
            }
        }

        private void OnSynchronizeComplete(CloudServicesSynchronizeResult result)
        {
            Log("Received synchronize finish callback.");
            Log("Status: " + result.Success);
        }

        #endregion

        #region Private methods

        private string GetKey()
        {
            string  key = m_keyField.text;
            return string.IsNullOrEmpty(key) ? null : m_keyField.text;
        }

        private bool GetInputValueAsBool()
        {
            int     value;
            if (int.TryParse(m_valueField.text, out value))
            {
                return value > 0;
            }
            else
            {
                return bool.TrueString.ToLower().Equals(m_valueField.text.ToLower());
            }
        }

        private long GetInputValueAsLong()
        {
            long    value;
            long.TryParse(m_valueField.text, out value);

            return value;
        }

        private double GetInputValueAsDouble()
        {
            double  value;
            double.TryParse(m_valueField.text, out value);

            return value;
        }

        private string GetInputValueAsString()
        {
            return m_valueField.text;
        }

        #endregion
    }
}

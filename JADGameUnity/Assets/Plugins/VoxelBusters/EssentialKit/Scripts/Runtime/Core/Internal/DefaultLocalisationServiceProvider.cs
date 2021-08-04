using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public class DefaultLocalisationServiceProvider : ILocalisationServiceProvider
    {
        #region ILocalisationServiceProvider implementation

        public string GetLocalisedString(string key, string defaultValue)
        {
            return defaultValue;
        }

        #endregion
    }
}
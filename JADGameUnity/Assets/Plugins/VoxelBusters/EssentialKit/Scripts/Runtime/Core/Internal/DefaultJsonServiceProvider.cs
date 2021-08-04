using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using JsonUtility = VoxelBusters.Parser.JsonUtility;

namespace VoxelBusters.EssentialKit
{
    public class DefaultJsonServiceProvider : IJsonServiceProvider
    {
        #region IJsonServiceProvider implementation

        public string ToJson(object obj)
        {
            return JsonUtility.ToJSON(obj);
        }

        public object FromJson(string jsonString)
        {
            return JsonUtility.FromJSON(jsonString);
        }

        #endregion
    }
}
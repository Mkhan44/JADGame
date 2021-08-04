using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public interface ILocalisationServiceProvider
    {
        string GetLocalisedString(string key, string defaultValue);
    }
}
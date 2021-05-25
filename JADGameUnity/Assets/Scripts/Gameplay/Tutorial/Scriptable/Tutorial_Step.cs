using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tutorial_Step : ScriptableObject
{
    public enum stepType
    {
        none,
        jumpButton,
        duckButton,
        hang,
        crouch,
        useHandwarmer,
        useDefroster,
        getHit,
        burning,
        frozen
    }

    [Tooltip("If hasStep is false, leave this as 'none'. Otherwise this should correspond to what step is needed.")]
    public stepType thisStepType;

    public bool hasStep;

    public bool isSpecial;

    public float waitTime;
}

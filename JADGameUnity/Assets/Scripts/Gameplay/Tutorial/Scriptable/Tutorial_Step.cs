using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    public enum arrowAnim
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    [Tooltip("If hasStep is false, leave this as 'none'. Otherwise this should correspond to what step is needed.")]
    public stepType thisStepType;

    [Tooltip("If an arrow is needed, this should not be none. Otherwise, leave it.")]
    public List<arrowAnim> arrowAnimationPositions = new List<arrowAnim>();


    [Tooltip("Names of objects to find in the scene.")]
    public List<string> objsInSceneRef = new List<string>();
    [Tooltip("Offsets based on each object we're referencing. Basically how far arrow should be from this. 0,0,0 = on top of it.")]
    public List<Vector3> arrowOffsets = new List<Vector3>();
    public bool hasStep;

    public bool isSpecial;

    public float waitTime;
}

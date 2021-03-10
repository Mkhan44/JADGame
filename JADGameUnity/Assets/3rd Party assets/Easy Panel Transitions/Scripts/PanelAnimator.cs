using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimator : MonoBehaviour {
    
	[Header ("In Animation")]

    [Tooltip("From (0,0) to (1,1), the curve used by the animation")]
	public AnimationCurve inAnim;

    [Tooltip("How long this Animation should take (Seconds)")]
	public float inAnimDuration;

    [Tooltip("What is the final position of this Animation?")]
	public Vector3 inAnimEndPosition;

	[Header("Out Animation")]

    [Tooltip("From (0,0) to (1,1), the curve used by the animation")]
	public AnimationCurve outAnim;

    [Tooltip("How long this Animation should take (Seconds)")]
	public float outAnimDuration;

    [Tooltip("What is the final position of this Animation?")]
	public Vector3 outAnimEndPosition;

	[Header("Animation Controllers")]

    [Tooltip("Should the object begin off-screen?")]
	public bool startOffScreen;

    [Tooltip("When the scene starts, should this panel animate in?")]
    public bool AnimateInOnStart;

    [Tooltip("The RectTransform of the UI Object")]

	private RectTransform animObject;
	public enum AnimState
	{
        none,
		into,
		outro
	}
	private AnimState animState;
	private Vector3 animStartPosition;
    private float animationStartTime;
	private float animationEndTime;

	public void Awake()
	{
		animState = AnimState.none;
	}

	// Use this for initialization
	void Start () 
    {
        animObject = GetComponent<RectTransform>();

		if(startOffScreen)
			animObject.localPosition = outAnimEndPosition;

        if (AnimateInOnStart)
        {
            animObject.localPosition = outAnimEndPosition;
            StartAnimIn();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
		switch(animState)
		{
			case AnimState.into:
			{
				AnimateIn();
				break;
			}
			case AnimState.outro:
			{
				AnimateOut();
				break;
			}
			case AnimState.none:
			{
				// do nothing
				break;
			}
		}
	}

    // Manage beginning the "in" animation
	public void StartAnimIn()
	{
        // set up variables and change the state
        animState = AnimState.none;
        animationStartTime = Time.timeSinceLevelLoad;
        animationEndTime = Time.timeSinceLevelLoad + inAnimDuration;
		animStartPosition = animObject.localPosition;
        animState = AnimState.into;
	}

    // Manage beginning the "out" animation
	public void StartAnimOut()
	{
        // set up variables and change the state
        animState = AnimState.none;
        animationStartTime = Time.timeSinceLevelLoad;
		animationEndTime = Time.timeSinceLevelLoad + outAnimDuration;
		animStartPosition = animObject.localPosition;
        animState = AnimState.outro;
	}

    private void AnimateIn()
	{
        // every frame, update the animation
        if (animationEndTime >= Time.timeSinceLevelLoad)
        {
            // Get the appropriate value from the AnimationCurve using a normalised value
            float curveValue = inAnim.Evaluate((Time.timeSinceLevelLoad - animationStartTime) / (animationEndTime - animationStartTime));

            // lerp between positions
            animObject.localPosition = Vector3.LerpUnclamped(animStartPosition, inAnimEndPosition, curveValue);
        }
		else
		{
			// clean up and end the animation
			animObject.localPosition = inAnimEndPosition; // Force the object to its destination
            animState = AnimState.none;
		}
	}

	private void AnimateOut()
	{
        // every frame, update the animation
		if(animationEndTime >= Time.timeSinceLevelLoad)
		{
            // Get the appropriate value from the AnimationCurve using a normalised value
            float curveValue = inAnim.Evaluate((Time.timeSinceLevelLoad - animationStartTime) / (animationEndTime - animationStartTime));

			// lerp between positions
            animObject.localPosition = Vector3.LerpUnclamped(animStartPosition, outAnimEndPosition, curveValue);
		}
		else
		{
			// clean up and end the animation
            animObject.localPosition = outAnimEndPosition; // Force the object to its destination
			animState = AnimState.none;
		}
	}
}

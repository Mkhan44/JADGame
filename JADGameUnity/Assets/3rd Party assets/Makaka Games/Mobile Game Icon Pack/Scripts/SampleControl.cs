using UnityEngine;
using System.Collections;

public class SampleControl : MonoBehaviour 
{
	public Animator gameOverMenuAnimator;

	void Start () 
	{
		if(gameOverMenuAnimator)
		{
			gameOverMenuAnimator.SetTrigger("Show");
			//Debug.Log("Game Over Menu Show");
		}
	}
}

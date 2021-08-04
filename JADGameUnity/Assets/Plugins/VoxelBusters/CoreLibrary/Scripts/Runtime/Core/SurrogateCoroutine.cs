using System.Collections;
using System;
using UnityEngine;

using UnityObject = UnityEngine.Object;

namespace VoxelBusters.CoreLibrary
{
	public static class SurrogateCoroutine
	{
		#region Static fields

		private     static      SurrogateBehaviour	    surrogateObject	    = null;

		#endregion

		#region Static methods

		public static void StartCoroutine(IEnumerator routine)
		{
            CreateSurrogateObject();
            surrogateObject.StartCoroutine(routine);
		}

		public static void StopCoroutine(IEnumerator routine)
		{
            CreateSurrogateObject();
            surrogateObject.StopCoroutine(routine);
		}

		public static void WaitUntilAndInvoke(Func<bool> predicate, Action action)
		{
            CreateSurrogateObject();
            surrogateObject.StartCoroutine(WaitUntilAndInvokeInternal(predicate, action));
		}

		public static void WaitUntilAndInvoke(IEnumerator coroutine, Action action)
		{
            CreateSurrogateObject();
            surrogateObject.StartCoroutine(WaitUntilAndInvokeInternal(coroutine, action));
		}

		public static void WaitUntilAndInvoke(YieldInstruction instruction, Action action)
		{
            CreateSurrogateObject();
			surrogateObject.StartCoroutine(WaitUntilAndInvokeInternal(instruction, action));
		}

		public static void WaitForEndOfFrameAndInvoke(Action action)
		{
			CreateSurrogateObject();
			surrogateObject.StartCoroutine(WaitUntilAndInvokeInternal(new WaitForEndOfFrame(), action));
		}

		public static void Invoke(Action action, float delay)
		{
			CreateSurrogateObject();
			surrogateObject.StartCoroutine(WaitUntilAndInvokeInternal(new WaitForSeconds(delay), action));
		}
			
		#endregion

		#region Private static methods

		private static void CreateSurrogateObject()
		{
            if (surrogateObject == null)
            {
    			// create new object
    			var     newGO	    = new GameObject("Surrogate");
    			newGO.hideFlags	    = HideFlags.HideInHierarchy;

    			// make it persistent 
    			UnityObject.DontDestroyOnLoad(newGO);

    			// store the reference
    			surrogateObject	    = newGO.AddComponent<SurrogateBehaviour>();
            }
		}

		private static IEnumerator WaitUntilAndInvokeInternal(Func<bool> predicate, Action action)
		{
			yield return new WaitUntil(predicate);
			action();
		}

		private static IEnumerator WaitUntilAndInvokeInternal(IEnumerator coroutine, Action action)
		{
			yield return coroutine;
			action();
		}

		private static IEnumerator WaitUntilAndInvokeInternal(YieldInstruction instruction, Action action)
		{
			yield return instruction;
			action();
		}

		private static IEnumerator InvokeInternal(Action action, float delay)
		{
			yield return new WaitForSeconds(delay);
			action();
		}

		#endregion

		#region Nested types

		private class SurrogateBehaviour : MonoBehaviour
		{}
			
		#endregion
	}
}
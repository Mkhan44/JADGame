using UnityEngine;
using System.Collections;

namespace VoxelBusters.Parser
{
	public static class JsonExtensions 
	{
		#region Methods

		public static string ToJSON(this IDictionary dictionary)
		{
			string  jsonStr = JsonUtility.ToJSON(dictionary);
			return JsonUtility.IsNull(jsonStr) ? null : jsonStr;
		}

		public static string ToJSON(this IList list)
		{
			string  jsonStr	= JsonUtility.ToJSON(list);
			return JsonUtility.IsNull(jsonStr) ? null : jsonStr;
		}

		#endregion
	}
}
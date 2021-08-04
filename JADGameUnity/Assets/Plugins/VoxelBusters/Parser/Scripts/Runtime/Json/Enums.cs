using UnityEngine;
using System.Collections;

namespace VoxelBusters.Parser
{
	public enum JsonToken
	{
		CurlyOpenBracket	= 0,

		CurlyCloseBracket,

		SquareOpenBracket,

		SquareCloseBracket,

		Colon,

		Comma,

		String,

		Number,

		WhiteSpace,

		True,

		False,

		Null,

		None,
	}
}
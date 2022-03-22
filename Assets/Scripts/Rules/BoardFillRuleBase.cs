namespace Rules
{
	using Client;
	using Configs;
	using UnityEngine;

	public abstract class BoardFillRuleBase : ScriptableObject
	{
		public abstract Cell[,] FillBoard(BoardConfig config);
	}
}
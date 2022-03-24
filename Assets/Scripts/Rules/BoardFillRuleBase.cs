namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using Configs;
	using UnityEngine;

	public class ExistingBoard
	{
		public List<Vector2Int> Holes;
		public Dictionary<CellConfig, int> cellConfigsPool = null;
	}

	public abstract class BoardFillRuleBase : ScriptableObject
	{
		public abstract Board FillBoard(GameConfig gameConfig, ExistingBoard existingBoard = null);
	}
}
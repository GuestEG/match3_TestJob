namespace Rules
{
	using Client;
	using Configs;
	using UnityEngine;

	public abstract class BoardFillRuleBase : ScriptableObject
	{
		public abstract Board FillBoard(GameConfig config);
	}
}
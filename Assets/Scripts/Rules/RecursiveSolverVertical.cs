namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using UnityEngine;

	[CreateAssetMenu(menuName = "SWG/RecursiveSolverVertical")]
	public sealed class RecursiveSolverVertical : SolutionRuleBase
	{
		[SerializeField] private int minimalChainLength = 3;

		private Cell[,] _boardCells;

		public override bool TryGetConnectedCells(Board board, Vector2Int cellCoords, out List<Cell> connectedCells)
		{
			throw new System.NotImplementedException();
		}
	}
}
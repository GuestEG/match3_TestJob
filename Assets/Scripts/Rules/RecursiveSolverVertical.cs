namespace Rules
{
	using Client;
	using UnityEngine;

	[CreateAssetMenu(menuName = "SWG/RecursiveSolverVertical")]
	public sealed class RecursiveSolverVertical : RecursiveLineSolver
	{
		private protected override Cell[] Neighbours(Board board, Vector2Int cellCoords)
		{
			var result = new Cell[2];
			result[0] = Top(board, cellCoords);
			result[1] = Bottom(board, cellCoords);
			return result;
		}
	}
}
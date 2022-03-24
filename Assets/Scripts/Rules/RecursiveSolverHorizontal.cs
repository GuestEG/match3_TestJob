namespace Rules
{
	using Client;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Match3Test/RecursiveSolverHorizontal")]
	public sealed class RecursiveSolverHorizontal : RecursiveLineSolver
	{
		private protected override Cell[] Neighbours(Cell[,] board, Vector2Int cellCoords)
		{
			var result = new Cell[2];
			result[0] = Left(board, cellCoords);
			result[1] = Right(board, cellCoords);
			return result;
		}
	}
}
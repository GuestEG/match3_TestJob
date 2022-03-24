namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using UnityEngine;

	public abstract class RecursiveLineSolver : SolutionRuleBase
	{
		[SerializeField] private int _minimalChainLength = 3;

		public override bool TryGetConnectedCells(Board board, Vector2Int cellCoords, out List<Cell> connectedCells)
		{
			connectedCells = GetConnectedCells(board, cellCoords);
			if (connectedCells.Count < _minimalChainLength)
			{
				return false;
			}

			return true;
		}

		private List<Cell> GetConnectedCells(Board board, Vector2Int cellCoords, List<Cell> excludeList = null)
		{
			var sampleCell = board.GetCell(cellCoords);

			var result = new List<Cell> { sampleCell };

			if (excludeList == null)
			{
				excludeList = new List<Cell> { sampleCell };
			}
			else
			{
				excludeList.Add(sampleCell);
			}

			var neighbours = Neighbours(board, cellCoords);
			foreach (var cell in neighbours)
			{
				if (cell == null || excludeList.Contains(cell) || cell.Config != sampleCell.Config)
				{
					continue;
				}

				result.AddRange(GetConnectedCells(board, cell.Coords, excludeList));
			}

			return result;
		}

		private protected abstract Cell[] Neighbours(Board board, Vector2Int cellCoords);
	}
}
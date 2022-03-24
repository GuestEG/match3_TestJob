namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using UnityEngine;

	public abstract class RecursiveLineSolver : SolutionRuleBase
	{
		[SerializeField] private int _minimalChainLength = 3;

		public override bool TryGetConnectedCells(Cell[,] board, Vector2Int cellCoords, out List<Cell> connectedCells)
		{
			connectedCells = GetConnectedCells(board, cellCoords);
			if (connectedCells.Count < _minimalChainLength)
			{
				return false;
			}

			return true;
		}
		
		private List<Cell> GetConnectedCells(Cell[,] board, Vector2Int cellCoords, List<Cell> excludeList = null)
		{
			var sampleCell = board[cellCoords.x, cellCoords.y];

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
				if (cell == null || cell.IsHole || excludeList.Contains(cell) || cell.Config != sampleCell.Config)
				{
					continue;
				}

				result.AddRange(GetConnectedCells(board, cell.Coords, excludeList));
			}

			return result;
		}

		private protected abstract Cell[] Neighbours(Cell[,] board, Vector2Int cellCoords);

		public override bool HasPotentialSolutions(Cell[,] board, Vector2Int cellCoords)
		{
			var connectedCells = GetConnectedCellsOverJump(board, cellCoords);
			if (connectedCells.Count < _minimalChainLength)
			{
				return false;
			}

			return true;

		}

		private Cell[] AllNeighbours(Cell[,] board, Vector2Int cellCoords)
		{
			var result = new Cell[4];
			result[0] = Left(board, cellCoords);
			result[1] = Top(board, cellCoords);
			result[2] = Right(board, cellCoords);
			result[3] = Bottom(board, cellCoords);
			return result;
		}

		private List<Cell> GetConnectedCellsOverJump(Cell[,] board, Vector2Int cellCoords, List<Cell> excludeList = null)
		{
			var sampleCell = board[cellCoords.x, cellCoords.y];

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
				//walk by rule
				if (cell == null
				    || excludeList.Contains(cell)
				    || cell.IsHole)
				{
					continue;
				}

				//but if wrong type
				if (cell.Config != sampleCell.Config)
				{
					//check ALL 4 neighbours
					foreach (var potentialJump in AllNeighbours(board, cell.Coords))
					{
						if (potentialJump == null
						    || potentialJump.IsHole
						    || excludeList.Contains(potentialJump)
						    || potentialJump.Config != sampleCell.Config)
						{
							continue;
						}

						result.Add(potentialJump);
						excludeList.Add(potentialJump);
					}
				}

				result.AddRange(GetConnectedCellsOverJump(board, cell.Coords, excludeList));
			}

			return result;
		}

	}
}
namespace Rules
{
	using System.Collections.Generic;
	using System.Linq;
	using Client;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Match3Test/CrossSolverRule")]
	public sealed class CrossSolver : SolutionRuleBase
	{
		[SerializeField] private int minimalChainLength = 3;

		private List<Cell> _connectedHorizontal;
		private List<Cell> _connectedVertical;

		private readonly List<Cell> _result = new List<Cell>();

		public override bool TryGetConnectedCells(Cell[,] board, Vector2Int cellCoords, out List<Cell> connectedCells)
		{
			//immediately return on empty cell
			var cell = board[cellCoords.x, cellCoords.y];
			if (cell == null || cell.IsHole)
			{
				connectedCells = null;
				return false;
			}

			connectedCells = new List<Cell>();
			var result = false;

			_connectedHorizontal = GetChainHorizontal(board, cellCoords);
			if (_connectedHorizontal.Count >= minimalChainLength)
			{
				//note: boxing
				connectedCells = connectedCells.Union(_connectedHorizontal).ToList();
				result = true;
			}

			_connectedVertical = GetChainVertical(board, cellCoords);
			if (_connectedVertical.Count >= minimalChainLength)
			{
				//note: boxing
				connectedCells = connectedCells.Union(_connectedVertical).ToList();
				result = true;
			}

			return result;
		}

		private List<Cell> GetChainVertical(Cell[,] board, Vector2Int cellCoords)
		{
			_result.Clear();
			var sampleCell = board[cellCoords.x, cellCoords.y];
			var sizeY = board.GetLength(1);
			
			_result.Add(sampleCell);
			
			//move up
			var checkCoords = cellCoords;
			checkCoords.y--;
			while (checkCoords.y >= 0)
			{
				var checkCell = board[checkCoords.x, checkCoords.y];
				if (checkCell.IsHole || checkCell.Config != sampleCell.Config)
				{
					break;
				}

				_result.Add(checkCell);
				checkCoords.y--;
			}

			//move down
			checkCoords = cellCoords;
			checkCoords.y++;
			while (checkCoords.y < sizeY)
			{
				var checkCell = board[checkCoords.x, checkCoords.y];
				if (checkCell == null || checkCell.Config != sampleCell.Config)
				{
					break;
				}

				_result.Add(checkCell);
				checkCoords.y++;
			}
			// Debug.Log($"{nameof(CrossSolver)}: Found {_result.Count} vertical chain");
			return _result;
		}

		private List<Cell> GetChainHorizontal(Cell[,] board, Vector2Int cellCoords)
		{
			_result.Clear();

			var sampleCell = board[cellCoords.x, cellCoords.y];
			var sizeX = board.GetLength(0);
			
			_result.Add(sampleCell);

			var checkCoords = cellCoords;

			//move left
			checkCoords.x--;
			while (checkCoords.x >= 0)
			{
				var checkCell = board[checkCoords.x, checkCoords.y];
				if (checkCell == null || checkCell.Config != sampleCell.Config)
				{
					break;
				}

				_result.Add(checkCell);
				checkCoords.x--;
			}

			//move right
			checkCoords = cellCoords;
			checkCoords.x++;
			while (checkCoords.x < sizeX)
			{
				var checkCell = board[checkCoords.x, checkCoords.y];
				if (checkCell == null || checkCell.Config != sampleCell.Config)
				{
					break;
				}

				_result.Add(checkCell);
				checkCoords.x++;
			}
			// Debug.Log($"{nameof(CrossSolver)}: Found {_result.Count} horizontal chain");
			return _result;
		}

		
	}
}
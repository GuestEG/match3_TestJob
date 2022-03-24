namespace Rules
{
	using System.Collections.Generic;
	using System.Linq;
	using Client;
	using UnityEngine;

	[CreateAssetMenu(menuName = "SWG/CrossSolverRule")]
	public sealed class CrossSolver : SolutionRuleBase
	{
		[SerializeField] private int minimalChainLength = 3;

		private List<Cell> _connectedHorizontal;
		private List<Cell> _connectedVertical;

		private readonly List<Cell> _result = new List<Cell>();

		public override bool TryGetConnectedCells(Board board, Vector2Int cellCoords, out List<Cell> connectedCells)
		{
			//immediately return on empty cell
			if (board.GetCell(cellCoords).IsEmpty)
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

		private List<Cell> GetChainVertical(Board board, Vector2Int cellCoords)
		{
			_result.Clear();
			var sampleCell = board.GetCell(cellCoords);
			var sizeY = board.Size.y;
			_result.Add(sampleCell);
			
			//move up
			var checkCoords = cellCoords;
			checkCoords.y--;
			while (checkCoords.y >= 0)
			{
				var checkCell = board.GetCell(checkCoords);
				if (checkCell.IsEmpty || checkCell.Config != sampleCell.Config)
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
				var checkCell = board.GetCell(checkCoords);
				if (checkCell == null || checkCell.Config != sampleCell.Config)
				{
					break;
				}

				_result.Add(checkCell);
				checkCoords.y++;
			}
			Debug.Log($"Found {_result.Count} vertical chain");
			return _result;
		}

		private List<Cell> GetChainHorizontal(Board board, Vector2Int cellCoords)
		{
			_result.Clear();

			var sampleCell = board.GetCell(cellCoords);
			var sizeX = board.Size.x;
			_result.Add(sampleCell);

			var checkCoords = cellCoords;

			//move left
			checkCoords.x--;
			while (checkCoords.x >= 0)
			{
				var checkCell = board.GetCell(checkCoords);
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
				var checkCell = board.GetCell(checkCoords);
				if (checkCell == null || checkCell.Config != sampleCell.Config)
				{
					break;
				}

				_result.Add(checkCell);
				checkCoords.x++;
			}
			Debug.Log($"Found {_result.Count} horizontal chain");
			return _result;
		}

		
	}
}
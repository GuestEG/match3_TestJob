namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using UnityEngine;

	[CreateAssetMenu(menuName = "SWG/CrossSolverRule")]
	public class CrossSolver : SolutionRuleBase
	{
		[SerializeField] private int minimalChainLength = 3;

		// private List<Cell> _neighbors = new List<Cell>();
		private List<Cell> _connected = new List<Cell>();

		private List<Cell> _result = new List<Cell>();

		// private List<Cell> GetNeighborsVertical(Cell[,] boardCells, Vector2Int cellCoords)
		// {
		// 	var sizeX = boardCells.GetLength(0);
		// 	var sizeY = boardCells.GetLength(1);
		//
		// 	_neighbors.Clear();
		// 	//TODO: move into properties?
		// 	//top
		// 	if (cellCoords.y > 0 && boardCells[cellCoords.x, cellCoords.y - 1] != null)
		// 	{
		// 		_neighbors.Add(boardCells[cellCoords.x, cellCoords.y - 1]);
		// 	}
		//
		// 	//bottom
		// 	if (cellCoords.y < sizeY - 1 && boardCells[cellCoords.x, cellCoords.y + 1] != null)
		// 	{
		// 		_neighbors.Add(boardCells[cellCoords.x, cellCoords.y + 1]);
		// 	}
		//
		// 	return _neighbors;
		// }
		//
		// private List<Cell> GetNeighborsHorizontal(Cell[,] board, Vector2Int cellCoords)
		// {
		// 	var sizeX = board.GetLength(0);
		// 	var sizeY = board.GetLength(1);
		//
		// 	_neighbors.Clear();
		// 	//TODO: move into properties?
		// 	//right
		// 	if (cellCoords.x < sizeX && board[cellCoords.x + 1, cellCoords.y] != null)
		// 	{
		// 		_neighbors.Add(board[cellCoords.x + 1, cellCoords.y]);
		// 	}
		// 	//left
		// 	if (cellCoords.x > 0 && board[cellCoords.x - 1, cellCoords.y] != null)
		// 	{
		// 		_neighbors.Add(board[cellCoords.x - 1, cellCoords.y]);
		// 	}
		//
		// 	return _neighbors;
		// }

		
		public override List<Cell> GetConnectedCells(Board board, Vector2Int cellCoords)
		{
			_connected.Clear();
			//immdiately return on empty cell
			if (board.GetCell(cellCoords).IsEmpty)
			{
				return null;
			}

			_connected = GetChainHorizontal(board, cellCoords);
			if (_connected.Count >= minimalChainLength)
			{
				return _connected;
			}

			_connected = GetChainVertical(board, cellCoords);
			if (_connected.Count >= minimalChainLength)
			{
				return _connected;
			}

			return null;
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
namespace Client
{
	using System.Collections.Generic;
	using System.Linq;
	using Configs;
	using Rules;
	using UnityEngine;

	public sealed class Board
	{
		private readonly BoardConfig _config;

		private Cell[,] _cells;

		public Cell[,] Cells => _cells; //property maybe?

		public Vector2Int Size => _config.BoardSize;
		public Board(BoardConfig config)
		{
			_config = config;

			_cells = new Cell[_config.BoardSize.x, _config.BoardSize.y];
		}

		public void FillBoard(Cell[,] cells)
		{
			if (cells.GetLength(0) != _config.BoardSize.x ||
			    cells.GetLength(1) != _config.BoardSize.y)
			{
				Debug.LogError($"{nameof(Board)}: provided cells are not the same size as the board configuration!");
				return;
			}
			_cells = cells;
		}

		public Cell GetCell(Vector2Int cellPosition) => _cells[cellPosition.x, cellPosition.y];
		
		public void SwapCells(Vector2Int cellPosition1, Vector2Int cellPosition2)
		{
			var cell1 = GetCell(cellPosition1);
			var cell2 = GetCell(cellPosition2);

			var cell1Config = cell1.Config;
			var cell2Config = cell2.Config;

			//swap configs
			cell1.Config = cell2Config;
			cell2.Config = cell1Config;
			
			//TODO: probably should be in the BoardView class?
			//swap icons according to new configs
			cell1.UpdateIconFromConfig();
			cell2.UpdateIconFromConfig();
			cell1.UpdatePopIconFromConfig();
			cell2.UpdatePopIconFromConfig();
		}

		public ExistingBoard GetExistingBoard()
		{
			var result = new ExistingBoard();
			var currentCells = _config.CellConfigs.ToDictionary(cellConfig => cellConfig, _ => 0);
			foreach (var cell in _cells)
			{
				if (cell.IsHole)
				{
					result.Holes.Add(cell.Coords);
				}
				currentCells[cell.Config]++;
			}

			result.cellConfigsPool = currentCells;

			return result;
		}

		//TODO: refactor, should not be there
		public void UpdateAllCells()
		{
			foreach (var cell in _cells)
			{
				cell.UpdateIconFromConfig();
				cell.UpdatePopIconFromConfig();
			}
		}
	}
}

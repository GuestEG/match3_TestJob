namespace Client
{
	using Configs;
	using Rules;
	using UnityEngine;

	public sealed class Board
	{
		private readonly BoardConfig _config;

		private Cell[,] _cells;

		public Vector2Int Size => _config.BoardSize;
		public Board(BoardConfig config)
		{
			_config = config;

			_cells = new Cell[_config.BoardSize.x, _config.BoardSize.y];
		}

		public void FillBoard(BoardFillRuleBase rule)
		{
			_cells = rule.FillBoard(_config);
		}

		public Cell GetCell(Vector2Int cellPosition) => _cells[cellPosition.x, cellPosition.y];
		public Cell GetCell(int x, int y) => _cells[x, y];
		public void SetCell(Cell newCell, Vector2Int cellPosition) => _cells[cellPosition.x, cellPosition.y] = newCell;

		public void SwapCells(Vector2Int cellPosition1, Vector2Int cellPosition2)
		{
			var tmpCell2 = GetCell(cellPosition2);
			SetCell(GetCell(cellPosition1), cellPosition2);
			SetCell(tmpCell2, cellPosition1);
			
			//reset visuals
			// GetCell(cellPosition1).View.Icon.transform.localPosition = Vector3.zero;
			// GetCell(cellPosition2).View.Icon.transform.localPosition = Vector3.zero;

			GetCell(cellPosition1).Coords = cellPosition1;
			GetCell(cellPosition2).Coords = cellPosition2;
		}
	}
}
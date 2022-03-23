namespace Client
{
	using System.Threading.Tasks;
	using Configs;
	using DG.Tweening;
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

		public async void SwapCells(Vector2Int cellPosition1, Vector2Int cellPosition2)
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

			//animate movement as if they are going to swap
			await ReverseSwapIcons(cell1, cell2, _config.SwapAnimationDuration);
		}

		//TODO: probably should be in the BoardView class?
		private async Task ReverseSwapIcons(Cell cell1, Cell cell2, float duration)
		{
			var icon1 = cell1.View.Icon;
			var icon2 = cell2.View.Icon;
			var icon1dest = icon2.transform.position;
			var icon2dest = icon1.transform.position;

			var sequence = DOTween.Sequence();
			//reverse move
			sequence.Join(icon1.transform.DOMove(icon1dest, duration).From());
			sequence.Join(icon2.transform.DOMove(icon2dest, duration).From());
			
			await sequence.Play().AsyncWaitForCompletion();
		}
	}
}
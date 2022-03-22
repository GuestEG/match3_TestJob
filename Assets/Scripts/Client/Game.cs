namespace Client
{
	using System;
	using System.Threading.Tasks;
	using Configs;
	using DG.Tweening;
	using UnityEngine;
	using Views;

	public sealed class Game
	{
		private readonly GameConfig _config;
		private readonly BoardView _view;

		private Cell[,] _board;

		public Game(BoardView view, GameConfig config)
		{
			_view = view;
			_view.SetConfig(config.BoardConfig);
			_config = config;
		}

		public void StartGame()
		{
			FillBoard();
			FillBoardView();
		}
		
		private void FillBoard()
		{
			_board = _config.BoardFillRule.FillBoard(_config.BoardConfig);
		}

		private void FillBoardView()
		{
			_view.FillBoard(_board, CellButtonClickHandler);
		}

		private bool _blocked = false;

		private async void CellButtonClickHandler(Vector2Int cellCoords)
		{
			if (_blocked)
			{
				return;
			}
			Debug.Log($"Clicked in coords {cellCoords}");
			//try to solve there
			var solution = _config.SolutionRule.GetConnectedCells(_board, cellCoords);
			if (solution != null)
			{
				_blocked = true;

				var sequence = DOTween.Sequence();
				var sequence2 = DOTween.Sequence();
				foreach (var cell in solution)
				{
					sequence.Join(cell.View.Icon.transform.DOScale(2, _config.AnimationDuration));
					sequence2.Join(cell.View.Icon.transform.DOScale(1, _config.AnimationDuration));
				}

				sequence.Append(sequence2);
				await sequence.Play().AsyncWaitForCompletion();
				_blocked = false;
				Debug.Log("Sequence completed");
			}
		}
	}
}
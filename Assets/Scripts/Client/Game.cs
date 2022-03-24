namespace Client
{
	using Configs;
	using UnityEngine;
	using Views;

	public sealed class Game
	{
		private readonly GameConfig _config;
		private readonly BoardView _boardView;
		
		private bool _inputBlocked = false;
		private Board _board;
		private Cell _selection = null;

		public Game(BoardView boardView, GameConfig config)
		{
			_boardView = boardView;
			_boardView.SetConfig(config.BoardConfig);
			_config = config;
		}

		public void StartGame()
		{
			_board = new Board(_config.BoardConfig);
			_board.FillBoard(_config.BoardFillRule);
			FillBoardView();
		}
		
		public void FillBoardView()
		{
			_boardView.FillBoard(_board, CellButtonClickHandler);
		}

		private async void CellButtonClickHandler(Vector2Int cellPosition)
		{
			if (_inputBlocked)
			{
				return;
			}
			Debug.Log($"Clicked in coords {cellPosition}");

			//new selection
			if(_selection == null)
			{
				_selection = _board.GetCell(cellPosition);
				_boardView.ShowHighlight(cellPosition, true);
				
				return;
			}

			//same one -- deselect maybe?
			if (_selection.Coords == cellPosition)
			{
				return;
			}

			//already had selected something - test for distance. Should be exactly one for neighbors
			Debug.Log($"Distance to previous selection = {Vector2Int.Distance(_selection.Coords, cellPosition)}");
			if (Mathf.Approximately(Vector2Int.Distance(_selection.Coords, cellPosition), 1))
			{
				_inputBlocked = true;
				_boardView.ShowHighlight(_selection.Coords, false);
				_board.SwapCells(_selection.Coords, cellPosition);
				//animate movement
				await _boardView.SwapIcons(_selection.Coords, cellPosition, _config.SwapAnimationDuration);

				_selection = null;
				_inputBlocked = false;
				Debug.Log("Swap completed");
				return;
			}

			//too far - switch selection
			_boardView.ShowHighlight(_selection.Coords, false);
			_selection = _board.GetCell(cellPosition);
			_boardView.ShowHighlight(_selection.Coords, true);
			return;


			//try to solve there
			// var solution = _config.SolutionRule.TryGetConnectedCells(_board, cellPosition);
			// if (solution != null)
			// {
			// 	_inputBlocked = true;
			//
			// 	var sequence = DOTween.Sequence();
			// 	var sequence2 = DOTween.Sequence();
			// 	foreach (var cell in solution)
			// 	{
			// 		sequence.Join(cell.View.Icon.transform.DOScale(2, _config.AnimationDuration));
			// 		sequence2.Join(cell.View.Icon.transform.DOScale(1, _config.AnimationDuration));
			// 	}
			//
			// 	sequence.Append(sequence2);
			// 	await sequence.Play().AsyncWaitForCompletion();
			// 	_inputBlocked = false;
			// 	Debug.Log("Sequence completed");
			// }
		}
	}
}
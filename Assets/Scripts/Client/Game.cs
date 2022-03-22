namespace Client
{
	using System.Threading.Tasks;
	using Configs;
	using DG.Tweening;
	using UnityEngine;
	using Views;

	public sealed class Game
	{
		private readonly GameConfig _config;
		private readonly BoardView _view;
		
		private bool _inputBlocked = false;
		private Board _board;
		private Cell _selection = null;

		public Game(BoardView view, GameConfig config)
		{
			_view = view;
			_view.SetConfig(config.BoardConfig);
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
			_view.FillBoard(_board, CellButtonClickHandler);
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
				_selection.View.ShowHighlight(true);
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
				_selection.View.ShowHighlight(false);
				await SwapIconPositions(_selection, _board.GetCell(cellPosition), _config.AnimationDuration);
				_board.SwapCells(_selection.Coords, cellPosition);
				_selection = null;
				_inputBlocked = false;
				Debug.Log("Swap completed");
				return;
			}

			//too far - switch selection
			_selection.View.ShowHighlight(false);
			_selection = _board.GetCell(cellPosition);
			_selection.View.ShowHighlight(true);
			return;


			//try to solve there
			// var solution = _config.SolutionRule.GetConnectedCells(_board, cellPosition);
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

		private async Task SwapIconPositions(Cell cell1, Cell cell2, float duration)
		{
			var icon1 = cell1.View.Icon;
			var icon2 = cell2.View.Icon;

			var sequence = DOTween.Sequence();
			sequence.Join(icon1.transform.DOMove(icon2.transform.position, duration));
			sequence.Join(icon2.transform.DOMove(icon1.transform.position, duration));
			await sequence.Play().AsyncWaitForCompletion();
			
			icon1.transform.SetParent(cell2.View.transform, true);
			icon2.transform.SetParent(cell1.View.transform, true);
		}
	}
}
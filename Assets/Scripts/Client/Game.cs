namespace Client
{
	using System.Collections.Generic;
	using System.Linq;
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
			_board = _config.BoardFillRule.FillBoard(_config);
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

			Debug.Log($"{nameof(Game)}: Clicked in coords {cellPosition}");

			// //try to solve there
			// if (_config.SolutionRules.TryGetConnectedCells(_board, cellPosition, out var solution))
			// {
			// 	_inputBlocked = true;
			// 	await _boardView.PopIcons(solution, _config.PopAnimationDuration);
			// 	_inputBlocked = false;
			// 	Debug.Log("{nameof(Game)}: Pop Sequence completed");
			// }
			// return;
			
			//new selection
			if (_selection == null)
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
			// Debug.Log($"Distance to previous selection = {Vector2Int.Distance(_selection.Coords, cellPosition)}");
			if (Mathf.Approximately(Vector2Int.Distance(_selection.Coords, cellPosition), 1))
			{
				_inputBlocked = true;
				
				_boardView.ShowHighlight(_selection.Coords, false);
				_board.SwapCells(_selection.Coords, cellPosition);
				//animate movement
				await _boardView.SwapIcons(_selection.Coords, cellPosition, _config.SwapAnimationDuration);
				
				//test for solution in both positions
				//TODO: extract list
				var haveSolution = false;
				var fullSolution = new List<Cell>();

				foreach (var solutionRule in _config.SolutionRules)
				{
					if (solutionRule.TryGetConnectedCells(_board, _selection.Coords, out var solution1))
					{
						fullSolution = fullSolution.Union(solution1).ToList();
						haveSolution = true;
					}

					if (solutionRule.TryGetConnectedCells(_board, cellPosition, out var solution2))
					{
						fullSolution = fullSolution.Union(solution2).ToList();
						haveSolution = true;
					}
				}
				
				if (haveSolution)
				{
					//pop animation
					await _boardView.PopIcons(fullSolution, _config.PopAnimationDuration);
				}
				else
				{
					//swap back
					_board.SwapCells(_selection.Coords, cellPosition);
					await _boardView.SwapIcons(_selection.Coords, cellPosition, _config.SwapAnimationDuration);
				}
				
				_selection = null;
				_inputBlocked = false;
				return;
			}

			//too far - switch selection
			_boardView.ShowHighlight(_selection.Coords, false);
			_selection = _board.GetCell(cellPosition);
			_boardView.ShowHighlight(_selection.Coords, true);
		}
	}
}
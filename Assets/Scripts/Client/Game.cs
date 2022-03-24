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
		private readonly RowView _offscreenRowView;

		private bool _inputBlocked = false;
		private Board _board;
		private Cell _selection = null;
		private Transform[] _offscreenCells;

		public Game(BoardView boardView, RowView offscreenRowView, GameConfig config)
		{
			_config = config;
			_boardView = boardView;
			_boardView.SetConfig(config.BoardConfig);
			_offscreenRowView = offscreenRowView;
		}

		public void StartGame()
		{
			_board = _config.BoardFillRule.FillBoard(_config);
			FillBoardView();
		}

		private void FillBoardView()
		{
			_boardView.FillBoard(_board, CellButtonClickHandler);
			_offscreenCells = new Transform[_board.Size.x];
			for (int i = 0; i < _board.Size.x; i++)
			{
				var cell = new GameObject($"offscreenCell_{i}", typeof(RectTransform));
				_offscreenCells[i] = cell.transform;
				_offscreenRowView.AddCell(cell.transform);
			}
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
				var haveSolution = TryGetSelectedSolution(cellPosition, out var solution);
				if (haveSolution)
				{
					//TODO: need to animate both popping and movement at once
					//pop animation
					await _boardView.PopIcons(solution, _config.PopAnimationDuration);
					//replace popped with new ones
					_board.FillBoard(_config.BoardMovementRule.FIllHoles(_board.Cells, solution, _config.BoardConfig));
					//and animate movement

					//keep trying until stable
					while (TryGetTotalSolution(_board, out solution))
					{
						await _boardView.PopIcons(solution, _config.PopAnimationDuration);
						_board.FillBoard(
							_config.BoardMovementRule.FIllHoles(_board.Cells, solution, _config.BoardConfig));
					}
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

		//todo: need to isolate method from class fields
		private bool TryGetSelectedSolution(Vector2Int cellPosition, out List<Cell> fullSolution)
		{
			var haveSolution = false;
			fullSolution = new List<Cell>();

			foreach (var solutionRule in _config.SolutionRules)
			{

				if (solutionRule.TryGetConnectedCells(_board.Cells, _selection.Coords, out var solution1))
				{
					fullSolution = fullSolution.Union(solution1).ToList();
					haveSolution = true;
				}

				if (solutionRule.TryGetConnectedCells(_board.Cells, cellPosition, out var solution2))
				{
					fullSolution = fullSolution.Union(solution2).ToList();
					haveSolution = true;
				}
			}

			return haveSolution;
		}

		private bool TryGetTotalSolution(Board board, out List<Cell> solution)
		{
			var haveSolution = false;
			solution = new List<Cell>();

			foreach (var cell in board.Cells)
			{
				foreach (var solutionRule in _config.SolutionRules)
				{
					if (solutionRule.TryGetConnectedCells(board.Cells, cell.Coords, out var connectedCells))
					{
						solution = solution.Union(connectedCells).ToList();
						haveSolution = true;
					}
				}
			}

			return haveSolution;
		}
	}
}
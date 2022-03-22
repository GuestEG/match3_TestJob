namespace Client
{
	using Configs;
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
			_view.FillBoard(_board);
		}
	}
}
namespace Client
{
	using Configs;

	public sealed class Game
	{
		private readonly GameConfig _config;
		private readonly BoardView _view;

		private Cell[,] _board;

		public Game(BoardView view, GameConfig config)
		{
			_view = view;
			_config = config;
		}

		public void StartGame()
		{
			FillBoard();
			FillBoardView();
		}

		private void FillBoardView()
		{
			throw new System.NotImplementedException();
		}

		private void FillBoard()
		{
			_board = _config.BoardFillRule.FillBoard(_config.BoardConfig);
		}
	}
}
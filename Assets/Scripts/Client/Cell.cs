namespace Client
{
	using Configs;
	using UnityEngine;
	using Views;

	public sealed class Cell
	{
		private CellView _view;
		private CellConfig _config;
		private bool _isEmpty;
		private Vector2Int _coords;
		
		public Cell(bool isEmpty, Vector2Int coords, CellConfig config)
		{
			_isEmpty = isEmpty;
			_coords = coords;
			_config = config;
		}

		public void SetView(CellView view)
		{
			_view = view;
		}
	}
}
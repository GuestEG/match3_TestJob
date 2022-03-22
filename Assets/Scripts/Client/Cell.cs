namespace Client
{
	using Configs;
	using UnityEngine;
	using Views;

	public sealed class Cell
	{
		public CellView _view { get; private set; }
		public CellConfig Config { get; }
		public bool IsEmpty { get; }


		private Vector2Int _coords; //do i need this?
		
		public Cell(bool isEmpty, Vector2Int coords, CellConfig config)
		{
			IsEmpty = isEmpty;
			_coords = coords;
			Config = config;
		}

		public void SetView(CellView view)
		{
			_view = view;
		}
	}
}
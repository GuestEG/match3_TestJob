namespace Client
{
	using Configs;
	using UnityEngine;
	using Views;

	public sealed class Cell
	{
		public CellView View { get; private set; }
		public CellConfig Config { get; }
		public bool IsEmpty { get; }
		public Vector2Int Coords { get; } //do i need this?
		
		public Cell(bool isEmpty, Vector2Int coords, CellConfig config)
		{
			IsEmpty = isEmpty;
			Coords = coords;
			Config = config;
		}

		public void SetView(CellView view)
		{
			View = view;
		}
	}
}
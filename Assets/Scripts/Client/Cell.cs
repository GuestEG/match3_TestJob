namespace Client
{
	using Configs;
	using UnityEngine;
	using Views;

	public sealed class Cell
	{
		public CellView View { get; set; }
		public CellConfig Config { get; set; }
		public bool IsEmpty => Config == null;
		public Vector2Int Coords { get; set; }
	}
}
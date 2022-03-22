namespace Rules
{
	using Client;
	using Configs;
	using UnityEngine;
	using Random = UnityEngine.Random;

	[CreateAssetMenu(menuName = "SWG/SimpleBoardFillRule")]
	public sealed class SimpleBoardFillRule : BoardFillRuleBase
	{
		//just random cell fill
		public override Cell[,] FillBoard(BoardConfig config)
		{
			var sizeX = config.BoardSize.x;
			var sizeY = config.BoardSize.y;
			Cell[,] result = new Cell[sizeX, sizeY];
			
			//fill in empty
			var emptyCells = config.EmptyCellsNum;
			while (emptyCells > 0)
			{
				var rndX = Random.Range(0, sizeX);
				var rndY = Random.Range(0, sizeY);
				if (result[rndX, rndY] == null)
				{
					var cell = new Cell();
					cell.Coords = new Vector2Int(rndX, rndY);
					result[rndX, rndY] = cell;
					emptyCells--;
				}
			}

			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					//skip prefilled empties
					if (result[x, y] != null)
					{
						continue;
					}

					var randomCellConfig = config.CellConfigs[Random.Range(0, config.CellConfigs.Count)];
					var cell = new Cell();
					cell.Config = randomCellConfig;
					cell.Coords = new Vector2Int(x, y);

					result[x, y] = cell;
				}
			}

			return result;
		}
	}
}
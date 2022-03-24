namespace Rules
{
	using Client;
	using Configs;
	using UnityEngine;
	using Random = UnityEngine.Random;

	[CreateAssetMenu(menuName = "SWG/SimpleBoardFillRule")]
	
	// Just a random cell fill, no checks
	public sealed class SimpleBoardFillRule : BoardFillRuleBase
	{
		public override Board FillBoard(GameConfig gameConfig)
		{
			var config = gameConfig.BoardConfig;
			var sizeX = config.BoardSize.x;
			var sizeY = config.BoardSize.y;
			Cell[,] cells = new Cell[sizeX, sizeY];
			
			//fill in empty
			var emptyCells = config.EmptyCellsNum;
			while (emptyCells > 0)
			{
				var rndX = Random.Range(0, sizeX);
				var rndY = Random.Range(0, sizeY);
				if (cells[rndX, rndY] == null)
				{
					var cell = new Cell();
					cell.Coords = new Vector2Int(rndX, rndY);
					cells[rndX, rndY] = cell;
					emptyCells--;
				}
			}

			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					//skip prefilled empties
					if (cells[x, y] != null)
					{
						continue;
					}

					var randomCellConfig = config.CellConfigs[Random.Range(0, config.CellConfigs.Count)];
					var cell = new Cell();
					cell.Config = randomCellConfig;
					cell.Coords = new Vector2Int(x, y);

					cells[x, y] = cell;
				}
			}

			Board result = new Board(config);
			result.FillBoard(cells);

			return result;
		}
	}
}
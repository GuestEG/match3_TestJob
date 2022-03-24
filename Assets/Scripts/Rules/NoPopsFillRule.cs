namespace Rules
{
	using Client;
	using Configs;
	using UnityEngine;
	using Random = UnityEngine.Random;

	[CreateAssetMenu(menuName = "SWG/NoPopsFillRule")]
	
	//This rule tries to generate random field until there is no immediate solution
	//WARNING: does not use match rules from the game!
	//requires refactoring of the solvers to work without Board class
	
	public sealed class NoPopsFillRule : BoardFillRuleBase
	{
		public override Board FillBoard(GameConfig gameConfig)
		{
			var cycleNum = 0;


			var boardConfig = gameConfig.BoardConfig;
			var solutionRules = gameConfig.SolutionRules;

			var sizeX = boardConfig.BoardSize.x;
			var sizeY = boardConfig.BoardSize.y;
			Cell[,] cells = new Cell[sizeX, sizeY];

			var board = new Board(boardConfig);
			
			//fill in empty - do not iterate variants here
			var emptyCells = boardConfig.EmptyCellsNum;
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

					//generate
					CellConfig randomCellConfig = null;

					var cycles = 0;

					//check 2 previous cells to the X and Y to not be the same - get rid of matches there
					do
					{
						Debug.Log($"{nameof(NoPopsFillRule)}: Gen cycle = {cycles}");
						randomCellConfig = boardConfig.CellConfigs[Random.Range(0, boardConfig.CellConfigs.Count)];
						cycles++;
						if (cycles > 10000)
						{
							Debug.Log($"{nameof(NoPopsFillRule)}: Aborting generation!");
							break;
						}
					} while (x >= 2 &&
					         cells[x - 1, y].Config == randomCellConfig &&
					         cells[x - 2, y].Config == randomCellConfig
					         ||
					         y >= 2 &&
					         cells[x, y - 1].Config == randomCellConfig &&
					         cells[x, y - 2].Config == randomCellConfig);

					var cell = new Cell();
					cell.Config = randomCellConfig;
					cell.Coords = new Vector2Int(x, y);
					
					cells[x, y] = cell;
				}
			}
			
			board.FillBoard(cells);
			return board;
		}
	}
}
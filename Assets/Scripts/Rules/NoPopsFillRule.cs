namespace Rules
{
	using Client;
	using Configs;
	using UnityEngine;
	using Random = UnityEngine.Random;

	[CreateAssetMenu(menuName = "Match3Test/NoPopsFillRule")]
	
	//This rule generates random board with no immediate solution
	public sealed class NoPopsFillRule : BoardFillRuleBase
	{
		public override Board FillBoard(GameConfig gameConfig)
		{
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
					var cell = new Cell();
					CellConfig randomCellConfig = null;

					bool haveSolution;
					var cycles = 0;
					do
					{
						// Debug.Log($"{nameof(NoPopsFillRule)}: Gen cycle = {cycles}");
						randomCellConfig = boardConfig.CellConfigs[Random.Range(0, boardConfig.CellConfigs.Count)];
						cycles++;
						if (cycles > 10000)
						{
							Debug.LogError($"{nameof(NoPopsFillRule)}: Aborting board fill on gen {cycles}!");
							break;
						}

						cell.Config = randomCellConfig;
						cell.Coords = new Vector2Int(x, y);
						cells[x, y] = cell;

						haveSolution = false;
						foreach (var solver in solutionRules)
						{
							haveSolution |= solver.TryGetConnectedCells(cells, cell.Coords, out _);
						}
					}
					while (haveSolution);
				}
			}
			
			board.FillBoard(cells);
			return board;
		}
	}
}
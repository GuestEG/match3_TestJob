namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using Configs;
	using UnityEngine;

	public sealed class CellMovement
	{
		public bool FromOffscreen = false;
		public Vector2Int Start;
		public Vector2Int End;
		public int Distance;
	}

	[CreateAssetMenu(menuName = "Match3Test/BoardMovementRule")]
	public sealed class BoardMovementRule : ScriptableObject
	{
		public Cell[,] FIllHoles(Cell[,] board, List<Cell> removedCells, BoardConfig config, out List<CellMovement> movements)
		{
			var columnsNum = board.GetLength(0);
			var rowsNum = board.GetLength(1);

			movements = new List<CellMovement>();

			//work with one column from left to right
			for (int column = 0; column < columnsNum; column++)
			{
				var columnBottom = 0;
				//check every row from top
				for (int row = 0; row < rowsNum; row++)
				{
					ref var boardCell = ref board[column, row];
					var lookupOffset = 0;
					//find a row where a missing piece will be
					if (!boardCell.IsHole && removedCells.Contains(boardCell))
					{
						var filled = false;
						//now scan downwards until hit a filled or end of the board
						for (int lookupRow = row + 1; lookupRow < rowsNum; lookupRow++)
						{
							var nextCell = board[column + lookupOffset, lookupRow];
							if (removedCells.Contains(nextCell))
							{
								continue;
							}

							if (nextCell.IsHole)
							{
								//shift lookup sideways
								if (column < columnsNum - 1)
								{
									lookupOffset++;
								}
								else
								{
									lookupOffset--;
								}

								//step back
								lookupRow--;
								continue;
							}

							//we found the piece, now we need to put it in the missing spot
							//board[column, row].Config = nextCell.Config;
							boardCell.Config = nextCell.Config;
							//and that cell becomes empty
							removedCells.Add(nextCell);
							boardCell.UpdateIconFromConfig();
							filled = true;

							//create movement for animation
							var movement = new CellMovement()
							{
								End = boardCell.Coords,
								Start = nextCell.Coords,
								Distance = nextCell.Coords.y - boardCell.Coords.y + nextCell.Coords.x - boardCell.Coords.x
								
							};
							movements.Add(movement);

							break;
						}
						
						//if we hit bottom line - we'll make a new config instead
						if (!filled)
						{
							var randomCellConfig = config.CellConfigs[Random.Range(0, config.CellConfigs.Count)];
							boardCell.Config = randomCellConfig;
							boardCell.UpdateIconFromConfig();

							columnBottom = Mathf.Max(columnBottom, rowsNum - boardCell.Coords.y);

							//create movement for animation
							var movement = new CellMovement()
							{
								End = boardCell.Coords,
								Distance = columnBottom,
								Start = Vector2Int.right * lookupOffset,
								FromOffscreen = true
							};
							movements.Add(movement);
						}
					}
				}
			}

			return board;
		}
	}
}
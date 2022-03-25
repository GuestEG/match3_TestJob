namespace Rules
{
	using System;
	using System.Collections.Generic;
	using Client;
	using Configs;
	using UnityEngine;
	using Random = UnityEngine.Random;

	public sealed class Move
	{
		public Vector2Int Start;
		public Vector2Int End;
		public bool FromOffscreen = false;
	}

	public sealed class CellMovement
	{
		public Vector2Int Target;
		public List<Move> Moves;
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
			// for (int column = 0; column < columnsNum; column++)
			for (int row = 0; row < rowsNum; row++)
			{
				//check every row from top
				// for (int row = 0; row < rowsNum; row++)
				for (int column = 0; column < columnsNum; column++)
				{
					// var columnBottom = 0;
					ref var boardCell = ref board[column, row];
					var moves = new List<Move>();
					var lookupOffset = 0;
					//find a row where a missing piece will be
					if (!boardCell.IsHole && removedCells.Contains(boardCell))
					{
						var filled = false;
						//now scan downwards until hit a filled or end of the board
						Cell nextCell = boardCell;
						for (int lookupRow = row + 1; lookupRow < rowsNum; lookupRow++)
						{
							var move = new Move
							{
								Start = nextCell.Coords
							};

							nextCell = board[column + lookupOffset, lookupRow];

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

							//do not add holes to moves!
							move.End = nextCell.Coords;
							moves.Add(move);

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
								Target = boardCell.Coords,
								Moves = moves,
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

							// columnBottom = Mathf.Max(columnBottom, rowsNum - boardCell.Coords.y);
							var columnBottom =  rowsNum - boardCell.Coords.y;

							moves.Add(new Move()
							{
								End = nextCell.Coords,
								Start = Vector2Int.right * lookupOffset, //offset
								FromOffscreen = true
							});
							//create movement for animation
							var movement = new CellMovement()
							{
								Target = boardCell.Coords,
								Distance = columnBottom,
								Moves = moves
							};
							movements.Add(movement);
						}
					}
				}
			}
			movements.Reverse();
			return board;
		}
	}
}
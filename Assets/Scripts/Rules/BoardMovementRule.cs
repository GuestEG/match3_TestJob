namespace Rules
{
	using System.Collections.Generic;
	using Client;
	using Configs;
	using UnityEngine;

	public sealed class CellMovement
	{
		public bool FromOffscreen;
		public Vector2Int Start;
		public Vector2Int End;
	}
	[CreateAssetMenu(menuName = "SWG/BoardMovementRule")]
	public sealed class BoardMovementRule : ScriptableObject
	{
		public Cell[,] FIllHoles(Cell[,] board, List<Cell> removedCells, BoardConfig config)
		{
			var columnsNum = board.GetLength(0);
			var rowsNum = board.GetLength(1);

			//work with one column from left to right
			for (int column = 0; column < columnsNum; column++)
			{
				//check every row from top
				for (int row = 0; row < rowsNum; row++)
				{
					ref var boardCell = ref board[column, row];
					//find a row where a missing piece will be
					if (!boardCell.IsHole && removedCells.Contains(boardCell))
					{
						var filled = false;
						//now scan downwards until hit a filled or end of the board
						for (int lookup = row+1; lookup < rowsNum; lookup++)
						{
							var nextCell = board[column, lookup];
							if (!nextCell.IsHole && !removedCells.Contains(nextCell))
							{
								//we found the piece, now we need to put it in the missing spot
								//board[column, row].Config = nextCell.Config;
								boardCell.Config = nextCell.Config;
								//and that cell becomes empty
								removedCells.Add(nextCell);
								boardCell.UpdateIconFromConfig();
								filled = true;
								break;
							}
						}

						//if we hit bottom line - we'll make a new config instead
						if(!filled)
						{
							var randomCellConfig = config.CellConfigs[Random.Range(0, config.CellConfigs.Count)];
							boardCell.Config = randomCellConfig;
							boardCell.UpdateIconFromConfig();
						}
					}
				}
			}

			return board;
		}

		/*
		 fillHoles() -> [[Cookie]] {
		    var columns: [[Cookie]] = []
		    // 1
		    for column in 0..<numColumns {
		      var array: [Cookie] = []
		      for row in 0..<numRows {
		        // 2
		        if tiles[column, row] != nil && cookies[column, row] == nil {
		          // 3
		          for lookup in (row + 1)..<numRows {
		            if let cookie = cookies[column, lookup] {
		              // 4
		              cookies[column, lookup] = nil
		              cookies[column, row] = cookie
		              cookie.row = row
		              // 5
		              array.append(cookie)
		              // 6
		              break
		            }
		          }
		        }
		      }
		      // 7
		      if !array.isEmpty {
		        columns.append(array)
		      }
		    }
		    return columns
		 */


		
	}
}
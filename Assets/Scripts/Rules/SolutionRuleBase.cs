namespace Rules
{
    using System.Collections.Generic;
    using Client;
    using UnityEngine;

    public abstract class SolutionRuleBase : ScriptableObject
    {
        public abstract bool TryGetConnectedCells(Cell[,] board, Vector2Int cellCoords, out List<Cell> connectedCells);

        protected static Cell Top(Cell[,] board, Vector2Int cellCoords)
        {
            if (cellCoords.y <= 0)
            {
                return null;
            }

            cellCoords.y -= 1;
            var neighbor = board[cellCoords.x, cellCoords.y];
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }

        protected static Cell Bottom(Cell[,] board, Vector2Int cellCoords)
        {
            var maxY = board.GetLength(1) - 1;

            if (cellCoords.y >= maxY)
            {
                return null;
            }

            cellCoords.y += 1;
            var neighbor = board[cellCoords.x, cellCoords.y];
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }

        protected static Cell Left(Cell[,] board, Vector2Int cellCoords)
        {
            if (cellCoords.x <= 0)
            {
                return null;
            }

            cellCoords.x -= 1;
            var neighbor = board[cellCoords.x, cellCoords.y];
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }

        protected static Cell Right(Cell[,] board, Vector2Int cellCoords)
        {
            var maxX = board.GetLength(0) - 1;
            if (cellCoords.x >= maxX)
            {
                return null;
            }

            cellCoords.x += 1;
            var neighbor = board[cellCoords.x, cellCoords.y];
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }
    }
}
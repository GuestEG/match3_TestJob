namespace Rules
{
    using System.Collections.Generic;
    using Client;
    using UnityEngine;

    public abstract class SolutionRuleBase : ScriptableObject
    {
        public abstract bool TryGetConnectedCells(Board board, Vector2Int cellCoords, out List<Cell> connectedCells);

        protected static Cell Top(Board board, Vector2Int cellCoords)
        {
            if (cellCoords.y <= 0)
            {
                return null;
            }

            cellCoords.y -= 1;
            var neighbor = board.GetCell(cellCoords);
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }

        protected static Cell Bottom(Board board, Vector2Int cellCoords)
        {
            if (cellCoords.y >= board.Size.y - 1)
            {
                return null;
            }

            cellCoords.y += 1;
            var neighbor = board.GetCell(cellCoords);
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }

        protected static Cell Left(Board board, Vector2Int cellCoords)
        {
            if (cellCoords.x <= 0)
            {
                return null;
            }

            cellCoords.x -= 1;
            var neighbor = board.GetCell(cellCoords);
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }

        protected static Cell Right(Board board, Vector2Int cellCoords)
        {
            if (cellCoords.x >= board.Size.x - 1)
            {
                return null;
            }

            cellCoords.x += 1;
            var neighbor = board.GetCell(cellCoords);
            if (neighbor == null || neighbor.IsEmpty)
            {
                return null;
            }

            return neighbor;
        }
    }
}
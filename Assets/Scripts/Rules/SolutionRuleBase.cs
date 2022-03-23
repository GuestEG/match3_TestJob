namespace Rules
{
    using System.Collections.Generic;
    using Client;
    using UnityEngine;

    public abstract class SolutionRuleBase : ScriptableObject
    {
        public abstract List<Cell> GetConnectedCells(Board board, Vector2Int cellCoords);
    }
}
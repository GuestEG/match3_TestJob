namespace Configs
{
    using System.Collections.Generic;
    using UnityEngine;
    using Views;

    [CreateAssetMenu(menuName = "Match3Test/BoardConfig")]
    public sealed class BoardConfig : ScriptableObject
    {
        public Vector2Int BoardSize = new Vector2Int(6,6);
        public int EmptyCellsNum = 3;
        public List<CellConfig> CellConfigs;
        [Header("Prefabs")] 
        public RowView RowPrefab;
        public CellView CellPrefab;
        public GameObject EmptyCellPrefab;
    }
}
namespace Configs
{
    using System.Collections.Generic;
    using UnityEngine;
    using Views;

    [CreateAssetMenu(menuName = "SWG/BoardConfig")]
    public sealed class BoardConfig : ScriptableObject
    {
        public Vector2Int BoardSize = new Vector2Int(6,6);
        public int EmptyCellsNum = 3;
        public float SwapAnimationDuration = 0.5f;
        public List<CellConfig> CellConfigs;
        [Header("Prefabs")] 
        public RowView RowPrefab;
        public CellView CellPrefab;
        public GameObject EmptyCellPrefab;
    }
}
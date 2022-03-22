namespace Configs
{
    using UnityEngine;
    using Views;

    [CreateAssetMenu(menuName = "SWG/BoardConfig")]
    public sealed class BoardConfig : ScriptableObject
    {
        public Vector2 BoardSize = new Vector2(6,6);
        public int EmptyCellsNum = 3;
        [Space] 
        public RowView RowPrefab;
        public CellView CellPrefab;
        public GameObject EmptyCellPrefab;
    }
}
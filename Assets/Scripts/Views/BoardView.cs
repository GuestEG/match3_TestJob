namespace Views
{
    using Client;
    using Configs;
    using UnityEngine;

    public class BoardView : MonoBehaviour
    {
        private BoardConfig _config;

        private CellView[,] _cells;

        public void SetConfig(BoardConfig config)
        {
            _config = config;
        }

        public void FillBoard(Cell[,] board)
        {
            _cells = new CellView[board.GetLength(0), board.GetLength(1)];

            for (int y = 0; y < board.GetLength(1); y++)
            {
                var row = Instantiate(_config.RowPrefab, this.transform);
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    var cell = board[x, y];

                    if (cell.IsEmpty)
                    {
                        _cells[x, y] = null;
                        Instantiate(_config.EmptyCellPrefab, row.transform, false);
                        continue;
                    }

                    var cellView = Instantiate(_config.CellPrefab, row.transform);
                    cellView.GetIcon().sprite = cell.Config.Icon;
                    cell.SetView(cellView);
                }
            }
        }
    }
}
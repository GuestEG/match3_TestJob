namespace Views
{
    using System;
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

        public void FillBoard(Board board, Action<Vector2Int> cellButtonClickHandler)
        {
            _cells = new CellView[board.Size.x, board.Size.y];

            for (int y = 0; y < board.Size.y; y++)
            {
                var row = Instantiate(_config.RowPrefab, this.transform);
                for (int x = 0; x < board.Size.x; x++)
                {
                    var cell = board.GetCell(x, y);

                    if (cell.IsEmpty)
                    {
                        _cells[x, y] = null;
                        Instantiate(_config.EmptyCellPrefab, row.transform, false);
                        continue;
                    }

                    var cellView = Instantiate(_config.CellPrefab, row.transform);
                    cellView.name = $"Cell {x},{y}";
                    cellView.AddButtonHandler(() => cellButtonClickHandler.Invoke(cell.Coords));
                    
                    cell.View = cellView;
                    cell.UpdateIconFromConfig();
                }
            }
        }
    }
}
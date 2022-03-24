namespace Views
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client;
    using Configs;
    using DG.Tweening;
    using UnityEngine;

    public class BoardView : MonoBehaviour
    {
        private BoardConfig _config;

        private CellView[,] _cellViews;

        public void SetConfig(BoardConfig config)
        {
            _config = config;
        }

        public void FillBoard(Board board, Action<Vector2Int> cellButtonClickHandler)
        {
            _cellViews = new CellView[board.Size.x, board.Size.y];

            for (int y = 0; y < board.Size.y; y++)
            {
                var row = Instantiate(_config.RowPrefab, this.transform);
                for (int x = 0; x < board.Size.x; x++)
                {
                    var cell = board.GetCell(x, y);

                    if (cell.IsEmpty)
                    {
                        _cellViews[x, y] = null;
                        Instantiate(_config.EmptyCellPrefab, row.transform, false);
                        continue;
                    }

                    var cellView = Instantiate(_config.CellPrefab, row.transform);
                    _cellViews[x, y] = cellView;
                    cellView.name = $"Cell {x},{y}";
                    cellView.AddButtonHandler(() => cellButtonClickHandler.Invoke(cell.Coords));
                    
                    cell.View = cellView;
                    cell.UpdateIconFromConfig();
                }
            }
        }

        public async Task SwapIcons(Vector2Int positon1, Vector2Int positon2, float duration)
        {
            var cell1 = _cellViews[positon1.x, positon1.y];
            var cell2 = _cellViews[positon2.x, positon2.y];
            var icon1 = cell1.Icon;
            var icon2 = cell2.Icon;
            var icon1dest = icon2.transform.position;
            var icon2dest = icon1.transform.position;

            var sequence = DOTween.Sequence();
            //reverse move
            sequence.Join(icon1.transform.DOMove(icon1dest, duration).From());
            sequence.Join(icon2.transform.DOMove(icon2dest, duration).From());
			
            await sequence.Play().AsyncWaitForCompletion();
        }

        public void ShowHighlight(Vector2Int cellPosition, bool show)
        {
            var cell = _cellViews[cellPosition.x, cellPosition.y];
            cell.ShowHighlight(show);
        }

        public async Task PopIcons(List<Cell> cells, float duration)
        {
            var sequence = DOTween.Sequence();
            var sequence2 = DOTween.Sequence();
            foreach (var cell in cells)
            {
                var cellView = _cellViews[cell.Coords.x, cell.Coords.y];
                var icon = cellView.Icon;
                sequence.Join(icon.transform.DOScale(2, duration / 2));
                sequence2.Join(icon.transform.DOScale(1, duration / 2));
            }

            sequence.Append(sequence2);

            await sequence.Play().AsyncWaitForCompletion();
        }
    }
}
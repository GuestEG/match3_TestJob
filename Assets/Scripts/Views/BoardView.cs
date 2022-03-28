namespace Views
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Client;
    using Configs;
    using DG.Tweening;
    using Rules;
    using UnityEngine;

    public class BoardView : MonoBehaviour
    {
        private BoardConfig _config;

        private CellView[,] _cellViews;

        private RowView[] _rows;

        public void SetConfig(BoardConfig config)
        {
            _config = config;
        }

        public void FillBoard(Board board, Action<Vector2Int> cellButtonClickHandler)
        {
            _cellViews = new CellView[board.Size.x, board.Size.y];
            _rows = new RowView[board.Size.y];

            for (int y = 0; y < board.Size.y; y++)
            {
                var row = Instantiate(_config.RowPrefab, this.transform);
                for (int x = 0; x < board.Size.x; x++)
                {
                    var cell = board.Cells[x, y];

                    if (cell.IsHole)
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
                    cell.UpdatePopIconFromConfig();
                }
                _rows[y] = row;
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

        public Sequence GetPopIconsSequence(List<Cell> cells, float duration)
        {
            var sequence = DOTween.Sequence();
            // var sequence2 = DOTween.Sequence();
            foreach (var cell in cells)
            {
                var iconSequence = DOTween.Sequence();
                var cellView = _cellViews[cell.Coords.x, cell.Coords.y];
                var popIcon = cellView.PopIcon;
                popIcon.gameObject.SetActive(true);
                iconSequence.Join(popIcon.transform.DOScale(2, duration));
                iconSequence.Join(popIcon.DOFade(0, duration));
                iconSequence.AppendCallback(() =>
                {
                    popIcon.gameObject.SetActive(false);
                    popIcon.transform.localScale = Vector3.one;
                    popIcon.color += Color.black; //resets Alpha back to 1
                    cell.UpdatePopIconFromConfig();

                });
                sequence.Join(iconSequence);
            }
            return sequence;
        }

        public Sequence GetMoveIconsSequence(List<CellMovement> movements, float rowOffset, float columnOffset, float duration)
        {
            var sequence = DOTween.Sequence();
            foreach (var movement in movements)
            {
                var targetCell = _cellViews[movement.Target.x, movement.Target.y];
                var targetIcon = targetCell.Icon;
                var movesCount = movement.Moves.Count;
                Vector3[] positions = new Vector3[movesCount + 1];
                //first position = end 
                positions[movesCount] = targetIcon.transform.position;

                for (int i = 0; i < movesCount; i++)
                {
                    //reverse
                    var move = movement.Moves[movesCount - 1 - i];
                    if (move.FromOffscreen)
                    {
                        //take offscreen position
                        positions[i] = targetIcon.transform.position
                                    + Vector3.down * rowOffset * move.RowOffset
                                    + Vector3.right * columnOffset * move.Column;
                        
                    }
                    else
                    {
                        var sourceCell = _cellViews[move.Coords.x, move.Coords.y];
                        positions[i] = sourceCell.Icon.transform.position;
                    }
                }

                //put icon in starting position immediately
                targetIcon.transform.position = positions[0];

                sequence.Join(
                    targetIcon.transform
                        .DOPath(
                            path: positions, 
                            duration:duration * movesCount,
                            pathMode: PathMode.Ignore)
                    );
                // var moveSequence = DOTween.Sequence();
                // var Vector3
                // foreach (var move in movement.Moves)
                // {
                //     
                //     // Vector3 startPosition;
                //     // Vector3 endPosition = _cellViews[move.End.x, move.End.y].transform.position;
                //     if (move.FromOffscreen)
                //     {
                //         //take offscreen position
                //         startPosition = targetIcon.transform.position
                //                         + Vector3.down * rowOffset * movement.Distance
                //                         + Vector3.right * move.Start.x;
                //         
                //     }
                //     else
                //     {
                //         var sourceCell = _cellViews[move.Start.x, move.Start.y];
                //         startPosition = sourceCell.Icon.transform.position;
                //     }
                //
                //     //reverse move
                //     moveSequence.PrependCallback(() => targetIcon.transform.position = endPosition);
                //     moveSequence.Append(targetIcon.transform.DOMove(startPosition, duration * movement.Distance).From());
                //     
                // }
                // sequence.Join(moveSequence);
            }

            return sequence;
        }

        public float GetRowHeight()
        {
            //might be wrong, depends on prefabs and the moment in time
            var row0 = _rows[0].transform;
            var row1 = _rows[1].transform;
            return Mathf.Abs(row0.position.y - row1.position.y);
        }

        public float GetColumnWidth()
        {
            var cell0 = _cellViews[0,0].transform;
            var cell1 = _cellViews[1,0].transform;
            return Mathf.Abs(cell0.position.x - cell1.position.x);
        }
    }
}
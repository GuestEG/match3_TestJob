namespace Rules
{
    using System;
    using Client;
    using Configs;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "SWG/DefaultBoardFillRule")]
    public sealed class DefaultBoardFillRule : BoardFillRuleBase
    {
        public override Cell[,] FillBoard(BoardConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
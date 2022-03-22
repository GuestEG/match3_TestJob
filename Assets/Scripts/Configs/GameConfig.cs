namespace Configs
{
    using System.Collections.Generic;
    using Rules;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "SWG/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Configs")]
        public List<CellConfig> CellConfigs;
        public BoardConfig BoardConfig;

        [Header("Rules")]
        public BoardFillRuleBase _boardFillRule;
    }
}
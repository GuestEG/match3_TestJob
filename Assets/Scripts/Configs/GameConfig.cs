namespace Configs
{
    using System.Collections.Generic;
    using Rules;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "SWG/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public float SwapAnimationDuration = 0.5f;
        public float PopAnimationDuration = 0.5f;

        [Header("Configs")]
        public BoardConfig BoardConfig;

        [Header("Rules")]
        public BoardFillRuleBase BoardFillRule;
        public BoardMovementRule BoardMovementRule;
        public List<SolutionRuleBase> SolutionRules;
    }
}
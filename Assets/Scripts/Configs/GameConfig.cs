namespace Configs
{
    using System.Collections.Generic;
    using Rules;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "Match3Test/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public float SwapAnimationDuration = 0.25f;
        public float PopAnimationDuration = 0.5f;
        public float FillAnimationDuration = 0.25f;

        [Header("Configs")]
        public BoardConfig BoardConfig;

        [Header("Rules")]
        public BoardFillRuleBase BoardFillRule;
        public BoardMovementRule BoardMovementRule;
        public ScoringRule ScoringRule;
        public List<SolutionRuleBase> SolutionRules;
    }
}
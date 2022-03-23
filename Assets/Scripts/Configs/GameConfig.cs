namespace Configs
{
    using Rules;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "SWG/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        public float SwapAnimationDuration = 0.5f;

        [Header("Configs")]
        public BoardConfig BoardConfig;

        [Header("Rules")]
        public BoardFillRuleBase BoardFillRule;
        public SolutionRuleBase SolutionRule;
    }
}
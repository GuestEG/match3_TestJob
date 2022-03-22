namespace Configs
{
    using Rules;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "SWG/GameConfig")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Configs")]
        public BoardConfig BoardConfig;

        [Header("Rules")]
        public BoardFillRuleBase BoardFillRule;
    }
}
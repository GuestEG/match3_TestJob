namespace Composition
{
    using Client;
    using Configs;
    using UnityEngine;

    public sealed class GameRoot : MonoBehaviour
    {
        [SerializeField] private BoardView _board;
        [SerializeField] private GameConfig _config;
        
        private Game _game;

        private void Start()
        {
            _game = new Game(_board, _config);
        }

        private void OnDestroy()
        {
            _game = null;
        }
    }
}
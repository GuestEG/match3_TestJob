namespace Composition
{
    using Client;
    using Configs;
    using UnityEngine;
    using Views;

    public sealed class GameRoot : MonoBehaviour
    {
        [SerializeField] private BoardView _board;
        [SerializeField] private GameConfig _config;
        
        private Game _game;

        private void Awake()
        {
            _game = new Game(_board, _config);
        }

        private void Start()
        {
            _game.StartGame();
        }

        private void OnDestroy()
        {
            _game = null;
        }
    }
}
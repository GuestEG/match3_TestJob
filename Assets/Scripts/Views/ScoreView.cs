namespace Views
{
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class ScoreView : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString("G6");
        }
    }
}
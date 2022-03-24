namespace Popup
{
    using System.Threading.Tasks;
    using DG.Tweening;
    using UnityEngine;

    public class ShufflePopup : MonoBehaviour
    {
        [SerializeField] private float _popupDuration = 1f;
        [SerializeField] private float _popupFadeDuration = 0.25f;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
        }

        public async Task ShowPopup()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(1, _popupFadeDuration));
            sequence.AppendInterval(_popupDuration);
            await sequence.Play().AsyncWaitForCompletion();
        }

        public async Task HidePopup()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(_canvasGroup.DOFade(0, _popupFadeDuration));
            await sequence.Play().AsyncWaitForCompletion();
        }
    }
}
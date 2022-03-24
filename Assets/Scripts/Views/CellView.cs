namespace Views
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class CellView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _highlight;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _popIcon;

        public Image Icon => _icon;
        public Image PopIcon => _popIcon;

        private void Start()
        {
            _highlight.gameObject.SetActive(false);
            _popIcon.gameObject.SetActive(false);
        }

        public void ShowHighlight(bool show)
        {
            _highlight.gameObject.SetActive(show);
        }

        public void AddButtonHandler(Action OnButtonClick)
        {
            _button.onClick.AddListener(()=>OnButtonClick.Invoke());
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
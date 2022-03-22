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

        public Transform GetTransform() => this.transform;
        public Image GetIcon() => _icon;

        public void ShowHighlight(bool show)
        {
            _highlight.gameObject.SetActive(show);
        }

        public void AddHandler(Action OnButtonClick)
        {
            _button.onClick.AddListener(()=>OnButtonClick.Invoke());
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
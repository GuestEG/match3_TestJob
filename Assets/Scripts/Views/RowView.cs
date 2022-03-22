namespace Views
{
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class RowView : MonoBehaviour
    {
        private HorizontalLayoutGroup _layoutGroup;

        public void Awake()
        {
            _layoutGroup = GetComponent<HorizontalLayoutGroup>();
        }

        public void AddCell(Transform cell)
        {
            cell.SetParent(_layoutGroup.transform, false);
        }
    }
}
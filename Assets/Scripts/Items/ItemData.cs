using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlackAle.Items
{
    public class ItemData
    {
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Color Color { get; set; }
        public Vector2Int GridPosition { get; set; }
        public bool Takeable { get; set; }
        public bool Usable { get; set; }
        public bool Combinable { get; set; }
        public string RequiredAbility { get; set; }
        public bool IsSelected { get; set; }
    }

    public class ItemView : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private RectTransform rectTransform;

        private ItemData _data;
        private Color _originalColor;

        public ItemData Data => _data;

        public void Initialize(ItemData data, float size)
        {
            _data = data;
            _originalColor = data.Color;

            if (itemImage == null)
                itemImage = GetComponent<Image>();
            if (rectTransform == null)
                rectTransform = GetComponent<RectTransform>();

            itemImage.color = _originalColor;
            rectTransform.sizeDelta = new Vector2(size, size);
            gameObject.name = $"Item_{data.ItemId}";
        }

        public void SetPosition(Vector2 localPos)
        {
            rectTransform.anchoredPosition = localPos;
        }

        public void Select()
        {
            _data.IsSelected = true;
            itemImage.color = Color.cyan;
        }

        public void Deselect()
        {
            _data.IsSelected = false;
            itemImage.color = _originalColor;
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}

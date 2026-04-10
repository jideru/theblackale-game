using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackAle.Config;
using BlackAle.Core;

namespace BlackAle.Items
{
    public class ItemManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform itemContainer;
        [SerializeField] private GridSystem gridSystem;

        [Header("Settings")]
        [SerializeField] private float itemSize = 36f;

        private List<ItemView> _items = new List<ItemView>();
        private ItemView _selectedItem;

        public ItemView SelectedItem => _selectedItem;
        public List<ItemView> Items => _items;

        public void SpawnItems(List<ItemConfig> itemConfigs)
        {
            ClearItems();

            foreach (var config in itemConfigs)
            {
                SpawnItem(config);
            }
        }

        public void SpawnItem(ItemConfig config)
        {
            // Create item game object
            GameObject itemObj = new GameObject($"Item_{config.itemId}", typeof(RectTransform), typeof(Image));
            itemObj.transform.SetParent(itemContainer, false);

            ItemView view = itemObj.AddComponent<ItemView>();

            ItemData data = new ItemData
            {
                ItemId = config.itemId,
                Name = config.name,
                Description = config.description,
                Color = config.color.ToColor(),
                GridPosition = new Vector2Int(config.position.x, config.position.y),
                Takeable = config.takeable,
                Usable = config.usable,
                Combinable = config.combinable,
                RequiredAbility = config.requiredAbility
            };

            view.Initialize(data, itemSize);
            view.SetPosition(gridSystem.GridToLocalPosition(config.position.x, config.position.y));

            // Mark grid cell as occupied
            gridSystem.SetOccupied(data.GridPosition, true);

            _items.Add(view);
        }

        public ItemView GetItemAt(Vector2Int gridPos)
        {
            foreach (var item in _items)
            {
                if (item.Data.GridPosition == gridPos)
                    return item;
            }
            return null;
        }

        public ItemView GetAdjacentItem(Vector2Int characterPos)
        {
            foreach (var item in _items)
            {
                if (gridSystem.IsAdjacent(characterPos, item.Data.GridPosition))
                    return item;
            }
            return null;
        }

        public List<ItemView> GetAdjacentItems(Vector2Int characterPos)
        {
            var adjacent = new List<ItemView>();
            foreach (var item in _items)
            {
                if (gridSystem.IsAdjacent(characterPos, item.Data.GridPosition))
                    adjacent.Add(item);
            }
            return adjacent;
        }

        public void SelectItem(ItemView item)
        {
            if (_selectedItem != null)
                _selectedItem.Deselect();

            _selectedItem = item;
            if (_selectedItem != null)
                _selectedItem.Select();
        }

        public void DeselectItem()
        {
            if (_selectedItem != null)
            {
                _selectedItem.Deselect();
                _selectedItem = null;
            }
        }

        public void RemoveItem(ItemView item)
        {
            if (item == _selectedItem)
                _selectedItem = null;

            gridSystem.SetOccupied(item.Data.GridPosition, false);
            _items.Remove(item);
            item.Remove();
        }

        private void ClearItems()
        {
            foreach (var item in _items)
            {
                if (item != null)
                {
                    gridSystem.SetOccupied(item.Data.GridPosition, false);
                    item.Remove();
                }
            }
            _items.Clear();
            _selectedItem = null;
        }
    }
}

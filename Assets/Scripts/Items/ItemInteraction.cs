using UnityEngine;
using BlackAle.Core;
using BlackAle.Character;
using BlackAle.UI;

namespace BlackAle.Items
{
    public class ItemInteraction : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ItemManager itemManager;
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private ContextMenuUI contextMenu;
        [SerializeField] private InspectWindowUI inspectWindow;
        [SerializeField] private CharacterPanelUI characterPanel;

        private ItemView _rightClickedItem;

        private void Update()
        {
            if (GameManager.Instance == null) return;

            var state = GameManager.Instance.CurrentState;

            if (state == GameState.Playing)
            {
                HandleRightClick();
            }
        }

        private void HandleRightClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector2Int clickedGrid = gridSystem.ScreenToGrid(Input.mousePosition);
                ItemView item = itemManager.GetItemAt(clickedGrid);

                if (item != null)
                {
                    // Check if character is adjacent
                    if (gridSystem.IsAdjacent(playerController.GridPosition, item.Data.GridPosition))
                    {
                        _rightClickedItem = item;

                        // Select the item (highlight cyan)
                        itemManager.SelectItem(item);

                        // Show context menu
                        contextMenu.Show(
                            Input.mousePosition,
                            item.Data,
                            OnInspect,
                            OnTake,
                            null, // Use - disabled
                            null  // Combine - disabled
                        );

                        GameManager.Instance.CurrentState = GameState.ContextMenu;
                    }
                }
            }
        }

        private void OnInspect()
        {
            if (_rightClickedItem == null) return;

            contextMenu.Hide();
            inspectWindow.Show(_rightClickedItem.Data.Name, _rightClickedItem.Data.Description);
            GameManager.Instance.CurrentState = GameState.Inspect;
        }

        private void OnTake()
        {
            if (_rightClickedItem == null) return;

            if (!_rightClickedItem.Data.Takeable)
            {
                contextMenu.Hide();
                inspectWindow.Show("Cannot Take", "This item cannot be picked up.");
                GameManager.Instance.CurrentState = GameState.Inspect;
                return;
            }

            var character = GameManager.Instance.ActiveCharacter;
            if (!character.CanAddItem())
            {
                contextMenu.Hide();
                inspectWindow.Show("Inventory Full", "Your inventory is full. Drop something first.");
                GameManager.Instance.CurrentState = GameState.Inspect;
                return;
            }

            // Add to inventory
            var invItem = new InventoryItem(
                _rightClickedItem.Data.ItemId,
                _rightClickedItem.Data.Name,
                _rightClickedItem.Data.Description,
                _rightClickedItem.Data.Color
            );
            character.AddItem(invItem);

            // Remove from scene
            itemManager.RemoveItem(_rightClickedItem);
            _rightClickedItem = null;

            // Update UI
            contextMenu.Hide();
            characterPanel.RefreshInventory();
            GameManager.Instance.CurrentState = GameState.Playing;
        }

        public void OnContextMenuClosed()
        {
            itemManager.DeselectItem();
            _rightClickedItem = null;

            if (GameManager.Instance.CurrentState == GameState.ContextMenu)
                GameManager.Instance.CurrentState = GameState.Playing;
        }

        public void OnInspectWindowClosed()
        {
            itemManager.DeselectItem();
            _rightClickedItem = null;

            if (GameManager.Instance.CurrentState == GameState.Inspect)
                GameManager.Instance.CurrentState = GameState.Playing;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackAle.Items;
using BlackAle.Core;

namespace BlackAle.UI
{
    public class ContextMenuUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private RectTransform menuRect;
        [SerializeField] private Button inspectButton;
        [SerializeField] private Button takeButton;
        [SerializeField] private Button useButton;
        [SerializeField] private Button combineButton;
        [SerializeField] private TextMeshProUGUI itemNameText;

        [Header("Button Texts")]
        [SerializeField] private TextMeshProUGUI inspectText;
        [SerializeField] private TextMeshProUGUI takeText;
        [SerializeField] private TextMeshProUGUI useText;
        [SerializeField] private TextMeshProUGUI combineText;

        private Action _onInspect;
        private Action _onTake;
        private Action _onUse;
        private Action _onCombine;
        private ItemInteraction _itemInteraction;

        private void Start()
        {
            _itemInteraction = FindObjectOfType<ItemInteraction>();
            Hide();
        }

        private void Update()
        {
            // Close menu on left click outside or Escape
            if (menuPanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
                {
                    // Small delay check to avoid closing immediately on the click that opens it
                    if (Time.frameCount > _showFrame + 1)
                    {
                        Hide();
                        _itemInteraction?.OnContextMenuClosed();
                    }
                }
            }
        }

        private int _showFrame;

        public void Show(Vector2 screenPosition, ItemData itemData, Action onInspect, Action onTake, Action onUse, Action onCombine)
        {
            _onInspect = onInspect;
            _onTake = onTake;
            _onUse = onUse;
            _onCombine = onCombine;
            _showFrame = Time.frameCount;

            // Set item name
            if (itemNameText != null)
                itemNameText.text = itemData.Name;

            // Configure buttons
            SetupButton(inspectButton, inspectText, "Inspect", _onInspect != null, OnInspectClicked);
            SetupButton(takeButton, takeText, itemData.Takeable ? "Take" : "Take (Can't)", _onTake != null && itemData.Takeable, OnTakeClicked);
            SetupButton(useButton, useText, "Use", false, null);
            SetupButton(combineButton, combineText, "Combine", false, null);

            // Position menu at click location
            menuPanel.SetActive(true);

            // Adjust position to stay on screen
            Vector2 pos = screenPosition;
            if (pos.x + 200 > Screen.width) pos.x = Screen.width - 200;
            if (pos.y - 200 < 0) pos.y = 200;

            menuRect.position = pos;
        }

        public void Hide()
        {
            menuPanel.SetActive(false);
            ClearListeners();
        }

        private void SetupButton(Button button, TextMeshProUGUI text, string label, bool interactable, Action callback)
        {
            if (button == null) return;

            button.onClick.RemoveAllListeners();
            button.interactable = interactable;

            if (text != null)
            {
                text.text = label;
                text.color = interactable ? Color.white : new Color(0.4f, 0.4f, 0.4f);
            }

            if (callback != null && interactable)
                button.onClick.AddListener(() => callback());
        }

        private void OnInspectClicked() => _onInspect?.Invoke();
        private void OnTakeClicked() => _onTake?.Invoke();

        private void ClearListeners()
        {
            inspectButton?.onClick.RemoveAllListeners();
            takeButton?.onClick.RemoveAllListeners();
            useButton?.onClick.RemoveAllListeners();
            combineButton?.onClick.RemoveAllListeners();
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackAle.Core;
using BlackAle.Character;

namespace BlackAle.UI
{
    public class CharacterPanelUI : MonoBehaviour
    {
        [Header("Character Info")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI classText;
        [SerializeField] private TextMeshProUGUI abilityText;
        [SerializeField] private Image portraitImage;

        [Header("Stats")]
        [SerializeField] private TextMeshProUGUI strText;
        [SerializeField] private TextMeshProUGUI dexText;
        [SerializeField] private TextMeshProUGUI wisText;
        [SerializeField] private TextMeshProUGUI chaText;

        [Header("Inventory")]
        [SerializeField] private RectTransform inventoryGrid;
        [SerializeField] private float slotSize = 48f;
        [SerializeField] private float slotSpacing = 4f;
        [SerializeField] private int inventoryCols = 3;

        private GameObject[] _inventorySlots;

        public void Initialize(CharacterData character)
        {
            UpdateCharacterInfo(character);
            CreateInventorySlots(character.maxInventorySize);
            RefreshInventory();
        }

        public void UpdateCharacterInfo(CharacterData character)
        {
            if (nameText != null)
                nameText.text = character.characterName;

            if (classText != null)
                classText.text = character.characterClass.ToString();

            if (abilityText != null)
                abilityText.text = $"Ability: {character.abilityName}";

            if (portraitImage != null)
                portraitImage.color = Color.magenta;

            if (strText != null)
                strText.text = $"STR: {character.stats.strength}";
            if (dexText != null)
                dexText.text = $"DEX: {character.stats.dexterity}";
            if (wisText != null)
                wisText.text = $"WIS: {character.stats.wisdom}";
            if (chaText != null)
                chaText.text = $"CHA: {character.stats.charisma}";
        }

        private void CreateInventorySlots(int count)
        {
            // Clear existing
            if (_inventorySlots != null)
            {
                foreach (var slot in _inventorySlots)
                    if (slot != null) Destroy(slot);
            }

            _inventorySlots = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                int col = i % inventoryCols;
                int row = i / inventoryCols;

                GameObject slot = new GameObject($"InvSlot_{i}", typeof(RectTransform), typeof(Image));
                slot.transform.SetParent(inventoryGrid, false);

                RectTransform rt = slot.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(slotSize, slotSize);
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);
                rt.anchoredPosition = new Vector2(
                    col * (slotSize + slotSpacing),
                    -row * (slotSize + slotSpacing)
                );

                Image img = slot.GetComponent<Image>();
                img.color = new Color(0.15f, 0.15f, 0.2f, 1f); // Dark slot background

                // Add item color indicator (child)
                GameObject itemIndicator = new GameObject("ItemColor", typeof(RectTransform), typeof(Image));
                itemIndicator.transform.SetParent(slot.transform, false);
                RectTransform itemRt = itemIndicator.GetComponent<RectTransform>();
                itemRt.anchorMin = new Vector2(0.15f, 0.15f);
                itemRt.anchorMax = new Vector2(0.85f, 0.85f);
                itemRt.offsetMin = Vector2.zero;
                itemRt.offsetMax = Vector2.zero;
                Image itemImg = itemIndicator.GetComponent<Image>();
                itemImg.color = Color.clear; // Empty

                // Add label
                GameObject labelObj = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
                labelObj.transform.SetParent(slot.transform, false);
                RectTransform labelRt = labelObj.GetComponent<RectTransform>();
                labelRt.anchorMin = Vector2.zero;
                labelRt.anchorMax = Vector2.one;
                labelRt.offsetMin = Vector2.zero;
                labelRt.offsetMax = Vector2.zero;
                TextMeshProUGUI label = labelObj.GetComponent<TextMeshProUGUI>();
                label.text = "";
                label.fontSize = 10;
                label.color = Color.white;
                label.alignment = TextAlignmentOptions.Center;
                label.enableWordWrapping = true;
                label.overflowMode = TextOverflowModes.Truncate;

                _inventorySlots[i] = slot;
            }
        }

        public void RefreshInventory()
        {
            if (GameManager.Instance == null) return;

            var character = GameManager.Instance.ActiveCharacter;
            if (character == null || _inventorySlots == null) return;

            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                if (_inventorySlots[i] == null) continue;

                Image itemImg = _inventorySlots[i].transform.Find("ItemColor")?.GetComponent<Image>();
                TextMeshProUGUI label = _inventorySlots[i].transform.Find("Label")?.GetComponent<TextMeshProUGUI>();

                if (i < character.inventory.Count)
                {
                    var item = character.inventory[i];
                    if (itemImg != null) itemImg.color = item.color;
                    if (label != null) label.text = item.name;
                }
                else
                {
                    if (itemImg != null) itemImg.color = Color.clear;
                    if (label != null) label.text = "";
                }
            }
        }
    }
}

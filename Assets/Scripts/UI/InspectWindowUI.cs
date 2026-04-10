using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackAle.Core;
using BlackAle.Items;

namespace BlackAle.UI
{
    public class InspectWindowUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject windowPanel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button backgroundBlocker;

        private ItemInteraction _itemInteraction;

        private void Start()
        {
            _itemInteraction = FindObjectOfType<ItemInteraction>();

            if (closeButton != null)
                closeButton.onClick.AddListener(Hide);

            if (backgroundBlocker != null)
                backgroundBlocker.onClick.AddListener(Hide);

            windowPanel.SetActive(false);
        }

        private void Update()
        {
            if (windowPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }

        public void Show(string title, string description)
        {
            if (titleText != null)
                titleText.text = title;

            if (descriptionText != null)
                descriptionText.text = description;

            windowPanel.SetActive(true);
        }

        public void Hide()
        {
            windowPanel.SetActive(false);
            _itemInteraction?.OnInspectWindowClosed();
        }
    }
}

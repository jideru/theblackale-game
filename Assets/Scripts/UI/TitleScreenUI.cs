using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackAle.Core;

namespace BlackAle.UI
{
    public class TitleScreenUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private Button startButton;
        [SerializeField] private TextMeshProUGUI startButtonText;
        [SerializeField] private TextMeshProUGUI versionText;

        private void Start()
        {
            SetupTitle();
            SetupButton();
        }

        private void SetupTitle()
        {
            if (titleText != null)
            {
                titleText.text = "The Black Ale\nAdventure";
                titleText.color = Color.white;
                titleText.fontSize = 72;
                titleText.fontStyle = FontStyles.Bold | FontStyles.Italic;
                titleText.alignment = TextAlignmentOptions.Center;
            }

            if (versionText != null)
            {
                versionText.text = "v0.01a";
                versionText.color = new Color(0.5f, 0.5f, 0.5f);
                versionText.fontSize = 18;
            }
        }

        private void SetupButton()
        {
            if (startButton != null)
            {
                startButton.onClick.AddListener(OnStartGame);

                // Style the button
                var colors = startButton.colors;
                colors.normalColor = new Color(0.1f, 0.1f, 0.2f);
                colors.highlightedColor = new Color(0.2f, 0.2f, 0.4f);
                colors.pressedColor = new Color(0.05f, 0.05f, 0.1f);
                startButton.colors = colors;
            }

            if (startButtonText != null)
            {
                startButtonText.text = "Start Game";
                startButtonText.color = Color.white;
                startButtonText.fontSize = 36;
            }
        }

        private void OnStartGame()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
            else
            {
                Debug.LogError("[TitleScreen] GameManager not found!");
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackAle.Core;

namespace BlackAle.UI
{
    /// <summary>
    /// Bootstrap script for the TitleScene. Creates the title screen UI programmatically.
    /// </summary>
    public class TitleSceneSetup : MonoBehaviour
    {
        private void Start()
        {
            Camera.main.backgroundColor = Color.black;

            // Ensure GameManager exists
            if (GameManager.Instance == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gmObj.AddComponent<GameManager>();
            }

            CreateTitleUI();
        }

        private void CreateTitleUI()
        {
            // Canvas
            GameObject canvasObj = new GameObject("TitleCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();

            // EventSystem
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            // Background
            GameObject bgObj = new GameObject("Background", typeof(RectTransform), typeof(Image));
            bgObj.transform.SetParent(canvasObj.transform, false);
            RectTransform bgRect = bgObj.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            bgObj.GetComponent<Image>().color = Color.black;
            bgObj.GetComponent<Image>().raycastTarget = false;

            // Title text
            GameObject titleObj = new GameObject("TitleText", typeof(RectTransform), typeof(TextMeshProUGUI));
            titleObj.transform.SetParent(canvasObj.transform, false);
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.1f, 0.45f);
            titleRect.anchorMax = new Vector2(0.9f, 0.85f);
            titleRect.offsetMin = Vector2.zero;
            titleRect.offsetMax = Vector2.zero;

            TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
            titleText.text = "The Black Ale\n<size=60>Adventure</size>";
            titleText.fontSize = 80;
            titleText.fontStyle = FontStyles.Bold | FontStyles.Italic;
            titleText.color = Color.white;
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.enableWordWrapping = true;

            // Subtitle / tagline
            GameObject taglineObj = new GameObject("Tagline", typeof(RectTransform), typeof(TextMeshProUGUI));
            taglineObj.transform.SetParent(canvasObj.transform, false);
            RectTransform tagRect = taglineObj.GetComponent<RectTransform>();
            tagRect.anchorMin = new Vector2(0.2f, 0.38f);
            tagRect.anchorMax = new Vector2(0.8f, 0.45f);
            tagRect.offsetMin = Vector2.zero;
            tagRect.offsetMax = Vector2.zero;

            TextMeshProUGUI tagline = taglineObj.GetComponent<TextMeshProUGUI>();
            tagline.text = "A Dwarven Quest for the Legendary Brew";
            tagline.fontSize = 20;
            tagline.fontStyle = FontStyles.Italic;
            tagline.color = new Color(0.78f, 0.66f, 0.19f); // Gold
            tagline.alignment = TextAlignmentOptions.Center;

            // Start Game button
            GameObject buttonObj = new GameObject("StartButton", typeof(RectTransform), typeof(Image), typeof(Button));
            buttonObj.transform.SetParent(canvasObj.transform, false);
            RectTransform btnRect = buttonObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.35f, 0.18f);
            btnRect.anchorMax = new Vector2(0.65f, 0.28f);
            btnRect.offsetMin = Vector2.zero;
            btnRect.offsetMax = Vector2.zero;

            Image btnImage = buttonObj.GetComponent<Image>();
            btnImage.color = new Color(0.1f, 0.1f, 0.2f, 1f);

            Outline btnOutline = buttonObj.AddComponent<Outline>();
            btnOutline.effectColor = new Color(0.78f, 0.66f, 0.19f);
            btnOutline.effectDistance = new Vector2(2, 2);

            Button startButton = buttonObj.GetComponent<Button>();
            var colors = startButton.colors;
            colors.normalColor = new Color(0.1f, 0.1f, 0.2f);
            colors.highlightedColor = new Color(0.2f, 0.2f, 0.35f);
            colors.pressedColor = new Color(0.05f, 0.05f, 0.1f);
            startButton.colors = colors;

            // Button text
            GameObject btnTextObj = new GameObject("ButtonText", typeof(RectTransform), typeof(TextMeshProUGUI));
            btnTextObj.transform.SetParent(buttonObj.transform, false);
            RectTransform btnTextRect = btnTextObj.GetComponent<RectTransform>();
            btnTextRect.anchorMin = Vector2.zero;
            btnTextRect.anchorMax = Vector2.one;
            btnTextRect.offsetMin = Vector2.zero;
            btnTextRect.offsetMax = Vector2.zero;

            TextMeshProUGUI btnText = btnTextObj.GetComponent<TextMeshProUGUI>();
            btnText.text = "Start Game";
            btnText.fontSize = 32;
            btnText.color = Color.white;
            btnText.alignment = TextAlignmentOptions.Center;

            startButton.onClick.AddListener(OnStartClicked);

            // Version text
            GameObject versionObj = new GameObject("VersionText", typeof(RectTransform), typeof(TextMeshProUGUI));
            versionObj.transform.SetParent(canvasObj.transform, false);
            RectTransform verRect = versionObj.GetComponent<RectTransform>();
            verRect.anchorMin = new Vector2(0.4f, 0.02f);
            verRect.anchorMax = new Vector2(0.6f, 0.08f);
            verRect.offsetMin = Vector2.zero;
            verRect.offsetMax = Vector2.zero;

            TextMeshProUGUI verText = versionObj.GetComponent<TextMeshProUGUI>();
            verText.text = "v0.01a";
            verText.fontSize = 16;
            verText.color = new Color(0.4f, 0.4f, 0.4f);
            verText.alignment = TextAlignmentOptions.Center;
        }

        private void OnStartClicked()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StartGame();
            }
        }
    }
}

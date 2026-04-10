using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlackAle.Core;
using BlackAle.Character;
using BlackAle.Items;
using BlackAle.UI;

namespace BlackAle.Core
{
    /// <summary>
    /// Bootstrap script for the GameScene. Creates all UI elements programmatically
    /// so the scene file can be minimal. Attach to an empty GameObject in GameScene.
    /// </summary>
    public class GameSceneSetup : MonoBehaviour
    {
        private GridSystem _gridSystem;
        private PlayerController _playerController;
        private ItemManager _itemManager;
        private ItemInteraction _itemInteraction;
        private ContextMenuUI _contextMenu;
        private InspectWindowUI _inspectWindow;
        private CharacterPanelUI _characterPanel;

        private Canvas _mainCanvas;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            if (_mainCamera != null)
                _mainCamera.backgroundColor = Color.black;

            CreateCanvas();
            CreateGameLayout();
            LoadScene("tavern");
        }

        private void CreateCanvas()
        {
            GameObject canvasObj = new GameObject("MainCanvas");
            _mainCanvas = canvasObj.AddComponent<Canvas>();
            _mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _mainCanvas.sortingOrder = 0;

            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObj.AddComponent<GraphicRaycaster>();

            // EventSystem
            if (FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }

        private void CreateGameLayout()
        {
            // === GAME VIEWPORT (Left 75%) ===
            GameObject viewportObj = CreatePanel("GameViewport", _mainCanvas.transform);
            RectTransform viewportRect = viewportObj.GetComponent<RectTransform>();
            viewportRect.anchorMin = new Vector2(0, 0);
            viewportRect.anchorMax = new Vector2(0.75f, 1);
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            Image viewportBg = viewportObj.GetComponent<Image>();
            viewportBg.color = Color.black;

            // Grid container
            GameObject gridObj = new GameObject("GridContainer", typeof(RectTransform));
            gridObj.transform.SetParent(viewportObj.transform, false);
            RectTransform gridRect = gridObj.GetComponent<RectTransform>();
            gridRect.anchorMin = Vector2.zero;
            gridRect.anchorMax = Vector2.one;
            gridRect.offsetMin = new Vector2(20, 20);
            gridRect.offsetMax = new Vector2(-20, -20);

            // Grid System
            _gridSystem = gridObj.AddComponent<GridSystem>();

            // Item container (on top of grid)
            GameObject itemContainerObj = new GameObject("ItemContainer", typeof(RectTransform));
            itemContainerObj.transform.SetParent(viewportObj.transform, false);
            RectTransform itemRect = itemContainerObj.GetComponent<RectTransform>();
            itemRect.anchorMin = Vector2.zero;
            itemRect.anchorMax = Vector2.one;
            itemRect.offsetMin = new Vector2(20, 20);
            itemRect.offsetMax = new Vector2(-20, -20);

            // Player character
            GameObject playerObj = CreatePlayerCharacter(viewportObj.transform);

            // Item Manager
            _itemManager = itemContainerObj.AddComponent<ItemManager>();

            // === CHARACTER PANEL (Right 25%) ===
            GameObject panelObj = CreatePanel("CharacterPanel", _mainCanvas.transform);
            RectTransform panelRect = panelObj.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.75f, 0);
            panelRect.anchorMax = new Vector2(1, 1);
            panelRect.offsetMin = Vector2.zero;
            panelRect.offsetMax = Vector2.zero;
            Image panelBg = panelObj.GetComponent<Image>();
            panelBg.color = new Color(0.1f, 0.1f, 0.18f, 1f);

            _characterPanel = CreateCharacterPanel(panelObj.transform);

            // === CONTEXT MENU ===
            _contextMenu = CreateContextMenu(_mainCanvas.transform);

            // === INSPECT WINDOW ===
            _inspectWindow = CreateInspectWindow(_mainCanvas.transform);

            // === ITEM INTERACTION ===
            GameObject interactionObj = new GameObject("ItemInteraction");
            _itemInteraction = interactionObj.AddComponent<ItemInteraction>();

            // Wire up serialized fields via reflection (since we're creating at runtime)
            SetPrivateField(_itemInteraction, "itemManager", _itemManager);
            SetPrivateField(_itemInteraction, "gridSystem", _gridSystem);
            SetPrivateField(_itemInteraction, "playerController", _playerController);
            SetPrivateField(_itemInteraction, "contextMenu", _contextMenu);
            SetPrivateField(_itemInteraction, "inspectWindow", _inspectWindow);
            SetPrivateField(_itemInteraction, "characterPanel", _characterPanel);

            SetPrivateField(_itemManager, "gridSystem", _gridSystem);
            SetPrivateField(_itemManager, "itemContainer", itemRect);

            SetPrivateField(_gridSystem, "gridContainer", gridRect);
        }

        private GameObject CreatePlayerCharacter(Transform parent)
        {
            // Background image
            GameObject playerObj = new GameObject("Player", typeof(RectTransform), typeof(Image));
            playerObj.transform.SetParent(parent, false);
            RectTransform playerRect = playerObj.GetComponent<RectTransform>();
            playerRect.sizeDelta = new Vector2(40, 40);
            Image playerImg = playerObj.GetComponent<Image>();
            playerImg.color = Color.magenta;

            // Label "C"
            GameObject labelObj = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            labelObj.transform.SetParent(playerObj.transform, false);
            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;
            TextMeshProUGUI label = labelObj.GetComponent<TextMeshProUGUI>();
            label.text = "C";
            label.fontSize = 24;
            label.color = Color.white;
            label.alignment = TextAlignmentOptions.Center;

            _playerController = playerObj.AddComponent<PlayerController>();
            SetPrivateField(_playerController, "characterRect", playerRect);
            SetPrivateField(_playerController, "characterLabel", label);
            SetPrivateField(_playerController, "characterImage", playerImg);

            return playerObj;
        }

        private CharacterPanelUI CreateCharacterPanel(Transform parent)
        {
            GameObject panelContent = new GameObject("PanelContent", typeof(RectTransform));
            panelContent.transform.SetParent(parent, false);
            RectTransform contentRect = panelContent.GetComponent<RectTransform>();
            contentRect.anchorMin = Vector2.zero;
            contentRect.anchorMax = Vector2.one;
            contentRect.offsetMin = new Vector2(10, 10);
            contentRect.offsetMax = new Vector2(-10, -10);

            float yPos = -10;

            // Portrait placeholder
            GameObject portrait = new GameObject("Portrait", typeof(RectTransform), typeof(Image));
            portrait.transform.SetParent(contentRect, false);
            RectTransform portraitRect = portrait.GetComponent<RectTransform>();
            portraitRect.anchorMin = new Vector2(0.5f, 1);
            portraitRect.anchorMax = new Vector2(0.5f, 1);
            portraitRect.pivot = new Vector2(0.5f, 1);
            portraitRect.anchoredPosition = new Vector2(0, yPos);
            portraitRect.sizeDelta = new Vector2(80, 80);
            Image portraitImg = portrait.GetComponent<Image>();
            portraitImg.color = Color.magenta;

            // "C" on portrait
            GameObject portraitLabel = new GameObject("PortraitLabel", typeof(RectTransform), typeof(TextMeshProUGUI));
            portraitLabel.transform.SetParent(portrait.transform, false);
            RectTransform plRect = portraitLabel.GetComponent<RectTransform>();
            plRect.anchorMin = Vector2.zero;
            plRect.anchorMax = Vector2.one;
            plRect.offsetMin = Vector2.zero;
            plRect.offsetMax = Vector2.zero;
            TextMeshProUGUI plText = portraitLabel.GetComponent<TextMeshProUGUI>();
            plText.text = "C";
            plText.fontSize = 48;
            plText.color = Color.white;
            plText.alignment = TextAlignmentOptions.Center;

            yPos -= 100;

            // Name
            TextMeshProUGUI nameText = CreateText("NameText", contentRect, yPos, 20, FontStyles.Bold);
            yPos -= 28;

            // Class
            TextMeshProUGUI classText = CreateText("ClassText", contentRect, yPos, 16, FontStyles.Normal);
            yPos -= 24;

            // Ability
            TextMeshProUGUI abilityText = CreateText("AbilityText", contentRect, yPos, 14, FontStyles.Italic);
            yPos -= 30;

            // Separator
            GameObject sep = new GameObject("Separator", typeof(RectTransform), typeof(Image));
            sep.transform.SetParent(contentRect, false);
            RectTransform sepRect = sep.GetComponent<RectTransform>();
            sepRect.anchorMin = new Vector2(0, 1);
            sepRect.anchorMax = new Vector2(1, 1);
            sepRect.pivot = new Vector2(0.5f, 1);
            sepRect.anchoredPosition = new Vector2(0, yPos);
            sepRect.sizeDelta = new Vector2(0, 2);
            sep.GetComponent<Image>().color = new Color(0.78f, 0.66f, 0.19f); // Gold

            yPos -= 15;

            // Stats
            TextMeshProUGUI strText = CreateText("STR", contentRect, yPos, 16, FontStyles.Normal);
            yPos -= 22;
            TextMeshProUGUI dexText = CreateText("DEX", contentRect, yPos, 16, FontStyles.Normal);
            yPos -= 22;
            TextMeshProUGUI wisText = CreateText("WIS", contentRect, yPos, 16, FontStyles.Normal);
            yPos -= 22;
            TextMeshProUGUI chaText = CreateText("CHA", contentRect, yPos, 16, FontStyles.Normal);
            yPos -= 30;

            // Separator 2
            GameObject sep2 = new GameObject("Separator2", typeof(RectTransform), typeof(Image));
            sep2.transform.SetParent(contentRect, false);
            RectTransform sep2Rect = sep2.GetComponent<RectTransform>();
            sep2Rect.anchorMin = new Vector2(0, 1);
            sep2Rect.anchorMax = new Vector2(1, 1);
            sep2Rect.pivot = new Vector2(0.5f, 1);
            sep2Rect.anchoredPosition = new Vector2(0, yPos);
            sep2Rect.sizeDelta = new Vector2(0, 2);
            sep2.GetComponent<Image>().color = new Color(0.78f, 0.66f, 0.19f);

            yPos -= 15;

            // Inventory label
            TextMeshProUGUI invLabel = CreateText("InventoryLabel", contentRect, yPos, 16, FontStyles.Bold);
            invLabel.text = "Inventory";
            yPos -= 25;

            // Inventory grid container
            GameObject invGrid = new GameObject("InventoryGrid", typeof(RectTransform));
            invGrid.transform.SetParent(contentRect, false);
            RectTransform invRect = invGrid.GetComponent<RectTransform>();
            invRect.anchorMin = new Vector2(0, 1);
            invRect.anchorMax = new Vector2(1, 1);
            invRect.pivot = new Vector2(0, 1);
            invRect.anchoredPosition = new Vector2(0, yPos);
            invRect.sizeDelta = new Vector2(0, 200);

            // Create CharacterPanelUI component
            CharacterPanelUI panelUI = panelContent.AddComponent<CharacterPanelUI>();
            SetPrivateField(panelUI, "nameText", nameText);
            SetPrivateField(panelUI, "classText", classText);
            SetPrivateField(panelUI, "abilityText", abilityText);
            SetPrivateField(panelUI, "portraitImage", portraitImg);
            SetPrivateField(panelUI, "strText", strText);
            SetPrivateField(panelUI, "dexText", dexText);
            SetPrivateField(panelUI, "wisText", wisText);
            SetPrivateField(panelUI, "chaText", chaText);
            SetPrivateField(panelUI, "inventoryGrid", invRect);

            return panelUI;
        }

        private TextMeshProUGUI CreateText(string name, RectTransform parent, float yPos, float fontSize, FontStyles style)
        {
            GameObject textObj = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            textObj.transform.SetParent(parent, false);
            RectTransform rt = textObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);
            rt.anchoredPosition = new Vector2(0, yPos);
            rt.sizeDelta = new Vector2(0, 24);

            TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
            tmp.fontSize = fontSize;
            tmp.color = Color.white;
            tmp.fontStyle = style;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = true;

            return tmp;
        }

        private ContextMenuUI CreateContextMenu(Transform parent)
        {
            // Menu panel
            GameObject menuPanelObj = new GameObject("ContextMenu", typeof(RectTransform), typeof(Image));
            menuPanelObj.transform.SetParent(parent, false);
            RectTransform menuRect = menuPanelObj.GetComponent<RectTransform>();
            menuRect.sizeDelta = new Vector2(180, 200);
            Image menuBg = menuPanelObj.GetComponent<Image>();
            menuBg.color = new Color(0.1f, 0.1f, 0.18f, 0.95f);

            // Add outline
            Outline outline = menuPanelObj.AddComponent<Outline>();
            outline.effectColor = new Color(0.78f, 0.66f, 0.19f);
            outline.effectDistance = new Vector2(2, 2);

            // Item name header
            TextMeshProUGUI itemName = CreateMenuText("ItemName", menuPanelObj.transform, -10, 18, FontStyles.Bold);

            // Separator
            GameObject sep = new GameObject("Sep", typeof(RectTransform), typeof(Image));
            sep.transform.SetParent(menuPanelObj.transform, false);
            RectTransform sepRect = sep.GetComponent<RectTransform>();
            sepRect.anchorMin = new Vector2(0.05f, 1);
            sepRect.anchorMax = new Vector2(0.95f, 1);
            sepRect.pivot = new Vector2(0.5f, 1);
            sepRect.anchoredPosition = new Vector2(0, -35);
            sepRect.sizeDelta = new Vector2(0, 1);
            sep.GetComponent<Image>().color = new Color(0.78f, 0.66f, 0.19f, 0.5f);

            // Buttons
            float buttonY = -45;
            float buttonHeight = 35;

            Button inspectBtn = CreateMenuButton("InspectBtn", menuPanelObj.transform, buttonY, "Inspect");
            TextMeshProUGUI inspectText = inspectBtn.GetComponentInChildren<TextMeshProUGUI>();
            buttonY -= buttonHeight;

            Button takeBtn = CreateMenuButton("TakeBtn", menuPanelObj.transform, buttonY, "Take");
            TextMeshProUGUI takeText = takeBtn.GetComponentInChildren<TextMeshProUGUI>();
            buttonY -= buttonHeight;

            Button useBtn = CreateMenuButton("UseBtn", menuPanelObj.transform, buttonY, "Use");
            TextMeshProUGUI useText = useBtn.GetComponentInChildren<TextMeshProUGUI>();
            buttonY -= buttonHeight;

            Button combineBtn = CreateMenuButton("CombineBtn", menuPanelObj.transform, buttonY, "Combine");
            TextMeshProUGUI combineText = combineBtn.GetComponentInChildren<TextMeshProUGUI>();

            ContextMenuUI contextUI = menuPanelObj.AddComponent<ContextMenuUI>();
            SetPrivateField(contextUI, "menuPanel", menuPanelObj);
            SetPrivateField(contextUI, "menuRect", menuRect);
            SetPrivateField(contextUI, "inspectButton", inspectBtn);
            SetPrivateField(contextUI, "takeButton", takeBtn);
            SetPrivateField(contextUI, "useButton", useBtn);
            SetPrivateField(contextUI, "combineButton", combineBtn);
            SetPrivateField(contextUI, "itemNameText", itemName);
            SetPrivateField(contextUI, "inspectText", inspectText);
            SetPrivateField(contextUI, "takeText", takeText);
            SetPrivateField(contextUI, "useText", useText);
            SetPrivateField(contextUI, "combineText", combineText);

            return contextUI;
        }

        private TextMeshProUGUI CreateMenuText(string name, Transform parent, float yPos, float fontSize, FontStyles style)
        {
            GameObject obj = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
            obj.transform.SetParent(parent, false);
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(0.5f, 1);
            rt.anchoredPosition = new Vector2(0, yPos);
            rt.sizeDelta = new Vector2(-20, 25);

            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            tmp.fontSize = fontSize;
            tmp.fontStyle = style;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;

            return tmp;
        }

        private Button CreateMenuButton(string name, Transform parent, float yPos, string label)
        {
            GameObject btnObj = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
            btnObj.transform.SetParent(parent, false);
            RectTransform rt = btnObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.05f, 1);
            rt.anchorMax = new Vector2(0.95f, 1);
            rt.pivot = new Vector2(0.5f, 1);
            rt.anchoredPosition = new Vector2(0, yPos);
            rt.sizeDelta = new Vector2(0, 30);

            Image img = btnObj.GetComponent<Image>();
            img.color = new Color(0.15f, 0.15f, 0.25f, 1f);

            Button btn = btnObj.GetComponent<Button>();
            var colors = btn.colors;
            colors.normalColor = new Color(0.15f, 0.15f, 0.25f);
            colors.highlightedColor = new Color(0.25f, 0.25f, 0.4f);
            colors.pressedColor = new Color(0.1f, 0.1f, 0.15f);
            colors.disabledColor = new Color(0.1f, 0.1f, 0.12f);
            btn.colors = colors;

            // Button label
            GameObject labelObj = new GameObject("Label", typeof(RectTransform), typeof(TextMeshProUGUI));
            labelObj.transform.SetParent(btnObj.transform, false);
            RectTransform labelRect = labelObj.GetComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            TextMeshProUGUI tmp = labelObj.GetComponent<TextMeshProUGUI>();
            tmp.text = label;
            tmp.fontSize = 16;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;

            return btn;
        }

        private InspectWindowUI CreateInspectWindow(Transform parent)
        {
            // Background blocker
            GameObject blockerObj = new GameObject("InspectBlocker", typeof(RectTransform), typeof(Image), typeof(Button));
            blockerObj.transform.SetParent(parent, false);
            RectTransform blockerRect = blockerObj.GetComponent<RectTransform>();
            blockerRect.anchorMin = Vector2.zero;
            blockerRect.anchorMax = Vector2.one;
            blockerRect.offsetMin = Vector2.zero;
            blockerRect.offsetMax = Vector2.zero;
            Image blockerImg = blockerObj.GetComponent<Image>();
            blockerImg.color = new Color(0, 0, 0, 0.5f);
            Button blockerBtn = blockerObj.GetComponent<Button>();

            // Window panel
            GameObject windowObj = new GameObject("InspectPanel", typeof(RectTransform), typeof(Image));
            windowObj.transform.SetParent(blockerObj.transform, false);
            RectTransform windowRect = windowObj.GetComponent<RectTransform>();
            windowRect.anchorMin = new Vector2(0.25f, 0.25f);
            windowRect.anchorMax = new Vector2(0.75f, 0.75f);
            windowRect.offsetMin = Vector2.zero;
            windowRect.offsetMax = Vector2.zero;
            Image windowBg = windowObj.GetComponent<Image>();
            windowBg.color = new Color(0.12f, 0.12f, 0.2f, 0.98f);

            Outline windowOutline = windowObj.AddComponent<Outline>();
            windowOutline.effectColor = new Color(0.78f, 0.66f, 0.19f);
            windowOutline.effectDistance = new Vector2(2, 2);

            // Title
            TextMeshProUGUI titleText = CreateMenuText("InspectTitle", windowObj.transform, -15, 28, FontStyles.Bold);

            // Separator
            GameObject sep = new GameObject("Sep", typeof(RectTransform), typeof(Image));
            sep.transform.SetParent(windowObj.transform, false);
            RectTransform sepRect = sep.GetComponent<RectTransform>();
            sepRect.anchorMin = new Vector2(0.05f, 1);
            sepRect.anchorMax = new Vector2(0.95f, 1);
            sepRect.pivot = new Vector2(0.5f, 1);
            sepRect.anchoredPosition = new Vector2(0, -50);
            sepRect.sizeDelta = new Vector2(0, 2);
            sep.GetComponent<Image>().color = new Color(0.78f, 0.66f, 0.19f, 0.5f);

            // Description
            GameObject descObj = new GameObject("Description", typeof(RectTransform), typeof(TextMeshProUGUI));
            descObj.transform.SetParent(windowObj.transform, false);
            RectTransform descRect = descObj.GetComponent<RectTransform>();
            descRect.anchorMin = new Vector2(0.05f, 0.1f);
            descRect.anchorMax = new Vector2(0.95f, 1);
            descRect.pivot = new Vector2(0.5f, 1);
            descRect.anchoredPosition = new Vector2(0, -60);
            descRect.sizeDelta = Vector2.zero;
            TextMeshProUGUI descText = descObj.GetComponent<TextMeshProUGUI>();
            descText.fontSize = 18;
            descText.color = new Color(0.85f, 0.85f, 0.85f);
            descText.alignment = TextAlignmentOptions.TopLeft;
            descText.enableWordWrapping = true;

            // Close button
            GameObject closeBtnObj = new GameObject("CloseBtn", typeof(RectTransform), typeof(Image), typeof(Button));
            closeBtnObj.transform.SetParent(windowObj.transform, false);
            RectTransform closeRect = closeBtnObj.GetComponent<RectTransform>();
            closeRect.anchorMin = new Vector2(1, 1);
            closeRect.anchorMax = new Vector2(1, 1);
            closeRect.pivot = new Vector2(1, 1);
            closeRect.anchoredPosition = new Vector2(-5, -5);
            closeRect.sizeDelta = new Vector2(30, 30);
            Image closeBg = closeBtnObj.GetComponent<Image>();
            closeBg.color = new Color(0.6f, 0.15f, 0.15f);
            Button closeBtn = closeBtnObj.GetComponent<Button>();

            GameObject closeLabel = new GameObject("X", typeof(RectTransform), typeof(TextMeshProUGUI));
            closeLabel.transform.SetParent(closeBtnObj.transform, false);
            RectTransform clRect = closeLabel.GetComponent<RectTransform>();
            clRect.anchorMin = Vector2.zero;
            clRect.anchorMax = Vector2.one;
            clRect.offsetMin = Vector2.zero;
            clRect.offsetMax = Vector2.zero;
            TextMeshProUGUI clText = closeLabel.GetComponent<TextMeshProUGUI>();
            clText.text = "X";
            clText.fontSize = 18;
            clText.color = Color.white;
            clText.alignment = TextAlignmentOptions.Center;

            InspectWindowUI inspectUI = blockerObj.AddComponent<InspectWindowUI>();
            SetPrivateField(inspectUI, "windowPanel", blockerObj);
            SetPrivateField(inspectUI, "titleText", titleText);
            SetPrivateField(inspectUI, "descriptionText", descText);
            SetPrivateField(inspectUI, "closeButton", closeBtn);
            SetPrivateField(inspectUI, "backgroundBlocker", blockerBtn);

            return inspectUI;
        }

        private void LoadScene(string sceneName)
        {
            if (GameManager.Instance == null)
            {
                // Create a temporary GameManager for testing
                GameObject gmObj = new GameObject("GameManager");
                gmObj.AddComponent<GameManager>();
                GameManager.Instance.StartGame();
            }

            GameManager.Instance.LoadSceneConfig(sceneName);
            var config = GameManager.Instance.CurrentSceneConfig;

            if (config == null)
            {
                Debug.LogError("[GameSceneSetup] Failed to load scene config!");
                return;
            }

            // Initialize grid
            _gridSystem.Initialize(config.gridWidth, config.gridHeight);

            // Initialize player
            _playerController.Initialize(_gridSystem, config.playerStart.x, config.playerStart.y);
            SetPrivateField(_playerController, "gridSystem", _gridSystem);

            // Spawn items
            _itemManager.SpawnItems(config.items);

            // Initialize character panel
            _characterPanel.Initialize(GameManager.Instance.ActiveCharacter);

            Debug.Log($"[GameSceneSetup] Scene loaded: {config.sceneName} - {config.description}");
        }

        private GameObject CreatePanel(string name, Transform parent)
        {
            GameObject obj = new GameObject(name, typeof(RectTransform), typeof(Image));
            obj.transform.SetParent(parent, false);
            return obj;
        }

        private void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(target, value);
            }
            else
            {
                Debug.LogWarning($"[GameSceneSetup] Could not find field '{fieldName}' on {target.GetType().Name}");
            }
        }
    }
}

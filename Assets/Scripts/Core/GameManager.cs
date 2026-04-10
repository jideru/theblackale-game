using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BlackAle.Character;
using BlackAle.Config;

namespace BlackAle.Core
{
    public enum GameState
    {
        Title,
        Playing,
        Inspect,
        Paused,
        ContextMenu
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("State")]
        [SerializeField] private GameState currentState = GameState.Title;

        public GameState CurrentState
        {
            get => currentState;
            set
            {
                var previous = currentState;
                currentState = value;
                OnGameStateChanged?.Invoke(previous, value);
            }
        }

        public SceneConfig CurrentSceneConfig { get; private set; }
        public CharacterData ActiveCharacter { get; private set; }
        public List<CharacterData> Party { get; private set; }

        public event Action<GameState, GameState> OnGameStateChanged;
        public event Action<SceneConfig> OnSceneLoaded;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeParty();
        }

        private void InitializeParty()
        {
            Party = new List<CharacterData>
            {
                CharacterData.CreateThief(),
                CharacterData.CreateWarrior(),
                CharacterData.CreateDruid(),
                CharacterData.CreatePriest()
            };

            // Alpha: Start with the Thief
            ActiveCharacter = Party[0];
        }

        public void StartGame()
        {
            CurrentState = GameState.Playing;
            SceneManager.LoadScene("GameScene");
        }

        public void LoadSceneConfig(string sceneName)
        {
            CurrentSceneConfig = SceneLoader.LoadScene(sceneName);
            if (CurrentSceneConfig != null)
            {
                OnSceneLoaded?.Invoke(CurrentSceneConfig);
                Debug.Log($"[GameManager] Loaded scene: {CurrentSceneConfig.sceneName}");
            }
        }

        public void SetActiveCharacter(int partyIndex)
        {
            if (partyIndex >= 0 && partyIndex < Party.Count)
            {
                ActiveCharacter = Party[partyIndex];
            }
        }

        public void ReturnToTitle()
        {
            CurrentState = GameState.Title;
            SceneManager.LoadScene("TitleScene");
        }
    }
}

using UnityEngine;
using BlackAle.Config;

namespace BlackAle.Core
{
    public class SceneLoader
    {
        public static SceneConfig LoadScene(string sceneName)
        {
            string path = $"Config/Scenes/{sceneName}";
            TextAsset jsonFile = Resources.Load<TextAsset>(path);

            if (jsonFile == null)
            {
                // Try loading from StreamingAssets or direct path
                string fullPath = System.IO.Path.Combine(Application.dataPath, "Config", "Scenes", sceneName + ".json");
                if (System.IO.File.Exists(fullPath))
                {
                    string json = System.IO.File.ReadAllText(fullPath);
                    return ParseAndValidate(json, sceneName);
                }

                Debug.LogError($"[SceneLoader] Could not find scene config: {sceneName}");
                return null;
            }

            return ParseAndValidate(jsonFile.text, sceneName);
        }

        private static SceneConfig ParseAndValidate(string json, string sceneName)
        {
            SceneConfig config = JsonUtility.FromJson<SceneConfig>(json);

            if (config == null)
            {
                Debug.LogError($"[SceneLoader] Failed to parse scene config: {sceneName}");
                return null;
            }

            ValidateConfig(config);
            return config;
        }

        private static void ValidateConfig(SceneConfig config)
        {
            if (string.IsNullOrEmpty(config.sceneId))
                Debug.LogWarning("[SceneLoader] Scene has no sceneId");

            if (config.gridWidth <= 0 || config.gridHeight <= 0)
                Debug.LogWarning($"[SceneLoader] Invalid grid size: {config.gridWidth}x{config.gridHeight}");

            if (config.playerStart == null)
                Debug.LogWarning("[SceneLoader] No player start position defined");

            if (config.items != null)
            {
                foreach (var item in config.items)
                {
                    if (item.position.x < 0 || item.position.x >= config.gridWidth ||
                        item.position.y < 0 || item.position.y >= config.gridHeight)
                    {
                        Debug.LogWarning($"[SceneLoader] Item '{item.itemId}' position {item.position} is outside grid bounds");
                    }

                    if (item.color != null)
                    {
                        Color c = item.color.ToColor();
                        if ((Mathf.Approximately(c.r, 1f) && Mathf.Approximately(c.g, 0f) && Mathf.Approximately(c.b, 1f)) ||
                            (Mathf.Approximately(c.r, 0f) && Mathf.Approximately(c.g, 1f) && Mathf.Approximately(c.b, 1f)))
                        {
                            Debug.LogWarning($"[SceneLoader] Item '{item.itemId}' uses a reserved color (cyan/magenta)");
                        }
                    }
                }
            }
        }
    }
}

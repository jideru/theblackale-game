#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlackAle.Editor
{
    /// <summary>
    /// Editor utility to set up The Black Ale Adventure scenes properly.
    /// Run from menu: Black Ale > Setup Scenes
    /// </summary>
    public static class ProjectSetup
    {
        [MenuItem("Black Ale/Setup Scenes")]
        public static void SetupScenes()
        {
            SetupTitleScene();
            SetupGameScene();
            SetupBuildSettings();
            Debug.Log("[ProjectSetup] All scenes set up successfully! Open TitleScene and press Play.");
        }

        [MenuItem("Black Ale/Setup Title Scene")]
        public static void SetupTitleScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Camera
            GameObject camObj = new GameObject("Main Camera");
            Camera cam = camObj.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5;
            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.transform.position = new Vector3(0, 0, -10);
            camObj.tag = "MainCamera";
            camObj.AddComponent<AudioListener>();

            // TitleSceneSetup
            GameObject setupObj = new GameObject("TitleSceneSetup");
            setupObj.AddComponent<UI.TitleSceneSetup>();

            string path = "Assets/Scenes/TitleScene.unity";
            EnsureDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, path);
            Debug.Log($"[ProjectSetup] Title scene created at {path}");
        }

        [MenuItem("Black Ale/Setup Game Scene")]
        public static void SetupGameScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Camera
            GameObject camObj = new GameObject("Main Camera");
            Camera cam = camObj.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5;
            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.transform.position = new Vector3(0, 0, -10);
            camObj.tag = "MainCamera";
            camObj.AddComponent<AudioListener>();

            // GameSceneSetup
            GameObject setupObj = new GameObject("GameSceneSetup");
            setupObj.AddComponent<Core.GameSceneSetup>();

            string path = "Assets/Scenes/GameScene.unity";
            EnsureDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, path);
            Debug.Log($"[ProjectSetup] Game scene created at {path}");
        }

        [MenuItem("Black Ale/Setup Build Settings")]
        public static void SetupBuildSettings()
        {
            EditorBuildSettings.scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/TitleScene.unity", true),
                new EditorBuildSettingsScene("Assets/Scenes/GameScene.unity", true),
            };
            Debug.Log("[ProjectSetup] Build settings updated with TitleScene and GameScene.");
        }

        private static void EnsureDirectory(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string parent = System.IO.Path.GetDirectoryName(path);
                string folder = System.IO.Path.GetFileName(path);
                AssetDatabase.CreateFolder(parent, folder);
            }
        }
    }
}
#endif

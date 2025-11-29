using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Reflection;

/// <summary>
/// Editor script to automatically add SoundManager to all scenes
/// </summary>
public class SoundManagerSetup : EditorWindow
{
    [MenuItem("Tools/Setup SoundManager with Audio Files")]
    public static void SetupSoundManagerWithAudioFiles()
    {
        // Load audio files
        AudioClip bgmClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/bgm.mp3");
        AudioClip goalClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/goal.mp3");
        AudioClip kickClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/kick.mp3");

        if (bgmClip == null || goalClip == null || kickClip == null)
        {
            Debug.LogError("[SoundManagerSetup] Could not find audio files in Assets/Audio/");
            EditorUtility.DisplayDialog("Error",
                "Could not find audio files!\n\nMake sure these files exist:\n" +
                "- Assets/Audio/bgm.mp3\n" +
                "- Assets/Audio/goal.mp3\n" +
                "- Assets/Audio/kick.mp3",
                "OK");
            return;
        }

        // Get all scene paths
        string[] scenePaths = new string[]
        {
            "Assets/Scenes/TitleScene.unity",
            "Assets/Scenes/MainScene.unity",
            "Assets/Scenes/ManualScene.unity",
            "Assets/Scenes/TeamScene.unity",
            "Assets/Scenes/LevelScene.unity",
            "Assets/Scenes/CoinScene.unity",
            "Assets/Scenes/ResultScene.unity"
        };

        foreach (string scenePath in scenePaths)
        {
            // Open scene
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            // Check if SoundManager already exists
            SoundManager existingManager = Object.FindObjectOfType<SoundManager>();

            SoundManager manager;
            if (existingManager == null)
            {
                // Create SoundManager GameObject
                GameObject soundManagerObj = new GameObject("SoundManager");
                manager = soundManagerObj.AddComponent<SoundManager>();

                Debug.Log($"[SoundManagerSetup] Added SoundManager to {scene.name}");
            }
            else
            {
                manager = existingManager;
                Debug.Log($"[SoundManagerSetup] SoundManager already exists in {scene.name}");
            }

            // Assign audio clips using reflection to access private fields
            AssignAudioClip(manager, "gameBGM", bgmClip);
            AssignAudioClip(manager, "goalSound", goalClip);
            AssignAudioClip(manager, "kickSound", kickClip);

            // Mark the manager as dirty so Unity saves the changes
            EditorUtility.SetDirty(manager);

            Debug.Log($"[SoundManagerSetup] Assigned audio files to {scene.name}");

            // Save scene
            EditorSceneManager.SaveScene(scene);
        }

        Debug.Log("[SoundManagerSetup] Setup complete for all scenes!");
        EditorUtility.DisplayDialog("Setup Complete",
            "SoundManager has been added to all scenes with audio files connected!\n\n" +
            "BGM: bgm.mp3\n" +
            "Goal: goal.mp3\n" +
            "Kick: kick.mp3",
            "OK");
    }

    private static void AssignAudioClip(SoundManager manager, string fieldName, AudioClip clip)
    {
        var field = typeof(SoundManager).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(manager, clip);
        }
    }

    [MenuItem("Tools/Create SoundManager Prefab")]
    public static void CreateSoundManagerPrefab()
    {
        // Create SoundManager GameObject
        GameObject soundManagerObj = new GameObject("SoundManager");
        SoundManager manager = soundManagerObj.AddComponent<SoundManager>();
        
        // Create prefab
        string prefabPath = "Assets/Prefabs/SoundManager.prefab";
        
        // Ensure Prefabs directory exists
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }
        
        // Save as prefab
        PrefabUtility.SaveAsPrefabAsset(soundManagerObj, prefabPath);
        
        // Destroy temporary GameObject
        Object.DestroyImmediate(soundManagerObj);
        
        Debug.Log($"[SoundManagerSetup] SoundManager prefab created at {prefabPath}");
        EditorUtility.DisplayDialog("Prefab Created", $"SoundManager prefab created at {prefabPath}", "OK");
    }
}


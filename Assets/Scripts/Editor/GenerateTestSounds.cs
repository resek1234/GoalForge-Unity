using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// Editor script to generate simple test sounds programmatically
/// This allows testing the sound system without downloading actual sound files
/// </summary>
public class GenerateTestSounds : EditorWindow
{
    [MenuItem("Tools/Generate Test Sounds")]
    public static void GenerateAllTestSounds()
    {
        // Ensure Audio directory exists
        string audioPath = "Assets/Audio";
        if (!AssetDatabase.IsValidFolder(audioPath))
        {
            AssetDatabase.CreateFolder("Assets", "Audio");
        }

        // Generate different test sounds
        GenerateBeepSound("kick", 440f, 0.1f);           // A4 note, short beep
        GenerateBeepSound("goal", 880f, 0.5f);           // A5 note, longer beep
        GenerateBeepSound("button_click", 1000f, 0.05f); // High pitch, very short
        GenerateBeepSound("item_pickup", 660f, 0.2f);    // E5 note, medium length
        GenerateBeepSound("dash", 220f, 0.15f);          // A3 note, short
        GenerateBeepSound("whistle", 2000f, 0.3f);       // Very high pitch
        
        AssetDatabase.Refresh();
        
        Debug.Log("[GenerateTestSounds] All test sounds generated in Assets/Audio/");
        EditorUtility.DisplayDialog("Test Sounds Generated", 
            "Test sounds have been generated in Assets/Audio/\n\n" +
            "These are simple beep sounds for testing purposes.\n" +
            "Replace them with real sound files for production.", 
            "OK");
    }

    private static void GenerateBeepSound(string name, float frequency, float duration)
    {
        int sampleRate = 44100;
        int sampleCount = Mathf.FloorToInt(sampleRate * duration);
        
        float[] samples = new float[sampleCount];
        
        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleRate;
            
            // Generate sine wave
            float sample = Mathf.Sin(2f * Mathf.PI * frequency * t);
            
            // Apply envelope (fade in/out) to avoid clicks
            float envelope = 1f;
            float fadeTime = 0.01f; // 10ms fade
            int fadeSamples = Mathf.FloorToInt(sampleRate * fadeTime);
            
            if (i < fadeSamples)
            {
                envelope = (float)i / fadeSamples;
            }
            else if (i > sampleCount - fadeSamples)
            {
                envelope = (float)(sampleCount - i) / fadeSamples;
            }
            
            samples[i] = sample * envelope * 0.5f; // 0.5f to reduce volume
        }
        
        // Create AudioClip
        AudioClip clip = AudioClip.Create(name, sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        
        // Save as asset
        string path = $"Assets/Audio/{name}.asset";
        AssetDatabase.CreateAsset(clip, path);
        
        Debug.Log($"[GenerateTestSounds] Generated {name}.asset ({frequency}Hz, {duration}s)");
    }

    [MenuItem("Tools/Clear Test Sounds")]
    public static void ClearTestSounds()
    {
        string[] soundNames = new string[] 
        { 
            "kick", "goal", "button_click", "item_pickup", "dash", "whistle" 
        };
        
        int deletedCount = 0;
        foreach (string name in soundNames)
        {
            string path = $"Assets/Audio/{name}.asset";
            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
                deletedCount++;
            }
        }
        
        AssetDatabase.Refresh();
        
        Debug.Log($"[GenerateTestSounds] Cleared {deletedCount} test sound(s)");
        EditorUtility.DisplayDialog("Test Sounds Cleared", 
            $"Cleared {deletedCount} test sound file(s)", 
            "OK");
    }
}


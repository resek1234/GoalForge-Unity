using UnityEngine;
using UnityEditor;

public class ApplyCustomSprites : MonoBehaviour
{
    [MenuItem("GoalForge/Apply Custom Sprites to Prefabs")]
    public static void ApplySpritesToPrefabs()
    {
        ConvertTexturesToSprites();

        ApplySpriteToItem("Speedboost", "Assets/Prefabs/Items/SpeedBoostItem.prefab");
        ApplySpriteToItem("Shield", "Assets/Prefabs/Items/ShieldItem.prefab");
        ApplySpriteToItem("Megaball", "Assets/Prefabs/Items/MegaBallItem.prefab");
        ApplySpriteToItem("Trap", "Assets/Prefabs/Items/TrapBombItem.prefab");
        ApplySpriteToItem("Wind", "Assets/Prefabs/Items/WindCurseItem.prefab");

        ConnectPrefabsToItemSpawner();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("✅ Custom sprites applied to all prefabs and connected to ItemSpawner!");
    }

    static void ConvertTexturesToSprites()
    {
        string[] textureNames = { "Speedboost", "Shield", "Megaball", "Trap", "Wind" };

        foreach (string textureName in textureNames)
        {
            string[] guids = AssetDatabase.FindAssets($"{textureName} t:Texture2D", new[] { "Assets/Sprites" });

            if (guids.Length == 0)
            {
                Debug.LogWarning($"⚠️ Texture not found: {textureName}");
                continue;
            }

            string texturePath = AssetDatabase.GUIDToAssetPath(guids[0]);
            TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;

            if (importer == null)
            {
                Debug.LogWarning($"⚠️ Failed to get importer for: {texturePath}");
                continue;
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 1024f;
            importer.filterMode = FilterMode.Bilinear;
            importer.SaveAndReimport();

            Debug.Log($"✅ Converted to sprite: {texturePath}");
        }

        AssetDatabase.Refresh();
    }
    
    static void ApplySpriteToItem(string spriteName, string prefabPath)
    {
        string[] guids = AssetDatabase.FindAssets($"{spriteName} t:Texture2D", new[] { "Assets/Sprites" });

        if (guids.Length == 0)
        {
            Debug.LogWarning($"⚠️ Sprite not found: {spriteName}");
            return;
        }

        string spritePath = AssetDatabase.GUIDToAssetPath(guids[0]);
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        if (sprite == null)
        {
            Debug.LogWarning($"⚠️ Failed to load sprite: {spritePath}");
            return;
        }
        
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogWarning($"⚠️ Prefab not found: {prefabPath}");
            return;
        }
        
        GameObject instance = PrefabUtility.LoadPrefabContents(prefabPath);
        SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
        
        if (sr == null)
        {
            Debug.LogWarning($"⚠️ SpriteRenderer not found in {prefabPath}");
            PrefabUtility.UnloadPrefabContents(instance);
            return;
        }
        
        sr.sprite = sprite;
        instance.transform.localScale = new Vector3(0.3f, 0.3f, 1f);

        PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
        PrefabUtility.UnloadPrefabContents(instance);

        Debug.Log($"✅ Applied {spritePath} to {prefabPath} with scale 0.3");
    }

    static void ConnectPrefabsToItemSpawner()
    {
        ItemSpawner spawner = Object.FindFirstObjectByType<ItemSpawner>();

        if (spawner == null)
        {
            Debug.LogWarning("⚠️ ItemSpawner not found in scene! Please add ItemSpawner to the scene first.");
            return;
        }

        GameObject speedBoostPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/SpeedBoostItem.prefab");
        GameObject shieldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/ShieldItem.prefab");
        GameObject megaBallPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/MegaBallItem.prefab");
        GameObject trapBombPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/TrapBombItem.prefab");
        GameObject windCursePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/WindCurseItem.prefab");

        SerializedObject serializedSpawner = new SerializedObject(spawner);

        serializedSpawner.FindProperty("speedBoostPrefab").objectReferenceValue = speedBoostPrefab;
        serializedSpawner.FindProperty("shieldPrefab").objectReferenceValue = shieldPrefab;
        serializedSpawner.FindProperty("megaBallPrefab").objectReferenceValue = megaBallPrefab;
        serializedSpawner.FindProperty("trapBombPrefab").objectReferenceValue = trapBombPrefab;
        serializedSpawner.FindProperty("windCursePrefab").objectReferenceValue = windCursePrefab;

        serializedSpawner.ApplyModifiedProperties();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);

        Debug.Log("✅ All item prefabs connected to ItemSpawner!");
    }

}


using UnityEngine;
using UnityEditor;
using System.IO;

public class GameSetupHelper : MonoBehaviour
{
    [MenuItem("GoalForge/Setup Items and Obstacles")]
    public static void SetupItemsAndObstacles()
    {
        CreateFolders();
        CreateItemPrefabs();
        CreateObstaclePrefabs();
        SetupSpawners();
        AddPlayerVisualEffects();

        Debug.Log("✅ Items and Obstacles setup complete!");
        AssetDatabase.Refresh();
    }

    static void AddPlayerVisualEffects()
    {
        PlayerController[] players = Object.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController player in players)
        {
            if (player.GetComponent<PlayerVisualEffects>() == null)
            {
                player.gameObject.AddComponent<PlayerVisualEffects>();
                Debug.Log($"✅ Added PlayerVisualEffects to {player.name}");
            }
        }
    }

    static void CreateFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Items"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Items");
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs/Obstacles"))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Obstacles");
        if (!AssetDatabase.IsValidFolder("Assets/Sprites"))
            AssetDatabase.CreateFolder("Assets", "Sprites");
    }

    static Sprite CreateAndSaveCircleSprite(string name, Color color, int size = 128)
    {
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 2;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    texture.SetPixel(x, y, color);
                }
                else
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }
        }

        texture.Apply();

        // Save texture as PNG
        byte[] pngData = texture.EncodeToPNG();
        string texturePath = $"Assets/Sprites/{name}.png";
        File.WriteAllBytes(texturePath, pngData);
        AssetDatabase.ImportAsset(texturePath);

        // Set texture import settings
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = 100f;
        importer.filterMode = FilterMode.Bilinear;
        importer.SaveAndReimport();

        // Load and return sprite
        return AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
    }

    static Sprite CreateAndSaveSquareSprite(string name, Color color, int size = 128)
    {
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();

        // Save texture as PNG
        byte[] pngData = texture.EncodeToPNG();
        string texturePath = $"Assets/Sprites/{name}.png";
        File.WriteAllBytes(texturePath, pngData);
        AssetDatabase.ImportAsset(texturePath);

        // Set texture import settings
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = 100f;
        importer.filterMode = FilterMode.Bilinear;
        importer.SaveAndReimport();

        // Load and return sprite
        return AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
    }

    static void CreateItemPrefabs()
    {
        CreateSpeedBoostPrefab();
        CreateShieldPrefab();
        CreateMegaBallPrefab();
    }

    static void CreateSpeedBoostPrefab()
    {
        GameObject item = new GameObject("SpeedBoostItem");

        Sprite sprite = CreateAndSaveCircleSprite("SpeedBoost", Color.yellow);
        SpriteRenderer sr = item.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 5;

        CircleCollider2D col = item.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.4f;

        SpeedBoostItem script = item.AddComponent<SpeedBoostItem>();

        string path = "Assets/Prefabs/Items/SpeedBoostItem.prefab";
        PrefabUtility.SaveAsPrefabAsset(item, path);
        DestroyImmediate(item);

        Debug.Log("Created: " + path);
    }

    static void CreateShieldPrefab()
    {
        GameObject item = new GameObject("ShieldItem");

        Sprite sprite = CreateAndSaveCircleSprite("Shield", new Color(0.3f, 0.6f, 1f));
        SpriteRenderer sr = item.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 5;

        CircleCollider2D col = item.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.4f;

        ShieldItem script = item.AddComponent<ShieldItem>();

        string path = "Assets/Prefabs/Items/ShieldItem.prefab";
        PrefabUtility.SaveAsPrefabAsset(item, path);
        DestroyImmediate(item);

        Debug.Log("Created: " + path);
    }

    static void CreateMegaBallPrefab()
    {
        GameObject item = new GameObject("MegaBallItem");

        Sprite sprite = CreateAndSaveCircleSprite("MegaBall", new Color(1f, 0.3f, 0.3f));
        SpriteRenderer sr = item.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 5;

        CircleCollider2D col = item.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.4f;

        MegaBallItem script = item.AddComponent<MegaBallItem>();

        string path = "Assets/Prefabs/Items/MegaBallItem.prefab";
        PrefabUtility.SaveAsPrefabAsset(item, path);
        DestroyImmediate(item);

        Debug.Log("Created: " + path);
    }

    static void CreateObstaclePrefabs()
    {
        CreateWallPrefab();
        CreateWindZonePrefab();
        CreateTrapPrefab();
    }

    static void CreateWallPrefab()
    {
        GameObject obstacle = new GameObject("WallObstacle");

        Sprite sprite = CreateAndSaveSquareSprite("Wall", new Color(0.3f, 0.3f, 0.3f));
        SpriteRenderer sr = obstacle.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 3;

        BoxCollider2D col = obstacle.AddComponent<BoxCollider2D>();
        col.isTrigger = false;
        col.size = new Vector2(1f, 1f);

        WallObstacle script = obstacle.AddComponent<WallObstacle>();

        string path = "Assets/Prefabs/Obstacles/WallObstacle.prefab";
        PrefabUtility.SaveAsPrefabAsset(obstacle, path);
        DestroyImmediate(obstacle);

        Debug.Log("Created: " + path);
    }

    static void CreateWindZonePrefab()
    {
        GameObject obstacle = new GameObject("WindZone");

        Sprite sprite = CreateAndSaveSquareSprite("Wind", new Color(0.4f, 0.7f, 1f, 0.6f));
        SpriteRenderer sr = obstacle.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 2;
        obstacle.transform.localScale = new Vector3(2.5f, 2.5f, 1f);

        BoxCollider2D col = obstacle.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = new Vector2(1f, 1f);

        WindZone script = obstacle.AddComponent<WindZone>();

        string path = "Assets/Prefabs/Obstacles/WindZone.prefab";
        PrefabUtility.SaveAsPrefabAsset(obstacle, path);
        DestroyImmediate(obstacle);

        Debug.Log("Created: " + path);
    }

    static void CreateTrapPrefab()
    {
        GameObject obstacle = new GameObject("TrapObstacle");

        Sprite sprite = CreateAndSaveCircleSprite("Trap", new Color(1f, 0.4f, 0f));
        SpriteRenderer sr = obstacle.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = 2;
        obstacle.transform.localScale = new Vector3(1.2f, 1.2f, 1f);

        CircleCollider2D col = obstacle.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.5f;

        TrapObstacle script = obstacle.AddComponent<TrapObstacle>();

        string path = "Assets/Prefabs/Obstacles/TrapObstacle.prefab";
        PrefabUtility.SaveAsPrefabAsset(obstacle, path);
        DestroyImmediate(obstacle);

        Debug.Log("Created: " + path);
    }

    static void SetupSpawners()
    {
        GameObject itemSpawner = GameObject.Find("ItemSpawner");
        if (itemSpawner == null)
        {
            itemSpawner = new GameObject("ItemSpawner");
            itemSpawner.AddComponent<ItemSpawner>();
        }

        ItemSpawner itemScript = itemSpawner.GetComponent<ItemSpawner>();
        if (itemScript != null)
        {
            SerializedObject so = new SerializedObject(itemScript);

            so.FindProperty("speedBoostPrefab").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/SpeedBoostItem.prefab");
            so.FindProperty("shieldPrefab").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/ShieldItem.prefab");
            so.FindProperty("megaBallPrefab").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Items/MegaBallItem.prefab");

            so.FindProperty("spawnInterval").floatValue = 10f;
            so.FindProperty("maxItemsOnField").intValue = 2;
            so.FindProperty("spawnAreaMin").vector2Value = new Vector2(-8f, -4f);
            so.FindProperty("spawnAreaMax").vector2Value = new Vector2(8f, 4f);

            so.ApplyModifiedProperties();
            Debug.Log("✅ ItemSpawner configured");
        }

        GameObject obstacleSpawner = GameObject.Find("ObstacleSpawner");
        if (obstacleSpawner == null)
        {
            obstacleSpawner = new GameObject("ObstacleSpawner");
            obstacleSpawner.AddComponent<ObstacleSpawner>();
        }

        ObstacleSpawner obstacleScript = obstacleSpawner.GetComponent<ObstacleSpawner>();
        if (obstacleScript != null)
        {
            SerializedObject so = new SerializedObject(obstacleScript);

            so.FindProperty("wallPrefab").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Obstacles/WallObstacle.prefab");
            so.FindProperty("windZonePrefab").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Obstacles/WindZone.prefab");
            so.FindProperty("trapPrefab").objectReferenceValue =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Obstacles/TrapObstacle.prefab");

            so.FindProperty("minObstacles").intValue = 2;
            so.FindProperty("maxObstacles").intValue = 5;
            so.FindProperty("spawnAreaMin").vector2Value = new Vector2(-7f, -3f);
            so.FindProperty("spawnAreaMax").vector2Value = new Vector2(7f, 3f);
            so.FindProperty("minDistanceBetweenObstacles").floatValue = 2f;

            so.ApplyModifiedProperties();
            Debug.Log("✅ ObstacleSpawner configured");
        }
    }

    static Sprite CreateCircleSprite(Color color)
    {
        int size = 64;
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f - 2;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance <= radius)
                {
                    pixels[y * size + x] = color;
                }
                else
                {
                    pixels[y * size + x] = Color.clear;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
    }

    static Sprite CreateSquareSprite(Color color)
    {
        int size = 64;
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
    }
}


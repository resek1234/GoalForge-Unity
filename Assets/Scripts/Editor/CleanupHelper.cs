using UnityEngine;
using UnityEditor;

public class CleanupHelper : MonoBehaviour
{
    [MenuItem("GoalForge/Clear All Items and Obstacles")]
    public static void ClearAllItemsAndObstacles()
    {
        ItemSpawner itemSpawner = Object.FindAnyObjectByType<ItemSpawner>();
        if (itemSpawner != null)
        {
            itemSpawner.ClearAllItems();
            Debug.Log("✅ Cleared all items");
        }

        ObstacleSpawner obstacleSpawner = Object.FindAnyObjectByType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.ClearObstacles();
            Debug.Log("✅ Cleared all obstacles");
        }

        PowerUpItem[] items = Object.FindObjectsByType<PowerUpItem>(FindObjectsSortMode.None);
        foreach (PowerUpItem item in items)
        {
            DestroyImmediate(item.gameObject);
        }

        Obstacle[] obstacles = Object.FindObjectsByType<Obstacle>(FindObjectsSortMode.None);
        foreach (Obstacle obstacle in obstacles)
        {
            DestroyImmediate(obstacle.gameObject);
        }

        Debug.Log("✅ All items and obstacles cleared!");
    }
}


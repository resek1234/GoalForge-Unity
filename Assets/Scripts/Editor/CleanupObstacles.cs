using UnityEngine;
using UnityEditor;

public class CleanupObstacles : MonoBehaviour
{
    [MenuItem("GoalForge/Clear All Obstacles from Scene")]
    public static void ClearAllObstacles()
    {
        int deletedCount = 0;

        WallObstacle[] walls = Object.FindObjectsByType<WallObstacle>(FindObjectsSortMode.None);
        foreach (WallObstacle wall in walls)
        {
            DestroyImmediate(wall.gameObject);
            deletedCount++;
        }

        WindZone[] winds = Object.FindObjectsByType<WindZone>(FindObjectsSortMode.None);
        foreach (WindZone wind in winds)
        {
            DestroyImmediate(wind.gameObject);
            deletedCount++;
        }

        TrapObstacle[] traps = Object.FindObjectsByType<TrapObstacle>(FindObjectsSortMode.None);
        foreach (TrapObstacle trap in traps)
        {
            DestroyImmediate(trap.gameObject);
            deletedCount++;
        }

        ObstacleSpawner spawner = Object.FindFirstObjectByType<ObstacleSpawner>();
        if (spawner != null)
        {
            spawner.gameObject.SetActive(false);
            Debug.Log("✅ ObstacleSpawner disabled");
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();

        Debug.Log($"✅ Deleted {deletedCount} obstacles from scene!");
        Debug.Log("✅ All obstacles removed! Now only items will spawn.");
    }
}


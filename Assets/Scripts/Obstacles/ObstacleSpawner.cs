using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject windZonePrefab;
    [SerializeField] private GameObject trapPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private int minObstacles = 2;
    [SerializeField] private int maxObstacles = 5;
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-7f, -3f);
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(7f, 3f);
    [SerializeField] private float minDistanceBetweenObstacles = 2f;

    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Start()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        ClearObstacles();

        int obstacleCount = Random.Range(minObstacles, maxObstacles + 1);
        List<Vector2> spawnedPositions = new List<Vector2>();

        for (int i = 0; i < obstacleCount; i++)
        {
            GameObject prefab = GetRandomObstaclePrefab();
            if (prefab == null) continue;

            Vector2 spawnPosition = GetValidSpawnPosition(spawnedPositions);
            if (spawnPosition == Vector2.zero) continue;

            GameObject obstacle = Instantiate(prefab, spawnPosition, Quaternion.identity);
            obstacle.transform.parent = transform;
            spawnedObstacles.Add(obstacle);
            spawnedPositions.Add(spawnPosition);
        }
    }

    GameObject GetRandomObstaclePrefab()
    {
        List<GameObject> availablePrefabs = new List<GameObject>();
        
        if (wallPrefab != null) availablePrefabs.Add(wallPrefab);
        if (windZonePrefab != null) availablePrefabs.Add(windZonePrefab);
        if (trapPrefab != null) availablePrefabs.Add(trapPrefab);

        if (availablePrefabs.Count == 0) return null;

        return availablePrefabs[Random.Range(0, availablePrefabs.Count)];
    }

    Vector2 GetValidSpawnPosition(List<Vector2> existingPositions)
    {
        int maxAttempts = 20;
        
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 candidatePosition = new Vector2(x, y);

            if (Mathf.Abs(x) < 2f && Mathf.Abs(y) < 2f)
                continue;

            bool tooClose = false;
            foreach (Vector2 existingPos in existingPositions)
            {
                if (Vector2.Distance(candidatePosition, existingPos) < minDistanceBetweenObstacles)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return candidatePosition;
        }

        return Vector2.zero;
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in spawnedObstacles)
        {
            if (obstacle != null)
                Destroy(obstacle);
        }
        spawnedObstacles.Clear();
    }
}


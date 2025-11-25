using UnityEngine;

public enum ObstacleType
{
    Wall,
    WindZone,
    Trap
}

public abstract class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] protected ObstacleType obstacleType;
    
    protected SpriteRenderer spriteRenderer;
    protected Collider2D obstacleCollider;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        obstacleCollider = GetComponent<Collider2D>();
    }

    public ObstacleType GetObstacleType() => obstacleType;
}


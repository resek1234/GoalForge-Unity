using UnityEngine;

public class WallObstacle : Obstacle
{
    protected override void Awake()
    {
        base.Awake();
        obstacleType = ObstacleType.Wall;
    }
}


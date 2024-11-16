using Unity.Entities;
using Unity.Mathematics;

public struct BallSpawner : IComponentData
{
    public Entity Prefab;
    public float ShootSpeed;
    public float3 SpawnPosition;
}
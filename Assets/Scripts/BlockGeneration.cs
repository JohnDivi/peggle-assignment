using Unity.Entities;
using Unity.Mathematics;

public struct BlockGeneration : IComponentData
{
    public Entity Prefab;
    public float Rotation;
    public float3 SpawnPosition;
}
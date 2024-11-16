using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BlockGenerationAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float Rotation;
    public float3 SpawnPosition;
}

public class BlockGenerationBaker : Baker<BlockGenerationAuthoring>
{
    public override void Bake(BlockGenerationAuthoring authoring)
    {
        Entity blockEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(blockEntity, new BlockGeneration
        {
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None),
            Rotation = authoring.Rotation,
            SpawnPosition = authoring.SpawnPosition,
        });
    }
}

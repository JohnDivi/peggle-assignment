using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BallSpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public float ShootSpeed;
    public float3 SpawnPosition;
}


public class BallSpawnerBaker : Baker<BallSpawnerAuthoring>
{
    public override void Bake(BallSpawnerAuthoring authoring)
    {
        Entity ballEntity = GetEntity(TransformUsageFlags.None);
        AddComponent(ballEntity, new BallSpawner
        {
            Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.None),
            ShootSpeed = authoring.ShootSpeed,
            SpawnPosition = authoring.SpawnPosition,
        });
    }
}


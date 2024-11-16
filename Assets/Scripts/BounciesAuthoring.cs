using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BounciesAuthoring : MonoBehaviour
{
    public float Speed;
}

public class BounciesBaker : Baker<BounciesAuthoring>
{
    public override void Bake(BounciesAuthoring authoring)
    {
        Entity bouncyEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(bouncyEntity, new Bouncies
        {
            Speed = authoring.Speed,
        });
    }
}

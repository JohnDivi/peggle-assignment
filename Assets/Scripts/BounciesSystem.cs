using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public partial struct BounciesSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {

        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (bouncies, localTransform) in SystemAPI.Query<RefRW<Bouncies>, RefRW<LocalTransform>>())
        {
            localTransform.ValueRW.Position += new float3(0, bouncies.ValueRW.Speed * deltaTime, 0);
            if (localTransform.ValueRO.Position.y < -5)
            {
                localTransform.ValueRW.Position.y = -5;
                bouncies.ValueRW.Speed *= -1;
            }

            if (localTransform.ValueRO.Position.y > 5)
            {
                localTransform.ValueRW.Position.y = 5;
                bouncies.ValueRW.Speed *= -1;
            }
        }
        
    }
}

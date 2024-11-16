using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct BlockGenerationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BlockGeneration>();
    }
    public void OnUpdate(ref SystemState state)
    {
        var prefab = SystemAPI.GetSingleton<BlockGeneration>().Prefab;
        var spawnPos = SystemAPI.GetSingleton<BlockGeneration>().SpawnPosition;
        var rotation = SystemAPI.GetSingleton<BlockGeneration>().Rotation;

        if (prefab != null)
        {
            // 4 levels of slants
            int levels = 4;
            int orientation = 1;

            var ecb = new EntityCommandBuffer(Allocator.Temp);
            for (int i = 0; i < levels; i++)
            {
                if (i % 2 == 0) { orientation = 1; rotation *= -1; }
                else { orientation = -1; rotation *= -1; }

                spawnPos -= new float3(0, 3, 0);

                for (int j = 0; j < 16; j++)
                {
                    spawnPos += new float3(1.35f*orientation, orientation*math.sin(math.radians(rotation)),0);

                    var instance = ecb.Instantiate(prefab);
                    ecb.AddComponent<ScoreObject>(instance);
                    ecb.SetComponent(instance, new LocalTransform
                    {
                        Position = spawnPos,
                        Rotation = quaternion.RotateZ(math.radians(rotation)),
                        Scale = 1f
                    });

                    
                }
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();

        }
        state.Enabled = false;
    }
}

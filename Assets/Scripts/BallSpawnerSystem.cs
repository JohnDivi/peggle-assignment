using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Assertions.Must;

public partial struct BallSpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BallSpawner>();
    }

    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var collisionWorld = physicsWorld.CollisionWorld;

            var raycastInput = new RaycastInput
            {
                Start = ray.origin,
                End = ray.origin + ray.direction * 100f,
                Filter = CollisionFilter.Default
            };

            if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit))
            {
                
                float3 hitPos = raycastHit.Position;
                hitPos.z = 0f;

                var prefab = SystemAPI.GetSingleton<BallSpawner>().Prefab;
                var spawnPos = SystemAPI.GetSingleton<BallSpawner>().SpawnPosition;
                var shootSpeed = SystemAPI.GetSingleton<BallSpawner>().ShootSpeed;

                if (prefab != Entity.Null)
                {
                    var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
                    var instance = ecb.Instantiate(prefab);
                    ecb.SetComponent(instance, LocalTransform.FromPosition(spawnPos));

                    // Get vector from spawnPos to hitPos
                    // Normalize
                    // Multiply by shootedSpeed
                    float3 aimVec = math.normalize(hitPos - spawnPos);
                    ecb.SetComponent(instance, new PhysicsVelocity
                    {
                        Linear = aimVec * shootSpeed,
                    });

                    ecb.Playback(state.EntityManager);
                    ecb.Dispose();
                }
            }
        }
    }
}

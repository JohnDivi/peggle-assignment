using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.VisualScripting;
using UnityEngine;

[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
public partial struct BlockDeletionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        var collisionJob = new CollisionJob
        {
            allPickups = SystemAPI.GetComponentLookup<ScoreObject>(true),
            commandBuffer = ecb,
        };
        SimulationSingleton simulationSingleton = SystemAPI.GetSingleton<SimulationSingleton>();
        state.Dependency = collisionJob.Schedule(simulationSingleton, state.Dependency);
        state.Dependency.Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private struct CollisionJob : ICollisionEventsJob
    {
        [ReadOnly] public ComponentLookup<ScoreObject> allPickups;
        public EntityCommandBuffer commandBuffer;
        public void Execute(CollisionEvent collisionEvent)
        {
            Entity entityA = collisionEvent.EntityA;
            Entity entityB = collisionEvent.EntityB;

            if (allPickups.HasComponent(entityA))
            {
                commandBuffer.DestroyEntity(entityA);
            }

            if (allPickups.HasComponent(entityB))
            {
                commandBuffer.DestroyEntity(entityB);
            }
        }
    }

}

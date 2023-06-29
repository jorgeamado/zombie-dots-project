using System.Runtime.InteropServices;
using ComponentsAndTags.Brain;
using ComponentsAndTags.Zombie;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using ZombieWalkAspect = ComponentsAndTags.Zombie.ZombieWalkAspect;

namespace Systems
{
    [UpdateAfter(typeof(SpawnZombieSystem))]
    public partial struct ZombieWalkSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BrainTag>();
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var brainEntity = SystemAPI.GetSingletonEntity<BrainTag>();
            var brainTransform = SystemAPI.GetComponent<LocalTransform>(brainEntity);
            var brainTransformScale = brainTransform.Scale;
            var brainRadius = brainTransformScale *5f + 0.5f;

            new ZombieWalkJob()
            {
                DT = deltaTime,
                BrainRadiusSq = brainRadius * brainRadius,
                ECB = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieWalkJob : IJobEntity
    {
        public float DT;
        public float BrainRadiusSq;
        public EntityCommandBuffer.ParallelWriter ECB;

        public void Execute([ChunkIndexInQuery] int sortKey, ZombieWalkAspect zombie)
        {
            zombie.Walk(DT);
            if (zombie.IsInStopRange(float3.zero, BrainRadiusSq))
            {
                ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.Entity, false);
                ECB.SetComponentEnabled<ZombieEatProperties>(sortKey, zombie.Entity, true);
            }
        }
    }
}
using ComponentsAndTags.Brain;
using ComponentsAndTags.Zombie;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems.Zombie
{
    [UpdateAfter(typeof(ZombieWalkSystem))]
    public partial struct ZombieEatSystem : ISystem
    {
        [BurstCompile]
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
            var brainSingleton = SystemAPI.GetSingletonEntity<BrainTag>();
            var localTransform = SystemAPI.GetComponent<LocalTransform>(brainSingleton);


            var localTransformScale = localTransform.Scale * 5f + 1f ;
            new ZombieEatJob()
            {
                DT = deltaTime,
                ECB = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                BrainEntity = brainSingleton,
                BrainRadiusSq = localTransformScale * localTransformScale
            }.ScheduleParallel();
        }

        [BurstCompile]
        private partial struct ZombieEatJob : IJobEntity
        {
            public float DT;
            public EntityCommandBuffer.ParallelWriter ECB;
            public Entity BrainEntity;
            public float BrainRadiusSq;

            public void Execute([ChunkIndexInQuery] int sortKey, ZombieEatAspect zombie)
            {
                if (zombie.IsInEatingRange(float3.zero, BrainRadiusSq))
                {
                    zombie.Eat(DT, ECB, sortKey, BrainEntity);
                }
                else
                {
                    ECB.SetComponentEnabled<ZombieEatProperties>(sortKey, zombie.Entity, false);
                    ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.Entity, true);
                }
            }
        }
    }
}
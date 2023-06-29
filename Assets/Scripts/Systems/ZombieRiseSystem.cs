using ComponentsAndTags.Zombie;
using Unity.Burst;
using Unity.Entities;
using ZombieRiseAspect = ComponentsAndTags.Zombie.ZombieRiseAspect;


namespace Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(SpawnZombieSystem))]
    public partial struct ZombieRiseSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            new ZombieRiseJob
            {
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                DeltaTime = deltaTime,
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct ZombieRiseJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        [BurstCompile]
        private void Execute([ChunkIndexInQuery] int sortKey, ZombieRiseAspect zombie)
        {
            zombie.Rise(DeltaTime);
            
            if(!zombie.IsAboveGround) return;
            zombie.SetAtGroundLevel();
            ECB.RemoveComponent<ZombieRiseRate>(sortKey, zombie.Entity);
            ECB.SetComponentEnabled<ZombieWalkProperties>(sortKey, zombie.Entity, true);
        }
    }
}
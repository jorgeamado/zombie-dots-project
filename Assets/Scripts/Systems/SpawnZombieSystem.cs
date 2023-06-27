using ComponentsAndTags;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    public partial struct SpawnZombieSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var timeDeltaTime = SystemAPI.Time.DeltaTime;
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            new SpawnZombieJob()
            {
                DeltaTime = timeDeltaTime,
                ECB = ecb.CreateCommandBuffer(state.WorldUnmanaged)
            }.Schedule();
        }
    }

    [BurstCompile]
    public partial struct SpawnZombieJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;

        private void Execute(GraveyardAspect graveyard)
        {
            graveyard.ZombieSpawnTimer -= DeltaTime;
            if (!graveyard.TimeToSpawnZombie) return;
            if (!graveyard.ZombieSpawnPointInitialized()) return;

            graveyard.ZombieSpawnTimer = graveyard.ZombieSpawnRate;
            var zombie = ECB.Instantiate(graveyard.ZombiePrefab);
            ECB.SetComponent(zombie, graveyard.GetZombieSpawnPoint());
        }
    }
}
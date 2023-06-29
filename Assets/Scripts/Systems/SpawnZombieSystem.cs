using ComponentsAndTags;
using ComponentsAndTags.Zombie;
using Unity.Burst;
using Unity.Entities;
using Utils;

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
            var zombieSpawnPoint = graveyard.GetZombieSpawnPoint();
            ECB.SetComponent(zombie, zombieSpawnPoint);

            var heading = MathHelpers.GetHeading(zombieSpawnPoint.Position, graveyard.Position);
            ECB.SetComponent(zombie, new ZombieHeading(){Value = heading});
        }
    }
}
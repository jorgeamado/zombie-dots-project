using ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GraveyardProperties>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if(!Input.GetKeyUp(KeyCode.Space))
                return;
            
            var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
            var graveyard = SystemAPI.GetAspectRW<GraveyardAspect>(graveyardEntity);


            var builder = new BlobBuilder(Allocator.Temp);
            ref var spawnPoints = ref builder.ConstructRoot<ZombieSpawnPointsBlob>();
            var arrayBuilder = builder.Allocate(ref spawnPoints.Value, graveyard.NumberTombstoneToSpawn);

            var ecb = new EntityCommandBuffer(Allocator.Temp);
           
            for (int i = 0; i < graveyard.NumberTombstoneToSpawn; i++)
            {
                var newTombstone = ecb.Instantiate(graveyard.TombstonePrefab);
                var randomTombstoneTransform = graveyard.GetRandomTombstoneTransform();
                ecb.SetComponent(newTombstone, randomTombstoneTransform);

                var newZombiePosition = randomTombstoneTransform.Position;
                arrayBuilder[i] = newZombiePosition;
            }
            
            var blobAsset = builder.CreateBlobAssetReference<ZombieSpawnPointsBlob>(Allocator.Persistent);
            ecb.SetComponent(graveyardEntity, new ZombieSpawnPoints{Value = blobAsset});
            builder.Dispose();
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
            
            state.Enabled = false; //Disable after 1 frame

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ComponentsAndTags
{
    public class GraveyardPropertiesAuthoring : MonoBehaviour
    {
        [Header("Graveyard Properties")]
        public float2 FieldDimensions;
        public int NumberTombstoneToSpawn;
        public GameObject TombstonePrefab;
        public float BrainSafeRadius = 10;
        public GameObject ZombiePrefab;
        public float ZombieSpawnRate;

        [Header("Random seed")]
        public uint RandomSeedValue;

        public class GraveyardPropertiesBaker : Baker<GraveyardPropertiesAuthoring>
        {
            public override void Bake(GraveyardPropertiesAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity,
                    new GraveyardRandom
                        { Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeedValue) });
                AddComponent(entity,
                    new GraveyardProperties
                    {
                        FieldDimensions = authoring.FieldDimensions,
                        NumberTombstoneToSpawn = authoring.NumberTombstoneToSpawn,
                        TombstonePrefab = GetEntity(authoring.TombstonePrefab, TransformUsageFlags.Dynamic),
                        ZombiePrefab = GetEntity(authoring.ZombiePrefab, TransformUsageFlags.Dynamic),
                        ZombieSpawnRate = authoring.ZombieSpawnRate,

                        BrainSafeRadiusSQ = authoring.BrainSafeRadius * authoring.BrainSafeRadius,
                    });

                AddComponent<ZombieSpawnPoints>(entity);
                AddComponent<ZombieSpawnTimer>(entity);
            }
        }
    }
}
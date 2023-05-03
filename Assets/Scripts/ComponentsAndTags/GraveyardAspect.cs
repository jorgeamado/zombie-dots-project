#region

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Utils;

#endregion

namespace ComponentsAndTags
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRW<GraveyardRandom> _graveyardRandom;
        private readonly RefRO<GraveyardProperties> _graveyardProperties;
        private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
        private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;

        public float ZombieSpawnTimer
        {
            get => _zombieSpawnTimer.ValueRO.Value;
            set => _zombieSpawnTimer.ValueRW.Value = value;
        }

        public float3 Position => _localTransform.ValueRO.Position;

        public float BrainSafeRadiusSq => _graveyardProperties.ValueRO.BrainSafeRadiusSQ;
        public Entity TombstonePrefab => _graveyardProperties.ValueRO.TombstonePrefab;
        public int NumberTombstoneToSpawn => _graveyardProperties.ValueRO.NumberTombstoneToSpawn;
        public float2 FieldDimensions => _graveyardProperties.ValueRO.FieldDimensions;
        
        
        public bool ZombieSpawnPointInitialized()
        {
            return _zombieSpawnPoints.ValueRO.Value.IsCreated && ZombieSpawnPointCount > 0;
        }

        private int ZombieSpawnPointCount => _zombieSpawnPoints.ValueRO.Value.Value.Value.Length;


        public ref Random Random => ref _graveyardRandom.ValueRW.Value;

        public LocalTransform GetRandomTombstoneTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale(0.5f)
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;

            var cookie = InfiniteLoopProtector.RunCookie();
            do
            {
                randomPosition = Random.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(Position, randomPosition) <= BrainSafeRadiusSq && cookie.NextLoop());

            return randomPosition;
        }

        private float3 MinCorner => Position - HalfDimensions;
        private float3 MaxCorner => Position + HalfDimensions;

        private float3 HalfDimensions => new()
        {
            x = _graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = _graveyardProperties.ValueRO.FieldDimensions.y * 0.5f
        };


        private quaternion GetRandomRotation()
        {
            return quaternion.RotateY(Random.NextFloat(-0.45f, 0.45f));
        }

        private float GetRandomScale(float min)
        {
            return Random.NextFloat(min, 1f);
        }

        public float2 GetRandomOffset()
        {
            return Random.NextFloat2();
        }
        
        private float3 GetRandomZombieSpawnPoint()
        {
            return GetZombieSpawnPoint(_graveyardRandom.ValueRW.Value.NextInt(ZombieSpawnPointCount));
        }

        private float3 GetZombieSpawnPoint(int i) => _zombieSpawnPoints.ValueRO.Value.Value.Value[i];
        
        

    }
}
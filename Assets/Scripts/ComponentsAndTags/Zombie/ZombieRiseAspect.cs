using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ComponentsAndTags.Zombie
{
    public readonly partial struct ZombieRiseAspect : IAspect
    {
        public readonly Entity Entity;
        // public readonly Tran
        private readonly RefRW<LocalTransform> _localTransform;

        private float3 Position
        {
            get => _localTransform.ValueRO.Position;
            set => _localTransform.ValueRW.Position = value;
        }

        private readonly RefRO<ZombieRiseRate> _zombieRiseRate;

        public bool IsAboveGround => _localTransform.ValueRO.Position.y >= 0;
        public void Rise(float deltaTime)
        {
            var valueRWPosition = math.up() * deltaTime * _zombieRiseRate.ValueRO.Value;
            Position += valueRWPosition;
        }

        public void SetAtGroundLevel()
        {
            var position = Position;
            position.y = 0;
            Position = position;
        }

    }
}
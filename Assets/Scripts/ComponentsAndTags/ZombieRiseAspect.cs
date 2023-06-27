using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ComponentsAndTags
{
    public readonly partial struct ZombieRiseAspect : IAspect
    {
        public readonly Entity Entity;
        // public readonly Tran
        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRO<ZombieRiseRate> _zombieRiseRate;

        public void Rise(float deltaTime)
        {
            var valueRWPosition = math.up() * deltaTime * _zombieRiseRate.ValueRO.Value;
            _localTransform.ValueRW.Position += valueRWPosition;
        }
        
    }
}
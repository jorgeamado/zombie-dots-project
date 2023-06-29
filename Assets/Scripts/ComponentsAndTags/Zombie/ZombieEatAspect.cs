using ComponentsAndTags.Brain;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ComponentsAndTags.Zombie
{
    public readonly partial struct ZombieEatAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRW<LocalTransform> _localTransform;


        private readonly RefRW<ZombieTimer> _timer;
        private readonly RefRO<ZombieEatProperties> _eatProperties;
        private readonly RefRO<ZombieHeading> _heading;

        private float EatFrequency => _eatProperties.ValueRO.EatFrequency;
        private float EatAmplitude => _eatProperties.ValueRO.EatAmplitude;
        public float EatDamagePerSecond => _eatProperties.ValueRO.EatDamagePerSecond;
        private float Heading => _heading.ValueRO.Value;

        private float Timer
        {
            get => _timer.ValueRO.Value;
            set => _timer.ValueRW.Value = value;
        }
        
        public float3 Position => _localTransform.ValueRO.Position;

        public quaternion Rotation
        {
            get => _localTransform.ValueRO.Rotation;
            set => _localTransform.ValueRW.Rotation = value;
        }


        public void Eat(float dt, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity brainEntity)
        {
            Timer += dt;
            var eatAngle = EatAmplitude * math.sin(EatFrequency * Timer);
            Rotation = quaternion.Euler(eatAngle, Heading, 0);

            var eatDamage = EatDamagePerSecond* dt;
            var brainDamageBufferElement = new BrainDamageBufferElement(){Value = eatDamage};
            ecb.AppendToBuffer(sortKey, brainEntity, brainDamageBufferElement);
        }

        public bool IsInEatingRange(float3 brainPosition, float brainRadiusSq)
        {
            return math.distancesq(brainPosition, Position) <= brainRadiusSq - 1;
        }
    }
}
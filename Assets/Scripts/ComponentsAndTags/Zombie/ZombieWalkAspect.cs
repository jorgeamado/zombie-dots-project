using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ComponentsAndTags.Zombie
{
    public readonly partial struct ZombieWalkAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _localTransform;
        private readonly RefRW<ZombieTimer> _walkTimer;
        private readonly RefRO<ZombieWalkProperties> _walkProperties;
        private readonly RefRO<ZombieHeading> _heading;

        private float WalkFrequency => _walkProperties.ValueRO.WalkFrequency;
        private float WalkAmplitude => _walkProperties.ValueRO.WalkAmplitude;
        private float WalkSpeed => _walkProperties.ValueRO.WalkSpeed;
        private float Heading => _heading.ValueRO.Value;


        private float3 LocalPosition
        {
            get => _localTransform.ValueRO.Position;
            set => _localTransform.ValueRW.Position = value;
        }
        public quaternion Rotation
        {
            get => _localTransform.ValueRO.Rotation;
            set => _localTransform.ValueRW.Rotation = value;
        }


        private float Timer
        {
            get => _walkTimer.ValueRO.Value;
            set => _walkTimer.ValueRW.Value = value;
        }

        public void Walk(float dt)
        {
            Timer += dt;
            LocalPosition += _localTransform.ValueRO.Forward() * WalkSpeed * dt;

            var swayAngle = WalkAmplitude * math.sin(WalkFrequency * Timer);
            Rotation = quaternion.Euler(0, Heading, swayAngle);            
        }
        
        
        public bool IsInStopRange(float3 brainPosition, float brainRadiusSq)
        {
            return math.distancesq(brainPosition, LocalPosition) <= brainRadiusSq;
        }
    }
}
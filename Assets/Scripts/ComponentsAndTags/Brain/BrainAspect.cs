using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ComponentsAndTags.Brain
{
    public readonly partial struct BrainAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _localTransform;

        public quaternion Rotation
        {
            get => _localTransform.ValueRO.Rotation;
            set => _localTransform.ValueRW.Rotation = value;
        }

        private readonly RefRW<BrainHealth> _brainHealth;
        private readonly DynamicBuffer<BrainDamageBufferElement> _brainDamageBuffer;

        public float Scale
        {
            get => _localTransform.ValueRO.Scale;
            set => _localTransform.ValueRW.Scale = value;
        }

        public float MaxBrainHealth
        {
            get => _brainHealth.ValueRO.MaxValue;
            set => _brainHealth.ValueRW.MaxValue = value;
        }

        public float Health3
        {
            get => _brainHealth.ValueRO.Value;
            set => _brainHealth.ValueRW.Value = value;
        }


        public void ProcessDamage()
        {
            foreach (var brainDamageBufferElement in _brainDamageBuffer)
            {
                Health3 = math.clamp(Health3 - brainDamageBufferElement.Value, 0, MaxBrainHealth);
            }
            
            _brainDamageBuffer.Clear();

            Scale = math.max(float.Epsilon, Health3 / MaxBrainHealth);
        }
    }
}
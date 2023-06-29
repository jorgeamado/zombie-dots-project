using Unity.Entities;

namespace ComponentsAndTags.Brain
{
    public struct BrainTag : IComponentData
    {
        
    }

    public struct BrainHealth : IComponentData
    {
        public float Value;
        public float MaxValue;
    }

    public struct BrainDamageBufferElement : IBufferElementData
    {
        public float Value;
    }
}
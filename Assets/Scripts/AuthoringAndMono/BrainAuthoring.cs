using ComponentsAndTags.Brain;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
    public class BrainAuthoring : MonoBehaviour
    {
        public float BrainHealth;

        public class BrainTagBaker : Baker<BrainAuthoring>
        {
            public override void Bake(BrainAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<BrainTag>(entity);
                AddComponent(entity,
                    new BrainHealth { Value = authoring.BrainHealth, MaxValue = authoring.BrainHealth });
                AddBuffer<BrainDamageBufferElement>(entity);
            }
        }
    }
}
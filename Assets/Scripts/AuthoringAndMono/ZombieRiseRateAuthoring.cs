using ComponentsAndTags;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
    public class ZombieRiseRateAuthoring : MonoBehaviour
    {
        public float ZombieRiseRate;

        public class ZombieRiseRateBaker : Baker<ZombieRiseRateAuthoring>
        {
            public override void Bake(ZombieRiseRateAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ZombieRiseRate { Value = authoring.ZombieRiseRate });
            }
        }
    }
}
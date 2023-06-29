using ComponentsAndTags.Zombie;
using Unity.Entities;
using UnityEngine;

namespace AuthoringAndMono
{
    public class ZombieAuthoring : MonoBehaviour
    {
        public float ZombieRiseRate;
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
        public float EatDamagePerSecond;
        public float EatAmplitude;
        public float EatFrequency;

        public class ZombieRiseRateBaker : Baker<ZombieAuthoring>
        {
            public override void Bake(ZombieAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new ZombieRiseRate { Value = authoring.ZombieRiseRate });
                
                
                AddComponent(entity,
                    new ZombieWalkProperties
                        {
                            WalkSpeed = authoring.WalkSpeed,
                            WalkAmplitude = authoring.WalkAmplitude,
                            WalkFrequency = authoring.WalkFrequency
                        });
                SetComponentEnabled<ZombieWalkProperties>(entity, false);
                
                
                AddComponent<ZombieTimer>(entity);
                AddComponent<ZombieHeading>(entity);
                AddComponent(entity,
                    new ZombieEatProperties
                        {
                            EatDamagePerSecond = authoring.EatDamagePerSecond,
                            EatAmplitude = authoring.EatAmplitude,
                            EatFrequency = authoring.EatFrequency
                        });
                SetComponentEnabled<ZombieEatProperties>(entity, false);
            }
        }
    }
}
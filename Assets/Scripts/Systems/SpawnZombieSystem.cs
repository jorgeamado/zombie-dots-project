using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    public partial struct SpawnZombieSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            UnityEngine.Assertions.Assert.IsNotNull();
        }
    }
    
    
    
}
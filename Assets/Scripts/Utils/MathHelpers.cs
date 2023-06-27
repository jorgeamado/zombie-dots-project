using Unity.Mathematics;

namespace Utils
{
    public static class MathHelpers
    {
        public static float GetHeading(float3 position, float3 target)
        {
            var float3 = position - target;
            return math.atan2(float3.x, float3.z) + math.PI;
        }
    }
}
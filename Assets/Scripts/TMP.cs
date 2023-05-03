using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ComponentsAndTags
{
    public class GraveyardPropertiesAuthoring1 : MonoBehaviour
    {
    public int ScavsCount;
    public float2 FieldDimensions;
    public 
    }

    public struct PropertiesComponentData : IComponentData
    {
        public int ScavsCount;
    }

    public class GraveyardPropertiesAuthoringBaker : Baker<GraveyardPropertiesAuthoring1>
    {
        public override void Bake(GraveyardPropertiesAuthoring1 authoring)
        {
            var getEntity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(getEntity, new GraveyardPropertiesComponentData { FieldDimensions = authoring.FieldDimensions });
            AddComponent(getEntity, new PropertiesComponentData { ScavsCount = authoring.ScavsCount });
        }
    }

    public struct GraveyardPropertiesComponentData : IComponentData
    {
        public float2 FieldDimensions;
    }
}
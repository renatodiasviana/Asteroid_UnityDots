using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class ParticleDestructionEffectSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<ParticleData>().ForEach((Entity entity) => 
        {
            ParticleData data = EntityManager.GetComponentData<ParticleData>(entity);
            data.TimePassedToDestruction += Time.DeltaTime;

            if (data.TimePassedToDestruction >= data.DestructionTime)
                EntityManager.DestroyEntity(entity);
            else
                EntityManager.SetComponentData(entity, data);
        });
    }
}

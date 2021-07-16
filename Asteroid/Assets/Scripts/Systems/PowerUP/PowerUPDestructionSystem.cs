using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PowerUPDestructionSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        float deltaTime = UnityEngine.Time.deltaTime;

        Entities.WithAll<PowerUPData>().ForEach((Entity entity) =>
        {
            PowerUPData data = EntityManager.GetComponentData<PowerUPData>(entity);
            data.TimeToDestruct -= deltaTime;

            EntityManager.SetComponentData(entity, data);

            if (data.TimeToDestruct <= 0.0f)
                EntityManager.DestroyEntity(entity);
        });
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class LaserShotSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<LaserData>().ForEach((Entity entity) =>
        {
            LaserData laserData = EntityManager.GetComponentData<LaserData>(entity);
            Translation translation = EntityManager.GetComponentData<Translation>(entity);

            Rotation rotation = new Rotation();
            rotation.Value = laserData.ShotRotation.Value;

            EntityManager.SetComponentData(entity, rotation);

           if (translation.Value.y < -12.0f || translation.Value.y > 12.0f ||
                translation.Value.x > 19.0f || translation.Value.x < -19.0f)
            {
                EntityManager.DestroyEntity(entity);
            }
        });
    }
}

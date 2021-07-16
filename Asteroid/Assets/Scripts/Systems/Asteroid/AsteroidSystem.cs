using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;

public class AsteroidSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float speed = 5.0f;
        float deltaTime = Time.DeltaTime * speed;

        Entities.WithAll<AsteroidData>().ForEach((ref AsteroidData managerData, ref Translation translation, ref Rotation rotation) =>
        {
            translation.Value += managerData.LinearVelocity * deltaTime;
            rotation.Value = math.mul(rotation.Value, quaternion.EulerXYZ(managerData.AngularVelocity * deltaTime));

            if (translation.Value.y < -15.0f)
                translation.Value.y = 15.0f;
            else if (translation.Value.y > 15.0f)
                translation.Value.y = -15.0f;

            if (translation.Value.x > 25.0f)
                translation.Value.x = -25.0f;
            else if (translation.Value.x < -25.0f)
                translation.Value.x = 25.0f;

        }).ScheduleParallel();
    }
};

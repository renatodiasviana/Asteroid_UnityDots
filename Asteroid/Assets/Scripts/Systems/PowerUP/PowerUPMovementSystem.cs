using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class PowerUPMovimentSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = UnityEngine.Time.deltaTime;

        Entities.ForEach((ref Entity entity, ref Translation translation, ref Rotation rotation, ref CompositeScale scale, ref PowerUPData data) =>
        {
            translation.Value += data.Velocity * deltaTime;

            float scaleValue = 3.0f + Unity.Mathematics.math.sin(deltaTime * 90.0f) * 2.0f;
            scale.Value = float4x4.Scale(scaleValue);

            if (translation.Value.y < -12.0f)
                translation.Value.y = 12.0f;
            else if (translation.Value.y > 12.0f)
                translation.Value.y = -12.0f;

            if (translation.Value.x > 19.0f)
                translation.Value.x = -19.0f;
            else if (translation.Value.x < -19.0f)
                translation.Value.x = 19.0f;

        }).ScheduleParallel();
    }
}

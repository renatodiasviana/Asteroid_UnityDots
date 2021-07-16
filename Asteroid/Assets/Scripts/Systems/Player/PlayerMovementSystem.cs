using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;

public class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.5f;

        Entities.WithAll<PlayerData>().ForEach((ref Translation translation, ref PhysicsVelocity velocity,
                                                ref Rotation rotation, in LocalToWorld ltw, in PlayerData playerData) =>
        {
            float3 lookDir = math.normalize( (float3) mousePosition - translation.Value);
            rotation.Value = quaternion.LookRotation((float3) Vector3.forward, lookDir);

            if (Input.GetKey(KeyCode.W))
                velocity.Linear += ltw.Up * playerData.ShipSpeed * 0.025f;
            else if (Input.GetKey(KeyCode.S))
                velocity.Linear += -velocity.Linear * playerData.ShipSpeed * 0.025f;

            if (translation.Value.y < -12.0f)
                translation.Value.y = 12.0f;
            else if (translation.Value.y > 12.0f)
                translation.Value.y = -12.0f;

            if (translation.Value.x > 19.0f)
                translation.Value.x = -19.0f;
            else if (translation.Value.x < -19.0f)
                translation.Value.x = 19.0f;

        }).Run();
    }
}
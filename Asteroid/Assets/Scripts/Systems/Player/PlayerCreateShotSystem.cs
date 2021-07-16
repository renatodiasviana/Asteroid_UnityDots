using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

public class PlayerCreateShotSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0.5f;

        Entities.WithAll<PlayerData>().ForEach((Entity entity) =>
        {
            if (Input.GetMouseButtonUp(0))
            {
                PlayerData playerData = EntityManager.GetComponentData<PlayerData>(entity);
                Translation translation = EntityManager.GetComponentData<Translation>(entity);

                if (playerData.BulletsCount == 0)
                    return;

                // Update number of bullets
                playerData.BulletsCount--;
                EntityManager.SetComponentData(entity, playerData);

                float3 lookDir = math.normalize( (float3) mousePosition - translation.Value);

                Rotation rotation = new Rotation();
                rotation.Value = quaternion.LookRotation( (float3) Vector3.forward, lookDir);

                Entity spawnedShot = Entity.Null;
                spawnedShot = EntityManager.Instantiate(playerData.LaserPrefab_01);

                LaserData laserData = EntityManager.GetComponentData<LaserData>(spawnedShot);
                laserData.ShotRotation = rotation;

                Translation shotTranslation = new Translation();
                shotTranslation.Value = translation.Value + lookDir * 0.5f;

                Rotation shotRotation = new Rotation();
                shotRotation.Value = rotation.Value;

                PhysicsVelocity shotVelocity = new PhysicsVelocity();
                shotVelocity.Linear = lookDir * 10.0f;

                EntityManager.SetComponentData(spawnedShot, shotTranslation);
                EntityManager.SetComponentData(spawnedShot, shotRotation);
                EntityManager.SetComponentData(spawnedShot, shotVelocity);
                EntityManager.SetComponentData(spawnedShot, laserData);
            }
        });
    }
}

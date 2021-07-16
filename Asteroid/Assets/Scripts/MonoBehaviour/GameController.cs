using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Physics.Systems;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

public class GameController : MonoBehaviour
{
    public UnityEngine.UI.Text Score;
    public UnityEngine.UI.Text BulletCount;
    public Transform Lifes;

    void RestartLevel(Entity player)
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Destroying Asteroids
        EntityQuery query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(AsteroidData) });
        entityManager.DestroyEntity(query);

        // Destroying Laser
        query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(LaserData) });
        entityManager.DestroyEntity(query);

        // Destroying Bullet Power UP
        query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(PowerUPData) });
        entityManager.DestroyEntity(query);

        // Destroying Particles
        query = entityManager.CreateEntityQuery(new ComponentType[] { typeof(ParticleData) });
        entityManager.DestroyEntity(query);

        // Restarting Player
        Translation translation = entityManager.GetComponentData<Translation>(player);
        PlayerData data = entityManager.GetComponentData<PlayerData>(player);
        PhysicsVelocity velocity = entityManager.GetComponentData<PhysicsVelocity>(player);

        velocity.Linear = new float3(0.0f, 0.0f, 0.0f);
        translation.Value = new float3(0.0f, 0.0f, 0.5f);
        data.Life = 3;
        data.BulletsCount = 100;
        data.Score = 0;

        entityManager.SetComponentData(player, translation);
        entityManager.SetComponentData(player, data);
        entityManager.SetComponentData(player, velocity);
    }

    void UpdateInterface(Entity player, PlayerData data)
    {
        Score.text = data.Score.ToString();
        BulletCount.text = data.BulletsCount.ToString();

        // Enable icons based on player life
        for (int i = 0; i < 3; i++)
            Lifes.GetChild(i).gameObject.SetActive(i < data.Life);
    }

    // Update is called once per frame
    void Update()
    {
        BuildPhysicsWorld worldBuilt = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        Entity player = worldBuilt.GetSingletonEntity<PlayerData>();

        ComponentDataFromEntity<PlayerData> playerData = worldBuilt.GetComponentDataFromEntity<PlayerData>();

        if (playerData.HasComponent(player))
        {
            PlayerData data = playerData[player];

            UpdateInterface(player, data);

            if (data.Life == 0)
                RestartLevel(player);
        }
    }
}

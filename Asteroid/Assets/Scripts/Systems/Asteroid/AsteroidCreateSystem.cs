using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;
using System;

public class AsteroidCreateSystem : ComponentSystem
{
    public float TimePassed = 7.5f;
    public float TimeToCreateAsteroid = 5.0f;

    protected void CreateAsteroid()
    {
        Entities.ForEach((ref AsteroidManagerData managerData) =>
        {
            Entity spawnedAsteroid = Entity.Null;
            int value = Random.Range(0, 6);

            switch (value)
            {
                case 0:
                    spawnedAsteroid = EntityManager.Instantiate(managerData.AsteroidPrefab_01);
                break;
                case 1:
                    spawnedAsteroid = EntityManager.Instantiate(managerData.AsteroidPrefab_02);
                break;
                case 2:
                    spawnedAsteroid = EntityManager.Instantiate(managerData.AsteroidPrefab_03);
                break;
                case 3:
                    spawnedAsteroid = EntityManager.Instantiate(managerData.AsteroidPrefab_04);
                break;
                case 4:
                    spawnedAsteroid = EntityManager.Instantiate(managerData.AsteroidPrefab_05);
                break;
                case 5:
                    spawnedAsteroid = EntityManager.Instantiate(managerData.AsteroidPrefab_06);
                break;
            };

            float x = 21.0f;
            float y = 13.0f;
            float speedX = -1.0f;
            float speedY = -1.0f;

            x *= (Random.value > 0.5f) ? 1.0f : -1.0f;
            y *= (Random.value > 0.5f) ? 1.0f : -1.0f;
            speedX *= (Random.value > 0.5f) ? 1.0f : -1.0f;
            speedY *= (Random.value > 0.5f) ? 1.0f : -1.0f;

            float valueP = Random.value;
            if (valueP < 0.25f)
                y *= 0.5f;
            else if (value < 0.5f)
                x *= 0.05f;

            Translation tranlation = new Translation();
            tranlation.Value = new float3(x, y, 0.5f);

            AsteroidData asteroidData = EntityManager.GetComponentData<AsteroidData>(spawnedAsteroid);
            asteroidData.LinearVelocity = new float3(speedX * Random.value, speedY * Random.value, 0.0f);

            asteroidData.AngularVelocity = new float3(0.0f, 0.0f, Random.RandomRange(-1.0f, 1.0f));

            EntityManager.SetComponentData(spawnedAsteroid, tranlation);
            EntityManager.SetComponentData(spawnedAsteroid, asteroidData);
        });
    }

    protected override void OnUpdate()
    {
        TimePassed += Time.DeltaTime;

        if (TimePassed >= TimeToCreateAsteroid)
        {
            TimePassed = 0.0f;
            CreateAsteroid();

            // Reduce the creation time during the game
            if (TimeToCreateAsteroid > 2.5f)
                TimeToCreateAsteroid -= 0.01f;
        }
    }
}

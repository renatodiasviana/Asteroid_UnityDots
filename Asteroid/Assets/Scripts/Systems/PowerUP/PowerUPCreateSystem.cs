using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

public class PowerUPCreateSystem : ComponentSystem
{
    public float TimePassed = 0.0f;

    protected override void OnUpdate()
    {
        TimePassed += Time.DeltaTime;
        if (TimePassed < 10.0f)
            return;

        TimePassed = 0.0f;

        Entities.ForEach((ref PowerUPManagerData managerData) => 
        {
            Entity spawnedObj = EntityManager.Instantiate(managerData.PowerUPEntity);

            float x = 21.0f;
            float y = 13.0f;
            float speedX = 3.0f * Random.value;
            float speedY = 3.0f * Random.value;

            x *= (Random.value > 0.5f) ? 1.0f : -1.0f;
            y *= (Random.value > 0.5f) ? 1.0f : -1.0f;
            speedX *= (Random.value > 0.5f) ? 1.0f : -1.0f;
            speedY *= (Random.value > 0.5f) ? 1.0f : -1.0f;

            float value = Random.value;
            if (value < 0.25f)
                y *= 0.5f;
            else if (value < 0.5f)
                x *= 0.05f;

            Translation tranlation = new Translation();
            tranlation.Value = new float3(x, y, 0.5f);

            PowerUPData data = EntityManager.GetComponentData<PowerUPData>(spawnedObj);
            data.Velocity = new float3(speedX, speedY, 0.0f);
            data.TimeToDestruct = 10.0f;

            EntityManager.SetComponentData(spawnedObj, tranlation);
            EntityManager.SetComponentData(spawnedObj, data);
        });
    }
}

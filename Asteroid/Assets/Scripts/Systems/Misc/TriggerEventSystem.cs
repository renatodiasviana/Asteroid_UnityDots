using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class TriggerEventSystem : JobComponentSystem
{
    private BuildPhysicsWorld buildPhysicsWorlds;
    private StepPhysicsWorld stepPhysicsWorlds;

    protected override void OnCreate()
    {
        base.OnCreate();
        buildPhysicsWorlds = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorlds = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    [BurstCompile]
    struct ObjectsTriggerSystemJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<PlayerData> players;
        [ReadOnly] public ComponentDataFromEntity<AsteroidData> asteroids;
        [ReadOnly] public ComponentDataFromEntity<LaserData> laserShots;
        [ReadOnly] public ComponentDataFromEntity<Translation> translations;
        [ReadOnly] public ComponentDataFromEntity<PowerUPData> powerUPs;
        [ReadOnly] public Entity playerEntity;

        public EntityCommandBuffer entityCommandBuffer;

        public void CreateAsteroidParticle(Entity asteroidEntity)
        {
            AsteroidData data = asteroids[asteroidEntity];

            Entity particleEntity = entityCommandBuffer.Instantiate(asteroids[asteroidEntity].DestructionParticle);

            Translation asteroidTranslation = translations[asteroidEntity];

            Translation particleTranslation = new Translation();
            particleTranslation.Value = asteroidTranslation.Value;

            entityCommandBuffer.SetComponent(particleEntity, particleTranslation);
        }

        void CheckLaserAsteroidColision(Entity obj1, Entity obj2)
        {
            PlayerData data;
            if ((asteroids.HasComponent(obj1) && laserShots.HasComponent(obj2)) ||
                (asteroids.HasComponent(obj2) && laserShots.HasComponent(obj1)))
            {
                if (asteroids.HasComponent(obj1))
                    CreateAsteroidParticle(obj1);
                else
                    CreateAsteroidParticle(obj2);

                data = players[playerEntity];
                data.Score++;

                entityCommandBuffer.SetComponent(playerEntity, data);

                entityCommandBuffer.DestroyEntity(obj1);
                entityCommandBuffer.DestroyEntity(obj2);
            }
        }

        void CheckAsteroidPlayerCollision(Entity obj1, Entity obj2)
        {
            PlayerData data;
            if ((asteroids.HasComponent(obj1) && players.HasComponent(obj2)) ||
                (asteroids.HasComponent(obj2) && players.HasComponent(obj1)))
            {
                if (players.HasComponent(obj1))
                {
                    CreateAsteroidParticle(obj2);
                    entityCommandBuffer.DestroyEntity(obj2);
                }
                else
                {
                    CreateAsteroidParticle(obj1);
                    entityCommandBuffer.DestroyEntity(obj1);
                }

                data = players[playerEntity];
                data.Life--;

                entityCommandBuffer.SetComponent(playerEntity, data);
            }
        }

        void CheckPlayerPowerUPCollision(Entity obj1, Entity obj2)
        {
            PlayerData data;
            if ((powerUPs.HasComponent(obj1) && players.HasComponent(obj2)) ||
                (powerUPs.HasComponent(obj2) && players.HasComponent(obj1)))
            {
                if (players.HasComponent(obj1))
                    entityCommandBuffer.DestroyEntity(obj2);
                else
                    entityCommandBuffer.DestroyEntity(obj1);

                data = players[playerEntity];
                data.BulletsCount += 50;

                entityCommandBuffer.SetComponent(playerEntity, data);
            }
        }

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity obj1 = triggerEvent.EntityA;
            Entity obj2 = triggerEvent.EntityB;

            CheckLaserAsteroidColision(obj1, obj2);
            CheckAsteroidPlayerCollision(obj1, obj2);
            CheckPlayerPowerUPCollision(obj1, obj2);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        ObjectsTriggerSystemJob job = new ObjectsTriggerSystemJob();
        job.players = GetComponentDataFromEntity<PlayerData>(true);
        job.asteroids = GetComponentDataFromEntity<AsteroidData>(true);
        job.laserShots = GetComponentDataFromEntity<LaserData>(true);
        job.translations = GetComponentDataFromEntity<Translation>(true);
        job.powerUPs = GetComponentDataFromEntity<PowerUPData>(true);

        BeginInitializationEntityCommandBufferSystem ecbSystem;

        ecbSystem = World.GetExistingSystem<BeginInitializationEntityCommandBufferSystem>();
        job.playerEntity = ecbSystem.GetSingletonEntity<PlayerData>();
       
        job.entityCommandBuffer = ecbSystem.CreateCommandBuffer();

        JobHandle jobHandle = job.Schedule(stepPhysicsWorlds.Simulation,
                                           ref buildPhysicsWorlds.PhysicsWorld, inputDeps);

        ecbSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}

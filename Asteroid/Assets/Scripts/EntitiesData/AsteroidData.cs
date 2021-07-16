using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AsteroidData : IComponentData
{
    public float3 LinearVelocity;

    public float3 AngularVelocity;

    public int NumberAsteroidsMustCreate;

    public Entity DestructionParticle;
}

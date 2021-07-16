using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct ParticleData : IComponentData
{
    public float DestructionTime;

    public float TimePassedToDestruction;
}

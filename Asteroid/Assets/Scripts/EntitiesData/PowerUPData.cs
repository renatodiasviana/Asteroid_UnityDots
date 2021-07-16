using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PowerUPData : IComponentData
{
    public float3 Velocity;

    public float TimeToDestruct;
}

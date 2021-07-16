using Unity.Entities;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct LaserData : IComponentData
{
    public float ShotPower;

    public float Speed;

    public Rotation ShotRotation;
};
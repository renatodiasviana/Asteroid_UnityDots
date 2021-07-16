using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerData : IComponentData
{
    public int Life;

    public int BulletsCount;

    public int Score;

    public float ShipSpeed;

    public Entity LaserPrefab_01;
}

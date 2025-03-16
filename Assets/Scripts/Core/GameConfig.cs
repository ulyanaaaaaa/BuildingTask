using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public Vector2Int GridSize; 
    public BuildingData[] BuildingData;
    public int PoolSize;
}
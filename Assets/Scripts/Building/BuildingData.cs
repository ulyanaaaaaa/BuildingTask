using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Buildings/Building Data")]
public class BuildingData : ScriptableObject
{
    public string BuildingName;
    public Building Prefab; 
    public int SizeX; 
    public int SizeY; 
}
using System;
using System.Collections.Generic;

[Serializable]
public class BuildingSaveData
{
    public string BuildingType; 
    public int PosX;           
    public int PosY;           
    public int SizeX;          
    public int SizeY;         

    public BuildingSaveData() { }

    public BuildingSaveData(string buildingType, int posX, int posY, int sizeX, int sizeY)
    {
        BuildingType = buildingType;
        PosX = posX;
        PosY = posY;
        SizeX = sizeX;
        SizeY = sizeY;
    }
}

[Serializable]
public class GridSaveData
{
    public List<BuildingSaveData> Buildings = new List<BuildingSaveData>();
    
    public void AddBuilding(string buildingType, int posX, int posY, int sizeX, int sizeY)
    {
        Buildings.Add(new BuildingSaveData(buildingType, posX, posY, sizeX, sizeY));
    }
    
    public void Clear()
    {
        Buildings.Clear();
    }
    
    public BuildingSaveData FindBuildingAt(int posX, int posY)
    {
        return Buildings.Find(b => b.PosX == posX && b.PosY == posY);
    }
}
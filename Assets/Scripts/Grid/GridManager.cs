using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridManager : MonoBehaviour
{
    private Dictionary<Vector2Int, Building> _buildings = new Dictionary<Vector2Int, Building>();
    [Inject] private BuildingPool _buildingPool; 
    
    public void AddBuilding(Building building, Vector2Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int gridPosition = new Vector2Int(position.x + x, position.y + y);
                _buildings[gridPosition] = building;
            }
        }
    }
    
    public void RemoveBuilding(Building building, Vector2Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int gridPosition = new Vector2Int(position.x + x, position.y + y);
                if (_buildings.ContainsKey(gridPosition) && _buildings[gridPosition] == building)
                {
                    _buildings.Remove(gridPosition);
                }
            }
        }
        
        _buildingPool.ReturnBuilding(building);
    }
    
    public bool IsPlaceTaken(Vector2Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int gridPosition = new Vector2Int(position.x + x, position.y + y);
                if (_buildings.ContainsKey(gridPosition))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public void Save(GridSaveData data)
    {
        data.Clear();

        foreach (var entry in _buildings)
        {
            Vector2Int position = entry.Key;
            Building building = entry.Value;
            
            if (building.transform.position.x == position.x && building.transform.position.z == position.y)
            {
                data.AddBuilding(
                    building.BuildingName,
                    position.x,
                    position.y,
                    building.Size.x,
                    building.Size.y
                );
            }
        }
    }
    
    public void Load(GridSaveData data, BuildingData[] buildingData)
    {
        ClearBuildings();

        if (data == null || data.Buildings == null || data.Buildings.Count == 0)
        {
            return;
        }

        foreach (var buildingDataEntry in data.Buildings)
        {
            BuildingData prefabData = Array.Find(buildingData, b =>
                b != null &&
                b.BuildingName.Length >= 4 &&
                buildingDataEntry.BuildingType.Length >= 4 &&
                b.BuildingName.Substring(0, 4) == buildingDataEntry.BuildingType.Substring(0, 4)
            );

            if (prefabData != null)
            {
                Vector2Int position = new Vector2Int(buildingDataEntry.PosX, buildingDataEntry.PosY);
                Vector2Int size = new Vector2Int(buildingDataEntry.SizeX, buildingDataEntry.SizeY);

                if (!IsPlaceTaken(position, size))
                {
                    Building buildingInstance = _buildingPool.GetBuilding(prefabData);
                    buildingInstance.Size = size;
                    buildingInstance.BuildingData = prefabData;
                    AddBuilding(buildingInstance, position, size);
                
                    buildingInstance.transform.position = new Vector3(position.x, 0, position.y);
                }
            }
        }
    }
    
    private void ClearBuildings()
    {
        foreach (var building in _buildings.Values)
        {
            _buildingPool.ReturnBuilding(building);
        }
        _buildings.Clear();
    }
}
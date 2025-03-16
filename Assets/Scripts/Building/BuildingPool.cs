using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BuildingPool : MonoBehaviour
{
    private Dictionary<BuildingData, ObjectPool<Building>> _pools = new Dictionary<BuildingData, ObjectPool<Building>>();

    public Building GetBuilding(BuildingData data)
    {
        if (!_pools.ContainsKey(data))
        {
            _pools[data] = new ObjectPool<Building>(
                () => Instantiate(data.Prefab),
                building => building.gameObject.SetActive(true),
                building => building.gameObject.SetActive(false)
            );
        }

        return _pools[data].Get();
    }

    public void ReturnBuilding(Building building)
    {
        BuildingData data = building.BuildingData;
        if (data != null && _pools.ContainsKey(data))
        {
            _pools[data].Release(building);
        }
    }

    public void PrewarmPool(BuildingData[] buildingData, int countPerType)
    {
        foreach (var data in buildingData)
        {
            if (!_pools.ContainsKey(data))
            {
                _pools[data] = new ObjectPool<Building>(
                    () => Instantiate(data.Prefab),
                    building => building.gameObject.SetActive(true),
                    building => building.gameObject.SetActive(false)
                );
            }

            var prewarmBuildings = new List<Building>();
            for (int i = 0; i < countPerType; i++)
            {
                prewarmBuildings.Add(_pools[data].Get());
            }

            foreach (var building in prewarmBuildings)
            {
                _pools[data].Release(building);
            }
        }
    }
}
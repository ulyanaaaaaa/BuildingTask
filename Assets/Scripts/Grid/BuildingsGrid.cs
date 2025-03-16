using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class BuildingsGrid : MonoBehaviour, ISaveable
{
    private int _sizeX;
    private int _sizeY;
    private BuildingData _flyingBuildingData; 
    private Building _flyingBuildingInstance;
    private Camera _mainCamera;
    [Inject] private BuildingData[] _buildingData;
    [Inject] private BuildingPool _buildingPool;
    [Inject] private GridManager _gridManager; 
    [Inject] private PlayerControls _inputActions; 
    
    public void SetSize(int gridSizeX, int gridSizeY)
    {
        _sizeX = gridSizeX;
        _sizeY = gridSizeY;
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _inputActions.DeleteBuilding.Delete.performed += _ => TryRemoveBuilding();
        
        var data = SaveSystem.Load();
        if (data != null)
        {
            Load(data);
        }
    }

    private void Update()
    {
        if (_flyingBuildingInstance != null)
        {
            UpdateFlyingBuildingPosition();
        }
    }

    private void UpdateFlyingBuildingPosition()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            int x = Mathf.RoundToInt(worldPosition.x);
            int y = Mathf.RoundToInt(worldPosition.z);
            
            bool available = x >= 0 && x <= _sizeX - _flyingBuildingData.SizeX && 
                             y >= 0 && y <= _sizeY - _flyingBuildingData.SizeY && 
                             !_gridManager.IsPlaceTaken(new Vector2Int(x, y),
                                 new Vector2Int(_flyingBuildingData.SizeX, _flyingBuildingData.SizeY));
            
            _flyingBuildingInstance.transform.position = new Vector3(x, 0, y);
            _flyingBuildingInstance.SetTransparent(available);

            if (available && Mouse.current.leftButton.wasPressedThisFrame)
            {
                PlaceFlyingBuilding(x, y);
            }
        }
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        Vector2Int position = new Vector2Int(placeX, placeY);
        Vector2Int size = new Vector2Int(_flyingBuildingData.SizeX, _flyingBuildingData.SizeY);

        if (_gridManager.IsPlaceTaken(position, size))
        {
            return;
        }

        _gridManager.AddBuilding(_flyingBuildingInstance, position, size);
        _flyingBuildingInstance.SetNormal();
        _flyingBuildingInstance = null;
        _flyingBuildingData = null;
    }

    private void TryRemoveBuilding()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Building buildingToRemove = hit.transform.GetComponent<Building>();
            if (buildingToRemove != null)
            {
                Vector2Int position = new Vector2Int(
                    Mathf.RoundToInt(buildingToRemove.transform.position.x),
                    Mathf.RoundToInt(buildingToRemove.transform.position.z)
                );
                _gridManager.RemoveBuilding(buildingToRemove, position, buildingToRemove.Size);
            }
        }
    }

    public void StartPlacingBuilding(BuildingData data)
    {
        if (_flyingBuildingInstance != null)
        {
            _buildingPool.ReturnBuilding(_flyingBuildingInstance);
        }

        _flyingBuildingData = data; 
        _flyingBuildingInstance = _buildingPool.GetBuilding(data); 
        _flyingBuildingInstance.Size = new Vector2Int(data.SizeX, data.SizeY); 
        _flyingBuildingInstance.BuildingData = data; 
    }

    public void Save(GridSaveData data)
    {
        _gridManager.Save(data);
    }

    public void Load(GridSaveData data)
    {
        _gridManager.Load(data, _buildingData);
    }

    private void OnApplicationQuit()
    {
        SaveSystem.Save(this);
    }
}
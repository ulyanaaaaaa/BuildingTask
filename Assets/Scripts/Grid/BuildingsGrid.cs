using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public Building buildingPrefab;

    private Building[,] grid;
    private Building flyingBuilding;
    private Camera mainCamera;
    private PlayerControls inputActions;

    public void Setup(PlayerControls input)
    {
        inputActions = input;
    }

    private void Start()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
        inputActions.DeleteBuilding.Delete.performed += _ => TryRemoveBuilding();
    }

    private void Update()
    {
        if (flyingBuilding)
        {
            UpdateFlyingBuildingPosition();
        }
    }

    private void UpdateFlyingBuildingPosition()
    {
        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);
            int x = Mathf.RoundToInt(worldPosition.x);
            int y = Mathf.RoundToInt(worldPosition.z);

            bool available = true;

            if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
            if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;

            if (available && IsPlaceTaken(x, y)) available = false;

            flyingBuilding.transform.position = new Vector3(x, 0, y);
            flyingBuilding.SetTransparent(available);

            if (available && Mouse.current.leftButton.wasPressedThisFrame)
            {
                PlaceFlyingBuilding(x, y);
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }
        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }
        
        flyingBuilding = Instantiate(buildingPrefab);
    }

    private void CancelBuilding()
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
            flyingBuilding = null;
        }
    }

    public void TryRemoveBuilding()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Building buildingToRemove = hit.transform.GetComponent<Building>();
            if (buildingToRemove != null)
            {
                RemoveBuilding(buildingToRemove);
            }
        }
    }

    private void RemoveBuilding(Building building)
    {
        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
            {
                for (int gridX = 0; gridX < GridSize.x; gridX++)
                {
                    for (int gridY = 0; gridY < GridSize.y; gridY++)
                    {
                        if (grid[gridX, gridY] == building)
                        {
                            grid[gridX, gridY] = null;
                        }
                    }
                }
            }
        }

        Destroy(building.gameObject);
    }
}
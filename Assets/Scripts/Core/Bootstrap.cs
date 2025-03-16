using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Inject] private PlayerControls _inputActions;
    [Inject] private ButtonsService _buttonsService;
    [Inject] private BuildingsGrid _buildingsGrid;
    [Inject] private PlaceButton _placeButton;
    [Inject] private DeleteButton _deleteButton;
    [Inject] private BuildingPool _buildingPool;
    [Inject] private GridManager _gridManager;
    [Inject] private GameConfig _gameConfig;
    [Inject(Id = "ButtonsParent")] private GameObject _buttonsParent;
    [Inject] private Canvas _canvas;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _buildingPool.PrewarmPool(_gameConfig.BuildingData, _gameConfig.PoolSize);
        
        _buildingsGrid.SetSize(_gameConfig.GridSize.x, _gameConfig.GridSize.y);
        
        CreateButtons();
        
        CreatePlaceButton();
        CreateDeleteButton();
    }

    private void CreateButtons()
    {
        foreach (var buildingData in _gameConfig.BuildingData)
        {
            var buttonInstance = Instantiate(Resources.Load<Button>(AssetsPath.Button + buildingData.BuildingName),
                _buttonsParent.transform);
            buttonInstance.onClick.AddListener(() =>
            {
                _buildingsGrid.StartPlacingBuilding(buildingData);
                _buttonsService.Place();
            });
        }
    }

    private void CreatePlaceButton()
    {
        _placeButton.transform.SetParent(_canvas.transform, false);
        _placeButton.GetComponent<Button>().onClick.AddListener(() => _buttonsService.Place());
    }

    private void CreateDeleteButton()
    {
        _deleteButton.transform.SetParent(_canvas.transform, false);
        _deleteButton.GetComponent<Button>().onClick.AddListener(() => _buttonsService.Delete());
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}
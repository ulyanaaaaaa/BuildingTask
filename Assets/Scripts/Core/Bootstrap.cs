using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameObject _buttonsParent;
    private PlayerControls _inputActions;
    private ButtonsService _buttonsService;
    private ButtonsService _houseButton;
    private ButtonsService _farmButton;
    private ButtonsService _towerButton;
    private BuildingsGrid _buildingsGrid;
    private Building _tower;
    private Building _house;
    private Building _farm;

    private void Awake()
    {
        _inputActions = new PlayerControls();
    }
    
    private void Start()
    {
        CreateGrid();
        CreateButtonsService();
        CreateBuilding();
        CreateButtons();
    }

    private void CreateButtonsService()
    {
        _buttonsService = Resources.Load<ButtonsService>("ButtonsService");
        Instantiate(_buttonsService).Setup(_inputActions);
    }

    private void CreateBuilding()
    {
        _tower = Resources.Load<Building>("Tower");
        _house = Resources.Load<Building>("House");
        _farm = Resources.Load<Building>("Farm");
    }

    private void CreateButtons()
    {
        _houseButton = Resources.Load<ButtonsService>("Button House");
        Instantiate(_houseButton, _buttonsParent.transform);
        
        //не привязывается действие
        _houseButton.GetComponent<Button>().onClick.AddListener(() =>
            _buildingsGrid.StartPlacingBuilding(_house));
        
        _farmButton = Resources.Load<ButtonsService>("Button Farm");
        Instantiate(_farmButton, _buttonsParent.transform);
        
        _farmButton.GetComponent<Button>().onClick.AddListener(() =>
            _buildingsGrid.StartPlacingBuilding(_farm));
        
        _towerButton = Resources.Load<ButtonsService>("Button Tower");
        Instantiate(_towerButton, _buttonsParent.transform);
        
        _towerButton.GetComponent<Button>().onClick.AddListener(() =>
            _buildingsGrid.StartPlacingBuilding(_tower));
    }

    private void CreateGrid()
    {
        _buildingsGrid = Resources.Load<BuildingsGrid>("BuildingGrid");
        Instantiate(_buildingsGrid).Setup(_inputActions);
    }
    
    private void OnDisable()
    {
        _inputActions.Disable();
    }
}

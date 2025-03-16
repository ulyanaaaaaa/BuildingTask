using UnityEngine;
using Zenject;

public class ButtonsService : MonoBehaviour
{
    [Inject] private PlayerControls _inputActions;

    private void Start()
    {
        _inputActions.PlaceBuilding.Enable();
        _inputActions.DeleteBuilding.Disable();
    }
    
    public void Delete()
    {
        if (_inputActions == null) return;

        _inputActions.PlaceBuilding.Disable();
        _inputActions.DeleteBuilding.Enable();
    }

    public void Place()
    {
        if (_inputActions == null) return;

        _inputActions.PlaceBuilding.Enable();
        _inputActions.DeleteBuilding.Disable();
    }
}
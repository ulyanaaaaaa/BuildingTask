using UnityEngine;

public class ButtonsService : MonoBehaviour
{
    private PlayerControls inputActions;

    public void Setup(PlayerControls controls)
    {
        inputActions = controls;
        StartButtons();
    }

    private void StartButtons()
    {
        inputActions.PlaceBuilding.Enable();
        inputActions.DeleteBuilding.Disable();
    }
    
    public void Remove()
    {
        //привязать
        inputActions.PlaceBuilding.Disable();
        inputActions.DeleteBuilding.Enable();
        Debug.Log(inputActions);
    }

    public void Place()
    {
        inputActions.PlaceBuilding.Enable();
        inputActions.DeleteBuilding.Disable();
    }
}

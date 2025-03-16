using UnityEngine;

public class Building : MonoBehaviour
{
    public Renderer MainRenderer;
    public Vector2Int Size { get; set; }
    public BuildingData BuildingData { get; set; } 

    public string BuildingName
    {
        get { return name; } 
    }

    public void SetTransparent(bool available)
    {
        MainRenderer.material.color = available ? Color.green : Color.red;
    }

    public void SetNormal()
    {
        MainRenderer.material.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(1, .1f, 1));
    }
}
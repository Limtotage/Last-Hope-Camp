using UnityEngine;
using UnityEngine.InputSystem;

public class WorkerPlacementStrategy : MonoBehaviour, IPlacementStrategy
{
    public GridSystem Grid;
    public void Handle(ResourcePlacementObjectsSO ResourceData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;
        Vector3 hitPoint = hit.point;
        Vector2Int gridPos = Grid.WorldToGrid(hitPoint);
        Vector3 snappedPos = Grid.GridToWorld(gridPos.x, gridPos.y);
        GridPlacementSystem.Instance.SetGhostPos(snappedPos);
        bool canPlace = Grid.CanPlace(gridPos.x, gridPos.y);

        var resourceType = Grid.IsNearResource(gridPos.x, gridPos.y);
        var resourceLocation = Grid.ResourceLocation(gridPos.x, gridPos.y);

        bool near = resourceType != Enums.ResourceType.None;
        bool enough = CardResourceSpend.Instance.IsResourceSufficient();

        if (canPlace && near && enough)
            GridPlacementSystem.Instance.SetGhostColor(Color.green);
        else
            GridPlacementSystem.Instance.SetGhostColor(Color.red);

        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace && near && enough)
        {
            CardResourceSpend.Instance.SpendResources();

            GameObject prefab = ResourceData.ObjectPrefab;
            GameObject obj = Instantiate(prefab, snappedPos, Quaternion.identity);

            Enums.Direction dir = GetDirection(gridPos, resourceLocation);
            obj.transform.rotation = GetRotation(dir);

            Grid.PlaceObject(gridPos.x, gridPos.y, obj);
        }
    }
    private Enums.Direction GetDirection(Vector2Int from, Vector2Int to)
    {
        Vector2Int diff = to - from;

        if (diff == Vector2Int.up) return Enums.Direction.Up;
        if (diff == Vector2Int.down) return Enums.Direction.Down;
        if (diff == Vector2Int.left) return Enums.Direction.Left;
        if (diff == Vector2Int.right) return Enums.Direction.Right;

        return Enums.Direction.Up;
    }
    private Quaternion GetRotation(Enums.Direction dir)
    {
        return dir switch
        {
            Enums.Direction.Up => Quaternion.Euler(0, 0, 0),
            Enums.Direction.Down => Quaternion.Euler(0, 180, 0),
            Enums.Direction.Left => Quaternion.Euler(0, -90, 0),
            Enums.Direction.Right => Quaternion.Euler(0, 90, 0),
            _ => Quaternion.identity
        };
    }
}

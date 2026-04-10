using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ResourcePlacementStrategy : MonoBehaviour, IPlacementStrategy
{
    public GridSystem Grid;
    public void Handle(ResourcePlacementObjectsSO resourceData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, GridPlacementSystem.Instance.GetPlacementLayer())) return;
        if (EventSystem.current.IsPointerOverGameObject()){
            GridPlacementSystem.Instance.HideGhost();
            return;
        }
        else
        {
            GridPlacementSystem.Instance.ShowGhost();
        }
        Vector3 hitPoint = hit.point;
        Vector2Int gridPos = Grid.WorldToGrid(hitPoint);
        Vector3 snappedPos = Grid.GridToWorld(gridPos.x, gridPos.y);
        GridPlacementSystem.Instance.SwitchGhost(resourceData.ResourceType);
        GridPlacementSystem.Instance.SetGhostPos(snappedPos);
        bool canPlace = Grid.CanPlace(
            gridPos.x,
            gridPos.y
        );

        bool isFull = Grid.IsResourceInRange(gridPos.x, gridPos.y, resourceData.ResourceRange);
        bool isResourceEnough = CardResourceSpend.Instance.IsResourceSufficient();
        GameObject resourcePrefab = resourceData.ObjectPrefab;
        if (canPlace && !isFull && isResourceEnough)
            GridPlacementSystem.Instance.SetGhostColor(Color.green);
        else
            GridPlacementSystem.Instance.SetGhostColor(Color.red);

        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace && !isFull && isResourceEnough)
        {
            CardResourceSpend.Instance.SpendResources();
            Debug.Log($"Placing {resourceData.ResourceType} at ({gridPos.x}, {gridPos.y})");
            GameObject obj = GameObject.Instantiate(resourcePrefab, snappedPos, Quaternion.identity);
            Grid.PlaceObject(
                gridPos.x,
                gridPos.y,
                obj
            );
        }
    }
}

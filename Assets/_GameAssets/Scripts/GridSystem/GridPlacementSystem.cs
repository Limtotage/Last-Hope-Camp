using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GridPlacementSystem : MonoBehaviour
{
    public static GridPlacementSystem Instance;
    [Header("References")]
    [SerializeField] private ResourcePlacementStrategy _resourceStrategy;
    [SerializeField] private WorkerPlacementStrategy _workerStrategy;
    [SerializeField] private GridSystem grid;
    [SerializeField] private ResourcePlacementObjectsSO _bushresourceData;
    [SerializeField] private ResourcePlacementObjectsSO _anvilresourceData;
    [SerializeField] private ResourcePlacementObjectsSO _treeresourceData;
    [SerializeField] private ResourcePlacementObjectsSO _mineresourceData;
    [SerializeField] private ResourcePlacementObjectsSO _mineWorkerData;
    [SerializeField] private ResourcePlacementObjectsSO _treeWorkerData;
    [SerializeField] private ResourcePlacementObjectsSO _bushWorkerData;
    [SerializeField] private ResourcePlacementObjectsSO _anvilWorkerData;
    [SerializeField] private GameObject _ghostWorkerPrefab;
    [SerializeField] private GameObject _ghostMinePrefab;
    [SerializeField] private GameObject _ghostBushPrefab;
    [SerializeField] private GameObject _ghostTreePrefab;
    [SerializeField] private GameObject _ghostAnvilPrefab;
    [SerializeField] private LayerMask _placementLayer;
    private GameObject _ghostInstance;
    private MeshRenderer _ghostRenderer;
    private Enums.CardTypes _currentCardType;
    private Dictionary<Enums.ResourceType, ResourcePlacementObjectsSO> _resourceMap;
    private Dictionary<Enums.CardTypes, ResourcePlacementObjectsSO> _cardMap;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _cardMap = new Dictionary<Enums.CardTypes, ResourcePlacementObjectsSO>()
        {
            { Enums.CardTypes.Mine, _mineresourceData },
            { Enums.CardTypes.Bush, _bushresourceData },
            { Enums.CardTypes.Tree, _treeresourceData },
            { Enums.CardTypes.Anvil, _anvilresourceData }
        };
        _resourceMap = new Dictionary<Enums.ResourceType, ResourcePlacementObjectsSO>()
        {
            { Enums.ResourceType.Mine, _mineWorkerData },
            { Enums.ResourceType.Tree, _treeWorkerData },
            { Enums.ResourceType.Bush, _bushWorkerData },
            { Enums.ResourceType.Sword, _anvilWorkerData }
        };
        _currentCardType = Enums.CardTypes.None;
    }
    public void DestroyCurrentGhost()
    {
        if (_ghostInstance != null)
        {
            Destroy(_ghostInstance);
        }
    }
    public void SwitchGhost(Enums.CardTypes cardType)
    {
        if (_currentCardType == cardType) return;
        DestroyCurrentGhost();
        switch (cardType)
        {
            case Enums.CardTypes.Mine:
                _currentCardType = Enums.CardTypes.Mine;
                _ghostInstance = Instantiate(_ghostMinePrefab);
                _ghostRenderer = _ghostInstance.GetComponent<MeshRenderer>();
                break;
            case Enums.CardTypes.Bush:
                _currentCardType = Enums.CardTypes.Bush;
                _ghostInstance = Instantiate(_ghostBushPrefab);
                _ghostRenderer = _ghostInstance.GetComponent<MeshRenderer>();
                break;
            case Enums.CardTypes.Tree:
                _currentCardType = Enums.CardTypes.Tree;
                _ghostInstance = Instantiate(_ghostTreePrefab);
                _ghostRenderer = _ghostInstance.GetComponent<MeshRenderer>();
                break;
            case Enums.CardTypes.Anvil:
                _currentCardType = Enums.CardTypes.Anvil;
                _ghostInstance = Instantiate(_ghostAnvilPrefab);
                _ghostRenderer = _ghostInstance.GetComponent<MeshRenderer>();
                break;
            case Enums.CardTypes.Worker:
                _currentCardType = Enums.CardTypes.Worker;
                _ghostInstance = Instantiate(_ghostWorkerPrefab);
                _ghostRenderer = _ghostInstance.GetComponent<MeshRenderer>();
                break;
            case Enums.CardTypes.Soldier:
                _currentCardType = Enums.CardTypes.Soldier;
                break;
            default:
                _ghostInstance.GetComponent<MeshFilter>().mesh = null;
                break;
        }
    }

    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            CardManager.Instance.ResetCardSelection();
            _currentCardType = Enums.CardTypes.None;
        }
        if (_ghostInstance == null) { return; }
        if (_currentCardType == Enums.CardTypes.Worker) { HandleWorkerPlacement(); }
        else if (_currentCardType != Enums.CardTypes.None)
        {
            HandleResourcePlacement();
        }
    }
    public void SetGhostPos(Vector3 pos)
    {
        if (_ghostInstance != null)
        {
            _ghostInstance.transform.position = pos;
        }
    }
    public void SetGhostColor(Color color)
    {
        if (_ghostRenderer != null)
        {
            _ghostRenderer.material.color = color;
        }
    }
    void HandleResourcePlacement()
    {
        if (!_cardMap.ContainsKey(_currentCardType)) return;
        _resourceStrategy.Handle(_cardMap[_currentCardType]);
    }
    void HandleWorkerPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _placementLayer)) return;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            HideGhost();
            return;
        }
        else
        {
            ShowGhost();
        }
        Vector3 hitPoint = hit.point;
        Vector2Int gridPos = grid.WorldToGrid(hitPoint);
        Vector3 snappedPos = grid.GridToWorld(gridPos.x, gridPos.y);
        SetGhostPos(snappedPos);
        bool canPlace = grid.CanPlace(gridPos.x, gridPos.y);

        var resourceType = grid.IsNearResource(gridPos.x, gridPos.y);
        var resourceLocation = grid.ResourceLocation(gridPos.x, gridPos.y);

        bool near = resourceType != Enums.ResourceType.None;
        bool enough = CardResourceSpend.Instance.IsResourceSufficient();

        if (canPlace && near && enough)
            SetGhostColor(Color.green);
        else
            SetGhostColor(Color.red);

        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace && near && enough)
        {
            CardResourceSpend.Instance.SpendResources();

            GameObject prefab = GetworkerPrefab(resourceType);
            GameObject obj = Instantiate(prefab, snappedPos, Quaternion.identity);

            Enums.Direction dir = GetDirection(gridPos, resourceLocation);
            obj.transform.rotation = GetRotation(dir);

            grid.PlaceObject(gridPos.x, gridPos.y, obj);
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
    private GameObject GetworkerPrefab(Enums.ResourceType resourceType)
    {
        if (!_resourceMap.ContainsKey(resourceType)) return null;
        return _resourceMap[resourceType].ObjectPrefab;
    }
    public Enums.CardTypes GetCurrentCardType()
    {
        return _currentCardType;
    }
    public LayerMask GetPlacementLayer()
    {
        return _placementLayer;
    }
    public void HideGhost()
    {
        if (_ghostInstance != null)
        {
            _ghostInstance.SetActive(false);
        }
    }
    public void ShowGhost()
    {
        if (_ghostInstance != null)
        {
            _ghostInstance.SetActive(true);
        }
    }
}



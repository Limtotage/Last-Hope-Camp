using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public int width = 11;
    public int height = 11;
    public float cellSize = 2f;

    private GridCell[,] grid;

    void Awake()
    {
        grid = new GridCell[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = new GridCell();
    }
    void Start()
    {
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.z / cellSize);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(x * cellSize + 1f, 0, y * cellSize + 1f);
    }

    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public bool CanPlace(int x, int y)
    {
        if (!IsInsideGrid(x, y)) return false;
        if (grid[x, y].isOccupied) return false;
        return true;
    }

    public void PlaceObject(int x, int y, GameObject obj)
    {
        grid[x, y].isOccupied = true;
        grid[x, y].placedObject = obj;
    }

    public Vector2Int ResourceLocation(int x, int y)
    {
        Vector2Int[] dirs =
        {
            new Vector2Int(0,-1),
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1)
        };

        foreach (var d in dirs)
        {
            int cx = x + d.x;
            int cy = y + d.y;

            if (!IsInsideGrid(cx, cy)) continue;

            var obj = grid[cx, cy].placedObject;
            if (obj != null)
            {
                if (obj.CompareTag("Mine") || obj.CompareTag("Bush") || obj.CompareTag("Tree") || obj.CompareTag("Anvil"))
                {
                    return new Vector2Int(cx, cy);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }
    public bool IsResourceInRange(int x, int y, int range)
    {
        Vector2Int[] dirs1 =
        {
        new Vector2Int(0,-1), // down
        new Vector2Int(-1,0), // left
        new Vector2Int(1,0),  // right
        new Vector2Int(0,1) ,  // up
        new Vector2Int(1,1)  , // up
        new Vector2Int(-1,1)   ,// up
        new Vector2Int(-1,-1)   ,// up
        new Vector2Int(1,-1)   ,// up
    };
        foreach (var d in dirs1)
        {
            int cx = x + d.x;
            int cy = y + d.y;

            if (!IsInsideGrid(cx, cy)) continue;

            var obj = grid[cx, cy].placedObject;
            if (obj != null)
            {
                if (obj.CompareTag("Mine") || obj.CompareTag("Bush") || obj.CompareTag("Tree") || obj.CompareTag("Anvil"))
                {
                    return true; // kaynak bulundu
                }
            }
        }
        Vector2Int[] dirs =
        {
        new Vector2Int(0,-1), // down
        new Vector2Int(-1,0), // left
        new Vector2Int(1,0),  // right
        new Vector2Int(0,1)   // up
    };

        foreach (var d in dirs)
        {
            for (int i = 2; i <= range; i++)
            {
                int cx = x + d.x * i;
                int cy = y + d.y * i;

                if (!IsInsideGrid(cx, cy)) break;

                var obj = grid[cx, cy].placedObject;

                if (obj != null)
                {
                    if (obj.CompareTag("Mine") ||
                        obj.CompareTag("Bush") ||
                        obj.CompareTag("Tree") ||
                        obj.CompareTag("Anvil"))
                    {
                        return true; // resource bulundu
                    }
                }
            }
        }

        return false;
    }
    public Enums.ResourceType IsNearResource(int x, int y)
    {
        Vector2Int[] dirs =
        {
            new Vector2Int(0,-1),
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1)
        };

        foreach (var d in dirs)
        {
            int cx = x + d.x;
            int cy = y + d.y;

            if (!IsInsideGrid(cx, cy)) continue;

            var obj = grid[cx, cy].placedObject;
            if (obj != null && obj.CompareTag("Mine"))
                return Enums.ResourceType.Mine;
            if (obj != null && obj.CompareTag("Bush"))
                return Enums.ResourceType.Bush;
            if (obj != null && obj.CompareTag("Tree"))
                return Enums.ResourceType.Tree;
            if (obj != null && obj.CompareTag("Anvil"))
                return Enums.ResourceType.Sword;

        }

        return Enums.ResourceType.None;
    }
}

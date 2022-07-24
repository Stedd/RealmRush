using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Assigned on start")]
    [SerializeField] PathFinder pathFinder;
    [SerializeField] EnemyHandler enemyHandler;

    [SerializeField] Tile[] tiles;
    [SerializeField] Vector2Int gridSize;
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    Dictionary<Vector2Int, Node> gridTemp = new Dictionary<Vector2Int, Node>();


    public Vector2Int GridSize { get { return gridSize; } }
    public Dictionary<Vector2Int, Node> Grid { get => grid; set => grid = value; }

    void Awake()
    {
        pathFinder = FindObjectOfType<PathFinder>();
        enemyHandler = FindObjectOfType<EnemyHandler>();
        tiles = FindObjectsOfType<Tile>();

        CreateGrid();
        CopyTileStatusToGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates));
            }
        }
    }

    void CopyTileStatusToGrid()
    {
        foreach (Tile _tile in tiles)
        {
            grid[GetVector2XZPosition(_tile)].isWalkable = _tile.IsWalkable;
            grid[GetVector2XZPosition(_tile)].isBuildable = _tile.IsBuildable;
        }
    }

    void ResetExploredStatus()
    {
        foreach (KeyValuePair<Vector2Int, Node> _node in grid)
        {
            _node.Value.parentNode = null;
            _node.Value.isPath = false;
            _node.Value.isExplored = false;
        }
    }

    public void CalculateNewPath()
    {
        ResetExploredStatus();
        pathFinder.CalculateNewPath();
        SetEnemyPath();
    }

    public bool CheckForValidPath()
    {
        ResetExploredStatus();
        pathFinder.CalculateNewPath();
        if (!pathFinder.PathIsValid)
        {
            return false;
        }
        SetEnemyPath();
        return true;
    }

    private void SetEnemyPath()
    {
        enemyHandler.SetPath(pathFinder.Path);
    }

    public Node GetNode(Node _node)
    {
        if (grid.ContainsKey(_node.coordinates))
        {
            return grid[_node.coordinates];
        }
        return null;
    }

    public Node GetNode(Vector2Int _coordinates)
    {
        if (grid.ContainsKey(_coordinates))
        {
            return grid[_coordinates];
        }
        return null;
    }

    public void SetNode(Node node)
    {
        if (grid.ContainsKey(node.coordinates))
        {
            grid[node.coordinates] = node;
        }
    }

    public void SetNode(Vector2Int _coordinates, Node node)
    {
        if (grid.ContainsKey(_coordinates))
        {
            grid[_coordinates] = node;
        }
    }

    public Vector2Int GetVector2XZPosition(Tile _o)
    {
        return new Vector2Int((Mathf.RoundToInt(_o.transform.position.x) / 10), (Mathf.RoundToInt(_o.transform.position.z / 10)));
    }
}

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


    public Vector2Int GridSize { get { return gridSize; } }
    public Dictionary<Vector2Int, Node> Grid { get => grid; }
    
    void Awake()
    {
        pathFinder = FindObjectOfType<PathFinder>();
        enemyHandler = FindObjectOfType<EnemyHandler>();
        tiles = FindObjectsOfType<Tile>();

        CreateGrid();
        CopyTileIsWalkableToGrid();
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }

    void CopyTileIsWalkableToGrid()
    {
        foreach (Tile _tile in tiles)
        {
            grid[GetVector2XZPosition(_tile)].isWalkable = _tile.IsWalkable;
        }
    }

    void ResetExploredStatus()
    {
        foreach (KeyValuePair<Vector2Int, Node> _entry in grid)
        {
            _entry.Value.parentNode = null;
            _entry.Value.isPath = false;
            _entry.Value.isExplored = false;
        }
    }

    public void CalculateNewPath()
    {
        ResetExploredStatus();
        //pathFinder.CalculateNewPath();
        //enemyHandler.SetPath(pathFinder.Path);
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

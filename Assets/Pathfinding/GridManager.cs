using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    public Dictionary<Vector2Int, Node> Grid { get; set; }

    private void Awake()
    {
        CreateGrid();
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

    public Node GetNode(Vector2Int _coordinates)
    {
        if (grid.ContainsKey(_coordinates))
        {
            return grid[_coordinates];
        }
        return null;
    }

    public void SetNode(Vector2Int _coordinates, Node node)
    {
        if (grid.ContainsKey(_coordinates))
        {
            grid[_coordinates] = node;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startNodeCoord;
    [SerializeField] Vector2Int goalNodeCoord;

    #region Private
    private GridManager gridManager;

    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    private Node startNode;
    private Node goalNode;
    private Node currentSearchNode;

    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    private Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    private Queue<Node> frontier = new Queue<Node>();

    private List<Node> path = new List<Node>();
    #endregion

    #region Public

    public List<Node> Path { get { return path; } }

    #endregion

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        if (gridManager != null)
        {
            grid = gridManager.Grid;
        }

        CalculateNewPath(startNodeCoord, goalNodeCoord);
    }

    public List<Node> CalculateNewPath(Vector2Int _startNodeCoord, Vector2Int _goalNodeCoord)
    {
        startNode = grid[_startNodeCoord];
        startNodeCoord = startNode.coordinates;

        goalNode = grid[_goalNodeCoord];
        goalNodeCoord = goalNode.coordinates;

        BreadthFirstSearch();

        return path;
    }


    void BreadthFirstSearch()
    {
        bool isRunning = true;

        frontier.Enqueue(startNode);
        reached.Add(startNode.coordinates, startNode);

        while (frontier.Count > 0 && isRunning)
        {
            currentSearchNode = frontier.Dequeue();
            ExploreNeighbours2(currentSearchNode);
            if (currentSearchNode.coordinates == goalNode.coordinates)
            {
                isRunning = false;
            }
        }

        BuildPath();

    }

    void ExploreNeighbours2(Node _currentSearchNode)
    {
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoord = _currentSearchNode.coordinates + direction;

            bool neighborIsOnGrid = grid.ContainsKey(neighborCoord);
            if (neighborIsOnGrid)
            {
                neighbors.Add(grid[neighborCoord]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            bool isNotReachedBefore = !reached.ContainsKey(neighbor.coordinates);
            bool isWalkable = neighbor.isWalkable;
            if (isNotReachedBefore && isWalkable)
            {
                reached.Add(neighbor.coordinates, neighbor);
                reached[neighbor.coordinates].parentNode = currentSearchNode;
                frontier.Enqueue(neighbor);
            }
        }
    }

    void BuildPath()
    {
        bool isRunning = true;
        path.Clear();
        Node node = reached[goalNode.coordinates];
        Vector2Int nodeCoord = node.coordinates;

        bool reachedEndOfGraph = reached[nodeCoord].parentNode == null;
        while (!reachedEndOfGraph && isRunning)
        {
            path.Add(node);
            grid[nodeCoord].isPath = true;
            if (node.coordinates == startNode.coordinates)
            {
                isRunning = false;
            }
            else
            {
                node = reached[nodeCoord].parentNode;
            }
        }
        path.Reverse();
    }
}

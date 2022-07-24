using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Vector2Int startNodeCoord;
    [SerializeField] Vector2Int goalNodeCoord;

    #region Private
    private bool pathIsValid;
    private GridManager gridManager;

    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    private Vector2Int[] buildabledirections = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left, new Vector2Int(1,1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1) };

    private Node startNode;
    private Node goalNode;
    private Node currentSearchNode;

    //private Dictionary<Vector2Int, Node> gridPrevious = new Dictionary<Vector2Int, Node>();
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    private Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>();

    private Queue<Node> frontier = new Queue<Node>();

    [SerializeField] List<Node> path = new List<Node>();
    #endregion

    #region Public
    public bool PathIsValid { get => pathIsValid; }
    public List<Node> Path { get { return path; } }

    #endregion

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        CalculateNewPath();
    }

    public List<Node> CalculateNewPath()
    {
        return CalculateNewPath(startNodeCoord, goalNodeCoord);
    }

    public List<Node> CalculateNewPath(Vector2Int _startNodeCoord, Vector2Int _goalNodeCoord)
    {
        //gridPrevious = grid;
        reached = new Dictionary<Vector2Int, Node>();
        grid = new Dictionary<Vector2Int, Node>();
        frontier.Clear();
        pathIsValid = false;

        if (gridManager != null)
        {
            grid = gridManager.Grid;
        }

        startNode = grid[_startNodeCoord];
        startNodeCoord = startNode.coordinates;

        goalNode = grid[_goalNodeCoord];
        goalNodeCoord = goalNode.coordinates;

        Debug.Log($"Calculating new path. Start{startNodeCoord} End{goalNodeCoord}");

        BreadthFirstSearch();

        return path;
    }


    void BreadthFirstSearch()
    {
        int safeGuard = 0;
        bool isRunning = true;

        frontier.Enqueue(startNode);
        reached.Add(startNode.coordinates, startNode);

        while (frontier.Count > 0 && isRunning && safeGuard < 1000)
        {
            currentSearchNode = frontier.Dequeue();
            ExploreNeighbours(currentSearchNode);
            reached[currentSearchNode.coordinates].isExplored = true;

            if (currentSearchNode.coordinates == goalNode.coordinates)
            {
                isRunning = false;
            }
        }

        if (reached.ContainsKey(goalNodeCoord))
        {
            pathIsValid = true;
            BuildPath();
        }
        else
        {
            foreach (Node node in path)
            {
                grid[node.coordinates].isPath = true;
            }
            Debug.Log("Path is not valid! (pathfinder)");
        }
        safeGuard++;
    }

    void ExploreNeighbours(Node _currentSearchNode)
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
        //Debug.Log($"{}");
        //Debug.Log($"Current Search Node{currentSearchNode.coordinates}");
        foreach (Node neighbor in neighbors)
        {
            bool isNotReachedBefore = !reached.ContainsKey(neighbor.coordinates);
            bool isWalkable = neighbor.isWalkable;
            if (isNotReachedBefore && isWalkable)
            {
                reached.Add(neighbor.coordinates, neighbor);
                reached[neighbor.coordinates].parentNode = currentSearchNode;
                frontier.Enqueue(neighbor);
                //Debug.Log($"Added neigbor: {reached[neighbor.coordinates].coordinates}. Parent:{reached[neighbor.coordinates].parentNode.coordinates}");
            }
        }
    }

    void BuildPath()
    {
        int safeGuard = 0;
        bool isRunning = true;
        path.Clear();
        Node node = reached[goalNode.coordinates];
        Vector2Int nodeCoord = node.coordinates;

        bool reachedEndOfGraph = reached[nodeCoord].parentNode == null;
        while (!reachedEndOfGraph && isRunning && safeGuard < 1000)
        {
            path.Add(node);
            grid[nodeCoord].isPath = true;
            if (nodeCoord == startNodeCoord)
            {
                isRunning = false;
            }
            else
            {
                node = reached[nodeCoord].parentNode;
                nodeCoord = node.coordinates;
                reached[nodeCoord].isPath = true;
            }
            safeGuard++;
        }
        path.Reverse();

    }
}

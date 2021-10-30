using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Node startNode;
    [SerializeField] Node goalNode;

    #region Private

    Node currentSearchNode;
    private Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    private GridManager gridManager;

    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    private List<Node> path = new List<Node>();
    private Queue<Node> neighbours = new Queue<Node>();

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
    }

    public void CalculateNewPath()
    {
        ExploreNeighbours();
    }

    private void ExploreNeighbours()
    {
        neighbours.Clear();

        startNode = grid[startNode.coordinates];
        goalNode = grid[goalNode.coordinates];

        neighbours.Enqueue(startNode);

        int safeGuard = 0;
        bool goalReached = false;

        while (!goalReached && safeGuard < 1000)
        {

            currentSearchNode = neighbours.Dequeue();


            Debug.Log($"Current Searchnode: {currentSearchNode.coordinates}");
            foreach (Vector2Int _direction in directions)
            {
                Node neighbourNode = new Node();
                neighbourNode.coordinates = currentSearchNode.coordinates + _direction;

                if (grid.ContainsKey(neighbourNode.coordinates))
                {
                    neighbourNode = grid[neighbourNode.coordinates];
                }
                else
                {
                    Debug.Log($"Neighbour: {neighbourNode.coordinates} is Outside Grid");
                }
                if (IsLegalNeighbour(neighbourNode))
                {
                    if (neighbourNode.coordinates == goalNode.coordinates)
                    {
                        Debug.Log("Goal Reached!");
                        grid[goalNode.coordinates].parentNode = currentSearchNode;
                        goalReached = true;
                        CreatePath();
                        break;
                    }

                    bool nodeIsAlreadyInList = false;
                    foreach (Node _node in neighbours)
                    {
                        if (neighbourNode.coordinates == _node.coordinates)
                        {
                            nodeIsAlreadyInList = true;
                        }
                    }

                    if (!nodeIsAlreadyInList)
                    {
                        Debug.Log($"Adding Neighbour: {neighbourNode.coordinates}");
                        grid[neighbourNode.coordinates].parentNode = currentSearchNode;
                        neighbours.Enqueue(neighbourNode);
                    }
                    else
                    {
                        Debug.Log($"Neighbour: {neighbourNode.coordinates} is already in Neighbours");
                    }

                }
                else
                {
                    Debug.Log($"Neighbour: {neighbourNode.coordinates} is NOT LEGAL");
                }
            }
            grid[currentSearchNode.coordinates].isExplored = true;
            //Debug.Log($"Current Searchnode: { currentSearchNode.coordinates} ** isExplored: {currentSearchNode.isExplored} **THIS NODE SHOULD NOT APPEAR AGAIN");
            safeGuard++;
        }
    }


    private void CreatePath()
    {
        path.Clear();

        Node _parentNode = grid[goalNode.coordinates].parentNode;
        grid[goalNode.coordinates].isPath = true;
        path.Add(grid[goalNode.coordinates]);

        bool reachedEnd = false;
        int safeGuard = 0;
        while (!reachedEnd && safeGuard < 100)
        {
            Node nextNode = grid[_parentNode.coordinates];
            if (nextNode.coordinates == startNode.coordinates)
            {
                grid[startNode.coordinates].isPath = true;
                path.Add(startNode);
                reachedEnd = true;
            }
            else
            {
                grid[nextNode.coordinates].isPath = true;
                path.Add(nextNode);
                _parentNode = nextNode.parentNode;
            }
            safeGuard++;
        }

        path.Reverse();

        //foreach (Node _node in path)
        //{
        //    Debug.Log($"Path: {_node.coordinates}");
        //}
    }

    private void UpdateGridNode(Node node)
    {
        gridManager.SetNode(node);
    }

    private bool IsLegalNeighbour(Node neighbourNode)
    {
        return
            neighbourNode != null &&
            !neighbourNode.isExplored &&
            neighbourNode.isWalkable;
    }
}

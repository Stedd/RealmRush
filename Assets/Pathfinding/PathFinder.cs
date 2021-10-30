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
    private List<Node> path = new List<Node>();

    #endregion

    #region Public

    public List<Node> Path { get { return path; } }

    #endregion

    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    public void CalculateNewPath()
    {
        ExploreNeighbours();
    }

    private void ExploreNeighbours()
    {
        startNode = gridManager.GetNode(startNode);
        goalNode = gridManager.GetNode(goalNode);
        currentSearchNode = startNode;
        List<Node> neighbours = new List<Node>();
        //Node neighbourNode  = new Node();
        int safeGuard = 0;
        bool goalReached = false;

        while (!goalReached && safeGuard < 1000)
        {
            if (neighbours.Count > 0)
            {
                bool newSearchNodeSelected = false;
                foreach (Node _neighbour in neighbours)
                {
                    if (!_neighbour.isExplored && !newSearchNodeSelected)
                    {
                        currentSearchNode = _neighbour;
                        newSearchNodeSelected = true;
                    }
                }
            }
            Debug.Log($"Current Searchnode: {currentSearchNode.coordinates}");
            foreach (Vector2Int _direction in directions)
            {
                Node neighbourNode = new Node();
                neighbourNode.coordinates = currentSearchNode.coordinates + _direction;
                if (NeighbourWithinGrid(neighbourNode))
                {
                    neighbourNode = gridManager.GetNode(neighbourNode.coordinates);
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
                        goalNode.parentNode = currentSearchNode;
                        UpdateGridNode(goalNode);
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
                        neighbourNode.parentNode = currentSearchNode;
                        UpdateGridNode(neighbourNode);
                        neighbours.Add(neighbourNode);
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
            currentSearchNode.isExplored = true;
            UpdateGridNode(currentSearchNode);
            //Debug.Log($"Current Searchnode: { currentSearchNode.coordinates} ** isExplored: {currentSearchNode.isExplored} **THIS NODE SHOULD NOT APPEAR AGAIN");
            safeGuard++;
        }
    }


    private void CreatePath()
    {
        path.Clear();

        Node _parentNode = goalNode.parentNode;
        goalNode.isPath = true;
        UpdateGridNode(goalNode);
        path.Add(goalNode);

        bool reachedEnd = false;
        int safeGuard = 0;
        while (!reachedEnd && safeGuard < 100)
        {
            Node nextNode = gridManager.GetNode(_parentNode.coordinates);
            if (nextNode.coordinates == startNode.coordinates)
            {
                startNode.isPath = true;
                UpdateGridNode(startNode);
                path.Add(startNode);
                reachedEnd = true;
            }
            else
            {
                nextNode.isPath = true;
                UpdateGridNode(nextNode);
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
    private bool NeighbourWithinGrid(Node neighbourNode)
    {
        return
            neighbourNode != null &&
            neighbourNode.coordinates.x >= 0 &&
            neighbourNode.coordinates.y >= 0 &&
            neighbourNode.coordinates.x <= gridManager.GridSize.x-1 &&
            neighbourNode.coordinates.y <= gridManager.GridSize.y-1;
    }
}

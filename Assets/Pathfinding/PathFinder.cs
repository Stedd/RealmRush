using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] Node currentSearchNode;
    [SerializeField] Node startNode;
    [SerializeField] Node goalNode;
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.down };

    GridManager gridManager;
    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startNode = currentSearchNode;
        ExploreNeighbours();
    }

    private void ExploreNeighbours()
    {
        List<Node> neighbours = new List<Node>();
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
            //Debug.Log($"Current Searchnode: {currentSearchNode.coordinates} ** isExplored: {currentSearchNode.isExplored} ** Starting looking in all directions");
            foreach (Vector2Int _direction in directions)
            {
                Node neighbourNode = gridManager.GetNode(currentSearchNode.coordinates + _direction);
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
                        //Debug.Log($"Adding Neighbour: {neighbourNode.coordinates} ** isExplored: {neighbourNode.isExplored}");
                        neighbourNode.parentNode = currentSearchNode;
                        UpdateGridNode(neighbourNode);
                        neighbours.Add(neighbourNode);
                    }
                    else
                    {
                        //Debug.Log($"Neighbour: {neighbourNode.coordinates} is already in Neighbours");
                    }

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
        List<Node> path = new List<Node>();
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

        foreach (Node _node in path)
        {
            Debug.Log($"Path: {_node.coordinates}");
        }
    }

    private void UpdateGridNode(Node node)
    {
        gridManager.SetNode(node.coordinates, node);
    }

    private bool IsLegalNeighbour(Node neighbourNode)
    {
        return neighbourNode != null && neighbourNode.isExplored == false && neighbourNode.coordinates.x >= 0 && neighbourNode.coordinates.y >= 0;
    }
}

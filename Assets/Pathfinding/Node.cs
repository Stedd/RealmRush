using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isBuildable;
    public bool isWalkable;
    public bool isExplored;
    public bool isPath;
    public Node parentNode;

    public Node()
    {
    }
    public Node(Vector2Int coordinates)
    {
        this.coordinates = coordinates;
    }
}

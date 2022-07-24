using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    [Header("Assigned on start")]
    [SerializeField] ScoreHandler scoreHandler;
    [SerializeField] GridManager gridManager;

    [Header("Assigned on start")]
    [SerializeField] int buildingSelector = 0;

    [Header("Prefabs")]
    [SerializeField] List<Tower> buildings = new List<Tower>();
    // Start is called before the first frame update
    void Start()
    {
        scoreHandler = FindObjectOfType<ScoreHandler>();
        gridManager = FindObjectOfType<GridManager>();
    }

    public void BuildTower(GameObject _tileGO)
    {
        //Tile _tile = _tileGO.GetComponentInChildren<Tile>();
        Node _node = gridManager.GetNode(GetVector2(_tileGO));

        //Debug.Log($"Placing tower on Tile: {_tileGO.transform.position}");
        //Debug.Log($"Placing tower on Node: {_node.coordinates}");

        if (_node.isBuildable)
        {
            if (scoreHandler.CurrentBalance - buildings[buildingSelector].Cost < 0)
            {
                print("Insufficient Funds!");
                return;
            }
            else
            {
                if (_node.isPath)
                {
                    _node.isBuildable = false;
                    _node.isWalkable = false;
                    gridManager.SetNode(_node);
                    //gridManager.CalculateNewPath();

                    if (!gridManager.CheckForValidPath())
                    {
                        _node.isBuildable = true;
                        _node.isWalkable = true;
                        gridManager.SetNode(_node);
                        print("Not allowed to block path!");
                        return;
                    }
                }

                scoreHandler.ModifyWealth(-buildings[buildingSelector].Cost);
                Instantiate(buildings[buildingSelector], _tileGO.transform.position, Quaternion.identity, transform);

                _node.isWalkable = false;
                _node.isBuildable = false;

                gridManager.SetNode(_node);
            }
        }
    }

    public Vector2Int GetVector2(GameObject _o)
    {
        return new Vector2Int((Mathf.RoundToInt(_o.transform.position.x) / 10), (Mathf.RoundToInt(_o.transform.position.z / 10)));
    }
}

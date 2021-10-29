using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    [Header("Assigned on start")] 
    [SerializeField] ScoreHandler scoreHandler;

    [Header("Assigned on start")]
    [SerializeField] int buildingSelector = 0;

    [Header("Prefabs")]
    [SerializeField] List<Tower> buildings = new List<Tower>();
    // Start is called before the first frame update
    void Start()
    {
        scoreHandler = FindObjectOfType<ScoreHandler>();
    }

    public void BuildTower(GameObject _tile_GO)
    {
        Tile _tile_Script = _tile_GO.GetComponentInChildren<Tile>();
        if(_tile_Script.IsPlaceable)
        {
            if(scoreHandler.CurrentBalance-buildings[buildingSelector].Cost < 0)
            {
                print("Insufficient Funds!");
            }
            else
            {
                scoreHandler.ModifyWealth(-buildings[buildingSelector].Cost);
                Instantiate(buildings[buildingSelector], _tile_GO.transform.position, Quaternion.identity, transform);
                _tile_Script.IsPlaceable = false;
            }
        }
    }
}

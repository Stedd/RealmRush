using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    [Header("Assigned on start")] 
    [SerializeField] ScoreHandler scoreHandler;

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
            if(scoreHandler.CurrentBalance-buildings[0].Cost < 0)
            {
                print("Insufficient Funds!");
            }
            else
            {
                scoreHandler.ModifyWealth(-buildings[0].Cost);
                Instantiate(buildings[0], _tile_GO.transform.position, Quaternion.identity, transform);
                _tile_Script.IsPlaceable = false;
            }
        }
    }
}

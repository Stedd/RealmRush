using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandler : MonoBehaviour
{
    [Header("Assigned on start")]
    [SerializeField] ScoreHandler scoreHandler;
    
    [Header("UI")]
    [SerializeField] GameObject _UI;
    [SerializeField] GameObject _buildMenu;

    [Header("Prefabs")]
    [SerializeField] List<Tower> buildings = new List<Tower>();


    public List<Tower> Buildings { get => buildings;}

    #region Privates
    GameObject buildmenu;
    #endregion

    void Start()
    {
        scoreHandler = FindObjectOfType<ScoreHandler>();
        //buildmenu = Instantiate(_buildMenuPrefab, _UI.transform);
        _buildMenu.SetActive(false);
    }

    public void OpenBuildMenu()
    {

    }

    public void OpenBuildMenu (GameObject _tile)
    {
        _buildMenu.SetActive(true);
        //_buildMenu.GetComponent<BuildPanel>().Buildings = buildings;
    }

    public void CloseBuildMenu()
    {
        _buildMenu.SetActive(false);
    }


    public void BuildTower(GameObject _tile)
    {
        Tile _tileScript = _tile.GetComponentInChildren<Tile>();
        if (_tileScript.IsPlaceable)
        {
            if (scoreHandler.CurrentBalance - buildings[0].Cost < 0)
            {
                print("Insufficient Funds!");
            }
            else
            {
                scoreHandler.ModifyWealth(-buildings[0].Cost);
                Instantiate(buildings[0], _tile.transform.position, Quaternion.identity, transform);
                _tileScript.IsPlaceable = false;
            }
        }
    }
}

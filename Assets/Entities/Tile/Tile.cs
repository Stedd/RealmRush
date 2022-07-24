using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Assigned on start")] 
    [SerializeField] BuildingHandler buildingHandler;
    [SerializeField] bool isBuildable = true;
    [SerializeField] bool isWalkable = true;

    public bool IsBuildable { get => isBuildable; }
    public bool IsWalkable { get => isWalkable; }

    private void Awake() {
        buildingHandler = FindObjectOfType<BuildingHandler>();
    }

    private void OnMouseDown() {
        //Debug.Log($"You Clicked me! {gameObject.name}");
        buildingHandler.BuildTower(gameObject);
        //buildingHandler.OpenBuildMenu(gameObject);
    }
}

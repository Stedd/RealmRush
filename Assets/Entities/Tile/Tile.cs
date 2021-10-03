using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Assigned on start")] 
    [SerializeField] BuildingHandler buildingHandler;
    [SerializeField] bool isPlaceable = true;

    public bool IsPlaceable { get => isPlaceable; set => isPlaceable = value; }

    private void Awake() {
        buildingHandler = FindObjectOfType<BuildingHandler>();
    }

    private void OnMouseDown() {
        buildingHandler.BuildTower(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] GameObject towerPrefab;
    [SerializeField] bool isPlaceable = true;

    public bool IsPlaceable { get => isPlaceable; set => isPlaceable = value; }

    private void OnMouseDown() {
        if(IsPlaceable){
            Instantiate(towerPrefab, transform.position, Quaternion.identity);
            IsPlaceable = false;
        }
    }
}

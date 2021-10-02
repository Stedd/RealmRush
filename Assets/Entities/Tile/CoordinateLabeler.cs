using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
[SerializeField] Color defaultColor = Color.white;
[SerializeField] Color occupiedColor = Color.gray;
Tile tile;
TextMeshPro label;
Vector2Int coord = new Vector2Int();

private void Awake() {
    tile    = GetComponentInParent<Tile>();
    label   = GetComponent<TextMeshPro>();
    label.enabled = false;
    UpdateLabelName();
}

private void Update() {

    ToggleLabels();
    
    if(label.enabled){
        UpdateLabelColor();
    }
    
    if(!Application.isPlaying){
        label.enabled = true;
        UpdateLabelName();
        UpdateObjectName();
    }
}

void ToggleLabels(){
    if(Input.GetKeyDown(KeyCode.L)){
        label.enabled = !label.enabled;
    }
}

void UpdateLabelName(){
    coord.x = Mathf.RoundToInt(transform.parent.position.x/UnityEditor.EditorSnapSettings.move.x);
    coord.y = Mathf.RoundToInt(transform.parent.position.z/UnityEditor.EditorSnapSettings.move.z);

    label.text = $"{coord}";
}

void UpdateLabelColor(){
    label.color = defaultColor;
    if(!tile.IsPlaceable){
        label.color = occupiedColor;
    }
}

void UpdateObjectName(){
    transform.parent.name = coord.ToString();
}

}

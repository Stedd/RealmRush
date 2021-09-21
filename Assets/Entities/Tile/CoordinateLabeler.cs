using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
TextMeshPro label;
Vector2Int coord = new Vector2Int();

private void Awake() {
    label=GetComponent<TextMeshPro>();
    UpdateLabelName();
}

private void Update() {
    UpdateLabelName();
    UpdateObjectName();
}

void UpdateLabelName(){
    coord.x = Mathf.RoundToInt(transform.parent.position.x/UnityEditor.EditorSnapSettings.move.x);
    coord.y = Mathf.RoundToInt(transform.parent.position.z/UnityEditor.EditorSnapSettings.move.z);

    label.text = $"{coord}";
}

void UpdateObjectName(){
    transform.parent.name = coord.ToString();
}

}

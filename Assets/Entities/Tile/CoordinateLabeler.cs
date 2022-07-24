using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color buildableColor = Color.black;
    [SerializeField] Color walkableColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);

    TextMeshPro label;
    Vector2Int coord = new Vector2Int();
    GridManager gridManager;


    private void Awake()
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = true;
        UpdateLabelName();
    }

    private void Update()
    {

        ToggleLabels();

        if (label.enabled)
        {
            UpdateLabelColor();
        }

        if (!Application.isPlaying)
        {
            label.enabled = true;
            UpdateLabelName();
            UpdateObjectName();
        }
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            label.enabled = !label.enabled;
        }
    }

    void UpdateLabelName()
    {
        coord.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coord.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        label.text = $"{coord}";
    }

    void UpdateLabelColor()
    {
        if (gridManager == null) { return; }

        Node node = gridManager.GetNode(coord);

        if (node == null) { return; }

        //Debug.Log($"Coloring {node.coordinates}");
        if (!node.isBuildable)
        {
            label.color = buildableColor;
        }
        else if(!node.isWalkable)
        {
            label.color = walkableColor;
        }
        else if (node.isPath)
        {
            label.color = pathColor;
        }
        else if (node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }
    }

    void UpdateObjectName()
    {
        transform.parent.name = coord.ToString();
    }

}

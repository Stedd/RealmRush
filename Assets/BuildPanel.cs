using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildPanel : MonoBehaviour
{
    [SerializeField] GameObject _buttonPrefab;
    [SerializeField] BuildingHandler _parent;
    [SerializeField] List<Tower> Buildings;
    [SerializeField] List<GameObject> _buttons;

    private RectTransform _startpos;

    void Start()
    {
        _startpos.;
    }

    void OnEnable()
    {
        //_parent = GetComponentInParent<BuildingHandler>();

        Buildings = _parent.Buildings;

        Debug.Log("asdf");
        foreach (Tower _tower in Buildings )
        {
            GameObject newButton = Instantiate(_buttonPrefab, transform);
            newButton.name = _tower.name;
            newButton.GetComponentInChildren<TMP_Text>().text = _tower.name;
            Debug.Log(newButton.name);
            _buttons.Add(newButton);
        }
    }

    //void MakeButtons()
    //{
    //    foreach (Tower _tower in Buildings)
    //    {
    //        GameObject newButton = Instantiate(_buttonPrefab, transform);
    //        newButton.GetComponentInChildren<TextMeshPro>().text = _tower.name;
    //        Debug.Log(newButton.name);
    //        _buttons.Add(newButton);
    //    }
    //}

    public void CloseBuildPanel()
    {
        gameObject.SetActive(false);
    }

}

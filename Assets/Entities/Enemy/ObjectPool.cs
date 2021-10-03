using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] List<GameObject> objects;

    public void AddObject(GameObject _object){
        //objects.Add(_object);
        _object.SetActive(false);
        GameObject newObject = Instantiate(_object, transform);
        objects.Add(newObject);
    }

    public void RemoveObject(GameObject _object){
        //objects.Remove(_object);
    }

    public void EnableFirstAvailableObject(){
        // gameObject.GetComponentsInChildren<GameObject>();
        foreach (GameObject _object in objects)
        {
            if(!_object.activeSelf)
            {
                _object.SetActive(true);
                break;
            }
            
            // Debug.Log($"Tried activating enemy but no free object in pool:{gameObject.name}");
        }
    }
}

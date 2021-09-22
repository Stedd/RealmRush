using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> allEnemies = new List<GameObject>();

    private void Start() {
        // allEnemies.Clear();
    }

    public void AddEnemyToAllEnemies(GameObject _enemy)
    {
        // Debug.Log(_enemy);
        allEnemies.Add(_enemy);
        // Debug.Log(allEnemies);
    }

    public List<GameObject> ReturnAllEnemies(){
        return allEnemies;
    }
    private void Update() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> allEnemies = new List<GameObject>();
    [SerializeField] List<Tile> path = new List<Tile>();

    private void Start() {
        GetPath();
        SpawnNewEnemy();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)){
            SpawnNewEnemy();
        }
    }

    void GetPath()
    {
        path.Clear();
        //Convert Array to List
        GameObject pathParent = GameObject.FindGameObjectWithTag("Path");
        Tile[] pathArray = pathParent.GetComponentsInChildren<Tile>();
        foreach (Tile _path in pathArray)
        {
            path.Add(_path);
        }
    }

    public void AddEnemyToAllEnemies(GameObject _enemy)
    {
        allEnemies.Add(_enemy);
    }

    public void RemoveEnemy(GameObject _enemy){
        allEnemies.Remove(_enemy);
    }

    public List<GameObject> ReturnAllEnemies(){
        return allEnemies;
    }

    void SpawnNewEnemy()
    {
        int spawnEnemyIndex = Mathf.RoundToInt(Random.Range(-0.49f,2.49f));
        print($"Spawned: {enemyPrefabs[spawnEnemyIndex].transform.name}");
        GameObject newEnemy = Instantiate(enemyPrefabs[spawnEnemyIndex], path[0].transform.position, path[0].transform.rotation);
        newEnemy.GetComponent<EnemyMovement>().SetPath(path);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] List<GameObject> allEnemies = new List<GameObject>();
    [SerializeField] List<Tile> path = new List<Tile>();

    private void Start() {
        SpawnNewEnemy();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.N)){
            SpawnNewEnemy();
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
        print(spawnEnemyIndex);
        GameObject newEnemy = Instantiate(enemyPrefabs[spawnEnemyIndex], path[0].transform.position, path[0].transform.rotation);
        newEnemy.GetComponent<EnemyMovement>().SetPath(path);
        //allEnemies.Add(newEnemy);
    }
}

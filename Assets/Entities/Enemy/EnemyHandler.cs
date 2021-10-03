using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField][Range(0.1f,2f)]float spawnRate = 1f;
    [SerializeField] int objectPoolSize = 15;

    [Header("Prefabs")]
    [SerializeField] GameObject objectPool;
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    
    [Header("Lists")]
    [SerializeField] List<Tile> path = new List<Tile>();
    [SerializeField] List<GameObject> enemyPools = new List<GameObject>();
    [SerializeField] List<GameObject> allEnemies = new List<GameObject>();

    private void Start() {
        GetPath();
        PopulateObjectPools();
        // SpawnNewEnemy();
        StartCoroutine(Spawner());
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

    void PopulateObjectPools(){
        foreach (GameObject enemy in enemyPrefabs)
        {
            GameObject newPool = Instantiate(objectPool, transform);
            newPool.transform.name = $"ObjectPool:{enemy.name}";
            ObjectPool poolScript = newPool.GetComponent<ObjectPool>();
            enemyPools.Add(newPool);

            for (int i = 0; i < objectPoolSize; i++)
            {
                enemy.GetComponent<EnemyMovement>().SetPath(path);
                poolScript.AddObject(enemy);
            }
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
        enemyPools[spawnEnemyIndex].GetComponent<ObjectPool>().EnableFirstAvailableObject();
    }

    IEnumerator Spawner()
    {
        while(true){
            SpawnNewEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
